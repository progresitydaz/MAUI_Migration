using System;
using WebViewApp.Xamarin.Core.Ioc;
using SQLite;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Repositories.Mocks
{
    public class SQLiteConnectionMock : ISQLiteConnection
    {
        public SQLiteConnectionMock()
        {

        }

        public SQLiteConnection GetConnection()
        {
            return AppDependencyResolver.Resolve<ISQLiteConnection>().GetConnection();
        }
    }
}
