using AppTFG.Modelos;
using Xamarin.Forms;
using Microsoft.EntityFrameworkCore;
using SQLite;

namespace AppTFG.Datos
{
    public class BaseDatos : DbContext
    {
        public DbSet<Pueblo> Pueblos { get; set; }
        public DbSet<Ruta> Rutas { get; set; }
        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<Foto> Foto { get; set; }
        public DbSet<Video> Video { get; set; }

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
            //dbPath.CreateTable<Pueblo>();
        }
    }
}

    //public class BaseDatos : DbContext
    //{
    //    public DbSet<Pueblo> Pueblos { get; set; }
    //    public DbSet<Ruta> Rutas { get; set; }
    //    public DbSet<Actividad> Actividades { get; set; }
    //    public DbSet<Foto> Foto { get; set; }
    //    public DbSet<Video> Video { get; set; }

    //    private readonly string rutaBD;
    //    SQLiteAsyncConnection database;
    //    //public BaseDatos(string rutaBD)
    //    //{
    //    //    this.rutaBD = rutaBD;
    //    //    Database.EnsureCreated();
    //    //}

    //    public BaseDatos(SQLiteAsyncConnection connection)
    //    {
    //        //database = DependencyService.Get<IBaseDatos>().GetConnection();
    //        //var databasePath = DependencyService.Get<IBaseDatos>().GetDatabasePath();
    //        //database = new SQLiteAsyncConnection(databasePath);
    //        database = connection;
    //        // create the tables
    //        CreateDatabaseAsync();
    //    }

    //    public void CreateDatabaseAsync()
    //    {
    //        database.CreateTablesAsync<Pueblo, Ruta, Actividad, Foto, Video>();
    //        //database.CreateTableAsync<Ruta>();
    //        //database.CreateTableAsync<Actividad>();
    //        //database.CreateTableAsync<Foto>();
    //        //database.CreateTableAsync<Video>();
    //    }
    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        //var dbPath = DependencyService.Get<IBaseDatos>().GetDatabasePath();
    //        //optionsBuilder.UseSqlite($"Filename={dbPath}");
    //        //var path = DependencyService.Get<IBaseDatos>().GetConnection();
    //        //optionsBuilder.UseSqlite($"Filename={path}");
    //        //dbPath.CreateTable<Pueblo>();
    //    }
    //}
//}
