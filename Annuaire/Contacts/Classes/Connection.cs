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
                    instance = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Programming\_GIT\Exercices_WPF\Annuaire\Table_Contacts.mdf;Integrated Security=True;Connect Timeout=30");

                }
                return instance;
            } 
        }

        private Connection()
        {

        }
    }
}
