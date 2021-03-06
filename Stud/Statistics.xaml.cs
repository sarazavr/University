﻿using Stud.Utils;
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
            ToggleCheckboxesTo(true, c1, c2, c3, c4, c5, c6);
        }

        private void ToggleCheckboxesTo(bool check, params CheckBox[] checkboxes) 
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

        #region FOR_HOTKEYS
        private void Toggle1Checkbox(object sender, RoutedEventArgs e)
        {
            c1.IsChecked = !c1.IsChecked;
        }
        private void Toggle2Checkbox(object sender, RoutedEventArgs e)
        {
            c2.IsChecked = !c2.IsChecked;
        }
        private void Toggle3Checkbox(object sender, RoutedEventArgs e)
        {
            c3.IsChecked = !c3.IsChecked;
        }
        private void Toggle4Checkbox(object sender, RoutedEventArgs e)
        {
            c4.IsChecked = !c4.IsChecked;
        }
        private void Toggle5Checkbox(object sender, RoutedEventArgs e)
        {
            c5.IsChecked = !c5.IsChecked;
        }
        private void Toggle6Checkbox(object sender, RoutedEventArgs e)
        {
            c6.IsChecked = !c6.IsChecked;
        }

        #endregion FOR_HOTKEYS
    }
}
