using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TsSoft.WebSamples.Models;

namespace TsSoft.WebSamples
{
    public class DatabaseConfig
    {
        public static void DropCreateIfModelChanges()
        {
            Database.SetInitializer<SocialContext>(new DropCreateDatabaseIfModelChanges<SocialContext>());
        }
    }
}