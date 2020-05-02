using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
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
    /// Логика взаимодействия для StudentModal.xaml
    /// </summary>
    public partial class StudentModal : Window
    {
        public enum Modes
        {
            EDIT,
            CREATE
        }

        private Modes Mode { get; set; }

        public MainWindow Parant { get; set; }

        public Student Student { get; set; }

        public ushort CurrentYear { get; } = (ushort)DateTime.Now.Year;

        public StudentModal(object owner, Modes mode = Modes.CREATE, Student student = null)
        {
            Student = mode == Modes.CREATE ? new Student() : student;
            Mode = mode;
           
            Parant = (MainWindow)owner;
            
            DataContext = this;

            InitializeComponent();

            if (mode == Modes.EDIT)
            {
                if (student is null)
                {
                    throw new ArgumentNullException("Student can not be null when modal opened in edit mode");
                }

                SetUpEditMode(student);
            }
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            FillInStudent();

            if (Mode == Modes.CREATE)
            {
                Parant.SelectedGroup.Push(Student);
 
                ClearForm();
            }

            Parant.RefreshStudentsList();
        }

        private void FillInStudent()
        {
            Student.Surname = SurnameTB.Text;
            Student.Name = NameTB.Text;
            Student.Patronimic = PatronimicTB.Text;
            Student.BirthYear = (ushort)YearOfBirthInput.Value;
            Student.AverageGrade = (float)AvgMarkInput.Value;
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }



        private void ClearForm()
        {
            Student = new Student();
            YearOfBirthInput.Value = null;
            AvgMarkInput.Value = null;
            NameTB.Text = SurnameTB.Text = PatronimicTB.Text = null;
            DisableOkBtn();
        }

        private void ValueChanged(object sender, RoutedEventArgs e)
        {
            if (IsValid()) EnableOkBtn();
            else DisableOkBtn();
          
        }

        private void EnableOkBtn()
        {
            OkBtn.IsEnabled = true;
            OkBtn.Opacity = 1;
        }

        private void DisableOkBtn()
        {
            OkBtn.IsEnabled = false;
            OkBtn.Opacity = 0.7;
        }

        private bool IsValid()
        {
            if (YearOfBirthInput.Value == null || AvgMarkInput.Value == null
                || NameTB.Text == string.Empty || SurnameTB.Text == string.Empty || PatronimicTB.Text == string.Empty)
            {
                return false;
            }
            
             return true;
        }
        private void SetUpEditMode(Student student)
        {
            YearOfBirthInput.Value = student.BirthYear;
            AvgMarkInput.Value = student.AverageGrade;
            NameTB.Text = student.Name;
            SurnameTB.Text = student.Surname;
            PatronimicTB.Text = student.Patronimic;

            Title = "Edit student";
            OkBtn.Content = "Save";
        }
    }
}
