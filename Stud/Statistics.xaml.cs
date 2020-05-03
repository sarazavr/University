using Stud.Utils;
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

        private AverageGradeStatistics statisticsCalculator = new AverageGradeStatistics();

        public Range PossibleCourses { get;  } = new Range(1, 6);
        public int[] Arr { get => new int[] { 1, 2, 3, 4, 5, 6 }; }

        public MainWindow Parant { get; set; }
        public Statistics(object owner)
        {
            Parant = (MainWindow)owner;

            InitializeComponent();
        }

        private void FromValueChanged(object sender, RoutedEventArgs e)
        {
            
        }

        

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public void CalculateStatistics(object sender, RoutedEventArgs e)
        {
            try
            {
                statisticsCalculator.Calculate(Parant.FacultyList, (group, course) => course < 3);

                MessageBox.Show($"Group with max average value of average students mark:" +
                    $" ${statisticsCalculator.GroupWithMaxAverageOfGrades.Name}, " +
                    $"avg: ${statisticsCalculator.MaxAverageOfGrades}");
            }
            catch (AverageGradeStatisticsHasNoResultsException ex)
            {
                MessageBox.Show($"An error accures while calculating statistics: \"${ex.Message}\"" +
                    "Make shure you have created faculties, groups and students.");
            }

        }
    }
}
