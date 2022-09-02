namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.QueryBuilder.Clauses {
    internal abstract class AbstractOrderBy : AbstractClause {

    }

    internal class OrderBy : AbstractOrderBy {
        public string Column { get; set; }
        public bool Ascending { get; set; } = true;

        /// <inheritdoc />
        public override AbstractClause Clone() {
            return new OrderBy {
                Engine = Engine,
                Component = Component,
                Column = Column,
                Ascending = Ascending
            };
        }
    }

    internal class RawOrderBy : AbstractOrderBy {
        public string Expression { get; set; }
        public object[] Bindings { set; get; }

        /// <inheritdoc />
        public override AbstractClause Clone() {
            return new RawOrderBy {
                Engine = Engine,
                Component = Component,
                Expression = Expression,
                Bindings = Bindings,
            };
        }
    }

    internal class OrderByRandom : AbstractOrderBy {
        /// <inheritdoc />
        public override AbstractClause Clone() {
            return new OrderByRandom {
                Engine = Engine,
            };
        }
    }
}