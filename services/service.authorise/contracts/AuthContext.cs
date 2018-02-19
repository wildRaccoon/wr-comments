using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using service.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace service.authorise.contracts
{
    public class AuthContext : DbContext
    {
        public ILogger<AuthContext> _logger { get; set; }

        public AuthContext(DbContextOptions<AuthContext> options,ILogger<AuthContext> logger) : base(options)
        {
            _logger = logger;
        }

        public DbSet<UserData> Users { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserData>()
                .ToTable("Users")
                .HasOne(c => c.Session)
                .WithOne(s => s.User)
                .HasForeignKey<UserSession>(d => d.UserDataId);

            modelBuilder.Entity<UserSession>()
                .ToTable("Sessions");
        }

        public void Ensure()
        {
            try
            {
                Database.EnsureCreated();

                if (Users.Any())
                {
                    return;
                }
                else
                {
                    Users.Add(new UserData()
                    {
                        UserIdentity = "us1",
                        Password = Utils.MD5Hash("password1"),
                        Session = new UserSession() {
                            Token = "aa",
                            UserDataId = 1
                        }
                    });

                    Users.Add(new UserData()
                    {
                        UserIdentity = "us2",
                        Password = Utils.MD5Hash("password2")
                    });

                    SaveChanges();

                    UserSessions.Add(new UserSession()
                    {
                        Token = "bb",
                        UserDataId = 2
                    });

                    SaveChanges();

                    var u1 = Users.Where(w => w.UserIdentity == "us1").First();
                    var u2 = Users.Where(w => w.UserIdentity == "us2").First();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when reate db.");
            }
        }

        public void Check()
        {
            try
            {
                var count = Users.Count();
                _logger.LogInformation($"Users exists: {count > 0}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when check db.");
            }
        }
    }
}
