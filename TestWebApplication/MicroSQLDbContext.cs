using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpheliaSuiteV2.Core.DataAccess.MicroSQL;
using OpheliaSuiteV2.Core.DataAccess.MicroSQL.Attributes;
using OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM;

namespace TestWebApplication {
    //"Server=127.0.0.1;Port=5432;Database=SEVENBPM;User Id=postgres;Password=#Rider170318;"
    //"Server=bpm-brm;Database=MicroSQL;User Id=seven;Password=sistemas12;MultipleActiveResultSets=True;App=ODK;Max Pool Size=30000;Pooling=true;"
    //"SERVER=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=132.147.180.141)(PORT=1522))(CONNECT_DATA=(SERVICE_NAME=PRUEBAS)));uid=OPHELIA_HOMI;pwd=Homi2018;"
    //"Server=localhost;Database=microsql;Uid=root;Pwd=#Rider170318";
    public class MicroSQLDbContext : AbstractDbContext {
        public MicroSQLDbContext() : base(DbContextOptions.Create(DbContextType.SqlServer,
            "Server=bpm-brm;Database=MicroSQL;User Id=seven;Password=sistemas12;MultipleActiveResultSets=True;App=ODK;Max Pool Size=30000;Pooling=true;")) {

        }
    }

    public class WebServiceRepo : Repository<WebService> {
        public WebServiceRepo(AbstractDbContext context) : base(context) {

        }
    }

    public class WebMethodRepo : Repository<WebMethod> {
        public WebMethodRepo(AbstractDbContext context) : base(context) {

        }
    }

    public class WebParamRepo : Repository<WebParam> {
        public WebParamRepo(AbstractDbContext context) : base(context) {

        }
    }

    [TableName("WF_WEBSE")]
    public class WebService {
        [Key(KeyType.Autoincrement)]
        [ColumnName("WEB_CONT")]
        public int Id { get; set; }
        [KeyPart]
        [ColumnName("EMP_CODI")]
        public short Company { get; set; }
        [ColumnName("WEB_CODI")]
        public string Code { get; set; }
        [ColumnName("WEB_RUTA")]
        public string Path { get; set; }
        [ColumnName("WEB_DESC")]
        public string Description { get; set; }
    }

    [TableName("WF_MWEBS")]
    public class WebMethod {
        [Key(KeyType.Autoincrement)]
        [ColumnName("MWE_CONT")]
        public int Id { get; set; }
        [KeyPart]
        [ForeignKey(nameof(Parent), nameof(WebService.Id))]
        [ColumnName("WEB_CONT")]
        public int ServiceId { get; set; }
        [KeyPart]
        [ForeignKey(nameof(Parent), nameof(WebService.Company))]
        [ColumnName("EMP_CODI")]
        public short Company { get; set; }
        [ColumnName("MWE_CODI")]
        public string Code { get; set; }
        [ColumnName("MWE_ACCI")]
        public string Method { get; set; }

        public WebService Parent { get; set; }
    }

    [TableName("WF_PMETO")]
    public class WebParam {
        [KeyPart]
        [ForeignKey(nameof(Method), nameof(WebMethod.Company))]
        public short EMP_CODI { get; set; }
        [KeyPart]
        [ForeignKey(nameof(Method), nameof(WebMethod.ServiceId))]
        public int WEB_CONT {get; set;}
        [KeyPart]
        [ForeignKey(nameof(Method), nameof(WebMethod.Id))]
        public int MWE_CONT {get; set;}
        [Key(KeyType.Autoincrement)]
        public int PME_CONT {get; set;}
        public string PME_CODI {get; set;}

        public WebMethod Method { get; set; }
    }
}
