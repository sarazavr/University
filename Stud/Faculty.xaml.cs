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
    public partial class University : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public MainWindow Parant { get; set; }



        public bool IsFacultySelected => !ReferenceEquals(Parant.SelectedFaculty, null);
        public bool IsGroupSelected => !ReferenceEquals(Parant.SelectedGroup, null);
        public University(object owner)
        {
            Parant = (MainWindow)owner;

            InitializeComponent();
        }

        private void AddFaculty(object sender, RoutedEventArgs e)
        {
            var newFaculty = new NamedDoubleLinkedList<NamedDoubleLinkedList<Student>>(FacultyNameInput.Text);
            Parant.FacultyList.Push(newFaculty);

            RefreshFaculty();
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

            DisplayFacultySelectionChanged();
        }

        public void DisplayFacultySelectionChanged()
        {
            RefreshGroupsList();
            OnPropertyChanged("IsFacultySelected");
            OnPropertyChanged("IsGroupSelected");
        }

        public void GroupSelectionChanged(object sender, RoutedEventArgs e)
        {
            var index = GroupListBox.SelectedIndex;

            if (index < 0) Parant.SelectedFaculty.UnsetCurrent();
            else Parant.SelectedFaculty.SetCurrent((uint)index);

            //Debugger.Break();

            DisplayGroupSelectionChanged();
        }

        public void DisplayGroupSelectionChanged()
        {
            Parant.NotifyGroupSelectionChanged();
            OnPropertyChanged("IsGroupSelected");
        }

        public void RefreshGroupsList()
        {
            Refresher.RefreshSelector(GroupListBox, Parant.SelectedFaculty);
        }

        private void GroupNameChanged(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text != string.Empty && Parant.SelectedFaculty is object) EnableOkBtn(AddGroupBtn);
            else DisableOkBtn(AddGroupBtn);
        }

        private void FacultyNameChanged(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text != string.Empty) EnableOkBtn(AddFacultyBtn);
            else DisableOkBtn(AddFacultyBtn);
        }


        public void DeleteSelectedFaculty(object sender, RoutedEventArgs e)
        {
            Parant.FacultyList.Remove(Parant.SelectedFaculty);
            RefreshFaculty();
            DisplayFacultySelectionChanged();
            DisableOkBtn(AddGroupBtn);
        }

        public void DeleteAllFaculty(object sender, RoutedEventArgs e)
        {
            DisableOkBtn(AddGroupBtn, AddFacultyBtn);
            Parant.FacultyList.Clear();
            RefreshFaculty();
            DisplayFacultySelectionChanged();
        }

        public void DeleteAllGroups(object sender, RoutedEventArgs e)
        {
            Parant.SelectedFaculty.Clear();
            RefreshGroupsList();
            DisplayGroupSelectionChanged();
        }
        public void RefreshFaculty()
        {
            Refresher.RefreshSelector(FacultyListBox, Parant.FacultyList);
        }

        public void DeleteSelectedGroup(object sender, RoutedEventArgs e)
        {
            //Debugger.Break();
            Parant.SelectedFaculty.Remove(Parant.SelectedGroup);
            RefreshGroupsList();
            DisplayGroupSelectionChanged();
        }
        private void DisableOkBtn(params Button[] btns)
        {
            foreach(var btn in btns)
            {
                btn.IsEnabled = false;
                btn.Opacity = 0.7;
            }
        }

        private void EnableOkBtn(params Button[] btns)
        {
            foreach (var btn in btns)
            {
                btn.IsEnabled = true;
                btn.Opacity = 1;
            }
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
