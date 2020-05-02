using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Stud
{
    /// <summary>
    /// Interaction logic for RenameForm.xaml
    /// </summary>
    public partial class RenameForm : Window
    {
        public delegate void OkAction(string newName);

        public OkAction OnOk;

        private string OldName;
        public RenameForm(string label, string oldName, OkAction ok)
        {
            InitializeComponent();

            Label.Text = label;
            OnOk = ok;
            OldName = oldName;
            NameInput.Text = oldName;

            DataContext = this;
            NameInput.Focus();
        }

        private void OkClicked(object sender, RoutedEventArgs e)
        {
            OnOk(NameInput.Text.Trim());
        }

        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NameChanged(object sender, RoutedEventArgs e)
        {
            var trimmed = NameInput.Text.Trim();

            if (trimmed != string.Empty && trimmed != OldName)
            {
                OkBtn.IsEnabled = true;
                OkBtn.Opacity = 1;
            }
            else
            {
                OkBtn.IsEnabled = false;
                OkBtn.Opacity = 0.7;
            }
        }
    }
}
