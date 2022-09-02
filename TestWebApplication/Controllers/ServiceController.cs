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
    public class ServiceController : ControllerBase
    {
        private readonly AbstractDbContext _context = new MicroSQLDbContext();
        private readonly WebServiceRepo _serviceRepo;
        private readonly WebMethodRepo _methodRepo;
        private readonly WebParamRepo _paramRepo;

        public ServiceController() {
            _serviceRepo = new WebServiceRepo(_context);
            _methodRepo = new WebMethodRepo(_context);
            _paramRepo = new WebParamRepo(_context);
        }

        [HttpGet]
        [Route("GetServices")]
        public IEnumerable<WebService> GetServices() {
            var res = _serviceRepo.List();
            return res;
        }

        [HttpGet]
        [Route("GetService")]
        public IEnumerable<WebService> GetService(int id) {
            var res = _serviceRepo.List("WHERE WEB_CONT={pWEB_CONT}", new
            {
                pWEB_CONT = id
            });
            return res;
        }

        [HttpGet]
        [Route("GetMethods")]
        public IEnumerable<WebMethod> GetMethods() {
            var res = _methodRepo.List().Include(m => m.Parent);
            return res;
        }

        [HttpGet]
        [Route("GetParameters")]
        public IEnumerable<WebParam> GetParameters() {
            var res = _paramRepo.List().Include(m => m.Method);
            return res;
        }

        [HttpGet]
        [Route("GetMethod")]
        public IEnumerable<WebMethod> GetMethod(int serviceId, int id) {
            var res = _methodRepo.List("WEB_CONT={pWEB_CONT} AND MWE_CONT={pMWE_CONT}", new {
                pWEB_CONT = serviceId,
                pMWE_CONT = id
            });
            return res;
        }

        [HttpPost]
        [Route("AddService")]
        public WebService AddService(WebService service) {
            _serviceRepo.Add(service);

            WebMethod method = new WebMethod {
                Code = "Meth1",
                Method = "Create",
                Parent = service
            };
            _methodRepo.Add(method);
            _paramRepo.Add(new WebParam {
                PME_CODI = "Id",
                Method = method
            });


            _methodRepo.Add(new WebMethod {
                Code = "Meth2",
                Method = "Update",
                Parent = service
            });
            _methodRepo.Add(new WebMethod {
                Code = "Meth3",
                Method = "Delete",
                Parent = service
            });
            _serviceRepo.Context.SaveChanges();
            return service;
        }

        [HttpPost]
        [Route("AddMethod")]
        public WebMethod AddMethod(WebMethod method) {
            WebService service = _serviceRepo.List("WEB_CONT={pWEB_CONT}", new { pWEB_CONT = method.ServiceId }).FirstOrDefault();
            if(service != null) {
                method.Parent = service;
                _methodRepo.Add(method);
                _methodRepo.Context.SaveChanges();
            }
            return method;
        }

        //[HttpDelete]
        //[Route("DeleteLink")]
        //public Link DeleteLink(int id) {
        //    var link = _repo.List("WHERE LIN_CONT={pLIN_CONT}", new {
        //        pLIN_CONT = id
        //    }).FirstOrDefault();
        //    if(link != null) {
        //        _repo.Remove(link);
        //        _repo.Context.SaveChanges();
        //    }
        //    return link;
        //}

        //[HttpPut]
        //[Route("UpdateLink")]
        //public Link UpdateLink(LinkDTO dto) {
        //    var link = _repo.List("WHERE LIN_CONT={pLIN_CONT}", new {
        //        pLIN_CONT = dto.Id
        //    }).FirstOrDefault();
        //    if (link != null) {
        //        link.State = "M";
        //        link.ModificationDate = DateTime.Now;
        //        link.Company = dto.Company;
        //        link.URL = dto.URL;
        //        link.Name = dto.Name;

        //        _repo.Modify(link);
        //        _repo.Context.SaveChanges();
        //    }
        //    return link;
        //}
    }
}