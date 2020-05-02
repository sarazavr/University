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

            var defaultFaculty = new NamedDoubleLinkedList<NamedDoubleLinkedList<Student>>("FFEKS");
            var f2 = new NamedDoubleLinkedList<NamedDoubleLinkedList<Student>>("FACULTY");

            var defaultGroup = new NamedDoubleLinkedList<Student>("ks-16-1");
            var g2 = new NamedDoubleLinkedList<Student>("ks-18-1");

            var g3 = new NamedDoubleLinkedList<Student>("mc-18-1");

            g3.Push(new Student("a", "a", "a", 1994, 24));
            g3.Push(new Student("b", "b", "b", 1994, 27));

            g2.Push(new Student("S", "N", "P", 1996, 100));
            g2.Push(new Student("M", "N", "P", 1996, 45));


            defaultGroup.Push(new Student("Surname", "Name", "Patronimic", 1996, 20));
            defaultGroup.Push(new Student("Surname1", "Name1", "Patronimic1", 1997, 35.3f));

            defaultFaculty.Push(defaultGroup);

            f2.Push(g3);
            facultyList.Push(defaultFaculty);
            facultyList.Push(f2);

            facultyList.MoveCurrentToHead();
            SelectedFaculty.MoveCurrentToHead();
            SelectedGroup.MoveCurrentToHead();

            facultyList.Sort();

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

            SelectedGroup?.MoveCurrentToHead();

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
            FacultyList?.Sort((l1,l2) => string.CompareOrdinal(l1?.Name, l2?.Name) <= 0);
            Refresher.RefreshSelector(FacultySelect, FacultyList, SelectedFaculty);
        }
        public void RefreshGroupsSelect()
        {
            SelectedFaculty?.Sort((l1, l2) => string.CompareOrdinal(l1?.Name, l2?.Name) <= 0);
            Refresher.RefreshSelector(GroupSelect, SelectedFaculty, SelectedGroup);
        }
        public void RefreshStudentsList()
        {
            SelectedGroup?.Sort((l1, l2) => l1.CompareTo(l2) <= 0);
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

        

            public void CalcStat(object sender, RoutedEventArgs e)
        {
            var win = new Statistics(this);

            win.ShowDialog();

        }
        public void OnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // _________________________ OPEN WINDOWS _________________________________
        public void OnFacultyManagerClosing(object sender, CancelEventArgs e)
        {
            //Debugger.Break();
            RefreshAll();
        }



        #region OPEN_WINDOWS
        private void OpenUniversityEditor(object sender, RoutedEventArgs e)
        {
            var gs = new University(this) { Owner = this, DataContext = DataContext };

            gs.ShowDialog();
        }

        private void OpenAddStudentModal(object sender, RoutedEventArgs e)
        {
            var gs = new StudentModal(this) { Owner = this };

            gs.ShowDialog();
        }

        private void OpenEditStudentModal(object sender, RoutedEventArgs e)
        {
            var gs = new StudentModal(this, StudentModal.Modes.EDIT, SelectedStudent) { Owner = this };

            gs.ShowDialog();
        }

        #endregion OPEN_WINDOWS

    }

    public static class CustomCommands
    {
        public static RoutedUICommand OpenUniversityEditor = new RoutedUICommand("OpenUniversityEditor",
            "OpenUniversityEditor", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.U, ModifierKeys.Control) });
    }
}
