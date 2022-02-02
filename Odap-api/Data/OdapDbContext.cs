using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OdapApi.Models.Device;
using Odap.Models;

namespace OdapApi.Data
{
    public class OdapDbContext : IdentityDbContext<AppUser>
    {
        public OdapDbContext(DbContextOptions<OdapDbContext> dbContextOptions): base(dbContextOptions)
        {

        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<DeviceData> DevicesData { get; set; }
        public  DbSet<RAngle>  RAngles { get; set; }
    }
}
