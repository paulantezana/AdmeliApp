using AdmeliApp.Interfaces;
using AdmeliApp.Model;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace AdmeliApp.Helpers
{
    public class DataAccess : IDisposable
    {
        private SQLiteConnection connection;
        public DataAccess()
        {
            var config = DependencyService.Get<IConfig>();
            connection = new SQLiteConnection(config.Platform,
                System.IO.Path.Combine(config.DirectoryDB, "Admeli.db3"));

            /// Crear Tablas en la base de datos sqlite
            connection.CreateTable<Personal>();
        }

        public void Insert<T>(T model)
        {
            connection.Insert(model);
        }

        public void Update<T>(T model)
        {
            connection.Update(model);
        }

        public void Delete<T>(T model)
        {
            connection.Delete(model);
        }

        public T First<T>() where T : class
        {
            return connection.Table<T>().FirstOrDefault();
        }

        public List<T> GetList<T>() where T : class
        {
            return connection.Table<T>().ToList();
        }

        public T Find<T>(int pk) where T : class
        {
            return connection.Table<T>().FirstOrDefault(m => m.GetHashCode() == pk);
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
