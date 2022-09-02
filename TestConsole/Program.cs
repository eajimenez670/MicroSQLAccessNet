using System;
using System.Linq;
using OpheliaSuiteV2.Core.DataAccess.MicroSQL;

namespace TestConsole {
    class Program {
        static void Main(string[] args) {
            IDbContext dbContext = new SevenBPMDbContext(DbContextOptions.Create(DbContextType.SqlServer, "Server=localhost;Database=sevenbpm;User Id=sa;Password=#Rider170318;MultipleActiveResultSets=True;App=ODK;Max Pool Size=30000;Pooling=true;"));
            var res = dbContext.ExecuteQuery<CustomTaskTryResult>("SELECT S.CAS_CONT, S.SEG_CONT, S.SEG_SUBJ, S.SEG_FREC, S.SEG_HREC, S.SEG_FLIM, S.SEG_HLIM, S.SEG_FCUL, S.SEG_HCUL, S.SEG_DIAE, S.SEG_DIAR, S.SEG_ESTE, S.SEG_UENC, D.DBP_CO01, V.VBP_CO01, D.DBP_CO02, V.VBP_CO02, D.DBP_CO03, V.VBP_CO03, D.DBP_CO04, V.VBP_CO04, D.DBP_CO05, V.VBP_CO05, D.DBP_CO06, V.VBP_CO06, D.DBP_CO07, V.VBP_CO07, D.DBP_CO08, V.VBP_CO08, D.DBP_CO09, V.VBP_CO09, D.DBP_CO10, V.VBP_CO10, D.DBP_CO11, V.VBP_CO11, D.DBP_CO12, V.VBP_CO12, D.DBP_CO13, V.VBP_CO13, D.DBP_CO14, V.VBP_CO14, D.DBP_CO15, V.VBP_CO15, D.DBP_CO16, V.VBP_CO16, D.DBP_CO17, V.VBP_CO17, D.DBP_CO18, V.VBP_CO18, D.DBP_CO19, V.VBP_CO19, D.DBP_CO20, V.VBP_CO20 FROM WF_SEGUI S LEFT JOIN WF_DBPER D ON S.EMP_CODI = D.EMP_CODI AND S.FLU_CONT = D.FLU_CONT LEFT JOIN WF_VBPER V ON S.EMP_CODI = V.EMP_CODI AND S.CAS_CONT = V.CAS_CONT WHERE S.SEG_ESTE IN ('H', 'P') AND S.EMP_CODI = {pEMP_CODI} AND S.FLU_CONT = {pFLU_CONT} AND S.SEG_UENC = {pSEG_UENC}	", 
                new { pEMP_CODI = short.Parse("102"), pFLU_CONT = 399142, pSEG_UENC = "NAUJOYAMAT" }).ToList();

            foreach (var ent in res) {
                Console.WriteLine($"SEG_DIAE: {ent.SEG_DIAE}");
            }

            Console.ReadKey();
        }
    }

    public class SevenBPMDbContext : DbContext {
        public SevenBPMDbContext(DbContextOptions options) : base(options) { }
    }

    public class CustomTaskTryResult {
        public string CAS_CONT { get; set; }
        public string SEG_CONT { get; set; }
        public string SEG_SUBJ { get; set; }
        public string SEG_UENC { get; set; }
        public DateTime SEG_FREC { get; set; }
        public DateTime SEG_HREC { get; set; }
        public DateTime SEG_FLIM { get; set; }
        public DateTime SEG_HLIM { get; set; }
        public DateTime? SEG_FCUL { get; set; }
        public DateTime? SEG_HCUL { get; set; }
        public decimal SEG_DIAE { get; set; }
        public decimal? SEG_DIAR { get; set; }
        public string SEG_ESTE { get; set; }
        public string VBP_CO01 { get; set; }
        public string VBP_CO02 { get; set; }
        public string VBP_CO03 { get; set; }
        public string VBP_CO04 { get; set; }
        public string VBP_CO05 { get; set; }
        public string VBP_CO06 { get; set; }
        public string VBP_CO07 { get; set; }
        public string VBP_CO08 { get; set; }
        public string VBP_CO09 { get; set; }
        public string VBP_CO10 { get; set; }
        public string VBP_CO11 { get; set; }
        public string VBP_CO12 { get; set; }
        public string VBP_CO13 { get; set; }
        public string VBP_CO14 { get; set; }
        public string VBP_CO15 { get; set; }
        public string VBP_CO16 { get; set; }
        public string VBP_CO17 { get; set; }
        public string VBP_CO18 { get; set; }
        public string VBP_CO19 { get; set; }
        public string VBP_CO20 { get; set; }
        public string DBP_CO01 { get; set; }
        public string DBP_CO02 { get; set; }
        public string DBP_CO03 { get; set; }
        public string DBP_CO04 { get; set; }
        public string DBP_CO05 { get; set; }
        public string DBP_CO06 { get; set; }
        public string DBP_CO07 { get; set; }
        public string DBP_CO08 { get; set; }
        public string DBP_CO09 { get; set; }
        public string DBP_CO10 { get; set; }
        public string DBP_CO11 { get; set; }
        public string DBP_CO12 { get; set; }
        public string DBP_CO13 { get; set; }
        public string DBP_CO14 { get; set; }
        public string DBP_CO15 { get; set; }
        public string DBP_CO16 { get; set; }
        public string DBP_CO17 { get; set; }
        public string DBP_CO18 { get; set; }
        public string DBP_CO19 { get; set; }
        public string DBP_CO20 { get; set; }
    }
}
