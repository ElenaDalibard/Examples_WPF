using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFichiers.Models
{
    class Extension
    {
        string type;

        public string Type { get => type; set => type = value; }
        public Extension()
        {

        }
        public Extension(string t)
        {
            Type = t;
        }

        static public List<Extension> GetExtensionsList(List<Fichier> listFiles)
        {
            if(listFiles != null)
            {
                List<Extension> extensions = new List<Extension>();
                foreach (Fichier file in listFiles)
                {
                    Extension ext = new Extension();
                    ext.Type = GetExtensionFromFile(file.NomFile);
                    if (!extensions.Exists(e => e.Type == ext.Type))
                    {
                        extensions.Add(ext);
                    }
                }
                return extensions;
            }
            else
            {
                return null;
            }
        }


        static public string GetExtensionFromFile(string fileName)
        {
            char[] fileNameinChar = fileName.ToCharArray();
            string extension = "";
            int pointPosition = 0;
            for (int i = 0; i < fileNameinChar.Length; i++)
            {
                if (fileNameinChar[i] == '.')
                {
                    pointPosition = i;
                }
            }
            if (pointPosition != 0)
            {
                for (int k = pointPosition + 1; k < fileNameinChar.Length; k++)
                {
                    extension += fileNameinChar[k];
                }
            }
            return extension;
        }

        public override string ToString()
        {
            return Type;
        }
    }
}
