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
    /// Interaction logic for RelaysControl.xaml
    /// </summary>
    public partial class RelaysControl : UserControl
    {
        MainWindow MainWindowReference;

        /* User Relay State */
        public static int mStateRelay = 0;

        public RelaysControl(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindowReference = mainWindow;
        }

        /* Relay Switches */

        private void ImgCompressor_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            mStateRelay = MainWindow.RELAY_COMPRESOR;
            MainWindowReference.toogleServiceRelays(MainWindow.RELAY_COMPRESOR);
        }

        private void ImgLamp_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            mStateRelay = MainWindow.RELAY_FOCO;
            MainWindowReference.toogleServiceRelays(MainWindow.RELAY_FOCO);
        }

        private void ImgResistor_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            mStateRelay = MainWindow.RELAY_RESISTENCIA;
            MainWindowReference.toogleServiceRelays(MainWindow.RELAY_RESISTENCIA);
        }

        private void ImgFan_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            mStateRelay = MainWindow.RELAY_VENTILADOR;
            MainWindowReference.toogleServiceRelays(MainWindow.RELAY_VENTILADOR);
        }

        private void ImgFilter_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.uHFD)
            {
                MainWindowReference.setFactorySettingHFD(MainWindow.OFF);
            }
            else
            {
                MainWindowReference.setFactorySettingHFD(MainWindow.ON);
            }
        }

        /**
         * Functions Called From MainWindow
         */

        /*
        public void updateChamberTemperature(float temperature)
        {
            if (MainWindow.uCF == MainWindow.Fharenheit)
            {
                ChamberTemperature.Content = temperature + "°F";
            }
            else
            {
                ChamberTemperature.Content = temperature + "°C";
            }
        }

        public void updateEvaporatorTemperature(float temperature)
        {
            if (MainWindow.uCF == MainWindow.Fharenheit)
            {
                EvaporatorTemperature.Content = temperature + "°F";
            }
            else
            {
                EvaporatorTemperature.Content = temperature + "°C";
            }
        }
         */

        public void setServiceRelays(string relays)
        {
            // Service Relays
            MainWindow.uCompresor = (relays[MainWindow.RELAY_COMPRESOR] == '1') ? true : false;
            MainWindow.uFoco = (relays[MainWindow.RELAY_FOCO] == '1') ? true : false;
            MainWindow.uResistencia = (relays[MainWindow.RELAY_RESISTENCIA] == '1') ? true : false;
            MainWindow.uVentilador = (relays[MainWindow.RELAY_VENTILADOR] == '1') ? true : false;

            updateServiceRelays();
        }

        public void updateServiceRelays()
        {

            // If message is received now really change Relay state
            switch (mStateRelay)
            {
                case MainWindow.RELAY_COMPRESOR:
                    MainWindow.uCompresor ^= true;
                    break;
                case MainWindow.RELAY_FOCO:
                    MainWindow.uFoco ^= true;
                    break;
                case MainWindow.RELAY_RESISTENCIA:
                    MainWindow.uResistencia ^= true;
                    break;
                case MainWindow.RELAY_VENTILADOR:
                    MainWindow.uVentilador ^= true;
                    break;
                default:
                    break;
            }

            /** Relays **/

            // Update uCompresor
            if (MainWindow.uCompresor)
            {
                ImgCompressor.Source = (ImageSource)FindResource("ImgRelaysOn");
            }
            else
            {
                ImgCompressor.Source = (ImageSource)FindResource("ImgRelaysOff");
            }

            // Update uFoco
            if (MainWindow.uFoco)
            {
                ImgLamp.Source = (ImageSource)FindResource("ImgRelaysOn");
            }
            else
            {
                ImgLamp.Source = (ImageSource)FindResource("ImgRelaysOff");
            }

            // Update uResistencia
            if (MainWindow.uResistencia)
            {
                ImgResistor.Source = (ImageSource)FindResource("ImgRelaysOn");
            }
            else
            {
                ImgResistor.Source = (ImageSource)FindResource("ImgRelaysOff");
            }

            // Update uVentilador
            if (MainWindow.uVentilador)
            {
                ImgFan.Source = (ImageSource)FindResource("ImgRelaysOn");
            }
            else
            {
                ImgFan.Source = (ImageSource)FindResource("ImgRelaysOff");
            }

            mStateRelay = 0;
        }

        public void updateuHDFButton()
        {
            if (MainWindow.uHFD)
            {
                ImgFilter.Source = (ImageSource)FindResource("ImgRelaysOn");
            }
            else
            {
                ImgFilter.Source = (ImageSource)FindResource("ImgRelaysOff");
            }
        }

        public void updateInputVoltage(string inputVoltage)
        {
            ValueVoltage.Content = inputVoltage;
        }

        public void updateLightSensor(string lightSensor)
        {
            ValueLight.Content = lightSensor;
        }

        /* Other */

        private void Back_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindowReference.setTestMode(false);
            MainWindowReference.MessagesControl_Back();
            ImgFactorySettings.IsEnabled = false;
        }

        private void EnableFactorySettings_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            ImgFactorySettings.IsEnabled = true;
        }

        private void FactorySettings_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindowReference.ShowDialog_PwdFactorySettingsControl();
        }

              
    }
}
