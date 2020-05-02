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
        public object SelectedFacultyBeforeNewCreation = null;
        public object SelectedGroupBeforeLastCreation = null;

        public event PropertyChangedEventHandler PropertyChanged;
        public MainWindow Parant { get; set; }

        public University(object owner)
        {
            Parant = (MainWindow)owner;

            InitializeComponent();

            Closing += Parant.OnFacultyManagerClosing;

            RefreshGroupsList();
            RefreshFaculty();
        }

        private void AddFaculty(object sender, RoutedEventArgs e)
        {
            SelectedFacultyBeforeNewCreation = FacultyListBox.SelectedItem;
            SelectedGroupBeforeLastCreation = GroupListBox.SelectedItem;

            //Debugger.Break();

            var newFaculty = new NamedDoubleLinkedList<NamedDoubleLinkedList<Student>>(FacultyNameInput.Text);
            Parant.FacultyList.Push(newFaculty);

            RefreshFaculty();
        }

        private void AddGroup(object sender, RoutedEventArgs e)
        {
            if(!Constants.GROUPF_REGEX.IsMatch(GroupNameInput.Text) )
            {
                MessageBox.Show("You entered an invalig group name, use format 'ks-16-1' or 'ks-16m-1'");

                return;
            }

            var newGroup = new NamedDoubleLinkedList<Student>(GroupNameInput.Text);

            SelectedGroupBeforeLastCreation = GroupListBox.SelectedItem;

            Parant.SelectedFaculty.Push(newGroup);

            RefreshGroupsList();
            Parant.RefreshGroupsSelect();
        }

        private void RenameFaculty(object sender, RoutedEventArgs e)
        {
            var form = new RenameForm("Rename Faculty", Parant.SelectedFaculty.Name, (newName) =>
            {
                Parant.SelectedFaculty.Name = newName;
                RefreshFaculty();
            });

            form.ShowDialog();
        }

        private void RenameGroup(object sender, RoutedEventArgs e)
        {
            var form = new RenameForm("Rename Group", Parant.SelectedGroup.Name, (newName) =>
            {
                if (!Constants.GROUPF_REGEX.IsMatch(GroupNameInput.Text))
                {
                    MessageBox.Show("You entered an invalig group name, use format 'ks-16-1' or 'ks-16m-1'");

                    return;
                }

                Parant.SelectedGroup.Name = newName;
                RefreshGroupsList();
            });

            form.ShowDialog();
        }

        // TODO: clear temp pointer to selected item after deleting selected item !!


        public void SelectFacultyChanged(object sender, RoutedEventArgs e)
        {
            var Item = (NamedDoubleLinkedList<NamedDoubleLinkedList<Student>>)FacultyListBox.SelectedItem;

            if(Item is null)
            {
                if (SelectedFacultyBeforeNewCreation is object)
                {
                    FacultyListBox.SelectedItem = SelectedFacultyBeforeNewCreation;
                } else
                {
                    Parant.FacultyList.UnsetCurrent();
                }
            } else
            {
                Parant.FacultyList.SetCurrentByReference(Item);
            }

            DisplayFacultySelectionChanged();
        }

        public void DisplayFacultySelectionChanged()
        {
            RefreshGroupsList();

            Parant.NotifyIsGroupSelectedChanged();
            Parant.NotifyIsFacultySelectedChanged();

        }

        public void GroupSelectionChanged(object sender, RoutedEventArgs e)
        {
            var item = (NamedDoubleLinkedList<Student>)GroupListBox.SelectedItem;

            if (item is null)
            {
                if (SelectedGroupBeforeLastCreation is object)
                {
                    GroupListBox.SelectedItem = SelectedGroupBeforeLastCreation;
                }
                else
                {
                    Parant.SelectedFaculty.UnsetCurrent();
                }
            }
            else
            {
                Parant.SelectedFaculty.SetCurrentByReference(item);
            }

            DisplayGroupSelectionChanged();
        }

        public void DisplayGroupSelectionChanged()
        {
            Parant.NotifyIsGroupSelectedChanged();
            // OnPropertyChanged("IsGroupSelected");
        }

        public void RefreshGroupsList()
        {
            Parant.RefreshGroupsSelect();
            Refresher.RefreshSelector(GroupListBox, Parant.SelectedFaculty, Parant.SelectedStudent);
        }

        private void GroupNameChanged(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text.Trim() != string.Empty
                && Parant.SelectedFaculty is object
                && AddGroupBtn is object)
            {
                EnableOkBtn(AddGroupBtn);
            }
            else DisableOkBtn(AddGroupBtn);
        }

        private void FacultyNameChanged(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text.Trim() != string.Empty) EnableOkBtn(AddFacultyBtn);
            else DisableOkBtn(AddFacultyBtn);
        }


        public void DeleteSelectedFaculty(object sender, RoutedEventArgs e)
        {
            Parant.FacultyList.Remove(Parant.SelectedFaculty);
            Parant.FacultyList.MoveCurrentToHead();
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
            Parant.RefreshAll();
            Refresher.RefreshSelector(FacultyListBox, Parant.FacultyList, Parant.SelectedGroup);
           
        }

        public void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public void DeleteSelectedGroup(object sender, RoutedEventArgs e)
        {
            Parant.SelectedFaculty.Remove(Parant.SelectedGroup);
            Parant.SelectedFaculty.MoveCurrentToHead();
            RefreshGroupsList();
            DisplayGroupSelectionChanged();
        }
        private void DisableOkBtn(params Button[] btns)
        {
            foreach(var btn in btns)
            {
                if (btn is null) continue;

                btn.IsEnabled = false;
                btn.Opacity = 0.7;
            }
        }

        private void EnableOkBtn(params Button[] btns)
        {
            foreach (var btn in btns)
            {
                if (btn is null) continue;

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
