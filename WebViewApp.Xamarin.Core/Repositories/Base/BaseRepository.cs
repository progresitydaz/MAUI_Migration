using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebViewApp.Xamarin.Core.Models;
using SQLite;
using System.Linq.Expressions;

namespace WebViewApp.Xamarin.Core.Repositories
{
    public interface ILocalRepository
    {
        // generic interface method to save or update object into local table
        int Save<T>(T obj) where T : BaseEntity, new();

        // Generic interface method to fetch all the objects from local table
        List<T> GetAll<T>() where T : BaseEntity, new();

        // Generic interface method to fetch filtered the objects from local table
        List<T> Filter<T>(Expression<Func<T, bool>> predExpr) where T : BaseEntity, new();

        // Generic interface method to find a specific object from the local table
        T Get<T>(int id) where T : BaseEntity, new();

        // Generic interfcae method to delete a specific object from the local table
        int Delete<T>(T obj) where T : BaseEntity, new();

        // Generic interfcae method to delete all object from the local table
        int DeleteAll<T>() where T : BaseEntity, new();
    }

    public class BaseRepository : ILocalRepository
    {
        private SQLiteConnection _connection;

        public BaseRepository(SQLiteConnection connection)
        {
            _connection = connection;
            _connection.CreateTable<EnvironmentSetting>();
        }

        public T Get<T>(int id) where T : BaseEntity, new()
        {
            if (_connection != null)
            {
                return _connection.Table<T>().Where(w => w.Id == id).FirstOrDefault();
            }
            else
            {
                throw new Exception("On Device Database connection has not been initiated! May be you have initiated InMemorySQLite database connection?");
            }
        }

        public List<T> GetAll<T>() where T : BaseEntity, new()
        {
            if (_connection != null)
            {
                return (from w in _connection.Table<T>() select w).ToList();
            }
            else
            {
                throw new Exception("On Device Database connection has not been initiated! May be you have initiated InMemorySQLite database connection?");
            }
        }

        public List<T> Filter<T>(Expression<Func<T, bool>> predExpr) where T : BaseEntity, new()
        {
            if (_connection != null)
            {
                return _connection.Table<T>().Where(predExpr).ToList();
            }
            else
            {
                throw new Exception("On Device Database connection has not been initiated! May be you have initiated InMemorySQLite database connection?");
            }
        }

        public int Save<T>(T obj) where T : BaseEntity, new()
        {
            if (_connection != null)
            {
                if (obj.Id != 0)
                {
                    _connection.Update(obj);
                    return obj.Id;
                }
                else
                {
                    return _connection.Insert(obj);
                }
            }
            else
            {
                throw new Exception("On Device Database connection has not been initiated! May be you have initiated InMemorySQLite database connection?");
            }
        }

        public int Delete<T>(T obj) where T : BaseEntity, new()
        {
            if (_connection != null)
            {
                return _connection.Delete<T>(obj.Id);
            }
            else
            {
                throw new Exception("On Device Database connection has not been initiated! May be you have initiated InMemorySQLite database connection?");
            }
        }

        public int DeleteAll<T>() where T : BaseEntity, new()
        {
            if (_connection != null)
            {
                return _connection.DeleteAll<T>();
            }
            else
            {
                throw new Exception("On Device Database connection has not been initiated! May be you have initiated InMemorySQLite database connection?");
            }
        }
    }
}
