using Contacts.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Contacts
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Contact> contacts;
        Contact newContact;
        bool isEdit = false;

        public bool IsEdit { get => isEdit; set => isEdit = value; }
        public Contact NewContact { get => newContact; set => newContact = value; }
        public List<Contact> Contacts { get => contacts; set => contacts = value; }

        public MainWindow()
        {
            InitializeComponent();
            Contacts = Contact.GetContactByTel();
            ListContacts.ItemsSource = Contacts;
            if(NewContact==null)
            {
                NewContact = new Contact();
            }
        }

        public MainWindow(Contact c) :this()
        {
            NewContact = c;
        }

        private void Click_AddEmail(object sender, RoutedEventArgs e)
        {
            if(TextBoxEmail.Text != "")
            {
                Email email = new Email(TextBoxEmail.Text, NewContact.Id);
                NewContact.Emails.Add(email);
                List<Email> tmpList = new List<Email>();
                tmpList.AddRange(NewContact.Emails);
                ListEmails.ItemsSource = tmpList;
                TextBoxEmail.Text = "";
            }
        }

        private void Click_Valider(object sender, RoutedEventArgs e)
        {
            NewContact.Nom = TextBoxNom.Text;
            NewContact.Prenom = TextBoxPrenom.Text;
            NewContact.Telephone = TextBoxTel.Text;
            if(IsEdit)
            {
                NewContact.UpdateContact();
            }
            else
            {
                NewContact.SaveContact();
                Contacts.Add(NewContact);
            }
            ListContacts.ItemsSource = Contact.GetContactByTel();
            ClearForm();
        }

        public void ClearForm()
        {
            NewContact = new Contact();
            TextBoxNom.Text = "";
            TextBoxPrenom.Text = "";
            TextBoxTel.Text = "";
            ListEmails.ItemsSource = null;
        }

        private void Click_VoirContact(object sender, RoutedEventArgs e)
        {
            if (ListContacts.SelectedItem is Contact c)
            {
                DetailContact contact = new DetailContact(c);
                contact.Show();
                this.Close();
            }
        }

        private void Click_Search(object sender, RoutedEventArgs e)
        {
            List<Contact> listSearch = Contact.GetContactByTel(SearchTel.Text);
            ListContacts.ItemsSource = listSearch;
        }

        private void Click_EditEmail(object sender, RoutedEventArgs e)
        {
            if (ListEmails.SelectedItem is Email m)
            {
                TextBoxEmail.Text = m.Mail;
                NewContact.Emails.Remove(m);
                List<Email> tmpList = new List<Email>();
                tmpList.AddRange(NewContact.Emails);
                ListEmails.ItemsSource = tmpList;
            }
        }

        private void Click_DeleteEmail(object sender, RoutedEventArgs e)
        {
            if (ListEmails.SelectedItem is Email m)
            {
                NewContact.Emails.Remove(m);
                m.DeleteEmail();
                List<Email> tmpList = new List<Email>();
                tmpList.AddRange(NewContact.Emails);
                ListEmails.ItemsSource = tmpList;
            }
        }
    }
}
