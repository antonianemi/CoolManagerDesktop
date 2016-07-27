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
    /// Interaction logic for PwdFactorySettingsControl.xaml
    /// </summary>
    public partial class PwdFactorySettingsControl : UserControl
    {
        MainWindow MainWindowReference;
        private const string PASSWORD = "CTBLE13TOR";

        public PwdFactorySettingsControl(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindowReference = mainWindow;
        }

        private void password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                MainWindowReference.DialogControl_Back();

                if (passwordBox.Password.Equals(PASSWORD))
                {
                    Console.WriteLine("Password Correct");                    
                    MainWindowReference.Show_FactorySettingsControl();
                }
                else
                {
                    MainWindowReference.MSG("ALERT", "Invalid Password", MSGControl.OPTION.NOTHING);
                }

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
            MainWindowReference.DialogControl_Back();

            if (passwordBox.Password.Equals(PASSWORD))
            {
                Console.WriteLine("Password Correct");                
                MainWindowReference.Show_FactorySettingsControl();
            }
            else
            {
                MainWindowReference.MSG("ALERT", "Invalid Password", MSGControl.OPTION.NOTHING);
            }


            passwordBox.Password = "";
        }
    }
}
