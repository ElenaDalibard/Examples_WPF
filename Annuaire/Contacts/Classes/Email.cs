using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Contacts.Classes
{
    public class Email
    {
        int id;
        string mail;
        int idContact;
        static SqlCommand command;
        static SqlDataReader reader;

        public int Id { get => id; set => id = value; }
        public string Mail { get => mail; set => mail = value; }
        public int IdContact { get => idContact; set => idContact = value; }

        public Email()
        {

        }

        public Email(string email, int idCont):this()
        {
            Mail = email;
            IdContact = idCont;
        }

        public override string ToString()
        {
            return Mail;
        }

        public bool SaveEmail()
        {
            string request = "insert into email (Email, id_contact) output inserted.id values(@email,@id)";

            command = new SqlCommand(request, Connection.Instance);
            command.Parameters.Add(new SqlParameter("@email", Mail));
            command.Parameters.Add(new SqlParameter("@id", IdContact));

            Connection.Instance.Open();
            Id = (int)command.ExecuteScalar();
            command.Dispose();
            Connection.Instance.Close();
            return Id > 0;
        }

        public bool DeleteEmail()
        {
            string request = "delete from email where id_Contact=@id";
            command = new SqlCommand(request, Connection.Instance);
            command.Parameters.Add(new SqlParameter("@id", Id));

            Connection.Instance.Open();
            int nbRows = command.ExecuteNonQuery();
            command.Dispose();
            Connection.Instance.Close();
            return nbRows == 1;
        }

        public bool UpdateEmail()
        {
            string request = "update email set email=@email where id_Contact=@id";
            command = new SqlCommand(request, Connection.Instance);
            command.Parameters.Add(new SqlParameter("@id", Id));
            command.Parameters.Add(new SqlParameter("@email", Mail));

            Connection.Instance.Open();
            int nbRows = command.ExecuteNonQuery();
            command.Dispose();
            Connection.Instance.Close();
            return nbRows == 1;
        }

        public static List<Email> GetEmails(int contactId)
        {
            List<Email> emails = new List<Email>();

            string request = "select * from email where id_Contact = @contactId";
            command = new SqlCommand(request, Connection.Instance);
            command.Parameters.Add(new SqlParameter("@contactId", contactId));

            Connection.Instance.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Email email = new Email(reader.GetString(1), reader.GetInt32(2))
                { 
                    Id = reader.GetInt32(0)
                };
                emails.Add(email);
            }
            reader.Close();
            command.Dispose();
            Connection.Instance.Close();

            return emails;
        }

    }
}
