using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM;

namespace TestWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkController : ControllerBase
    {
        private readonly AbstractDbContext _context = new SevenBPMDbContext();
        private readonly LinksRepo _repo;

        public LinkController() {
            _repo = new LinksRepo(_context);
        }

        [HttpGet]
        [Route("GetLinks")]
        public IEnumerable<Link> GetLinks() {
            var res = _repo.List().ToList();
            return res;
        }

        [HttpGet]
        [Route("GetLink")]
        public IEnumerable<Link> GetLink(int id) {
            var res = _repo.List("WHERE LIN_CONT={pLIN_CONT}", new {
                pLIN_CONT = id
            }).ToList();
            return res;
        }

        [HttpPost]
        [Route("AddLink")]
        public Link AddLink(LinkDTO link) {
            var res = _repo.Add(new Link {
                Company = link.Company,
                Name = link.Name,
                URL = link.URL,
                State = "A",
                ModificationUser = "NaujOyamat",
                ModificationDate = DateTime.Now
            });
            _repo.Context.SaveChanges();
            return res;
        }

        [HttpDelete]
        [Route("DeleteLink")]
        public Link DeleteLink(int id) {
            var link = _repo.List("WHERE LIN_CONT={pLIN_CONT}", new {
                pLIN_CONT = id
            }).FirstOrDefault();
            if(link != null) {
                _repo.Remove(link);
                _repo.Context.SaveChanges();
            }
            return link;
        }

        [HttpPut]
        [Route("UpdateLink")]
        public Link UpdateLink(LinkDTO dto) {
            var link = _repo.List("WHERE LIN_CONT={pLIN_CONT}", new {
                pLIN_CONT = dto.Id
            }).FirstOrDefault();
            if (link != null) {
                link.State = "M";
                link.ModificationDate = DateTime.Now;
                link.Company = dto.Company;
                link.URL = dto.URL;
                link.Name = dto.Name;

                _repo.Modify(link);
                _repo.Context.SaveChanges();
            }
            return link;
        }
    }
}