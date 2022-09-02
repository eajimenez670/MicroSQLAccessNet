using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpheliaSuiteV2.Core.DataAccess.MicroSQL;
using OpheliaSuiteV2.Core.DataAccess.MicroSQL.Attributes;
using OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM;

namespace TestWebApplication {
    public class SevenBPMDbContext : AbstractDbContext, ISevenBPMDbContext {
        public SevenBPMDbContext() : base(DbContextOptions.Create(DbContextType.SqlServer, "Server=bpm-brm;Database=sevenbpm;User Id=seven;Password=sistemas12;MultipleActiveResultSets=True;App=ODK;Max Pool Size=30000;Pooling=true;")) {

        }
    }

    public interface ISevenBPMDbContext {
    }

    public class LinksRepo : Repository<Link> {
        public LinksRepo(AbstractDbContext context) : base(context) {

        }
    }

    public class SeguiRepo : Repository<WF_SEGUI> {
        public SeguiRepo(AbstractDbContext context) : base(context) {

        }
    }

    [TableName("WF_LINKS")]
    public class Link {
        [Key(KeyType.Autoincrement)]
        [ColumnName("LIN_CONT")]
        public int Id {
            get; set;
        }
        [KeyPart]
        [ColumnName("EMP_CODI")]
        public short Company {
            get; set;
        }
        [ColumnName("LIN_NOMB")]
        public string Name {
            get; set;
        }
        [ColumnName("LIN_CODP")]
        public int? ParentCode {
            get; set;
        }
        [ColumnName("LIN_DURL")]
        public string URL {
            get; set;
        }
        [ColumnName("AUD_ESTA")]
        public string State {
            get; set;
        }
        [ColumnName("AUD_USUA")]
        public string ModificationUser {
            get; set;
        }
        [ColumnName("AUD_UFAC")]
        public DateTime ModificationDate {
            get; set;
        }

    }

    public class LinkDTO {
        public int Id {
            get; set;
        }
        public short Company {
            get; set;
        }
        public string Name {
            get; set;
        }
        public string URL {
            get; set;
        }
    }

    public class WF_SEGUI {
        [KeyPart]
        [ForeignKey(nameof(Caso))]
        [ForeignKey(nameof(Flujo))]
        public short EMP_CODI { get; set; }
        [KeyPart]
        [ForeignKey(nameof(Caso))]
        public string CAS_CONT { get; set; }
        [Key(KeyType.Uniqueidentifier)]
        public string SEG_CONT { get; set; }
        public string SEG_CONA { get; set; }
        [ForeignKey(nameof(Flujo))]
        public int FLU_CONT { get; set; }
        public short ETA_CONT { get; set; }
        public string SEG_SUBJ { get; set; }
        public string SEG_PRIO { get; set; }
        public DateTime SEG_FREC { get; set; }
        public DateTime SEG_HREC { get; set; }
        public DateTime SEG_FLIM { get; set; }
        public DateTime SEG_HLIM { get; set; }
        public decimal SEG_DIAE { get; set; }
        public DateTime? SEG_FCUL { get; set; }
        public DateTime? SEG_HCUL { get; set; }
        public decimal? SEG_DIAR { get; set; }
        public decimal? SEG_DIAD { get; set; }
        public string SEG_ESTC { get; set; }
        public string SEG_ABRE { get; set; }
        public string SEG_UORI { get; set; }
        public string SEG_UENC { get; set; }
        public string SEG_COME { get; set; }
        public string SEG_ESTE { get; set; }
        public string SEG_RECO { get; set; }
        public string AUD_ESTA { get; set; }
        public string AUD_USUA { get; set; }
        public DateTime AUD_UFAC { get; set; }

        public WF_CASOS Caso { get; set; }

        public WF_FLUJO Flujo { get; set; }
    }

    public class WF_CASOS {
        [KeyPart]
        public Int16 EMP_CODI { get; set; }
        [Key(KeyType.Uniqueidentifier)]
        public string CAS_CONT { get; set; }
        public string CAS_DESC { get; set; }
        public int FLU_CONT { get; set; }
        public decimal? TER_CODI { get; set; }
        public string USU_CODI { get; set; }
        public DateTime CAS_FLIM { get; set; }
        public DateTime CAS_HLIM { get; set; }
        public decimal CAS_DIAE { get; set; }
        public DateTime CAS_FECI { get; set; }
        public DateTime CAS_HORI { get; set; }
        public DateTime? CAS_FECF { get; set; }
        public DateTime? CAS_HORF { get; set; }
        public decimal? CAS_DIAR { get; set; }
        public decimal? CAS_DIAD { get; set; }
        public string CAS_ESTA { get; set; }
        public string CAS_RECO { get; set; }
        public string AUD_ESTA { get; set; }
        public string AUD_USUA { get; set; }
        public DateTime AUD_UFAC { get; set; }
        public string CAS_PRIO { get; set; }
    }

    public class WF_FLUJO {
        [KeyPart]
        public Int16 EMP_CODI { set; get; }
        [Key(KeyType.Autoincrement)]
        public int FLU_CONT { set; get; }
        public string FLU_NOMB { set; get; }
        public Int16 FOR_CONT { set; get; }
        public string FLU_DESC { set; get; }
        public Int16 GAB_CONT { set; get; }
        public Int16 CCA_CONT { set; get; }
        public Int16 FLU_DLIM { set; get; }
        public DateTime FLU_HLIM { set; get; }
        public string FLU_CLIM { set; get; }
        public Int16 FLU_DREC { set; get; }
        public DateTime FLU_HREC { set; get; }
        public string FLU_CREC { set; get; }
        public string FLU_EDIT { set; get; }
        public string FLU_USUE { set; get; }
        public string FLU_EMAI { set; get; }
        public string AUD_ESTA { set; get; }
        public string AUD_USUA { set; get; }
        public DateTime AUD_UFAC { set; get; }
        public string FLU_OBJE { set; get; }
        public string FLU_ALCA { set; get; }
        public string FLU_RESP { set; get; }
        public string FLU_COME { set; get; }
        public string FLU_EMIN { set; get; }
        public string FLU_VERS { set; get; }
        public string FLU_ESTA { set; get; }
    }
}
