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
    /// Interaction logic for TemperatureControl.xaml
    /// </summary>
    public partial class TemperatureControl : UserControl
    {

        MainWindow MainWindowReference;

        /* Set Pont Green Text Effect Variables */
        public const int GREEN_TEXT_SECONDS = 100;
        public static int greenTextCounter = GREEN_TEXT_SECONDS;
        public static bool flagGreenTextToWithe = false;

        /* Timer */
        System.Windows.Threading.DispatcherTimer greenTextTimer = new System.Windows.Threading.DispatcherTimer();

        public TemperatureControl(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindowReference = mainWindow;

            //Timer Setup
            greenTextTimer.Tick += new EventHandler(greenTextTimer_Tick);
            greenTextTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);

            //Set Point Send Button Visibility
            ImgSendSetPont.Visibility = Visibility.Hidden;
        }

        private void ImgFharenheit_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.uCF == MainWindow.Celsius)
            {
                MainWindowReference.setDegreeUnits(MainWindow.Fharenheit);
            }
        }

        private void ImgCelsius_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.uCF == MainWindow.Fharenheit)
            {
                MainWindowReference.setDegreeUnits(MainWindow.Celsius);
            }
        }

        private void ImgArrUp_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            ImgArrUp.Source = (ImageSource)FindResource("ImgTemperaturaArrUpGreen");
            startGreenTextEffect();

            if (MainWindow.userfSpc < MainWindow.fLs_Spc)
            {
                if (MainWindow.uCF == MainWindow.Fharenheit)
                {
                    MainWindow.userfSpc++;
                    SetPoint.Content = MainWindow.userfSpc + "°F";
                }
                else
                {
                    MainWindow.userfSpc = MainWindow.userfSpc + 0.5;
                    SetPoint.Content = MainWindow.userfSpc + "°C";
                }
            }
            else
            {
                Console.WriteLine("ImgArrUp_ButtonUp - Upper Limit Reached");
            }
        }

        private void ImgArrUp_ButtonDown(object sender, MouseButtonEventArgs e)
        {
            ImgArrUp.Source = (ImageSource)FindResource("ImgTemperaturaArrUpRed");
        }

        private void ImgArrUp_ButtonOut(object sender, MouseEventArgs e)
        {
            ImgArrUp.Source = (ImageSource)FindResource("ImgTemperaturaArrUpGreen");
        }

        private void ImgArrDown_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            ImgArrDwon.Source = (ImageSource)FindResource("ImgTemperaturaArrDownGreen");
            startGreenTextEffect();

            if (MainWindow.userfSpc > MainWindow.fLi_Spc)
            {
                if (MainWindow.uCF == MainWindow.Fharenheit)
                {
                    MainWindow.userfSpc--;
                    SetPoint.Content = MainWindow.userfSpc + "°F";
                }
                else
                {
                    MainWindow.userfSpc = MainWindow.userfSpc - 0.5;
                    SetPoint.Content = MainWindow.userfSpc + "°C";
                }
            }
            else
            {
                Console.WriteLine("ImgArrDown_ButtonUp - Lower Limit Reached");
            }
        }

        private void ImgArrDown_ButtonDown(object sender, MouseButtonEventArgs e)
        {
            ImgArrDwon.Source = (ImageSource)FindResource("ImgTemperaturaArrDownBlue");
        }

        private void ImgArrDown_ButtonOut(object sender, MouseEventArgs e)
        {
            ImgArrDwon.Source = (ImageSource)FindResource("ImgTemperaturaArrDownGreen");
        }

        private void ImgModes_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindowReference.Show_ModesControl();
        }

        private void ImgSendSetPoint_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindowReference.setSetPoint(MainWindow.userfSpc);
            ImgSendSetPont.Background = new ImageBrush((ImageSource)FindResource("ImgTemperaturaSendSetPointGreen"));
        }

        private void ImgSendSetPoint_ButtonDown(object sender, MouseButtonEventArgs e)
        {
            ImgSendSetPont.Background = new ImageBrush((ImageSource)FindResource("ImgTemperaturaSendSetPointRed"));
        }

        private void ImgSendSetPoint_ButtonOut(object sender, MouseEventArgs e)
        {
            ImgSendSetPont.Background = new ImageBrush((ImageSource)FindResource("ImgTemperaturaSendSetPointGreen"));
        }

        /**
         * Functions Called From MainWindow
         */
        public void ImgModes_Foco()
        {
            ImgModes.Source = (ImageSource)FindResource("ImgTemperaturaModeFoco");
        }

        public void ImgModes_FocoOff()
        {
            ImgModes.Source = (ImageSource)FindResource("ImgTemperaturaModeFocoOff");
        }

        public void ImgModes_Eco()
        {
            ImgModes.Source = (ImageSource)FindResource("ImgTemperaturaModeEco");
        }

        public void updateDegrees(bool unit)
        {
            if (unit == MainWindow.Fharenheit)
            {
                ImgCelsius.Source = (ImageSource)FindResource("ImgTemperaturaCrojo");
                ImgFharenheit.Source = (ImageSource)FindResource("ImgTemperaturaFgris");
            }
            else
            {
                ImgCelsius.Source = (ImageSource)FindResource("ImgTemperaturaCgris");
                ImgFharenheit.Source = (ImageSource)FindResource("ImgTemperaturaFazul");
            }
        }

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

        public void updateTypeControl(string deviceName)
        {
            /* Updates the type of control to CTI or CTF, enabling or not the EchoMode */
            if (MainWindow.TypeControl == '1')
            {
                /* CTI Mode, Show Echo Mode */
                ImgModes.Visibility = Visibility.Visible;
                /*
                if (deviceName != null)
                    ValueDeviceName.Content = deviceName + " - DTCR";
                else
                    ValueDeviceName.Content = "(no name) - DCTR";
                 */
            }
            else if (MainWindow.TypeControl == '2')
            {
                /* CTF Mode, Hide Echo Mode  */
                ImgModes.Visibility = Visibility.Hidden;
                /*
                if (deviceName != null)
                    ValueDeviceName.Content = deviceName + " - DTCF";
                else
                    ValueDeviceName.Content = "(no name) - DTCF";
                 */
            }
            else
            {
                /* Unknown Mode, Show Echo Mode */
                ImgModes.Visibility = Visibility.Visible;
                //ValueDeviceName.Content = deviceName + " - DTC?";
                Console.WriteLine("!!!Error, Undefined Type of control");
            }
        }

        public void updateSetPoint()
        {
            /* Updates SetPoint when user sends a new SetPoint or due to setPointTimer TimeOut */
            MainWindow.userfSpc = MainWindow.fSpc;
            if (MainWindow.uCF)
            {
                SetPoint.Content = MainWindow.userfSpc + "°F";
                if (MainWindow.TypeControl != '1')
                {
                    if (MainWindow.fSpc <= MainWindow.EcoModeShowLimitFahrenh)
                    {
                        /* Hide Echo Mode */
                        ImgModes.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        /* Show Echo Mode */
                        if (MainWindow.TypeControl != '2') ImgModes.Visibility = Visibility.Visible;
                    }
                }
            }
            else
            {
                SetPoint.Content = MainWindow.userfSpc + "°C";
                if (MainWindow.TypeControl != '1')
                {
                    if (MainWindow.fSpc <= MainWindow.EcoModeShowLimitCelsius)
                    {
                        /* Hide Echo Mode */
                        ImgModes.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        /* Show Echo Mode */
                        if (MainWindow.TypeControl != '2') ImgModes.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        public void updateControlRelays(string relays)
        {
            // Control Releays
            MainWindow.uMode = relays[MainWindow.RELAY_MODE];
            MainWindow.uDefrost = (relays[MainWindow.RELAY_DEFROST] == '1') ? true : false;
            MainWindow.uEngine = (relays[MainWindow.RELAY_ENGINE] == '1') ? true : false;
            MainWindow.uDoor = (relays[MainWindow.RELAY_DOOR] == '1') ? true : false;

            /** Controls **/

            // Update uModeEco
            if (MainWindow.uMode == '0')
                ImgModes.Source = (ImageSource)FindResource("ImgTemperaturaModeFoco");
            else if (MainWindow.uMode == '1')
                ImgModes.Source = (ImageSource)FindResource("ImgTemperaturaModeFocoOff");
            else
                ImgModes.Source = (ImageSource)FindResource("ImgTemperaturaModeEco");

            // Update uDoor
            if (MainWindow.uDoor)
            {
                ImgDoorOpen.Source = (ImageSource)FindResource("ImgTemperaturaDoorOpenON");
            }
            else
            {
                ImgDoorOpen.Source = (ImageSource)FindResource("ImgTemperaturaDoorOpenOFF");
            }

            // Update Engine
            if (MainWindow.uEngine)
            {
                ImgTools.Source = (ImageSource)FindResource("ImgTemperaturaToolsON");
            }
            else
            {
                ImgTools.Source = (ImageSource)FindResource("ImgTemperaturaToolsOFF");
            }

            // Update Defrost
            if (MainWindow.uDefrost)
            {
                ImgDefrost.Source = (ImageSource)FindResource("ImgTemperaturaDefrostON");
            }
            else
            {
                ImgDefrost.Source = (ImageSource)FindResource("ImgTemperaturaDefrostOFF");
            }

        }

        public void startGreenTextEffect()
        {
            greenTextTimer.Stop();
            MainWindow.flagChangingSetpoint = false;

            greenTextCounter = GREEN_TEXT_SECONDS;
            SetPoint.Foreground = new SolidColorBrush(Colors.Lime);
            SetPoint.Opacity = 1;
            flagGreenTextToWithe = false;
            ImgSendSetPont.Visibility = Visibility.Visible;
            greenTextTimer.Start();
            MainWindow.flagChangingSetpoint = true;
        }

        public void temporallyChangeSetPoint()
        {
            if (MainWindow.uCF == MainWindow.Fharenheit)
            {
                SetPoint.Content = MainWindow.fSpc + "°F";
            }
            else
            {
                SetPoint.Content = MainWindow.fSpc + "°C";
            }
            ImgSendSetPont.Visibility = Visibility.Hidden;
        }

        public void renwSetPoint()
        {
            greenTextTimer.Stop();
            MainWindow.flagChangingSetpoint = false;
            flagGreenTextToWithe = false;
            SetPoint.Foreground = new SolidColorBrush(Colors.White);
            SetPoint.Opacity = 1;
            ImgSendSetPont.Visibility = Visibility.Hidden;
            updateSetPoint();
        }

        // Timer Function
        private void greenTextTimer_Tick(object sender, EventArgs e)
        {
            if (greenTextCounter <= GREEN_TEXT_SECONDS && greenTextCounter >= 80)
            {
                SetPoint.Opacity = 1;
            }
            else if (greenTextCounter < 80 && greenTextCounter >= 40)
            {
                SetPoint.Opacity -= 0.1;
                if (SetPoint.Opacity <= 0) SetPoint.Opacity = 1;
            }
            else if (greenTextCounter < 40 && greenTextCounter >= 1)
            {
                if (flagGreenTextToWithe == false)
                {
                    flagGreenTextToWithe = true;
                    temporallyChangeSetPoint(); //TODO move this function to handler. 
                }
                SetPoint.Foreground = new SolidColorBrush(Colors.White);
                SetPoint.Opacity -= 0.1;
                if (SetPoint.Opacity <= 0) SetPoint.Opacity = 1;
            }
            else if (greenTextCounter == 0)
            {
                /* This is send to handler because external activities also call renewSetPint() function */
                renwSetPoint();
            }
            greenTextCounter--;
        }

    }
}
