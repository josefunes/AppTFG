using SQLite;

namespace AppTFG.Datos
{
    public interface IBaseDatos
    {
        string GetDatabasePath();
        //SQLiteConnection GetConnection();
    }
}
