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
    /// 24. Разработайте соответствующее диалоговое окно и осуществите ввод информации о диапазоне номеров курсов,
    /// в котором нужно определить специальность с максимальным средним значением среднего балла студентов.
    /// Полученные данные (шифр специальности и среднее значение среднего балла студентов) вывести с помощью разработанного диалогового окна.
    /// Запуск задания осуществить через главное меню.
    /// 
    /// </summary>
    public partial class Statistics : Window
    {
        private AverageGradeStatistics statisticsCalculator = new AverageGradeStatistics();
        public Range PossibleCourses { get;  } = new Range(1, 6);
        public int[] Arr { get => new int[] { 1, 2, 3, 4, 5, 6 }; }

        private Range possibleCourses = new Range(1, 6);
        public MainWindow Parant { get; set; }
        public Statistics(object owner)
        {
            Parant = (MainWindow)owner;

            InitializeComponent();
        }

        private void FromValueChanged(object sender, RoutedEventArgs e)
        {
            
        }

        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            GroupNameText.Text = "";
            AverageText.Text = "";
        }



        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public void CalculateStatistics(object sender, RoutedEventArgs e)
        {
            try
            {
                var courses = GetCheckedCourses();

                if(courses.Count == 0)
                {
                    MessageBox.Show("Please, select at least one course", "No course(s) selected",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                statisticsCalculator.Calculate(
                    Parant.FacultyList, 
                    (group, course) => course < Constants.MAX_COURCE_NUMBER && courses.Contains(course)
                );



                GroupNameText.Text = GetNameGorStatisticResult(statisticsCalculator);
                AverageText.Text = statisticsCalculator.MaxAverageOfGrades.ToString();
            }
            catch (AverageGradeStatisticsHasNoResultsException ex)
            {
                MessageBox.Show($"An error occuresd while statistics calculation: \"${ex.Message}\"" +
                    "Make shure you have created faculties, groups and students with selected course(s)",
                    "Invalid name", MessageBoxButton.OK, MessageBoxImage.Information);

                GroupNameText.Text = "";
                AverageText.Text = "";
            }
        }

        private string GetNameGorStatisticResult(AverageGradeStatistics statistics)
        {
            var list = statistics.GetGroupsWithMaxAverageGrades();

            string res = list.First().Name;

            if(list.Count > 1)
            {
                for (int i = 1; i < list.Count; i++)
                {
                    res += ", " + list.ElementAt(i).Name;
                }
            }

            return res;
        }

     
        private void CheckAll(object sender, RoutedEventArgs e)
        {
            ToggleCheckboxes(true, c1, c2, c3, c4, c5, c6);
        }

        private void ToggleCheckboxes(bool check, params CheckBox[] checkboxes) 
        {
            foreach(var checkbox in checkboxes)
            {
                checkbox.IsChecked = check;
            }
        }

        private List<int> GetCheckedCourses()
        {
            var courses = new List<int>();

            if (c1.IsChecked.HasValue && c1.IsChecked.Value) courses.Add(1);
            if (c2.IsChecked.HasValue && c2.IsChecked.Value) courses.Add(2);
            if (c3.IsChecked.HasValue && c3.IsChecked.Value) courses.Add(3);
            if (c4.IsChecked.HasValue && c4.IsChecked.Value) courses.Add(4);
            if (c5.IsChecked.HasValue && c5.IsChecked.Value) courses.Add(5);
            if (c6.IsChecked.HasValue && c6.IsChecked.Value) courses.Add(6);

            return courses;
        }
    }
}
