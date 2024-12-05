using System;
using System.IO;
using WebViewApp.Xamarin.Core.Repositories;
using WebViewApp.Xamarin.Droid.Dependency;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDBConnection))]
namespace WebViewApp.Xamarin.Droid.Dependency
{
    public class SQLiteDBConnection : ISQLiteConnection
    {
        public SQLiteDBConnection()
        {
        }

        public SQLiteConnection GetConnection()
        {
            string filename = "ReppidoCaretakerApp.db3";

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string dbFilePath = Path.Combine(path, filename);

            return new SQLiteConnection(dbFilePath);
        }
    }
}
