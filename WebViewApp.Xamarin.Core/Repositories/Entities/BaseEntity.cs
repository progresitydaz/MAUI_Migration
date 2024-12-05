using System;
using SQLite;

namespace WebViewApp.Xamarin.Core.Models
{
    public class BaseEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int Status { get; set; }
    }
}
