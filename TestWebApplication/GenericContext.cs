using OpheliaSuiteV2.Core.DataAccess.MicroSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApplication {
    public class GenericContext: DbContext {
        public GenericContext() : base(DbContextOptions.Create(DbContextType.SqlServer,
            "Server=bpm-brm;Database=sevenbpm;User Id=seven;Password=sistemas12;MultipleActiveResultSets=True;App=ODK;Max Pool Size=30000;Pooling=true;")) {

        }
    }
}
