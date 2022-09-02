using OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM.Internal;
using OpheliaSuiteV2.Core.DataAccess.MicroSQL.QueryBuilder.Clauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.QueryBuilder {
    internal partial class Query : BaseQuery<Query> {
        public bool IsDistinct { get; set; } = false;
        public string QueryAlias { get; set; }
        public string Method { get; set; } = "select";
        public int CountParameters { get; set; } = 0;
        public List<Parameter> Parameters { get; } = new List<Parameter>();

        public Query() : base() {
        }

        public Query(string table) : base() {
            From(table);
        }

        public bool HasOffset(string engineCode = null) {
            var limitOffset = this.GetOneComponent<LimitOffset>("limit", engineCode);

            return limitOffset?.HasOffset() ?? false;
        }

        public bool HasLimit(string engineCode = null) {
            var limitOffset = this.GetOneComponent<LimitOffset>("limit", engineCode);

            return limitOffset?.HasLimit() ?? false;
        }

        internal int GetOffset(string engineCode = null) {
            var limitOffset = this.GetOneComponent<LimitOffset>("limit", engineCode);

            return limitOffset?.Offset ?? 0;
        }

        internal int GetLimit(string engineCode = null) {
            var limitOffset = this.GetOneComponent<LimitOffset>("limit", engineCode);

            return limitOffset?.Limit ?? 0;
        }

        public override Query Clone() {
            var clone = base.Clone();
            clone.QueryAlias = QueryAlias;
            clone.IsDistinct = IsDistinct;
            clone.Method = Method;
            clone.Parameters.AddRange(Parameters);
            return clone;
        }

        public Query As(string alias) {
            QueryAlias = alias;
            return this;
        }

        public Query For(string engine, Func<Query, Query> fn) {
            EngineScope = engine;

            var result = fn.Invoke(this);

            // reset the engine
            EngineScope = null;

            return result;
        }

        public Query With(Query query) {
            // Clear query alias and add it to the containing clause
            if (string.IsNullOrWhiteSpace(query.QueryAlias)) {
                throw new InvalidOperationException("No Alias found for the CTE query");
            }

            query = query.Clone();

            var alias = query.QueryAlias.Trim();

            // clear the query alias
            query.QueryAlias = null;

            return AddComponent("cte", new QueryFromClause {
                Query = query,
                Alias = alias,
            });
        }

        public Query With(Func<Query, Query> fn) {
            return With(fn.Invoke(new Query()));
        }

        public Query With(string alias, Query query) {
            return With(query.As(alias));
        }

        public Query With(string alias, Func<Query, Query> fn) {
            return With(alias, fn.Invoke(new Query()));
        }

        public Query WithRaw(string alias, string sql, params object[] bindings) {
            return AddComponent("cte", new RawFromClause {
                Alias = alias,
                Expression = sql,
                Bindings = bindings,
            });
        }

        public Query Limit(int value) {
            var clause = GetOneComponent("limit", EngineScope) as LimitOffset;

            if (clause != null) {
                clause.Limit = value;
                return this;
            }

            return AddComponent("limit", new LimitOffset {
                Limit = value
            });
        }

        public Query Offset(int value) {
            var clause = GetOneComponent("limit", EngineScope) as LimitOffset;

            if (clause != null) {
                clause.Offset = value;
                return this;
            }

            return AddComponent("limit", new LimitOffset {
                Offset = value
            });
        }

        /// <summary>
        /// Alias for Limit
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public Query Take(int limit) {
            return Limit(limit);
        }

        /// <summary>
        /// Alias for Offset
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public Query Skip(int offset) {
            return Offset(offset);
        }

        /// <summary>
        /// Set the limit and offset for a given page.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public Query ForPage(int page, int perPage = 15) {
            return Skip((page - 1) * perPage).Take(perPage);
        }

        public Query Distinct() {
            IsDistinct = true;
            return this;
        }

        /// <summary>
        /// Apply the callback's query changes if the given "condition" is true.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="whenTrue">Invoked when the condition is true</param>
        /// <param name="whenFalse">Optional, invoked when the condition is false</param>
        /// <returns></returns>
        public Query When(bool condition, Func<Query, Query> whenTrue, Func<Query, Query> whenFalse = null) {
            if (condition && whenTrue != null) {
                return whenTrue.Invoke(this);
            }

            if (!condition && whenFalse != null) {
                return whenFalse.Invoke(this);
            }

            return this;
        }

        /// <summary>
        /// Apply the callback's query changes if the given "condition" is false.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public Query WhenNot(bool condition, Func<Query, Query> callback) {
            if (!condition) {
                return callback.Invoke(this);
            }

            return this;
        }

        public Query OrderBy(params string[] columns) {
            foreach (var column in columns) {
                AddComponent("order", new OrderBy {
                    Column = column,
                    Ascending = true
                });
            }

            return this;
        }

        public Query OrderByDesc(params string[] columns) {
            foreach (var column in columns) {
                AddComponent("order", new OrderBy {
                    Column = column,
                    Ascending = false
                });
            }

            return this;
        }

        public Query OrderByRaw(string expression, params object[] bindings) {
            return AddComponent("order", new RawOrderBy {
                Expression = expression,
                Bindings = Helper.Flatten(bindings).ToArray()
            });
        }

        public Query OrderByRandom(string seed) {
            return AddComponent("order", new OrderByRandom { });
        }

        public Query GroupBy(params string[] columns) {
            foreach (var column in columns) {
                AddComponent("group", new Column {
                    Name = column
                });
            }

            return this;
        }

        public Query GroupByRaw(string expression, params object[] bindings) {
            AddComponent("group", new RawColumn {
                Expression = expression,
                Bindings = bindings,
            });

            return this;
        }

        public override Query NewQuery() {
            return new Query();
        }

        /// <summary>
        /// Crea una clausula WHERE a partir de una lista de <see cref="PropertySnapshot"/>
        /// </summary>
        /// <param name="properties">Lista de propiedades</param>
        /// <returns>Consulta</returns>
        public Query Where(IEnumerable<PropertySnapshot> properties) {
            StringBuilder conditions = new StringBuilder();

            foreach (PropertySnapshot par in properties) {
                string parName = $"p{CountParameters}";
                if (conditions.Length == 0)
                    conditions.Append($"[{par.Descriptor.ColumnName}]={{{parName}}} ");
                else
                    conditions.Append($"AND [{par.Descriptor.ColumnName}]={{{parName}}} ");

                Parameters.Add(new Parameter(parName, par.Value, par.Descriptor.Property.PropertyType));
                CountParameters++;
            }
            return WhereRaw(conditions.ToString());
        }
    }
}
