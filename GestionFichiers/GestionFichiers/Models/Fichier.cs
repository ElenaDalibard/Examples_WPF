using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace GestionFichiers.Models
{
    class Fichier
    {
        string path;
        string nom;
        string extension;

        public string PathFile { get => path; set => path = value; }
        public string NomFile { get => nom; set => nom = value; }
        public string Ext { get => extension; set => extension = value; }

        public Fichier()
        {

        }
        public Fichier(string nom, string path) : this()
        {
            PathFile = path;
            NomFile = nom;
            Ext = Extension.GetExtensionFromFile(PathFile);
        }

        static public List<Fichier> GetFilesList(string chemin, bool withFolders)
        {
            List<Fichier> listFiles = new List<Fichier>();
            try
            {
                List<string> listFolders = Directory.EnumerateDirectories(chemin).ToList();
                string[] tabFilePaths = Directory.GetFiles(chemin);

                for (int i = 0; i < tabFilePaths.Length; i++)
                {
                    string path = tabFilePaths[i];
                    string nom = Path.GetFileName(path);
                    Fichier fichier = new Fichier(nom, path);
                    listFiles.Add(fichier);
                }

                if (withFolders && listFolders.Count() > 0)
                {
                    listFolders.ForEach(f => listFiles.AddRange(GetFilesList(f, withFolders)));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de chemin");
            }
            return listFiles;
        }
    }
}
