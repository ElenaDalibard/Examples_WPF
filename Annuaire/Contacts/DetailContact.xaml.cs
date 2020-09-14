using Contacts.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Contacts
{
    /// <summary>
    /// Logique d'interaction pour DetailContact.xaml
    /// </summary>
    public partial class DetailContact : Window
    {
        Contact contact;
        public DetailContact()
        {
            InitializeComponent();
        }

        public DetailContact(Contact c) : this()
        {
            contact = c;
            NomPrenom.Text = c.Nom + " " + c.Prenom;
            Telephone.Text = c.Telephone;
            Emails.ItemsSource = c.Emails;
        }

        private void Click_Edit(object sender, RoutedEventArgs e)
        {
            MainWindow addContact = new MainWindow(contact);
            addContact.Show();
            addContact.TextBoxNom.Text = contact.Nom;
            addContact.TextBoxPrenom.Text = contact.Prenom;
            addContact.TextBoxTel.Text = contact.Telephone;
            addContact.ListEmails.ItemsSource = contact.Emails;
            addContact.NewContact = contact;
            addContact.IsEdit = true;
            this.Close();
        }

        private void Click_Delete(object sender, RoutedEventArgs e)
        {
            contact.DeleteContact();
            MainWindow addContact = new MainWindow(contact);
            this.Close();
            addContact.Show();
        }
    }
}
