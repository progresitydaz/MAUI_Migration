using System;
using SQLite;

namespace WebViewApp.Xamarin.Core.Repositories
{
    public interface ISQLiteConnection
    {
        SQLiteConnection GetConnection();
    }
}
