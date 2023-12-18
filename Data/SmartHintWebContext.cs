using Microsoft.EntityFrameworkCore;
using SmartHintTeste.Models;

namespace SmartHintTeste.Data
{
    public class SmartHintWebContext : DbContext
    {
        public SmartHintWebContext(DbContextOptions<SmartHintWebContext> options) : base(options) { }

        public DbSet<ClienteModel> Cliente { get; set; }
        public DbSet<UsuarioModel> Usuarios { get; set; }
    }
}
