using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GestionFichiers.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;

namespace GestionFichiers.ViewModels
{
    class ViewModel :ViewModelBase
    {
        public bool WithFolders { get; set; }
        public bool NoFolders { get; set; }
        public Dossier Dossier { get; set; }
        public List<Fichier> Fichiers { get; set; }
        public ObservableCollection<Extension> Extensions { get; set; }
        public Extension SelectedExtension { get; set; }
        public string PathCreatedFile { get; set; }
        public string SelectedDossier { get; set; }
        public string Result { get; set; }
        public ICommand Command_Select { get; set; }
        public ICommand Command_Export { get; set; }

        public ViewModel()
        {
            NoFolders = true;
            WithFolders = false;
            Command_Select = new RelayCommand(SelectDossier);
            Command_Export = new RelayCommand(()=>ExportFileList(SelectedExtension));
        }

        private void SelectDossier()
        {
            Extensions = new ObservableCollection<Extension>();
            Fichiers = new List<Fichier>();

            Dossier = Dossier.SelectDossier();
            RaisePropertyChanged("Dossier");
            SelectedDossier = Dossier.PathDossier;
            RaisePropertyChanged("PathDossier");
            if(SelectedDossier == "")
            {
                Result = "Aucun dossier choisi";
            }
            else
            {
                try
                {
                    Result = "En cours de chargement...";
                    RaisePropertyChanged("Result");
                    Task.Factory.StartNew(() =>
                    {
                        List<Fichier> tmpFichiers = Fichier.GetFilesList(Dossier.PathDossier, WithFolders);
                        Dispatcher.CurrentDispatcher.Invoke(() =>
                        {
                            Fichiers = tmpFichiers;
                            Result = "";
                            RaisePropertyChanged("Result");
                            RaisePropertyChanged("Fichiers");

                            if (Fichiers.Count() > 0)
                            {
                                Result = "En cours de chargement...";
                                Task.Factory.StartNew(() =>
                                {
                                    List<Extension> tmpExtensions = Extension.GetExtensionsList(Fichiers);
                                    Dispatcher.CurrentDispatcher.Invoke(() =>
                                    {
                                        Extensions = tmpExtensions.CastToObservable();
                                        RaisePropertyChanged("Extensions");
                                        Result = "Chargement fini";
                                        RaisePropertyChanged("Result");
                                    });
                                });
                            }
                            else if (Fichiers.Count() == 0)
                            {
                                Result = "Aucun fichier dans ce dossier";
                                RaisePropertyChanged("Result");
                            }
                        });
                    });
                }
                catch (Exception ex)
                {
                    Result = "Erreur, veuillez choisir le dossier de nouveau";
                    RaisePropertyChanged("Result");
                }
            }
        }

        private void ExportFileList(Extension extension)
        {
            string pathDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            int index = 1;

            string[] tabFiles = Directory.GetFiles(pathDirectory);
            do
            {
                PathCreatedFile = Path.Combine(pathDirectory, $"fichiers_{extension}_{index}.csv");
                index++;
            }
            while (tabFiles.Contains(PathCreatedFile));

            StreamWriter writerCsv = new StreamWriter(PathCreatedFile);
            writerCsv.WriteLine("Nom Fichier; Chemin");

            if(SelectedExtension == null)
            {
                Result = "Choisissez une extension";
                RaisePropertyChanged("Result");
            }
            else
            {
                foreach (Fichier f in Fichiers)
                {
                    if (f.Ext == SelectedExtension.Type)
                    {
                        writerCsv.WriteLine($"{f.NomFile}; {f.PathFile}");
                    }
                }
                writerCsv.Close();
                RaisePropertyChanged("PathCreatedFile");
            }
        }
    }
}
