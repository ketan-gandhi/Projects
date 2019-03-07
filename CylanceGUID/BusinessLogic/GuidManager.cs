using CylanceGUID.Exceptions;
using CylanceGUID.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CylanceGUID.BusinessLogic
{
    public class GuidManager
    {
        private readonly ICaching _cache;
        private readonly GuidsDBContext _context;
       

        public GuidManager(GuidsDBContext context, ICaching cache)
        {
            _context = context;
            _cache = cache;
        }
        public bool IsModelValid(GuidDataModel guidModel)
        {
            if (guidModel.Expire == 0 && guidModel.User == null && guidModel.Guid.Equals(Guid.Empty))
                return false;
            return true;
        }
        
        public async Task<GuidDataModel> Get(Guid guid)
        {
            var item = _cache.Get<GuidDataModel>(guid);

            if (item != null)
                return item;
            else
            {
                item = await _context.GuidList.FindAsync(guid);
                _cache.Add<GuidDataModel>(guid, item);
            }

            if (item == null)
                throw new RecordNotFound(Constants.GUID_NOT_FOUND);

            return item;
        }

        public async Task<GuidDataModel> Update(Guid guid, GuidAPIModel updatedModel)
        {

            if (updatedModel.Guid.HasValue)
                throw new InvalidRequestParameter(Constants.GUID_NOT_UPDATABLE);
             
            GuidDataModel originalGuid = await _context.GuidList.FindAsync(guid);
           
            if (originalGuid == null)
                throw new RecordNotFound(Constants.GUID_NOT_FOUND);
            
            if (updatedModel.Expire.HasValue)
                originalGuid.Expire = updatedModel.Expire.Value;

            if (updatedModel.User != null)
                originalGuid.User = updatedModel.User;
            
            await _context.SaveChangesAsync();

            if (_cache.Get<GuidDataModel>(guid) != null)
                _cache.Delete(guid);
            _cache.Add<GuidDataModel>(guid, originalGuid);

            return originalGuid;
        }

        public async Task<GuidDataModel> Add(GuidAPIModel inputModel)
        {
            GuidDataModel guidModel = inputModel.ToDataModel();
            if (!IsModelValid(guidModel))
                throw new InvalidRequestParameter(Constants.INVALID_MODEL);

            //check if guid already exists
           
            var item =  await _context.GuidList.FindAsync(guidModel.Guid);
            if (item != null)
            {
                throw new RecordAlreadyExists(Constants.GUID_ALREADY_EXISTS);
            }
            else
            {
                guidModel.Expire = guidModel.Expire;
                _context.GuidList.Add(guidModel);
                await _context.SaveChangesAsync();
            }

            return guidModel;
        }

        public void Delete(Guid guid)
        {
            GuidDataModel existingGuid = _context.GuidList.Find(guid);
            if (existingGuid == null)
                throw new RecordNotFound(Constants.GUID_NOT_FOUND);
            else
            {
                _context.GuidList.Remove(existingGuid);
                _context.SaveChanges();

                _cache.Delete(guid);
            }
        }
    }
}
