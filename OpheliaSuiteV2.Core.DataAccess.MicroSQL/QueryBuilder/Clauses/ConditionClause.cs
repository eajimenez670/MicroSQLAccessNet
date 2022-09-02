using System.Collections.Generic;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.QueryBuilder.Clauses {
    internal abstract class AbstractCondition : AbstractClause {
        public bool IsOr { get; set; } = false;
        public bool IsNot { get; set; } = false;
    }

    /// <summary>
    /// Represents a comparison between a column and a value.
    /// </summary>
    internal class BasicCondition : AbstractCondition {
        public string Column { get; set; }
        public string Operator { get; set; }
        public virtual object Value { get; set; }

        /// <inheritdoc />
        public override AbstractClause Clone() {
            return new BasicCondition {
                Engine = Engine,
                Column = Column,
                Operator = Operator,
                Value = Value,
                IsOr = IsOr,
                IsNot = IsNot,
                Component = Component,
            };
        }
    }

    internal class BasicStringCondition : BasicCondition {
        public bool CaseSensitive { get; set; } = false;

        /// <inheritdoc />
        public override AbstractClause Clone() {
            return new BasicStringCondition {
                Engine = Engine,
                Column = Column,
                Operator = Operator,
                Value = Value,
                IsOr = IsOr,
                IsNot = IsNot,
                CaseSensitive = CaseSensitive,
                Component = Component,
            };
        }
    }

    internal class BasicDateCondition : BasicCondition {
        public string Part { get; set; }

        /// <inheritdoc />
        public override AbstractClause Clone() {
            return new BasicDateCondition {
                Engine = Engine,
                Column = Column,
                Operator = Operator,
                Value = Value,
                IsOr = IsOr,
                IsNot = IsNot,
                Part = Part,
                Component = Component,
            };
        }
    }

    /// <summary>
    /// Represents a comparison between two columns.
    /// </summary>
    internal class TwoColumnsCondition : AbstractCondition {
        public string First { get; set; }
        public string Operator { get; set; }
        public string Second { get; set; }

        /// <inheritdoc />
        public override AbstractClause Clone() {
            return new TwoColumnsCondition {
                Engine = Engine,
                First = First,
                Operator = Operator,
                Second = Second,
                IsOr = IsOr,
                IsNot = IsNot,
                Component = Component,
            };
        }
    }

    /// <summary>
    /// Represents a comparison between a column and a full "subquery".
    /// </summary>
    internal class QueryCondition<T> : AbstractCondition where T : BaseQuery<T> {
        public string Column { get; set; }
        public string Operator { get; set; }
        public Query Query { get; set; }

        /// <inheritdoc />
        public override AbstractClause Clone() {
            return new QueryCondition<T> {
                Engine = Engine,
                Column = Column,
                Operator = Operator,
                Query = Query.Clone(),
                IsOr = IsOr,
                IsNot = IsNot,
                Component = Component,
            };
        }
    }

    /// <summary>
    /// Represents a "is in" condition.
    /// </summary>
    internal class InCondition<T> : AbstractCondition {
        public string Column { get; set; }
        public IEnumerable<T> Values { get; set; }
        public override AbstractClause Clone() {
            return new InCondition<T> {
                Engine = Engine,
                Column = Column,
                Values = new List<T>(Values),
                IsOr = IsOr,
                IsNot = IsNot,
                Component = Component,
            };
        }

    }

    /// <summary>
    /// Represents a "is in subquery" condition.
    /// </summary>
    internal class InQueryCondition : AbstractCondition {
        public Query Query { get; set; }
        public string Column { get; set; }
        public override AbstractClause Clone() {
            return new InQueryCondition {
                Engine = Engine,
                Column = Column,
                Query = Query.Clone(),
                IsOr = IsOr,
                IsNot = IsNot,
                Component = Component,
            };
        }
    }

    /// <summary>
    /// Represents a "is between" condition.
    /// </summary>
    internal class BetweenCondition<T> : AbstractCondition {
        public string Column { get; set; }
        public T Higher { get; set; }
        public T Lower { get; set; }
        public override AbstractClause Clone() {
            return new BetweenCondition<T> {
                Engine = Engine,
                Column = Column,
                Higher = Higher,
                Lower = Lower,
                IsOr = IsOr,
                IsNot = IsNot,
                Component = Component,
            };
        }
    }

    /// <summary>
    /// Represents an "is null" condition.
    /// </summary>
    internal class NullCondition : AbstractCondition {
        public string Column { get; set; }

        /// <inheritdoc />
        public override AbstractClause Clone() {
            return new NullCondition {
                Engine = Engine,
                Column = Column,
                IsOr = IsOr,
                IsNot = IsNot,
                Component = Component,
            };
        }
    }

    /// <summary>
    /// Represents a boolean (true/false) condition.
    /// </summary>
    internal class BooleanCondition : AbstractCondition {
        public string Column { get; set; }
        public bool Value { get; set; }

        /// <inheritdoc />
        public override AbstractClause Clone() {
            return new BooleanCondition {
                Engine = Engine,
                Column = Column,
                IsOr = IsOr,
                IsNot = IsNot,
                Component = Component,
                Value = Value,
            };
        }
    }

    /// <summary>
    /// Represents a "nested" clause condition.
    /// i.e OR (myColumn = "A")
    /// </summary>
    internal class NestedCondition<T> : AbstractCondition where T : BaseQuery<T> {
        public T Query { get; set; }
        public override AbstractClause Clone() {
            return new NestedCondition<T> {
                Engine = Engine,
                Query = Query.Clone(),
                IsOr = IsOr,
                IsNot = IsNot,
                Component = Component,
            };
        }
    }

    /// <summary>
    /// Represents an "exists sub query" clause condition.
    /// </summary>
    internal class ExistsCondition : AbstractCondition {
        public Query Query { get; set; }

        /// <inheritdoc />
        public override AbstractClause Clone() {
            return new ExistsCondition {
                Engine = Engine,
                Query = Query.Clone(),
                IsOr = IsOr,
                IsNot = IsNot,
                Component = Component,
            };
        }
    }

    internal class RawCondition : AbstractCondition {
        public string Expression { get; set; }
        public object[] Bindings { set; get; }

        /// <inheritdoc />
        public override AbstractClause Clone() {
            return new RawCondition {
                Engine = Engine,
                Expression = Expression,
                Bindings = Bindings,
                IsOr = IsOr,
                IsNot = IsNot,
                Component = Component,
            };
        }
    }
}