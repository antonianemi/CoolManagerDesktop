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
using BgApiDriver;
using System.Text.RegularExpressions;

namespace CoolManager
{
    public delegate void updateScanListCallBack(List<bd_addr> deviceList);

    public class ScanListItem
    {
        public string name { get; set; }
        public string address { get; set; }
        public int rssi { get; set; }
        public bd_addr sender { get; set; }
    }

    /// <summary>
    /// Interaction logic for DeviceListControl.xaml
    /// </summary>
    public partial class DeviceListControl : UserControl
    {
        MainWindow MainWindowReference;

        public DeviceListControl(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindowReference = mainWindow;
        }

        private void ScanListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ScanListBox.SelectedItem != null)
            {
                ScanListItem obj = new ScanListItem();
                obj = (ScanListItem)ScanListBox.SelectedItem;

                //Set Connection Name, Address and Sender
                MainWindow.deviceName = obj.name;
                MainWindow.deviceSender = obj.sender;
                MainWindow.deviceAddress = BitConverter.ToString(obj.sender.Address).Replace("-", ":");
                MainWindow.deviceAddressFile = MainWindow.deviceAddress.Replace(":", "") + ".txt";
                //MainWindow.deviceAddressFile = "E0C79D62FBEC.txt"; //TODO remove this, test only
                
                /*Password and Address validation*/
                Microsoft.Win32.RegistryKey key;
                key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\TORREY\\CoolManager");
                key.Close();

                MainWindow.userPassword = VerifyPasswordAddress(MainWindow.deviceAddress);
               
                if (MainWindow.userPassword == null) MainWindowReference.ShowDialog_PwdForDeviceControl(); //Ask for password
                else
                {
                    //Password Exists, try to connect automatically
                    MainWindowReference.DialogControl_Back();
                    //Connect with Bluetooth Le
                    MainWindowReference.connectBluetoothLE(MainWindow.deviceSender);
                }

            }
        }

        private String VerifyPasswordAddress(String Address)
        {
            String Password=null;

            try
            {

                Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\TORREY\\CoolManager");
                Password = registryKey.GetValue(Address).ToString();
                registryKey.Close();

                MainWindow.flagPasswordWasRetrievedFromDB = true;
                return Password;
        
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                MainWindow.flagPasswordWasRetrievedFromDB = false;
                return null;
            }



            
        }


        private void Back_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            //Stop scan list if running
            MainWindowReference.objScanList.stop();
            MainWindowReference.MessagesControl_Back();
        }

        /*
         * Functions called from MainWindow
         */
        public void updateScanList(List<BgApi.ble_msg_gap_scan_response_evt_t> deviceList)
        {
            //Function called from a thread, thus use Invoke
            MainWindowReference.Dispatcher.BeginInvoke(new Action(delegate()
            {
                ScanListBox.Items.Clear();

                foreach (BgApi.ble_msg_gap_scan_response_evt_t item in deviceList.ToList())
                {
                    string devName = (new Regex("[^a-zA-Z0-9 -]")).Replace(System.Text.Encoding.UTF8.GetString(item.data), "");
                    ScanListBox.Items.Add(new ScanListItem()
                    {
                        name = devName,
                        address = BitConverter.ToString(item.sender.Address).Replace("-", ":"),
                        rssi = (sbyte)item.rssi,
                        sender = item.sender
                    });
                }

            }));
        }

        public void clearScanList()
        {
            ScanListBox.Items.Clear();
        }     

    }
}
