using Register.Models;
using System.Data.Entity;

namespace Register.DAL {
    public class RegisterDBContext : DbContext {

        public DbSet<Developer> Developers { get; set; }
        public DbSet<Stack> Stacks { get; set; }
        public DbSet<Technology> Technologies { get; set; }


    }
}