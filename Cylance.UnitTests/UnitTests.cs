using CylanceGUID;
using CylanceGUID.Controllers;
using CylanceGUID.Exceptions;
using CylanceGUID.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Cylance.UnitTests
{
    public class UnitTests
    {
        GuidsDBContext _dbContext;
        GuidController _guidController;
        ICaching _caching;

        
        public UnitTests()
        {
            _caching = new CacheMock();
            _dbContext = DbContextMock.GetDBContext("Cylance");
            _guidController = new GuidController(_dbContext, _caching);
        }

        [Fact]
        public void Get_InvalidGuid()
        {
            Guid newGuid = Guid.NewGuid();
            Assert.ThrowsAsync<RecordNotFound>(async () => await _guidController.GetGuid(newGuid));           
        }

        [Fact]
        public async Task Get_ValidGuid()
        {
            _dbContext.GuidList.Add(new GuidDataModel
            {
                Guid = new Guid("6F729C08-A503-4C16-9262-46AAEF1F2CA1"),
                Expire = 185823,
                User = "test user 1"
            });
            _dbContext.SaveChanges();

            Guid newGuid = new Guid("6F729C08-A503-4C16-9262-46AAEF1F2CA1");

            var result = await _guidController.GetGuid(newGuid);

            Assert.IsType<GuidAPIModel>(result.Value);
            Assert.Equal(newGuid, (result.Value).Guid);
            Assert.Equal("test user 1", (result.Value).User);
            Assert.Equal(185823, (result.Value).Expire);            
        }

        [Fact]
        public async Task Post_ValidRequest_GuidPresent()
        {
            GuidAPIModel guidModel = new GuidAPIModel()
            {
                Guid = new Guid("FCC19AA2-42B5-4195-85BC-FC0A923D6125"),
                Expire = 155849,
                User = "New text from Post method"
            };

            var result = await _guidController.PostGuid(guidModel);

            Assert.IsType<GuidAPIModel>(result.Value);
            Assert.Equal(new Guid("FCC19AA2-42B5-4195-85BC-FC0A923D6125"), (result.Value as GuidAPIModel).Guid);
            Assert.Equal("New text from Post method", (result.Value as GuidAPIModel).User);
            Assert.Equal(155849, (result.Value as GuidAPIModel).Expire);
        }

        [Fact]
        public async Task Post_ValidRequest_GuidAbsent()
        {
            GuidAPIModel guidModel = new GuidAPIModel()
            {
                Expire = 155849,
                User = "New text from Post method - 2"
            };

            var result = await _guidController.PostGuid(guidModel);

            Assert.IsType<GuidAPIModel>(result.Value);
            Assert.NotEqual(Guid.Empty, (result.Value as GuidAPIModel).Guid);
            Assert.Equal("New text from Post method - 2", (result.Value as GuidAPIModel).User);
            Assert.Equal(155849, (result.Value as GuidAPIModel).Expire);
        }

        [Fact]
        public async Task Post_ValidRequest_GuidAndExpireAbsent()
        {
            GuidAPIModel guidModel = new GuidAPIModel()
            {
                User = "New text from Post method - 3"
            };

            var result = await _guidController.PostGuid(guidModel);

            Assert.IsType<GuidAPIModel>(result.Value);
            Assert.NotEqual(Guid.Empty, (result.Value as GuidAPIModel).Guid);
            Assert.Equal("New text from Post method - 3", (result.Value as GuidAPIModel).User);
            Assert.NotEqual(0, (result.Value as GuidAPIModel).Expire);
        }

        [Fact]
        public async Task Post_Invalid_GuidAlreadyPresent()
        {
            GuidAPIModel guidModel = new GuidAPIModel()
            {
                Guid = new Guid("6F729C08-A503-4C16-9262-46AAEF1F2CFF"),
                Expire = 185823,
                User = "Updated text from PUT"
            };

            _dbContext.Add(guidModel.ToDataModel());
            _dbContext.SaveChanges();

            await Assert.ThrowsAsync<RecordAlreadyExists>(async () => await _guidController.PostGuid(guidModel));

        }

        [Fact]
        public async Task Put_InvalidRequest()
        {
            GuidAPIModel guidModel = new GuidAPIModel();
            Guid newGuid = Guid.NewGuid();
            guidModel.Guid = newGuid;

            await Assert.ThrowsAsync<InvalidRequestParameter>(async () => await _guidController.PutGuid(newGuid, guidModel));
        }

        [Fact]
        public async Task Put_RecordNotFound()
        {
            GuidAPIModel guidModel = new GuidAPIModel();
            Guid newGuid = Guid.NewGuid();

            await Assert.ThrowsAsync<RecordNotFound>(async () => await _guidController.PutGuid(newGuid, guidModel));
        }

        [Fact]
        public async Task Put_ValidRequest_GuidPresent()
        {
            Guid newGuid = new Guid("6F729C08-A503-4C16-9262-46AAEF1F2CA2");
            _dbContext.GuidList.Add(new GuidDataModel
            {
                Guid = new Guid("6F729C08-A503-4C16-9262-46AAEF1F2CA2"),
                Expire = 100,
                User = "test user 1"
            });
            _dbContext.SaveChanges();

            GuidAPIModel guidModel = new GuidAPIModel()
            {
                Expire = 185823,
                User = "Updated text from PUT"
            };
            _dbContext.Add(guidModel.ToDataModel());
            _dbContext.SaveChanges();
            var result = await _guidController.PutGuid(newGuid, guidModel);

            Assert.IsType<GuidAPIModel>(result.Value);
            Assert.Equal(newGuid, (result.Value as GuidAPIModel).Guid);
            Assert.Equal("Updated text from PUT", (result.Value as GuidAPIModel).User);
            Assert.Equal(185823, (result.Value as GuidAPIModel).Expire);
        }


        [Fact]
        public void Delete_InvalidRequest()
        {
            Guid newGuid = Guid.NewGuid();
            Assert.Throws<RecordNotFound>(() =>  _guidController.DeleteGuid(newGuid));

        }

        [Fact]
        public void Delete_ValidRequest()
        {
            _dbContext.GuidList.Add(new GuidDataModel
            {
                Guid = new Guid("6F729C08-A503-4C16-9262-46AAEF1F2CB2"),
                Expire = 185823,
                User = "test user 1"
            });
            _dbContext.SaveChanges();

            Guid newGuid = new Guid("6F729C08-A503-4C16-9262-46AAEF1F2CB2");
            var result = _guidController.DeleteGuid(newGuid);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
