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
    /// Interaction logic for OkCancelControl.xaml
    /// </summary>
    public partial class OkCancelControl : UserControl
    {

        MainWindow MainWindowReference;
        public static OPTION opt = OPTION.NOTHING;

        public enum OPTION
        {
            NOTHING,
            FACOTRY_RESET,
            ERASE_EEPROM,
            ERASE_EEPROM_SURE,
            DISCONNECT,
            ERASE_SAVED_PASSWORDS
        };

        public OkCancelControl(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindowReference = mainWindow;
        }

        public void ButtonOk_Click(object sender, RoutedEventArgs e)
        {

            switch (opt)
            {
                case OPTION.NOTHING:
                    MainWindowReference.DialogControl_Back();
                    break;
                case OPTION.FACOTRY_RESET:
                    MainWindowReference.resetFactorySettings();
                    MainWindowReference.DialogControl_Back();
                    MainWindowReference.getFactorySettings();
                    break;
                case OPTION.ERASE_EEPROM:
                    MainWindowReference.DialogControl_Back();
                    MainWindowReference.ShowDialog_OkCancelControl(
                        "WARNING",
                        "This task can take up to 30 minutes to finish, do you want to continue?",
                        OPTION.ERASE_EEPROM_SURE);
                    break;
                case OPTION.ERASE_EEPROM_SURE:
                    MainWindowReference.DialogControl_Back();
                    MainWindowReference.eraseEEprom();
                    break;

                case OPTION.DISCONNECT:
                    MainWindowReference.DialogControl_Back();
                    MainWindowReference.scanBluetoothLE();
                    break;

                case OPTION.ERASE_SAVED_PASSWORDS:
                    MainWindowReference.DialogControl_Back();
                    try
                    {
                        Microsoft.Win32.Registry.CurrentUser.DeleteSubKey("Software\\TORREY\\CoolManager");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;

                default:
                    break;
            }
        }

        public void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindowReference.DialogControl_Back();
        } 

        public void updateContent(string controlTitle, string controlMessage)
        {
            Title.Content = controlTitle;
            MessageBlock.Text = controlMessage;
        }
    }
}
