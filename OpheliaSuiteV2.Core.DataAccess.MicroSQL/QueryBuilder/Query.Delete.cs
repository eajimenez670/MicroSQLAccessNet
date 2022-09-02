namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.QueryBuilder {
    internal partial class Query {
        public Query AsDelete() {
            Method = "delete";
            return this;
        }

    }
}