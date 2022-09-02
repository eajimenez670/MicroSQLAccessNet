using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpheliaSuiteV2.Core.DataAccess.MicroSQL;

namespace TestWebApplication {
    public class AssuranceDbContext : DbContext, IAssuranceDbContext {
    }

    public interface IAssuranceDbContext: IDbContext {
    }
}
