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
    /// Interaction logic for MSGControl.xaml
    /// </summary>
    public partial class MSGControl : UserControl
    {
        MainWindow MainWindowReference;
        public static OPTION opt = OPTION.NOTHING;

        public enum OPTION
        {
            NOTHING,
            WAIT_UNITL_FINISH,
            BLE_DISCONNECT,
            BLE_WILL_DISCONNECT,
            CONNECTED,
        };

        public MSGControl(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindowReference = mainWindow;
        }


        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            MainWindowReference.DialogControl_Back();

            switch (opt)
            {
                case OPTION.NOTHING:
                    break;

                case OPTION.CONNECTED:
                    MainWindowReference.MessagesControl_Back();
                    MainWindowReference.DialogControl_Back();
                    MainWindowReference.GotoTemperatureScreen();

                    break;

                case OPTION.WAIT_UNITL_FINISH:
                    MainWindowReference.MSG(
                    "MESSAGE", 
                    "Bluetooth connection will END, retry connection after device has finished erasing all data.", 
                    MSGControl.OPTION.BLE_WILL_DISCONNECT);                                       
                    break;

                case OPTION.BLE_DISCONNECT:
                    //Hide Dialogs
                    MainWindowReference.MessagesControl_Back();
                    MainWindowReference.DialogControl_Back();
                    break;

                case OPTION.BLE_WILL_DISCONNECT:
                    MainWindowReference.MSG("DISCONNECTED!", "Could not establish connection", MSGControl.OPTION.NOTHING);
                    break;
                
                default:
                    break;
            }
        }

        public void updateContent(string controlTitle, string controlMessage)
        {
            Title.Content = controlTitle;
            MessageBlock.Text = controlMessage;
        }
    }
}
