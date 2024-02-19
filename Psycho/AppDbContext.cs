using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

using Psycho;

namespace SampleApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        //public DbSet<Message>? Messages { get; set; }

        public static string FirstName { get; set; }

        public static string LastName { get; set; }

        public static string MiddleName { get; set; }
        public static DbSet<FrameData> Frames { get; set; }
    }
}