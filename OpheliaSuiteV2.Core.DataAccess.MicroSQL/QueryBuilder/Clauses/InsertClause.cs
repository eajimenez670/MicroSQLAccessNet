using System;
using System.Collections.Generic;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.QueryBuilder.Clauses {
    internal abstract class AbstractInsertClause : AbstractClause {

    }

    internal class InsertClause : AbstractInsertClause {
        public List<Type> Types { get; set; }
        public List<string> Columns { get; set; }
        public List<object> Values { get; set; }
        public List<string> ParameterNames { get; set; }
        public bool ReturnId { get; set; } = false;

        public override AbstractClause Clone() {
            return new InsertClause {
                Engine = Engine,
                Component = Component,
                Types = Types,
                Columns = Columns,
                Values = Values,
                ParameterNames = ParameterNames,
                ReturnId = ReturnId,
            };
        }
    }

    internal class InsertQueryClause : AbstractInsertClause {
        public List<string> Columns { get; set; }
        public Query Query { get; set; }

        public override AbstractClause Clone() {
            return new InsertQueryClause {
                Engine = Engine,
                Component = Component,
                Columns = Columns,
                Query = Query.Clone(),
            };
        }
    }
}