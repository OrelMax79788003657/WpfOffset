using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfOffset.Models;

namespace WpfOffset.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName]string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        private string construction;
        public string Construction
        {
            get => construction;
            set
            {
                construction = value;
                OnPropertyChanged();
            }
        }

        private string position;
        public string Position
        {
            get => position;
            set
            {
                position = value;
                OnPropertyChanged();
            }
        }


        private bool steelClassA240C;
        public bool SteelClassA240C
        {
            get => steelClassA240C;
            set
            {
                steelClassA240C = value;
                SteelClass = steelClassA240C ? "A240C" : "A500C";
                OnPropertyChanged();
            }
        }

        private bool steelClassA500C;
        public bool SteelClassA500C
        {
            get => steelClassA500C;
            set
            {
                steelClassA500C = value;
                SteelClass = steelClassA240C ? "A240C" : "A500C";
                OnPropertyChanged();
            }
        }

        private string steelClass;
        public string SteelClass
        {
            get => steelClass;
            set
            {
                steelClass = value;
                OnPropertyChanged();
            }
        }

        private int diametr;
        public int Diametr
        {
            get => diametr;
            set
            {
                diametr = value;
                OnPropertyChanged();
            }
        }

        private int length;
        public int Length
        {
            get => length;
            set
            {
                length = value;
                OnPropertyChanged();
            }
        }

        private int amount;
        public int Amount
        {
            get => amount;
            set
            {
                amount = value;
                OnPropertyChanged();
            }
        }


        private string historyNotes;
        public string HistoryNotes
        {
            get => historyNotes;
            set
            {
                historyNotes = value;
                OnPropertyChanged();
            }
        }

        private string reportText;
        public string ReportText
        {
            get => reportText;
            set
            {
                reportText = value;
                OnPropertyChanged();
            }
        }

        List<string> historyList = new List<string>();


        class Entry
        {
            public string Construction { get; set; }
            public string Position { get; set; }
            public string SteelClass { get; set; }
            public int Diametr { get; set; }
            public int Length { get; set; }
            public int Amount { get; set; }
            public double Kilo { get; set; }
        }


       
        private List<Entry> allEntries = new List<Entry>();
        
        public void WriteNewEntry() 
        {
            double[] calcWeight = Steel.CalcTonas(Diametr, Length, Amount); 

            double kilos = Math.Round(calcWeight[0],4);
            double tonas = Math.Round(calcWeight[1],6);
            tonas = kilos>1 ? Math.Round(calcWeight[1],6):0;

            int DIAMETR = Steel.diametrInTonas.Keys.ToArray()[Diametr];
            float LENGTH = (float)Length / 1000;
            
            string entryText = $"{Construction}: {Position}: Класс {SteelClass}: ø{DIAMETR}: {LENGTH} м: {Amount} шт";
            HistoryNotes += $"> {entryText} => {kilos} кг {tonas} тонн;\n";
            historyList.Add($"> {entryText} => {kilos} кг {tonas} тонн;\n");

            Entry newEntry = new Entry()
            {
                Construction = this.Construction,
                Position = this.Position,
                SteelClass = this.SteelClass,
                Diametr = Steel.diametrInTonas.Keys.ToArray()[this.Diametr],
                Length = this.Length,
                Amount = this.Amount,
                Kilo = kilos
            };

            allEntries.Add(newEntry);
        }



        public void ClearAndWriteNewReport()
        {
            ReportText = String.Empty;
            OnPropertyChanged();

            string newReportLine = String.Empty;

            string newConstruction = String.Empty;
            Dictionary<int,double> newDiametrsA500C = new Dictionary<int, double>();
            Dictionary<int,double> newDiametrsA240C = new Dictionary<int, double>();

            Dictionary<string, string> allConstructions = new Dictionary<string, string>();

            foreach (Entry entry in allEntries)
            {
                if (allConstructions.ContainsKey(entry.Construction))
                {
                    continue;
                }
                allConstructions.Add(entry.Construction, "");

                newReportLine = String.Empty;

                newConstruction = entry.Construction;
                

                newDiametrsA500C = new Dictionary<int, double>();
                newDiametrsA240C = new Dictionary<int, double>();

                List<Entry> constructionEntries = new List<Entry>();

                foreach (Entry findentry in allEntries)
                {
                    if (findentry.Construction == entry.Construction)
                    {
                        constructionEntries.Add(findentry);
                    }
                }
                foreach (Entry findentry in constructionEntries)
                {
                    if (findentry.SteelClass=="A500C")
                    {
                        if (newDiametrsA500C.ContainsKey(findentry.Diametr))
                        {
                            newDiametrsA500C[findentry.Diametr] += findentry.Kilo;
                        }
                        else
                        {
                            newDiametrsA500C.Add(findentry.Diametr, findentry.Kilo);
                        }
                    }

                    if (findentry.SteelClass == "A240C")
                    {
                        if (newDiametrsA240C.ContainsKey(findentry.Diametr))
                        {
                            newDiametrsA240C[findentry.Diametr] += findentry.Kilo;
                        }
                        else
                        {
                            newDiametrsA240C.Add(findentry.Diametr, findentry.Kilo);
                        }
                    }
                }
               
                newReportLine += $"Конструкция {newConstruction}:\n";

                newReportLine += $"Сталь А240С:\n";
                foreach (int d in newDiametrsA240C.Keys)
                {
                    double newTonas = newDiametrsA240C[d] / 1000d;
                    newReportLine += $"ø {d}: {newDiametrsA240C[d]} кг ({newTonas} тонн)\n";
                }

                newReportLine += $"Сталь А500С:\n";
                foreach (int d in newDiametrsA500C.Keys)
                {
                    double newTonas = newDiametrsA500C[d] / 1000d;
                    newReportLine += $"ø {d}: {newDiametrsA500C[d]} кг ({newTonas} тонн)\n";
                }
                newReportLine += "\n";
                ReportText += newReportLine;
            }

            newDiametrsA500C = new Dictionary<int, double>();
            newDiametrsA240C = new Dictionary<int, double>();

            newReportLine = String.Empty;
            newReportLine += "ОБЩАЯ ВЕДОМОСТЬ СТАЛИ:\n";
            newReportLine += "Класс A240C:\n";
            foreach (Entry entry in allEntries)
            {
                if (entry.SteelClass == "A240C")
                {
                    if (newDiametrsA240C.ContainsKey(entry.Diametr))
                    {
                        newDiametrsA240C[entry.Diametr] += entry.Kilo;
                    }
                    else
                    {
                        newDiametrsA240C.Add(entry.Diametr, entry.Kilo);
                    }
                }
            }
            foreach (int d in newDiametrsA240C.Keys)
            {
                double newTonas = newDiametrsA240C[d] / 1000d;
                newReportLine += $"ø {d}: {newDiametrsA240C[d]} кг ({newTonas} тонн)\n";
            }

            newReportLine += "Класс A500C:\n";
            foreach (Entry entry in allEntries)
            {
                if (entry.SteelClass == "A500C")
                {
                    if (newDiametrsA500C.ContainsKey(entry.Diametr))
                    {
                        newDiametrsA500C[entry.Diametr] += entry.Kilo;
                    }
                    else
                    {
                        newDiametrsA500C.Add(entry.Diametr, entry.Kilo);
                    }
                }
            }
            foreach (int d in newDiametrsA500C.Keys)
            {
                double newTonas = newDiametrsA500C[d] / 1000d;
                newReportLine += $"ø {d}: {newDiametrsA500C[d]} кг ({newTonas} тонн)\n";
            }
            newReportLine += "\n";
            ReportText += newReportLine;

        }



        public void PopLastEntryCommand()
        {
            allEntries.RemoveAt(allEntries.Count - 1);

            

            historyList.RemoveAt(historyList.Count - 1);
            HistoryNotes = String.Join("", historyList);
        }





        public ICommand CalcTonasCommand { get; }
        private bool CanCalcTonasCommandExecuted(object p)
        {
            if (Construction != "" && Position != "" && SteelClass != null && Length > 0 && Amount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void OnCalcTonasCommandExecute(object p)
        {
            WriteNewEntry();
        }


        public ICommand WriteReportCommand { get; }
        private bool CanWriteReportCommandExecuted(object p)
        {
            if (allEntries.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void OnWriteReportCommandExecute(object p)
        {
            ClearAndWriteNewReport();
        }

        public ICommand PopLastCommand { get; }
        private bool CanPopLastCommandExecuted(object p)
        {
            if (allEntries.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void OnPopLastCommandExecute(object p)
        {
            PopLastEntryCommand();
        }

        public MainWindowViewModel()
        {
            Construction = "К1";
            Position = "Поз-1";
            Diametr = 1;
            SteelClassA500C = true;
            SteelClassA240C = false;
            Length = 1;
            Amount = 1;
            CalcTonasCommand = new RelayCommand(OnCalcTonasCommandExecute, CanCalcTonasCommandExecuted);
            WriteReportCommand = new RelayCommand(OnWriteReportCommandExecute, CanWriteReportCommandExecuted);
            PopLastCommand = new RelayCommand(OnPopLastCommandExecute, CanPopLastCommandExecuted);
        }
    }
}
