using CylanceGUID.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cylance.UnitTests
{
    public static class DbContextMock
    {
        public static GuidsDBContext GetDBContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<GuidsDBContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new GuidsDBContext(options);
           
            return dbContext;
        }
    }
}
