using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Contacts.Classes
{
    public class Contact
    {
        int id;
        string nom;
        string prenom;
        string telephone;
        List<Email> emails;
        static SqlCommand command;
        static SqlDataReader reader;

        public string Nom { get => nom; set => nom = value; }
        public string Prenom { get => prenom; set => prenom = value; }
        public List<Email> Emails { get => emails; set => emails = value; }
        public int Id { get => id; set => id = value; }
        public string Telephone { get => telephone; set => telephone = value; }

        public Contact()
        {
            Emails = new List<Email>();
        }

        public Contact(string n, string p, string tel) :this()
        {
            Nom = n;
            Prenom = p;
            Telephone = tel;
        }

        public Contact(int id, string n, string p, string tel) :this(n, p, tel)
        {
            Id = id;
        }

        public override string ToString()
        {
            string retour = "";
            //foreach (Email e in Emails)
            //{
            //    retour += e.ToString() + "\n";
            //}
            return Nom + " " + Prenom + " - " + Telephone + "\n" + retour;
        }

        public bool SaveContact()
        {
            string request = "insert into contact (Nom, Prenom, Telephone) output inserted.id values(@nom,@prenom,@telephone)";
            command = new SqlCommand(request, Connection.Instance);
            command.Parameters.Add(new SqlParameter("@nom", Nom));
            command.Parameters.Add(new SqlParameter("@prenom", Prenom));
            command.Parameters.Add(new SqlParameter("@telephone", Telephone));

            Connection.Instance.Open();
            Id = (int)command.ExecuteScalar();
            command.Dispose();
            Connection.Instance.Close();
            bool retour = Id > 0;
            if(retour)
            {
                Emails.ForEach(e => { e.IdContact = Id; e.SaveEmail(); });

            }
            return retour;
        }

        public bool DeleteContact()
        {
            string request = "delete from contact where id=@id";
            command = new SqlCommand(request, Connection.Instance);
            command.Parameters.Add(new SqlParameter("@id", Id));

            Connection.Instance.Open();
            int nbRows = command.ExecuteNonQuery();
            command.Dispose();
            Connection.Instance.Close();
            if(nbRows==1)
            {
                Emails.ForEach(e => e.DeleteEmail());
            }
            return nbRows==1;
        }

        public bool UpdateContact()
        {
            string request = "update contact set nom=@nom, prenom=@prenom, telephone=@tel where id=@id";
            command = new SqlCommand(request, Connection.Instance);
            command.Parameters.Add(new SqlParameter("@id", Id));
            command.Parameters.Add(new SqlParameter("@nom", Nom));
            command.Parameters.Add(new SqlParameter("@prenom", Prenom));
            command.Parameters.Add(new SqlParameter("@tel", Telephone));

            Connection.Instance.Open();
            int nbRows = command.ExecuteNonQuery();
            command.Dispose();
            Connection.Instance.Close();
            return nbRows ==1;
        }

        public static List<Contact> GetContactByTelOrName(string search=null)
        {
            List<Contact> contacts = new List<Contact>();
            string request = "select * from contact";
            if(search != null)
            {
                request += " where telephone like @search or nom like @search or prenom like @search";
            }

            command = new SqlCommand(request, Connection.Instance);

            if (search != null)
            {
                command.Parameters.Add(new SqlParameter("@search", "%" + search + "%"));
            }

            Connection.Instance.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Contact contact = new Contact(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
                contacts.Add(contact);
            }
            reader.Close();
            command.Dispose();
            Connection.Instance.Close();
            contacts.ForEach(c => { c.Emails = Email.GetEmails(c.Id); }) ;
            return contacts;
        }

        public static Contact GetContactById(int id)
        {
            Contact contact = new Contact();
            string request = "select * from contact where id = @id";

            command = new SqlCommand(request, Connection.Instance);
            command.Parameters.Add(new SqlParameter("@id", id));
            Connection.Instance.Open();
            reader = command.ExecuteReader();
            if(reader.Read())
            {
                contact = new Contact(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
            }
            reader.Close();
            command.Dispose();
            Connection.Instance.Close();
            if(contact != null)
            {
                contact.Emails = Email.GetEmails(id);
            }
            return contact;
        }

    }
}
