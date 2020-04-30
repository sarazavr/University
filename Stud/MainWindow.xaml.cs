using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Stud
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void OpenUniversityEditor(object sender, RoutedEventArgs e)
        {
            var gs = new University() { Owner = this } ;

            gs.Show();
        }
    }

    public static class CustomCommands
    {
        public static RoutedUICommand OpenUniversityEditor = new RoutedUICommand("OpenUniversityEditor",
            "OpenUniversityEditor", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.U, ModifierKeys.Control) });
    }
}
