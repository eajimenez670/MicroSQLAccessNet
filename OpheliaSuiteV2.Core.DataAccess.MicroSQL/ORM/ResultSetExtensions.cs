using OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM.Internal;
using OpheliaSuiteV2.Core.DataAccess.MicroSQL.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM {

    /// <summary>
    /// Provee métodos de extensión para los conjuntos de resultados
    /// </summary>
    internal static class ResultSetExtensions {

        #region Expression

        /// <summary>
        /// Obtiene el acceso a la propiedad en la expresión
        /// </summary>
        /// <param name="propertyAccessExpression">Expresión a evaluar</param>
        /// <returns>Propiedad en la expresión</returns>
        internal static PropertyInfo GetPropertyAccess(this LambdaExpression propertyAccessExpression) {
            var parameterExpression = propertyAccessExpression.Parameters.Single();
            var propertyInfo = parameterExpression.MatchSimplePropertyAccess(propertyAccessExpression.Body);

            if (propertyInfo == null)
                throw Error.InvalidPropertyExpressionException();

            var declaringType = propertyInfo.DeclaringType;
            var parameterType = parameterExpression.Type;

            if (declaringType != null
                && declaringType != parameterType
                && declaringType.GetTypeInfo().IsInterface
                && declaringType.GetTypeInfo().IsAssignableFrom(parameterType.GetTypeInfo())) {
                var propertyGetter = propertyInfo.GetMethod;
                var interfaceMapping = parameterType.GetTypeInfo().GetRuntimeInterfaceMap(declaringType);
                var index = Array.FindIndex(interfaceMapping.InterfaceMethods, p => propertyGetter.Equals(p));
                var targetMethod = interfaceMapping.TargetMethods[index];
                foreach (var runtimeProperty in parameterType.GetRuntimeProperties()) {
                    if (targetMethod.Equals(runtimeProperty.GetMethod)) {
                        return runtimeProperty;
                    }
                }
            }

            return propertyInfo;
        }

        private static PropertyInfo MatchSimplePropertyAccess(this Expression parameterExpression, Expression propertyAccessExpression) {
            var propertyInfos = MatchPropertyAccess(parameterExpression, propertyAccessExpression);

            int count = (propertyInfos == null ? 0 : propertyInfos.Count);

            // Aqui restringimos el uso de inclusiones a un solo nivel.
            // TODO: Luego se debe implelemtar la inclusión a más de un nivel.
            if (count > 1)
                throw Error.InvalidIncludeOperationException();

            return count > 0 ? propertyInfos[count - 1] : null;
        }

        private static IReadOnlyList<PropertyInfo> MatchPropertyAccess(
            this Expression parameterExpression, Expression propertyAccessExpression) {
            var propertyInfos = new List<PropertyInfo>();

            MemberExpression memberExpression;

            do {
                memberExpression = RemoveTypeAs(propertyAccessExpression.RemoveConvert()) as MemberExpression;

                if (!(memberExpression?.Member is PropertyInfo propertyInfo)) {
                    return null;
                }

                propertyInfos.Insert(0, propertyInfo);

                propertyAccessExpression = memberExpression.Expression;
            }
            while (RemoveTypeAs(memberExpression.Expression.RemoveConvert()) != parameterExpression);

            return propertyInfos;
        }

        private static Expression RemoveTypeAs(this Expression expression) {
            while ((expression?.NodeType == ExpressionType.TypeAs)) {
                expression = ((UnaryExpression)expression.RemoveConvert()).Operand;
            }

            return expression;
        }

        private static Expression RemoveConvert(this Expression expression) {
            while (expression != null
                   && (expression.NodeType == ExpressionType.Convert
                       || expression.NodeType == ExpressionType.ConvertChecked)) {
                expression = RemoveConvert(((UnaryExpression)expression).Operand);
            }

            return expression;
        }

        #endregion

        #region Include

        /// <summary>
        /// Incluye en el conjunto de resultados la
        /// entidad que referencia la propiedad de navegación
        /// </summary>
        /// <typeparam name="TEntity">Tipo de la entidad del conjunto de resultados</typeparam>
        /// <param name="resultSet">Conjunto de resultados que extiende el método</param>
        /// <param name="navigationPropertyExpression">Propiedad de navegación en la entidad</param>
        /// <returns>Conjunto de resultados</returns>
        public static ResultSet<TEntity> Include<TEntity>(this ResultSet<TEntity> resultSet, Expression<Func<TEntity, object>> navigationPropertyExpression) where TEntity : class, new() {
            //Validamos valores nulos
            if (resultSet == null)
                return null;
            if (navigationPropertyExpression == null)
                throw Error.ArgumentException(nameof(navigationPropertyExpression));

            //Obtenemos la propiedad en la expresión
            PropertyInfo navigationProperty = navigationPropertyExpression.GetPropertyAccess();
            if (navigationProperty == null)
                throw Error.ArgumentException(nameof(navigationPropertyExpression));

            //Validamos que la propiedad exista en la entidad
            EntityDescriptor entityDescriptor = EntityDescriptorMapper.GetOrAddDescriptor(typeof(TEntity));
            if (entityDescriptor.EntityType.GetProperty(navigationProperty.Name) == null)
                throw Error.NavigationPropertyDontExistsException(navigationProperty.Name, entityDescriptor.EntityType.Name);

            //Validamos que la propiedad sea de tipo navegación
            if (!Consts.IsValidNavigationPropertyType(navigationProperty.PropertyType))
                throw Error.InvalidNavigationPropertyException(navigationProperty.Name);

            EntityDescriptor foreingEntityDescriptor = EntityDescriptorMapper.GetOrAddDescriptor(navigationProperty.PropertyType);

            //Recorremos el conjunto de resultados para ejecutar las consultas de
            //la entidad relacionada
            Query query = null; // new Query(Helpers.GetTableName(navigationProperty.PropertyType)).Select(Helpers.GetColumns<TEntity>().ToArray());
            foreach (TEntity entity in resultSet.Result) {
                if (navigationProperty.GetValue(entity) == null) {
                    //Obtenemos la relación entre las dos entidades
                    if (resultSet.Context.StateManager.TryGetRelationship(foreingEntityDescriptor.EntityType, entityDescriptor.EntityType, out Relationship relationship)) {
                        Query subQuery = new Query(foreingEntityDescriptor.TableName).Select(foreingEntityDescriptor.Properties.Select(kv => kv.Key).ToArray());
                        subQuery.CountParameters = query?.CountParameters ?? 0;
                        //Creamos la lista de propiedades a usar en la consulta
                        List<PropertySnapshot> properties = new List<PropertySnapshot>();
                        foreach (PropertyPair prop in relationship.Properties) {
                            PropertySnapshot property = new PropertySnapshot(new PropertyDescriptor(prop.PrincipalProperty), null);
                            property.Value = prop.ForeignProperty.GetValue(entity);
                            properties.Add(property);
                        }
                        subQuery.Where(properties);
                        if (query == null) {
                            query = subQuery;
                        } else {
                            query.UnionAll(subQuery);
                            query.CountParameters = subQuery.CountParameters;
                            query.Parameters.AddRange(subQuery.Parameters);
                        }
                    }
                }
            }
            SqlResult sqlResult = resultSet.Context.InternalDbContext.Builder.SqlCompiler.Compile(query);
            //Ejecutamos el metodo de consulta
            MethodInfo ExecuteQuery = resultSet.Context.InternalDbContext.GetType().GetMethod("ExecuteQuery", 1, new Type[] { typeof(string), typeof(Parameter[]) });
            ExecuteQuery = ExecuteQuery.MakeGenericMethod(navigationProperty.PropertyType);
            var res = ((IEnumerable<object>)ExecuteQuery.Invoke(resultSet.Context.InternalDbContext, new object[] { sqlResult.Sql, sqlResult.Parameters.ToArray() })).ToList();
            resultSet.Context.AttachEntities(res, EntityState.Unchanged);

            return new ResultSet<TEntity>(resultSet.Context, resultSet.Result);
        }

        #endregion
    }
}
