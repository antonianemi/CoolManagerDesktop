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
    /// Interaction logic for WaitBarControl.xaml
    /// </summary>
    public partial class WaitBarControl : UserControl
    {
        MainWindow MainWindowReference;

        /* Timer */
        System.Windows.Threading.DispatcherTimer waitBarTimer
            = new System.Windows.Threading.DispatcherTimer();

        private const int NUM_CIRCLES = 8;
        private const int MILLISECONDS = 150;
        private const int TIMEOUT_LIMIT = 135;  //(135 * 150 millisec) = 20 seg aprox.
        private static int circlePosition = 1;
        private static int TimeOutCounter = 0;
        public static bool flagInConnection = false;

        public WaitBarControl(MainWindow MainWindow)
        {
            InitializeComponent();
            MainWindowReference = MainWindow;

            //Timer Setup
            waitBarTimer.Tick += new EventHandler(WaitBarTimer_Tick);
            waitBarTimer.Interval = new TimeSpan(0, 0, 0, 0, MILLISECONDS);
        }

        public void Start()
        {
            circlePosition = 1;
            TimeOutCounter = 0;
            ReceviedData.Content = "";
            waitBarTimer.Start();  
        }

        public void Stop()
        {
            waitBarTimer.Stop();
        }
      
        public void updateReceviedData(string Data)
        {
            //Updates Log Data and sets the TimeOutCounter to Zero!
            ReceviedData.Content = "Received Data = " + Data;
            TimeOutCounter = 0;
        }

        private void WaitBarTimer_Tick(object sender, EventArgs e)
        {
            //Reset all circles to white
            Circle1.Fill = new SolidColorBrush(Colors.White);
            Circle2.Fill = new SolidColorBrush(Colors.White);
            Circle3.Fill = new SolidColorBrush(Colors.White);
            Circle4.Fill = new SolidColorBrush(Colors.White);
            Circle5.Fill = new SolidColorBrush(Colors.White);
            Circle6.Fill = new SolidColorBrush(Colors.White);
            Circle7.Fill = new SolidColorBrush(Colors.White);
            Circle8.Fill = new SolidColorBrush(Colors.White);

            //Fill de circle position to blue
            switch (circlePosition)
            {
                case 1:
                    Circle1.Fill = new SolidColorBrush(Colors.DodgerBlue);
                    Circle8.Fill = new SolidColorBrush(Colors.LightSkyBlue);
                    Circle7.Fill = new SolidColorBrush(Colors.LightCyan);
                    break;
                case 2:
                    Circle2.Fill = new SolidColorBrush(Colors.DodgerBlue);
                    Circle1.Fill = new SolidColorBrush(Colors.LightSkyBlue);
                    Circle8.Fill = new SolidColorBrush(Colors.LightCyan);
                    break;
                case 3:
                    Circle3.Fill = new SolidColorBrush(Colors.DodgerBlue);
                    Circle2.Fill = new SolidColorBrush(Colors.LightSkyBlue);
                    Circle1.Fill = new SolidColorBrush(Colors.LightCyan);
                    break;
                case 4:
                    Circle4.Fill = new SolidColorBrush(Colors.DodgerBlue);
                    Circle3.Fill = new SolidColorBrush(Colors.LightSkyBlue);
                    Circle2.Fill = new SolidColorBrush(Colors.LightCyan);
                    break;
                case 5:
                    Circle5.Fill = new SolidColorBrush(Colors.DodgerBlue);
                    Circle4.Fill = new SolidColorBrush(Colors.LightSkyBlue);
                    Circle3.Fill = new SolidColorBrush(Colors.LightCyan);
                    break;
                case 6:
                    Circle6.Fill = new SolidColorBrush(Colors.DodgerBlue);
                    Circle5.Fill = new SolidColorBrush(Colors.LightSkyBlue);
                    Circle4.Fill = new SolidColorBrush(Colors.LightCyan);
                    break;
                case 7:
                    Circle7.Fill = new SolidColorBrush(Colors.DodgerBlue);
                    Circle6.Fill = new SolidColorBrush(Colors.LightSkyBlue);
                    Circle5.Fill = new SolidColorBrush(Colors.LightCyan);
                    break;
                case 8:
                    Circle8.Fill = new SolidColorBrush(Colors.DodgerBlue);
                    Circle7.Fill = new SolidColorBrush(Colors.LightSkyBlue);
                    Circle6.Fill = new SolidColorBrush(Colors.LightCyan);
                    break;
                default:
                    break;
            }

            //Change to the next position
            circlePosition++;

            if (circlePosition > NUM_CIRCLES) 
                circlePosition = 1;

            //If is Waiting for Connection check timeout limit
            if (flagInConnection)
            {
                TimeOutCounter++;

                if (TimeOutCounter == TIMEOUT_LIMIT)
                {
                    MainWindowReference.WaitBarControl_Hide();
                    MainWindowReference.MSG("UNABLE TO CONNECT", "Connection Time Out", MSGControl.OPTION.NOTHING);

                    //Reset and diconnnect from device
                    MainWindowReference.disconnectDevice();

                    //Close Search List Window
                    MainWindowReference.MessagesControl_Back();
                }
            }
        }
    }
}
