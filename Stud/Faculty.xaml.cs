using Stud.Utils;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
    /// Логика взаимодействия для University.xaml
    /// </summary>
    public partial class University : Window
    {

        public MainWindow Parant { get; set; }

        public University(object owner)
        {
            Parant = (MainWindow)owner;

            InitializeComponent();
        }

        private void AddFaculty(object sender, RoutedEventArgs e)
        {
            var newFaculty = new NamedDoubleLinkedList<NamedDoubleLinkedList<Student>>(FacultyNameInput.Text);
            Parant.FacultyList.Push(newFaculty);

            Refresher.RefreshSelector(FacultyListBox, Parant.FacultyList);
        }

        private void AddGroup(object sender, RoutedEventArgs e)
        {
            var newGroup = new NamedDoubleLinkedList<Student>(GroupNameInput.Text);

            Parant.SelectedFaculty.Push(newGroup);

            RefreshGroupsList();
            Parant.RefreshGroupsSelect();
        }

        public void SelectFacultyChanged(object sender, RoutedEventArgs e)
        {
            var index = FacultyListBox.SelectedIndex;

            if (index < 0) Parant.FacultyList.UnsetCurrent();
            else Parant.FacultyList.SetCurrent((uint)index);

            RefreshGroupsList();
        }

        public void RefreshGroupsList()
        {
            Refresher.RefreshSelector(GroupListBox, Parant.SelectedFaculty);
        }

        private void GroupNameChanged(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text != string.Empty && Parant.SelectedFaculty is object) Enable(AddGroupBtn);
            else Disable(AddGroupBtn);
        }

        private void FacultyNameChanged(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text != string.Empty) Enable(AddFacultyBtn);
            else Disable(AddFacultyBtn);
        }

        private void Disable(Button btn)
        {
            btn.IsEnabled = false;
            btn.Opacity = 0.7;
        }

        private void Enable(Button btn)
        {
            btn.IsEnabled = true;
            btn.Opacity = 1;
        }

    }
}
