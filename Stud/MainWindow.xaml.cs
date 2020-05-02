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

            RefreshAll();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }


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

        public NamedDoubleLinkedList<NamedDoubleLinkedList<Student>> SelectedFaculty => FacultyList?.Current();

        public NamedDoubleLinkedList<Student> SelectedGroup => SelectedFaculty?.Current();

        public Student SelectedStudent => SelectedGroup?.Current();


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

            SelectedGroup?.UnsetCurrent();

            NotifyIsStudentSelectedChanged();
            RefreshStudentsList();
        }

        public bool IsFacultySelected => !ReferenceEquals(SelectedFaculty, null);
        public bool IsGroupSelected => !ReferenceEquals(SelectedGroup, null);
        public bool IsStudentSelected => !ReferenceEquals(SelectedStudent, null);
  
       

        #region UI_REFRESHERS

        public void NotifyIsGroupSelectedChanged()
        {
            OnPropertyChanged("IsGroupSelected");
        }

        public void NotifyIsFacultySelectedChanged()
        {
            OnPropertyChanged("IsFacultySelected");
        }
        public void NotifyIsStudentSelectedChanged()
        {
            OnPropertyChanged("IsStudentSelected");
        }

        public void NotifyFacultyListChanged()
        {
            OnPropertyChanged("FacultyList");
        }

        public void RefreshAll()
        {
            RefreshFacultySelect();
            RefreshGroupsSelect();
            RefreshStudentsList();
            RefreshSelectedStudentInfo();
            NotifyIsGroupSelectedChanged();
            NotifyIsFacultySelectedChanged();
            NotifyIsStudentSelectedChanged();
            NotifyIsStudentSelectedChanged();
        }

        public void RefreshFacultySelect()
        {
            Refresher.RefreshSelector(FacultySelect, FacultyList, SelectedFaculty);
        }
        public void RefreshGroupsSelect()
        {
            Refresher.RefreshSelector(GroupSelect, SelectedFaculty, SelectedGroup);
        }
        public void RefreshStudentsList()
        {
            Refresher.RefreshSelector(StudentsListBox, SelectedGroup, SelectedStudent);
        }
        public void RefreshSelectedStudentInfo()
        {
            StudentNameText.Text = SelectedStudent?.FullName ?? "";
            StudentYearText.Text = SelectedStudent?.BirthYear.ToString() ?? "";
            StudentMarkText.Text = SelectedStudent?.AverageGrade.ToString() ?? "";
        }

        #endregion UI_REFRESHERS

        #region SELECTION_CHANHES

        public void GroupsSelectionChanged(object sender, RoutedEventArgs e)
        {
            var Item = (NamedDoubleLinkedList<Student>)GroupSelect.SelectedItem;

            if (GroupSelect.SelectedIndex < 0) SelectedFaculty?.UnsetCurrent();
            else SelectedFaculty?.SetCurrentByReference(Item);


            Refresher.RefreshSelector(StudentsListBox, SelectedGroup, SelectedStudent);
            NotifyIsGroupSelectedChanged();
            NotifyIsStudentSelectedChanged();
            // todo: filter ?
        }

        public void FacultySelectionChanged(object sender, RoutedEventArgs e)
        {
            var Item = (NamedDoubleLinkedList<NamedDoubleLinkedList<Student>>)FacultySelect.SelectedItem;

            if (FacultySelect.SelectedIndex < 0) FacultyList?.UnsetCurrent();
            else FacultyList?.SetCurrentByReference(Item);

            RefreshGroupsSelect();
            RefreshStudentsList();
            RefreshSelectedStudentInfo();

            NotifyIsFacultySelectedChanged();
            NotifyIsGroupSelectedChanged();
            
            // todo: filter ?
        }

        public void StudentSelectionChanged(object sender, RoutedEventArgs e)
        {
            var Item = (Student)StudentsListBox.SelectedItem;

            if (StudentsListBox.SelectedIndex < 0) SelectedGroup?.UnsetCurrent();
            else SelectedGroup?.SetCurrentByReference(Item);

            RefreshSelectedStudentInfo();
            NotifyIsStudentSelectedChanged();
            // Refresher.RefreshSelector(StudentsListBox, SelectedGroup);

            // todo: show student info
        }

        #endregion SELECTION_CHANHES


        public void OnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // _________________________ OPEN WINDOWS _________________________________
        public void OnFacultyManagerClosing(object sender, CancelEventArgs e)
        {
            RefreshAll();
        }



        #region OPEN_WINDOWS
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

        #endregion OPEN_WINDOWS

    }

    public static class CustomCommands
    {
        public static RoutedUICommand OpenUniversityEditor = new RoutedUICommand("OpenUniversityEditor",
            "OpenUniversityEditor", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.U, ModifierKeys.Control) });
    }
}
