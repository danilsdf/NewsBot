using System.Data.Entity;

namespace NewsBotTelegram
{
    public class UserContextForDB : DbContext
    {
        public UserContextForDB()
            : base("DbConnection")
        { }

        public DbSet<UserSettings> Users { get; set; }
    }
}
