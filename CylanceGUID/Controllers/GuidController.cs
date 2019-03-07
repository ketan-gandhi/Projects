using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CylanceGUID.Models;
using System;
using Microsoft.EntityFrameworkCore;
using CylanceGUID.BusinessLogic;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CylanceGUID.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuidController : ControllerBase
    {
        private readonly GuidsDBContext _context;

        private readonly ICaching _cache;
        private GuidManager _manager;
        
        public GuidController(GuidsDBContext context, ICaching cache)
        {
            _context = context;
            _cache = cache;
            _manager = new GuidManager(_context, _cache);
        }

        // GET: api/Guid/6F729C08-A503-4C16-9262-46AAEF1F2CA2
        [HttpGet("{Guid}")]
        public async Task<ActionResult<GuidAPIModel>> GetGuid(Guid guid)
        {
            GuidDataModel guidItem = await _manager.Get(guid);
            return guidItem.ToAPIModel();
        }

        // POST: api/Guid
        [HttpPost]
        public async Task<ActionResult<GuidAPIModel>> PostGuid(GuidAPIModel inputModel)
        {
            GuidDataModel guidModel = await _manager.Add(inputModel); 
            return guidModel.ToAPIModel();
        }

       
        // PUT: api/Guid/6F729C08-A503-4C16-9262-46AAEF1F2CA2
        [HttpPut("{Guid}")]
        public async Task<ActionResult<GuidAPIModel>> PutGuid(Guid guid, GuidAPIModel inputModel)
        {
           // try
            {
                var guidModel = await _manager.Update(guid, inputModel);
                return guidModel.ToAPIModel();
            }
            
        }

        // DELETE: api/Guid/6F729C08-A503-4C16-9262-46AAEF1F2CA2
        [HttpDelete("{Guid}")]
        public ActionResult DeleteGuid(Guid guid)
        {
             _manager.Delete(guid);
            return NoContent();
        }
    }
}
