using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Contacts.Classes
{
    public class Connection
    {
        static SqlConnection instance = null;

        public static SqlConnection Instance 
        { 
            get 
            { 
                if(instance==null)
                {
                    instance = new SqlConnection(@"data source=(localdb)\mssqllocaldb;attachdbfilename=d:\programming\_git\exercices_wpf\annuaire\table_contacts.mdf;integrated security=true;connect timeout=30");
                }
                return instance;
            } 
        }

        private Connection()
        {

        }
    }
}
