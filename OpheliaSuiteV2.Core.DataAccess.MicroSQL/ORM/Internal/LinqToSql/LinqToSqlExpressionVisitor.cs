using OpheliaSuiteV2.Core.DataAccess.MicroSQL.QueryBuilder;
using OpheliaSuiteV2.Core.DataAccess.MicroSQL.QueryBuilder.Compilers;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM.Internal.LinqToSql {

    /// <summary>
    /// Ejecuta una visita recursiva a todo el arbol de una expresión
    /// para generar un comando Sql
    /// </summary>
    internal sealed class LinqToSqlExpressionVisitor<TEntity> : ExpressionVisitor where TEntity : class, new() {

        #region Fields

        /// <summary>
        /// Compilador Sql
        /// </summary>
        private readonly Compiler _sqlCompiler;

        #endregion

        #region Properties

        /// <summary>
        /// Comando sql resultado
        /// </summary>
        public Query Query { get; private set; }

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="sqlCompiler">Compilador Sql</param>
        public LinqToSqlExpressionVisitor(Compiler sqlCompiler) {
            _sqlCompiler = sqlCompiler ?? throw Error.ArgumentException(nameof(sqlCompiler));
            Query = new Query(Helpers.GetTableName<TEntity>()).Select(Helpers.GetColumns<TEntity>().ToArray());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Obtiene los parámetros de la consulta
        /// </summary>
        /// <param name="node">Expresión a evaluar</param>
        private void GetParameters(Expression node) {
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        public override Expression Visit(Expression node) {
            Console.WriteLine(node);
            return base.Visit(node);
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitBinary(BinaryExpression node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitBlock(BlockExpression node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override CatchBlock VisitCatchBlock(CatchBlock node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitConditional(ConditionalExpression node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitConstant(ConstantExpression node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitDebugInfo(DebugInfoExpression node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitDefault(DefaultExpression node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitDynamic(DynamicExpression node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override ElementInit VisitElementInit(ElementInit node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitExtension(Expression node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitGoto(GotoExpression node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitIndex(IndexExpression node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitInvocation(InvocationExpression node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitLabel(LabelExpression node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override LabelTarget VisitLabelTarget(LabelTarget node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitLambda<T>(Expression<T> node) {
            Console.WriteLine(node);
            return base.VisitLambda<T>(node);
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitListInit(ListInitExpression node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitLoop(LoopExpression node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitMember(MemberExpression node) {
            Console.WriteLine(node);
            return base.VisitMember(node);
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override MemberAssignment VisitMemberAssignment(MemberAssignment node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override MemberBinding VisitMemberBinding(MemberBinding node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitMemberInit(MemberInitExpression node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override MemberListBinding VisitMemberListBinding(MemberListBinding node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitMethodCall(MethodCallExpression node) {
            Console.WriteLine(node);
            return base.VisitMethodCall(node);
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitNew(NewExpression node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitNewArray(NewArrayExpression node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitSwitch(SwitchExpression node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override SwitchCase VisitSwitchCase(SwitchCase node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitTry(TryExpression node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitTypeBinary(TypeBinaryExpression node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera comandos
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitUnary(UnaryExpression node) {
            Console.WriteLine(node);
            throw Error.UnsupportedExpressionException(node.ToString());
        }

        /// <summary>
        /// Genera los parámetros
        /// </summary>
        /// <param name="node">Nodo a visitar</param>
        /// <returns>Expresión a continuar</returns>
        protected override Expression VisitParameter(ParameterExpression node) {
            Console.WriteLine(node);
            return base.VisitParameter(node);
        }

        #endregion
    }
}
