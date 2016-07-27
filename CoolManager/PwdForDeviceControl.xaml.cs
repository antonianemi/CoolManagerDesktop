using System;
using System.Collections.Generic;
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

namespace CoolManager
{
    /// <summary>
    /// Interaction logic for PwdForDeviceControl.xaml
    /// </summary>
    public partial class PwdForDeviceControl : UserControl
    {
        MainWindow MainWindowReference;

        public PwdForDeviceControl(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindowReference = mainWindow;
        }

        private void password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                MainWindow.userPassword = passwordBox.Password;
                MainWindowReference.DialogControl_Back();
                //Connect with Bluetooth Le
                MainWindowReference.connectBluetoothLE(MainWindow.deviceSender);

                passwordBox.Password = "";
            }

            
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindowReference.DialogControl_Back();
            passwordBox.Password = "";
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.userPassword = passwordBox.Password;
            MainWindowReference.DialogControl_Back();
            //Connect with Bluetooth Le
            MainWindowReference.connectBluetoothLE(MainWindow.deviceSender);

            passwordBox.Password = "";
        }
    }


}
