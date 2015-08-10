
#region Using Namespaces
using System.Data.Entity;
#endregion

namespace CustomAuthentication
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection")
        {
        }
        public DataContext(string connectionName) : base(connectionName)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            CustomAuthentication.CustomMembershipProvider.OnModelCreating(modelBuilder);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}