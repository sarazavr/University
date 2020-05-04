using System.ComponentModel;
using System.Linq;
using System.Windows;
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

        public bool IsFacultySelected => !ReferenceEquals(SelectedFaculty, null);
        public bool IsGroupSelected => !ReferenceEquals(SelectedGroup, null);
        public bool IsStudentSelected => !ReferenceEquals(SelectedStudent, null);


        public MainWindow()
        {
            facultyList = StubData.CreateFacultyList();

            InitializeComponent();

            DataContext = this;
            FacultyList = facultyList;

            RefreshAll();
        }

        public void FilterStudentsChanged(object sender, RoutedEventArgs e)
        {
            PerfomFilterStudents(FilterStudentsInput.Text);
        }

        private void PerfomFilterStudents(string filter)
        {
            var filtered = FilterUtils.GetFiltered(
                SelectedGroup,
                filter,
                (student, search) => FilterUtils.ContainsIgnoreCase(student.FullName, search)
            );

            var first = filtered.FirstOrDefault();

            SelectedGroup.SetCurrentByReference(first);

            RefreshSelectedStudentInfo();
            Refresher.RefreshSelector(StudentsListBox, filtered, first);
        }

        
        public void DeleteSelectedStudent(object sender, RoutedEventArgs e)
        {
            foreach (var groups in FacultyList)
            {
                foreach (var group in groups) group.Remove(SelectedStudent);
            }

            SelectedGroup?.MoveCurrentToHead();

            NotifyIsStudentSelectedChanged();
            RefreshStudentsList();
        }


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

            NotifyIsGroupSelectedChanged();
            NotifyIsFacultySelectedChanged();
            NotifyIsStudentSelectedChanged();

            RefreshSelectedStudentInfo();
        }

        public void RefreshFacultySelect()
        {
            //FacultyList?.Sort((l1,l2) => string.CompareOrdinal(l1?.Name, l2?.Name) <= 0);
            FacultyList?.Sort();
            Refresher.RefreshSelector(FacultySelect, FacultyList, SelectedFaculty);
        }
        public void RefreshGroupsSelect()
        {
            //SelectedFaculty?.Sort((l1, l2) => string.CompareOrdinal(l1?.Name, l2?.Name) <= 0);
            SelectedFaculty?.Sort();
            Refresher.RefreshSelector(GroupSelect, SelectedFaculty, SelectedGroup);
        }
        public void RefreshStudentsList()
        {
            SelectedGroup?.Sort();
            Refresher.RefreshSelector(StudentsListBox, SelectedGroup, SelectedStudent);
        }
        public void RefreshSelectedStudentInfo()
        {
            StudentNameText.Text = SelectedStudent?.FullName ?? "";
            StudentYearText.Text = SelectedStudent?.BirthYear.ToString() ?? "";
            StudentMarkText.Text = SelectedStudent?.AverageGrade.ToString() ?? "";
        }

        #endregion UI_REFRESHERS

        public void OnFacultyManagerClosing(object sender, CancelEventArgs e)
        {
            //Debugger.Break();
            RefreshAll();
        }

        #region OPEN_CHILD_WINDOWS

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

        #endregion OPEN_CHILD_WINDOWS
    }
}
