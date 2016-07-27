using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace CoolManager
{
    /// <summary>
    /// Interaction logic for ChangesControl.xaml
    /// </summary>
    public partial class ChangesControl : UserControl
    {
        MainWindow MainWindowReference;

        public ChangesControl(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindowReference = mainWindow;
        }

        /* Change Name */

        private void NameButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindowReference.setDeviceName(NameBox.Text);
        }

        /* Change Password */

        private void PasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CurrentPasswordBox.Text.Equals(MainWindow.ctblePassword))
            {
                MainWindowReference.MSG("ALERT", "Current Password does not match with device password.", MSGControl.OPTION.NOTHING);
                clearChangesContent();
                return;
            }

            if (NewPasswordBox.Text.Length < MainWindow.MIN_DEVICE_PASSWORD_LENGTH)
            {
                MainWindowReference.MSG("ALERT", "New Password must be from 4 to 15 numbers long.", MSGControl.OPTION.NOTHING);
                clearChangesContent();
                return;
            }


            int result;
            if (!int.TryParse(NewPasswordBox.Text, out result))
            {
                MainWindowReference.MSG("ALERT", "New Password must only contain numbers.", MSGControl.OPTION.NOTHING);
                clearChangesContent();
                return;
            }

            if (!NewPasswordBox.Text.Equals(ConfirmPasswordBox.Text))
            {
                MainWindowReference.MSG("ALERT", "New Password and Confirm Password does not match.", MSGControl.OPTION.NOTHING);
                clearChangesContent();
                return;
            }

            MainWindowReference.setDevicePassword(NewPasswordBox.Text);
            clearChangesContent();
        }

        /* Other */

        public void updateDeviceName(string name)
        {
            NameBox.Text = name;
        }

        public void updateDevicePassword(string password)
        {
            CurrentPasswordBox.Text = password;
        }

        public void clearChangesContent()
        {
            CurrentPasswordBox.Text = "";
            NewPasswordBox.Text = "";
            ConfirmPasswordBox.Text = "";
        }

        private void Back_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindowReference.MessagesControl_Back();
            clearChangesContent();
        }
    }
}
