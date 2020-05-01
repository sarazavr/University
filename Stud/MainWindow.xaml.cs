using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Collections.ObjectModel;
using UnivirsityModels;
using System.Runtime.CompilerServices;
using Stud.Utils;

namespace Stud
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        DoubleLinkedList<NamedDoubleLinkedList<NamedDoubleLinkedList<Student>>> facultyList;

        public DoubleLinkedList<NamedDoubleLinkedList<NamedDoubleLinkedList<Student>>> FacultyList
        {
            get
            {
                return facultyList;
            }
            set
            {
                facultyList = value;
                NotifyFacultyListChanged();
            }
        }

        private NamedDoubleLinkedList<Student> selectedGroupFromJoinedList;
        private Student selectedStudentFromJoinedList;

        public NamedDoubleLinkedList<NamedDoubleLinkedList<Student>> SelectedFaculty => FacultyList?.Current();

        public NamedDoubleLinkedList<Student> SelectedGroup => SelectedFaculty?.Current();

        public IEnumerable<NamedDoubleLinkedList<Student>> AllGroups => FacultyList.SelectMany(groups => groups);

        public NamedDoubleLinkedList<Student>  SelectedGroupFromJoinedList
        {
            get => selectedGroupFromJoinedList;
            set
            {
                selectedGroupFromJoinedList = value;
                SelectedStudent = null;
                OnPropertyChanged("SelectedGroupFromJoinedList");
                OnPropertyChanged("IsGroupFromGoinedListSelected");
            }
        }

        public bool IsGroupFromGoinedListSelected => !ReferenceEquals(SelectedGroupFromJoinedList, null);

        public void OnFacultyRemoved()
        {

        }

        public void OnGroupRemoved()
        {

        }

        public void DeleteSelectedStudent(object sender, RoutedEventArgs e)
        {
           foreach(var groups in FacultyList)
            {
                foreach( var group in groups) group.Remove(SelectedStudent);
            }

            Refresher.RefreshSelector(StudentsListBox, SelectedGroupFromJoinedList);
            SelectedStudent = null;
        }

        public Student SelectedStudent
        {
            get => selectedStudentFromJoinedList;
            set
            {
                selectedStudentFromJoinedList = value;
                OnPropertyChanged("SelectedStudentFromJoinedList");
            }
        }

        public bool IsStudentSelected => !ReferenceEquals(SelectedStudent, null);
  
        public void NotifyStudentSelectionChanged()
        {
            OnPropertyChanged("IsStudentSelected");
        }



        public void NotifyFacultyListChanged()
        {
            OnPropertyChanged("FacultyList");
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void NotifyFacultySelected()
        {
            OnPropertyChanged("SelectedFaculty");
        }

        public void NotifyGroupSelectionChanged()
        {
            OnPropertyChanged("SelectedGroup");
        }

        public MainWindow()
        {
            facultyList = new DoubleLinkedList<NamedDoubleLinkedList<NamedDoubleLinkedList<Student>>>();

            var defaultFaculty = new NamedDoubleLinkedList<NamedDoubleLinkedList<Student>>("Faculty1");
            var defaultGroup = new NamedDoubleLinkedList<Student>("Group1");

            defaultGroup.Push(new Student("Surname", "Name", "Patronimic", 1996, 20));
            defaultGroup.Push(new Student("Surname1", "Name1", "Patronimic1", 1997, 35.3f));

            defaultFaculty.Push(defaultGroup);
            facultyList.Push(defaultFaculty);

            facultyList.MoveCurrentToHead();
            SelectedFaculty.MoveCurrentToHead();
            SelectedGroup.MoveCurrentToHead();

            InitializeComponent();

            DataContext = this;
            FacultyList = facultyList;
        }


        public void RefreshAll()
        {

            RefreshStudentsList();
            RefreshGroupsSelect();
        }

        public void RefreshStudentsList()
        {
            Refresher.RefreshSelector(StudentsListBox, SelectedGroupFromJoinedList);
        }
        public void RefreshGroupsSelect()
        {
            Refresher.RefreshSelector(GroupsSelect, AllGroups);
        }


        private void GroupSelectionChanged(object sender, RoutedEventArgs e) {
            RefreshStudentsList();
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void OpenUniversityEditor(object sender, RoutedEventArgs e)
        {
            var gs = new University(this) { Owner = this, DataContext = DataContext };

            gs.Show();
        }

        private void OpenAddStudentModal(object sender, RoutedEventArgs e)
        {
            var gs = new StudentModal(this) { Owner = this };

            gs.Show();
        }

        private void OpenEditStudentModal(object sender, RoutedEventArgs e)
        {
            var gs = new StudentModal(this, StudentModal.Modes.EDIT, SelectedStudent) { Owner = this };

            gs.Show();
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        //public void GroupsSelectionChanged(object sender, RoutedEventArgs e)
        //{
        //    var index = GroupsSelect.SelectedIndex;

        //    if (index < 0) FacultyList.Current()?.UnsetCurrent();
        //    else FacultyList.Current()?.SetCurrent((uint)index);

        //    Refresher.RefreshSelector(StudentsListBox, FacultyList.Current()?.Current());
        //    // todo: filter ?
        //}

        public void StudentSelectionChanged(object sender, RoutedEventArgs e)
        {
            NotifyStudentSelectionChanged();
            // todo: show student info
        }
    }

    public static class CustomCommands
    {
        public static RoutedUICommand OpenUniversityEditor = new RoutedUICommand("OpenUniversityEditor",
            "OpenUniversityEditor", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.U, ModifierKeys.Control) });
    }
}
