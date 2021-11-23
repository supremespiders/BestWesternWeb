using System.IO;
using System.Reflection;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class DbContext:Microsoft.EntityFrameworkCore.DbContext
    {
        private readonly string _path = Path.GetDirectoryName((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath);
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={_path}/sys.db");
        public DbSet<Log> Logs { get; set; }
        public DbSet<User> Users { get; set; }
    }
}