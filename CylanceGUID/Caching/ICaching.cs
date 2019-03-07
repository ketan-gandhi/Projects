using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CylanceGUID
{
    public interface ICaching
    {
        void Add<GuidDataModel>(Guid key, GuidDataModel value);

        TItem Get<TItem>(Guid key) where TItem : class;

        void Delete(Guid key);
    }
}
