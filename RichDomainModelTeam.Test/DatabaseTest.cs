using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RichDomainModelTeam.Application.Infrastructure;
using System;
using System.IO;


namespace RichDomainModelTeam.Test
{
    public class DatabaseTest : IDisposable
    {
        protected readonly TeamsContext _db;
        public DatabaseTest()
        {
            var options = new DbContextOptionsBuilder<TeamsContext>()
                //.UseSqlite(_connection)  // Connect to in-memory database
                .UseSqlite("Data Source=RichDomainModelTeam.db")
                .UseLazyLoadingProxies()
                .Options;
            _db = new TeamsContext(options);
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
   
  
}
