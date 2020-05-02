using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UnivirsityModels;

namespace Stud
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : Window
    {

        public int[] Arr { get => new int[] { 1, 2, 3, 4, 5, 6 }; }

        
        

        public MainWindow Parant { get; set; }
        public Statistics(object owner)
        {
            Parant = (MainWindow)owner;

            InitializeComponent();
        }


        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public void CalculateStatistics(object sender, RoutedEventArgs e)
        {
            float? maxVal = null;

            NamedDoubleLinkedList<Student> group = null;

            foreach (var f in Parant.FacultyList )
            {
                foreach(var gr in f) {
                    float average = gr.Average(s => s.AverageGrade);

                    if(!maxVal.HasValue || maxVal < average)
                    {
                        maxVal = average;
                        group = gr;
                    }
                }
            }

            MessageBox.Show($"Group with max average value of average students mark: ${group.Name}, avg: ${maxVal}");
        }



    }
}
