using MembershipSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace MembershipSystem
{
    public class MembersContext : DbContext
    {

        public MembersContext(DbContextOptions<MembersContext> options) : base(options)
        { }

        public MembersContext() : base() { }

        public DbSet<Member> Members { get; set; }

    }
}
