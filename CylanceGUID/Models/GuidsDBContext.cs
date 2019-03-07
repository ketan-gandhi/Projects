using Microsoft.EntityFrameworkCore;

namespace CylanceGUID.Models
{
    public class GuidsDBContext : DbContext
    {
        public GuidsDBContext(DbContextOptions<GuidsDBContext> options)
        : base(options)
        {
        }
        public DbSet<GuidDataModel> GuidList { get; set; }
    }
}
