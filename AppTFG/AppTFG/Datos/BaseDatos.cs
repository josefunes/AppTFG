using AppTFG.Modelos;
using Xamarin.Forms;
using Microsoft.EntityFrameworkCore;

namespace AppTFG.Datos
{
    public class BaseDatos : DbContext
    {
        public DbSet<Pueblo> Pueblos { get; set; }
        public DbSet<Ruta> Rutas { get; set; }
        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<Foto> Foto { get; set; }

        private readonly string rutaBD;

        public BaseDatos(string rutaBD)
        {
            this.rutaBD = rutaBD;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = DependencyService.Get<IBaseDatos>().GetDatabasePath();
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }
    }
}
