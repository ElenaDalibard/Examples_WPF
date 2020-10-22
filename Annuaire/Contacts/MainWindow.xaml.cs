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

        public MainWindow()
        {
            InitializeComponent();
            contacts = Contact.GetContactByTelOrName();
            ListContacts.ItemsSource = contacts;
            if(newContact==null)
            {
                newContact = new Contact();
            }
        }

        public MainWindow(Contact c) :this()
        {
            newContact = c;
        }

        private void Click_AddEmail(object sender, RoutedEventArgs e)
        {
            Email email = new Email(TextBoxEmail.Text, newContact.Id);
            newContact.Emails.Add(email);
            List<Email> tmpList = new List<Email>();
            tmpList.AddRange(newContact.Emails);
            ListEmails.ItemsSource = tmpList;
            TextBoxEmail.Text = "";
        }

        private void Click_Valider(object sender, RoutedEventArgs e)
        {
            newContact.Nom = TextBoxNom.Text;
            newContact.Prenom = TextBoxPrenom.Text;
            newContact.Telephone = TextBoxTel.Text;
            contacts.Add(newContact);
            List<Contact> tmpList = new List<Contact>();
            tmpList.AddRange(contacts);
            ListContacts.ItemsSource = tmpList;
            newContact.SaveContact();
            ClearForm();
        }

        public void ClearForm()
        {
            newContact = new Contact();
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
            }
        }

        private void Click_Search(object sender, RoutedEventArgs e)
        {
            List<Contact> listSearch = Contact.GetContactByTelOrName(SearchTel.Text);
            ListContacts.ItemsSource = listSearch;
        }

        private void Click_EditEmail(object sender, RoutedEventArgs e)
        {
            if (ListEmails.SelectedItem is Email m)
            {
                TextBoxEmail.Text = m.Mail;
                newContact.Emails.Remove(m);
                List<Email> tmpList = new List<Email>();
                tmpList.AddRange(newContact.Emails);
                ListEmails.ItemsSource = tmpList;
            }
        }

        private void Click_DeleteEmail(object sender, RoutedEventArgs e)
        {
            if (ListEmails.SelectedItem is Email m)
            {
                newContact.Emails.Remove(m);
                m.DeleteEmail();
                List<Email> tmpList = new List<Email>();
                tmpList.AddRange(newContact.Emails);
                ListEmails.ItemsSource = tmpList;
            }
        }
    }
}
