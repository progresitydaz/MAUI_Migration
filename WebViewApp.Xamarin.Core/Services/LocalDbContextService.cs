using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using SQLite;
using WebViewApp.Xamarin.Core.Constants;
using WebViewApp.Xamarin.Core.Dependency;
using WebViewApp.Xamarin.Core.Enums;
using WebViewApp.Xamarin.Core.Helpers;
using WebViewApp.Xamarin.Core.Ioc;
using WebViewApp.Xamarin.Core.Models;
using WebViewApp.Xamarin.Core.Repositories;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core
{
    public interface ILocalDbContextService
    {
        int SaveEntity<T>(T entity) where T : BaseEntity, new();

        int DeleteEntity<T>(T entity) where T : BaseEntity, new();

        List<T> GetEntityList<T>() where T : BaseEntity, new();

        T GetEntity<T>(int id) where T : BaseEntity, new();

        List<T> FilterEntity<T>(Expression<Func<T, bool>> filter) where T : BaseEntity, new();

        #region Helper Methods

        EnvironmentSetting GetCurrentEnvironmentSetting();

        void UpdateUnreadMessageCount(int count, bool isCumulative = true);

        #endregion
    }

    public class LocalDbContextService : ILocalDbContextService, IDisposable
    {
        private SQLiteConnection _connection;
        private ILocalRepository _repository;

        public LocalDbContextService()
        {
            _connection = GetConnection();

            _connection.CreateTable<EnvironmentSetting>();
            _repository = new BaseRepository(_connection);
        }

        private SQLiteConnection GetConnection()
        {
            string filename = DbConstants.DatabaseFilename;

            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            string dbFilePath = Path.Combine(libFolder, filename);

            var connection = new SQLiteConnection(dbFilePath, DbConstants.Flags);

            return connection;
        }

        public int SaveEntity<T>(T entity) where T : BaseEntity, new()
        {
            int result = 0;

            try
            {
                result = _repository.Save(entity);
            }
            catch (Exception ex)
            {
                LogHelper.LogException("SaveEntity", ex);
            }

            return result;
        }

        public int DeleteEntity<T>(T entity) where T : BaseEntity, new()
        {
            int result = 0;

            try
            {
                result = _repository.Delete(entity);
            }
            catch (Exception ex)
            {
                LogHelper.LogException("DeleteEntity", ex);
            }

            return result;
        }

        public int DeleteAll<T>() where T : BaseEntity, new()
        {
            int result = 0;

            try
            {
                result = _repository.DeleteAll<T>();
            }
            catch (Exception ex)
            {
                LogHelper.LogException("DeleteAllEntities", ex);
            }

            return result;
        }

        public List<T> GetEntityList<T>() where T : BaseEntity, new()
        {
            List<T> result = new List<T>();

            try
            {
                result = _repository.GetAll<T>();
            }
            catch (Exception ex)
            {
                LogHelper.LogException("GetEntityList", ex);
            }

            return result;
        }

        public T GetEntity<T>(int id) where T : BaseEntity, new()
        {
            T entity = null;

            try
            {
                entity = _repository.Get<T>(id);

            }
            catch (Exception ex)
            {
                LogHelper.LogException("GetEntity", ex);
            }

            return entity;
        }

        public List<T> FilterEntity<T>(Expression<Func<T, bool>> filter) where T : BaseEntity, new()
        {
            List<T> result = new List<T>();

            try
            {
                result = _repository.Filter(filter);

            }
            catch (Exception ex)
            {
                LogHelper.LogException("FilterEntity", ex);
            }

            return result;
        }

        public void Dispose()
        {
            try
            {
                if (_connection != null)
                {
                    _connection.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        #region Helper Methods

        public EnvironmentSetting GetCurrentEnvironmentSetting()
        {
            List<EnvironmentSetting> entityList = GetEntityList<EnvironmentSetting>();

            EnvironmentSetting entity = entityList?.FirstOrDefault();

            return entity;
        }

        public void UpdateUnreadMessageCount(int count, bool isCumulative = true)
        {
             
        }

        #endregion
    }
}
