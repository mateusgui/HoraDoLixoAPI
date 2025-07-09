using Microsoft.EntityFrameworkCore;
using HoraDoLixo.Model;

namespace HoraDoLixo.Data
{
    public class HoraDoLixoDbContext : DbContext
    {
        public HoraDoLixoDbContext(DbContextOptions<HoraDoLixoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ProgramacaoColetaComum> ProgramacoesColetaComum { get; set; }
        public DbSet<ProgramacaoColetaSeletiva> ProgramacoesColetaSeletiva { get; set; }
        public DbSet<ZonaColetaComum> ZonasColetaComum { get; set; }
        public DbSet<ZonaColetaSeletiva> ZonasColetaSeletiva { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapeia explicitamente os nomes das tabelas (opcional, mas recomendado)
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<ProgramacaoColetaComum>().ToTable("ProgramacoesColetaComum");
            modelBuilder.Entity<ProgramacaoColetaSeletiva>().ToTable("ProgramacoesColetaSeletiva");
            modelBuilder.Entity<ZonaColetaComum>().ToTable("ZonasColetaComum");
            modelBuilder.Entity<ZonaColetaSeletiva>().ToTable("ZonasColetaSeletiva");

            // Aqui você pode adicionar relacionamentos (foreign keys), se quiser
        }
    }
}
