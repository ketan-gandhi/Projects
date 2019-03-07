using CylanceGUID;
using CylanceGUID.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cylance.UnitTests
{
    public  class CacheMock:ICaching
    {
       // private  CacheEntryMock mockObj = new CacheEntryMock();
        
        public void Add<GuidDataModel>(Guid key, GuidDataModel value)
        {
           // throw new NotImplementedException();
        }

        public  void Delete(Guid key)
        {
           // mockObj.Delete(key);
        }


        public  TItem Get<TItem>(Guid key) where TItem : class
        {
            return null;
           // return mockObj.Get<TItem>(key);
        }

    }


    //public class CacheEntryMock : ICaching
    //{
    //    public void Add<GuidDataModel>(Guid key, GuidDataModel value)
    //    {
    //       // throw new NotImplementedException();
    //    }

    //    public void Delete(Guid key)
    //    {
    //       // throw new NotImplementedException();
    //    }

    //    public TItem Get<TItem>(Guid key) where TItem : class
    //    {
    //        return null;
    //        //throw new NotImplementedException();
    //    }
    //}
}
