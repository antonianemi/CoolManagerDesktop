using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for SearchControl.xaml
    /// </summary>
    public partial class SearchControl : UserControl
    {
        MainWindow MainWindowReference;

        public SearchControl(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindowReference = mainWindow;
        }

        private void SearchButton_LeftMouseUp(object sender, MouseButtonEventArgs e)
        {
            SearchButton.Background = new ImageBrush((ImageSource)FindResource("ImgSearchAzul"));
            MainWindowReference.scanBluetoothLE();
        }

        private void SearchButton_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            SearchButton.Background = new ImageBrush((ImageSource)FindResource("ImgSearchVerde"));
        }

        private void SearchButton_MouseLeave(object sender, MouseEventArgs e)
        {
            SearchButton.Background = new ImageBrush((ImageSource)FindResource("ImgSearchAzul"));
        }

        private void ChangeButton_LeftMouseUp(object sender, MouseButtonEventArgs e)
        {
            //MainWindowReference.Show_ChangesControl();
            //MainWindowReference.scanBluetoothLE();

            MainWindowReference.ShowDialog_OkCancelControl(
                        "ALERT",
                        "Are you sure that you want to disconnect from the current device?",
                        OkCancelControl.OPTION.DISCONNECT);
        }

        public void Show_GridConnectionInfo()
        {
            GridConnectionInfo.Visibility = Visibility.Visible;
        }

        public void Hide_GridConnectionInfo()
        {
            GridConnectionInfo.Visibility = Visibility.Hidden;
        }

        public void updateDeviceName(string name)
        {
            ValueDeviceName.Content = name;
        }

        public void updateDeviceAddress(string name)
        {
            ValueDeviceAddress.Content = name;
        }

    }   
}
