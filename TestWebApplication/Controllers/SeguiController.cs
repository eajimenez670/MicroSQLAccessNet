using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpheliaSuiteV2.Core.DataAccess.MicroSQL;
using OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM;

namespace TestWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguiController : ControllerBase
    {
        private readonly AbstractDbContext _context = new SevenBPMDbContext();
        private readonly DbContext _genericContext = new GenericContext();
        private readonly LinksRepo _linkRepo;
        private readonly SeguiRepo _seguiRepo;

        public SeguiController() {
            _linkRepo = new LinksRepo(_context);
            _seguiRepo = new SeguiRepo(_context);
        }

        [HttpGet]
        [Route("GetLinks")]
        public IEnumerable<Link> GetLinks() {
            var res = _linkRepo.List();
            return res;
        }

        [HttpGet]
        [Route("GetSeguis")]
        public IEnumerable<WF_SEGUI> GetSeguis() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var res = _seguiRepo.List().Include(s => s.Caso).Include(s => s.Flujo);
            sw.Stop();
            return res;
        }
    }
}