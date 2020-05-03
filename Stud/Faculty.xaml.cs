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
using System.Windows.Controls.Primitives;
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

        private bool isGroupsSearchModeOn = false;
        public bool IsGroupsSearchModeOn
        {
            get => isGroupsSearchModeOn; set
            {
                isGroupsSearchModeOn = value;
                OnPropertyChanged("IsGroupsSearchModeOn");
            }
        }

        private bool isFacultiesSearchModeOn = false;
        public bool SsFacultiesSearchModeOn
        {
            get => isFacultiesSearchModeOn; set
            {
                isFacultiesSearchModeOn = value;
                OnPropertyChanged("IsGroupsSearchModeOn");
            }
        }

        private void RememberLastSelectedItems()
        {
            SelectedFacultyBeforeNewCreation = FacultyListBox.SelectedItem;
            SelectedGroupBeforeLastCreation = GroupListBox.SelectedItem;
        }

        private void AddFaculty(object sender, RoutedEventArgs e)
        {

            RememberLastSelectedItems();
            //Debugger.Break();

            var newFaculty = new NamedDoubleLinkedList<NamedDoubleLinkedList<Student>>(FacultyNameInput.Text);
            Parant.FacultyList.Push(newFaculty);

            RefreshFaculty();
        }


        private void AddGroup(object sender, RoutedEventArgs e)
        {
            if (!Constants.GROUPF_REGEX.IsMatch(GroupNameInput.Text))
            {
                MessageBox.Show("You entered an invalig group name, use format 'ks-16-1' or 'ks-16m-1'");

                return;
            }

            RememberLastSelectedItems();

            var newGroup = new NamedDoubleLinkedList<Student>(GroupNameInput.Text);

            Parant.SelectedFaculty.Push(newGroup);

            RefreshGroupsList();
            Parant.RefreshGroupsSelect();
        }

        private void RenameFaculty(object sender, RoutedEventArgs e)
        {
            var form = new RenameForm("Rename Faculty", Parant.SelectedFaculty.Name, (newName, closeForm) =>
            {
                RememberLastSelectedItems();
                Parant.SelectedFaculty.Name = newName;
                closeForm();

                if (FacultiesSearchModeSwitch.IsChecked) PerformFacultiesSerach(FacultyNameInput.Text);
                else RefreshFaculty();
            });

            form.ShowDialog();
        }

        private void RenameGroup(object sender, RoutedEventArgs e)
        {
            var form = new RenameForm("Rename Group", Parant.SelectedGroup.Name, (newName, closeForm) =>
            {
                if (!Constants.GROUPF_REGEX.IsMatch(GroupNameInput.Text))
                {
                    MessageBox.Show("You entered an invalig group name, use format 'ks-16-1' or 'ks-16m-1'");

                    return;
                }
                RememberLastSelectedItems();
                Parant.SelectedGroup.Name = newName;
                closeForm();

                if (GroupsSearchModeSwitch.IsChecked) PerformGroupsSerach(GroupNameInput.Text);
                else RefreshFaculty();

                RefreshGroupsList();
            });

            form.ShowDialog();
        }


        // TODO: clear temp pointer to selected item after deleting selected item !!


        public void FacultySelectionChanged(object sender, RoutedEventArgs e)
        {
            var item = (NamedDoubleLinkedList<NamedDoubleLinkedList<Student>>)FacultyListBox.SelectedItem;

            if (item is object || item is null && SelectedFacultyBeforeNewCreation is object)
            {
                FacultyListBox.SelectedItem = item ?? SelectedFacultyBeforeNewCreation;
                Parant.FacultyList.SetCurrentByReference(item);
            }
            else
            {
                Parant.FacultyList.UnsetCurrent();
            }

            //if(Item is null)
            //{
            //    if (SelectedFacultyBeforeNewCreation is object)
            //    {
            //        FacultyListBox.SelectedItem = SelectedFacultyBeforeNewCreation;
            //    } else
            //    {
            //        Parant.FacultyList.UnsetCurrent();
            //    }
            //} else
            //{
            //    Parant.FacultyList.SetCurrentByReference(item);
            //}

            DisplayFacultySelectionChanged();
        }


        public void GroupSelectionChanged(object sender, RoutedEventArgs e)
        {
            var item = (NamedDoubleLinkedList<Student>)GroupListBox.SelectedItem;


            if (item is object || item is null && SelectedFacultyBeforeNewCreation is object)
            {
                GroupListBox.SelectedItem = item ?? SelectedGroupBeforeLastCreation;
                // TODO: move SetCurrentByReference to setter of property SelectedFaculty/
                Parant.SelectedFaculty?.SetCurrentByReference(item);
            }
            else
            {
                // TODO: remove ?. and check for work whith renaming and default selection
                Parant.SelectedFaculty?.UnsetCurrent();
            }

            //if (item is null)
            //{
            //    if (SelectedGroupBeforeLastCreation is object)
            //    {
            //        GroupsListBox.SelectedItem = SelectedGroupBeforeLastCreation;
            //    }
            //    else
            //    {
            //        Parant.SelectedFaculty.UnsetCurrent();
            //    }
            //}
            //else
            //{
            //    Parant.SelectedFaculty.SetCurrentByReference(item);
            //}

            DisplayGroupSelectionChanged();
        }

        public void DisplayGroupSelectionChanged()
        {
            Parant.NotifyIsGroupSelectedChanged();
            // OnPropertyChanged("IsGroupSelected");
        }


        public void DisplayFacultySelectionChanged()
        {
            RefreshGroupsList();

            Parant.NotifyIsGroupSelectedChanged();
            Parant.NotifyIsFacultySelectedChanged();

        }

        public void RefreshGroupsList()
        {
            Parant.RefreshGroupsSelect();
            Refresher.RefreshSelector(GroupListBox, Parant.SelectedFaculty, Parant.SelectedStudent);
        }

        private void GroupNameChanged(object sender, RoutedEventArgs e)
        {
            string text = GroupNameInput.Text.Trim();

            bool isEnabled = text != string.Empty && Parant.SelectedFaculty is object;

            ToggleBtnsEnabledTo(isEnabled, AddGroupBtn);

            if (GroupsSearchModeSwitch.IsChecked) PerformGroupsSerach(text);
        }


        private void FacultyNameChanged(object sender, RoutedEventArgs e)
        {
            string text = FacultyNameInput.Text.Trim();
            bool isEnabled = text != string.Empty;

            ToggleBtnsEnabledTo(isEnabled, AddFacultyBtn);

            if (FacultiesSearchModeSwitch.IsChecked) PerformFacultiesSerach(text);
        }

        private void GroupsSearchModeOff(object sender, RoutedEventArgs e)
        {
            PerformGroupsSerach(FilterUtils.RESET_FILTER_VALUE);
        }

        private void FacultiessSearchModeOff(object sender, RoutedEventArgs e)
        {
            PerformFacultiesSerach(FilterUtils.RESET_FILTER_VALUE);
        }

        private void GroupsSearchModeOn(object sender, RoutedEventArgs e)
        {
            PerformGroupsSerach(GroupNameInput.Text.Trim());
        }

        private void FacultiessSearchModeOn(object sender, RoutedEventArgs e)
        {
            PerformFacultiesSerach(FacultyNameInput.Text.Trim());
        }
        private void PerformFacultiesSerach(string search)
        {
            var filtered = FilterUtils.GetFiltered(
                Parant.FacultyList,
                search,
                (item, search) => FilterUtils.ContainsIgnoreCase(item.Name, search)
            );

            var first = filtered.FirstOrDefault();

            if (search != FilterUtils.RESET_FILTER_VALUE)
            {
                Parant.FacultyList.SetCurrentByReference(first);
            }

            Refresher.RefreshSelector(FacultyListBox, filtered, Parant.SelectedFaculty);
            Parant.RefreshGroupsSelect();
            DisplayFacultySelectionChanged();
        }

        private void PerformGroupsSerach(string search)
        {
            var filtered = FilterUtils.GetFiltered(
                Parant.SelectedFaculty,
                search,
                (item, search) => FilterUtils.ContainsIgnoreCase(item.Name, search)
            );

            var first = filtered.FirstOrDefault();

            if (search != FilterUtils.RESET_FILTER_VALUE)
            {
                Parant.SelectedFaculty.SetCurrentByReference(first);
            }

            Refresher.RefreshSelector(GroupListBox, filtered, Parant.SelectedGroup);
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

        private void ToggleBtnsEnabledTo(bool isEnabled, params Button[] btns)
        {
            if (isEnabled) EnableOkBtn(btns);
            else DisableOkBtn(btns);
        }

        private void DisableOkBtn(params Button[] btns)
        {
            foreach (var btn in btns)
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
