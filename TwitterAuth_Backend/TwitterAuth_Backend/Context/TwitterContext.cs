using Microsoft.EntityFrameworkCore;
using TwitterAuth_Backend.Model;

namespace TwitterAuth_Backend.Context
{
    public class TwitterContext : DbContext
    {
        public TwitterContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<UserInfoModel> TwitterUserInfo { get; set; }
    }
}
