using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

using WpfOffset.ViewModels;
using WpfOffset.Models;

namespace WpfOffset
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void SaveExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if (textBox != null)
            {
                StreamWriter sw = new StreamWriter("document.txt");
                sw.Write(textBox.Text);
                sw.Close();
            }
            MessageBox.Show("Документ coxpaнен");
        }

        private void OpenExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if (textBox != null)
            {
                StreamReader sr = new StreamReader("document.txt");

                textBox.Text = sr.ReadToEnd();
                sr.Close();
            }
            MessageBox.Show("Документ открыт");
        }

        private void ExitExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (textBox != null)
            {
                Application.Current.Shutdown();
            }
                
        }

        private void ScrollToEndVoid(object sender, TextChangedEventArgs e)
        {
            textBox.ScrollToEnd();
        }
    }
}
