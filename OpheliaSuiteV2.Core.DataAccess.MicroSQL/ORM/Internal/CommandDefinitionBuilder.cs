using OpheliaSuiteV2.Core.DataAccess.MicroSQL.QueryBuilder;

using System;
using System.Collections.Generic;
using System.Linq;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM.Internal {

    /// <summary>
    /// Constructor de <see cref="CommandDefinition"/>
    /// </summary>
    internal static class CommandDefinitionBuilder {

        #region Methods

        /// <summary>
        /// Construye el listado de comandos listos para ejecutar
        /// </summary>
        /// <param name="stateManager">Administrador de estados</param>
        /// <param name="context">Contexto de datos</param>
        /// <param name="withCreateCommand">Valor que indica si las definiciones van a ejecutar la creación de comandos</param>
        /// <returns>Lista de comandos</returns>
        public static IEnumerable<CommandDefinition> Build(StateManager stateManager, DbContext context, bool withCreateCommand = false) {
            //List<CommandDefinition> commands = new List<CommandDefinition>();
            foreach (EntityEntry entity in stateManager.GetEntitiesToProcess()) {
                yield return BuildCommand(entity, context, withCreateCommand);
            }

            //return commands;
        }

        /// <summary>
        /// Construye un comando a partir de una entrada de entidad
        /// </summary>
        /// <param name="entity">Entrada de entidad</param>
        /// <param name="context">Contexto de datos</param>
        /// <param name="withCreateCommand">Valor que indica si se crea el comando de una</param>
        /// <returns>Definición de comando</returns>
        private static CommandDefinition BuildCommand(EntityEntry entity, DbContext context, bool withCreateCommand) {
            CommandDefinition command = null;
            switch (entity.State) {
                case EntityState.Added:
                    command = BuildInsertCommand(entity, context, withCreateCommand);
                    break;
                case EntityState.Modified:
                    command = BuildUpdateCommand(entity, context, withCreateCommand);
                    break;
                case EntityState.Removed:
                    command = BuildDeleteCommand(entity, context, withCreateCommand);
                    break;
            }
            return command;
        }

        /// <summary>
        /// Construye un comando de inserción
        /// </summary>
        /// <param name="entity">Entrada de entidad</param>
        /// <param name="context">Contexto de datos</param>
        /// <param name="withCreateCommand">Valor que indica si se crea el comando de una</param>
        /// <returns>Definición de comando</returns>
        private static CommandDefinition BuildInsertCommand(EntityEntry entity, DbContext context, bool withCreateCommand) {
            //Si la llave primaria es Autoincrement, realizamos el calculo
            PropertySnapshot identityKeyProperty = null;
            if (entity.Key.Type == Attributes.KeyType.Autoincrement)
                CalcAutoincrementIdKey(entity, context);
            if (entity.Key.Type == Attributes.KeyType.Uniqueidentifier)
                CalcUniqueidentifierIdKey(entity);
            if (entity.Key.Type == Attributes.KeyType.Identity)
                identityKeyProperty = entity.Key.Properties.FirstOrDefault(p => p.Descriptor.IsKey);
            //Armamos el comando sql a partir de la entidad
            Query query = new Query(entity.Descriptor.TableName);
            //Si la llave primaria es Identity, filtramos las propiedades para no enviar al insert dicha propiedad
            query.AsInsert((identityKeyProperty == null ? entity.CurrentValues.Properties : entity.CurrentValues.Properties.Where(p => p.Descriptor.Name != identityKeyProperty.Descriptor.Name)));
            var sqlResult = context.Builder.SqlCompiler.Compile(query);
            return new CommandDefinition(sqlResult.Sql, context, sqlResult.Parameters, context?.TransactionInfo?.Transaction, withCreateCommand, entity);
        }

        /// <summary>
        /// Calcula el Id de la llave primaria
        /// </summary>
        /// <param name="entity">Entrada de entidad</param>
        private static void CalcUniqueidentifierIdKey(EntityEntry entity) {
            PropertySnapshot propKey = entity.Key.Properties.First(p => p.Descriptor.IsKey);
            propKey.Descriptor.Property.SetValue(entity.Entity, Guid.NewGuid());
        }

        /// <summary>
        /// Calcula el Id de la llave primaria
        /// </summary>
        /// <param name="entity">Entrada de entidad</param>
        /// <param name="context">Contexto de datos</param>
        private static void CalcAutoincrementIdKey(EntityEntry entity, DbContext context) {
            PropertySnapshot propKey = entity.Key.Properties.First(p => p.Descriptor.IsKey);
            Query query = new Query(entity.Descriptor.TableName).AsMax(propKey.Descriptor.ColumnName);

            query.Where(entity.CurrentValues.Properties.Where(prop => entity.Key.Properties.Where(p => p.Descriptor.IsKeyPart).Any(pp => pp.Descriptor.Name == prop.Descriptor.Name)).ToList());
            var sqlResult = context.Builder.SqlCompiler.Compile(query);
            dynamic result = context.ExecuteQuery(sqlResult.Sql, sqlResult.Parameters.ToArray()).FirstOrDefault();
            if (result == null) {
                propKey.Descriptor.Property.SetValue(entity.Entity, 1);
            } else {
                propKey.Descriptor.Property.SetValue(entity.Entity, Convert.ChangeType((result.max == null ? 1 : (result.max + 1)), propKey.Descriptor.Property.PropertyType));
            }
        }

        /// <summary>
        /// Construye un comando de actualización
        /// </summary>
        /// <param name="entity">Entrada de entidad</param>
        /// <param name="context">Contexto de datos</param>
        /// <param name="withCreateCommand">Valor que indica si se crea el comando de una</param>
        /// <returns>Definición de comando</returns>
        private static CommandDefinition BuildUpdateCommand(EntityEntry entity, DbContext context, bool withCreateCommand) {
            //Armamos el comando sql a partir de la entidad
            Query query = new Query(entity.Descriptor.TableName);
            //Obtenemos los valores actuales
            EntityStateSnapshot currentValues = entity.CurrentValues;
            List<PropertySnapshot> updateProperties = new List<PropertySnapshot>();
            //Recorremos solo las propiedades que cambiaron
            foreach (PropertySnapshot originalProp in entity.OriginalValues.Properties) {
                PropertySnapshot currentProp = currentValues.Properties.First(p => p.Descriptor.Name == originalProp.Descriptor.Name);
                if (originalProp.FootPrint != currentProp.FootPrint) {
                    updateProperties.Add(currentProp);
                }
            }
            //Armamos las columnas llave con sus valores para la condición de actualización
            query.AsUpdate(updateProperties).Where(entity.Key.Properties);
            var sqlResult = context.Builder.SqlCompiler.Compile(query);
            return new CommandDefinition(sqlResult.Sql, context, sqlResult.Parameters, context?.TransactionInfo?.Transaction, withCreateCommand, entity);
        }

        /// <summary>
        /// Construye un comando de eliminación
        /// </summary>
        /// <param name="entity">Entrada de entidad</param>
        /// <param name="context">Contexto de datos</param>
        /// <param name="withCreateCommand">Valor que indica si se crea el comando de una</param>
        /// <returns>Definición de comando</returns>
        private static CommandDefinition BuildDeleteCommand(EntityEntry entity, DbContext context, bool withCreateCommand) {
            //Armamos el comando sql a partir de la entidad
            Query query = new Query(entity.Descriptor.TableName);

            query.AsDelete().Where(entity.Key.Properties);
            var sqlResult = context.Builder.SqlCompiler.Compile(query);

            return new CommandDefinition(sqlResult.Sql, context, sqlResult.Parameters.ToArray(), context?.TransactionInfo?.Transaction, withCreateCommand, entity);
        }

        #endregion
    }
}
