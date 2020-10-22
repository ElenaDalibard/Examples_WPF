using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GalaSoft.MvvmLight;

namespace GestionFichiers.Models
{
    class Dossier
    {
        string pathDossier;

        public string PathDossier { get => pathDossier; set => pathDossier = value; }

        public Dossier()
        {
        }
        static public Dossier SelectDossier()
        {
            Dossier dossier = new Dossier();
            FolderBrowserDialog open = new FolderBrowserDialog();
            open.ShowDialog();
            dossier.PathDossier = open.SelectedPath;
            return dossier;
        }
    }
}
