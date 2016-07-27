/* 
 * TORREY
 * CoolManager for PC
 * 2014 Jorge Garza (electronica2@fabatsa.com.mx)
 * 2014 Modified - Paul Velasquez (hardware@fabatsa.com.mx)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Management;
using System.IO.Ports;
using System.Globalization;
using Abt.Controls.SciChart.Visuals;

namespace CoolManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /* User Control References */
        public static MainWindow MainWindowReference;
        public static SearchControl SearchControl;
        public static SettingsControl SettingsControl;
        public static DeviceListControl DeviceListControl;
        public static TemperatureControl TemperatureControl;
        public static ModesControl ModesControl;
        public static LogsControl LogsControl;
        public static ServicesControl ServicesControl;
        public static RelaysControl RelaysControl;
        public static FactorySettingsControl FactorySettingsControl;
        public static PwdFactorySettingsControl PwdFactorySettingsControl;
        public static ChangesControl ChangesControl;
        public static HelpControl HelpControl;      
        public static PwdForDeviceControl PwdForDeviceControl;
        public static MSGControl MSGControl;
        public static OkCancelControl OkCancelControl;
        public static WaitBarControl WaitBarControl;
        public static CustomSampleTimeControl CustomSampleTimeControl;

        /* Extra Config Parameters for CT_BLE_CONFIG_UUID (0xFFAE) */
        public const char CTBLE_SPC     	='A';	//SetPointControl
        public const char CTBLE_LISPC    	='B';	//LimiteInferior Alarma SPC
        public const char CTBLE_LSSPC    	='C';	//LimiteSuperior Alarma SPC
        public const char CTBLE_SID      	='D';	//Inicio Deshielo
        public const char CTBLE_STD      	='E';	//Fin Deshielo
        public const char CTBLE_SDIF     	='F';	//Diferencia para activar Deshielo
        public const char CTBLE_tDI      	='G';
        public const char CTBLE_tES0     	='H';
        public const char CTBLE_tDUD     	='I';
        public const char CTBLE_tDESH    	='J';
        public const char CTBLE_tRAD     	='K';
        public const char CTBLE_tTCC     	='L';
        public const char CTBLE_SPSL     	='M';
        public const char CTBLE_CALMV    	='N';
        public const char CTBLE_tDESH2   	='O';
        public const char CTBLE_CDESH    	='P';
        public const char CTBLE_TMAX     	='Q';
        public const char CTBLE_tDUDM    	='R';
        public const char CTBLE_tUD      	='S';
        public const char CTBLE_HFD      	='T';	//Filtro de display ( 0 = OFF  |  1 = ON  )
        public const char CTBLE_BTN         ='f';   //Enable de botonera externa
        public const char CTBLE_CUT      	='U';	//Cambio de Unidades (  0 = ºC  |  1 = ºF  )
        public const char CTBLE_DEL      	='V';	//Erease EEprom
        public const char CTBLE_BLK      	='W';	//Blinker display ( 1 = 0N   |  2 = OFF )
        public const char CTBLE_TPC      	='X';	//Tipo Control (1 = CTI  |  2 = CTF  |  3 = CTR)
        public const char CTBLE_PSS      	='Y';	//Password ( 1 = Read  |  2 = Write )
        public const char CTBLE_RSF      	='Z';	//ResetFactorySetting
        public const char CTBLE_Z			='a';
        public const char CTBLE_tGOTEO		='b';
        public const char CTBLE_SETTIME  	='0';	//SetTime YYMMDDHHmmss
        public const char CTBLE_TESTMODE 	='1';
        public const char CTBLE_ECOMODE 	='2';	//EcoMode (0=ON, 1=OFF, 2=AUTO)
        public const char CTBLE_CFRV  		='3';	//Activa o desactiva Relays de (Compresor, Focos, Resistecia, Compresor)
        public const char CTBLE_TLOGGER 	='4';	//Timpo de muestreo de temperatura del Controlador.
        public const char CTBLE_SETLOG  	='5';	//Activa el log para lectura (0 -> Lee toda la memoria, 1->
        public const char CTBLE_NAME 		='6';	//CTBLE Bluetooth name
        public const char CTBLE_VOLTAGE 	='8';	//CTBLE Input Voltage Counts from ADC 
        public const char CTBLE_LIGHTSENSOR ='9';	//CTBLE Light Sensor Counts from ADC

        /* CTBLE Services and Characteristics charHandler UUIDs */
        public const int CT_BLE_TMP_UUID            = 0x12;     //Equivalent to 0xFFA0    
        public const int CT_BLE_CONFIG_UUID         = 0x18;     //Equivalent to 0xFFAE
        public const int CT_BLE_GRL_UUID            = 0x14;     //Equivalent to 0xFFA2
        public const int CT_BLE_VERSION_UUID        = 0x16;     //Equivalent to 0cFFAA
        public const int CT_BLE_ACTIVATELOG_UUID    = 0x1C;     //Equivalent to 0cFFAB
        public const int CT_BLE_LOGS_UUID           = 0x1A;     //Equivalent to 0cFFAC
        public const int CT_BLE_LOGS_NOTIFY_UUID    = 0x1B;     //Equivalent to 0cFFAC - 2803

        /* DTC Constant Variables */
        public const char ON                            = '1';
        public const char OFF                           = '0';
        public const bool Fharenheit                    = true;
        public const bool Celsius                       = false;
        public const int RELAY_DOOR 			        = 0;
	    public const int RELAY_COMPRESOR		        = 1;
	    public const int RELAY_FOCO 			        = 2;
	    public const int RELAY_RESISTENCIA 		        = 3;
	    public const int RELAY_VENTILADOR 		        = 4;
	    public const int RELAY_ENGINE 			        = 5;
	    public const int RELAY_DEFROST			        = 6;
        public const int RELAY_MODE                     = 7;
        public static double EcoModeShowLimitCelsius    = 2;		//	Menor a este Limite el eco mode ya no se muestra (Celisus)
        public static double EcoModeShowLimitFahrenh    = 36;	    //	Menor a este Limite el eco mode ya no se muestra (Fahrenheits)
        public static int MIN_DEVICE_NAME_LENGTH 	    = 3;
        public static int MIN_DEVICE_PASSWORD_LENGTH    = 4;
        public static int LOG_MAX_FRAME_LENGTH			= 20;

        /** Control Model definitions */
        public const bool MODEL_DTC	    = false;
        public const bool MODEL_LDTC    = true;
    
    

        /* DTC Specific Global Varibales */
        public static int conntectionNumber     = 0;            //   Number of bluetooth conection.
        public static string deviceName         = null;         //   Name of the device connected with 
        public static bd_addr deviceSender      = null;         //   Data received from device connection intent
        public static string deviceAddress      = null;         //   Address of the device connected with 
        public static string deviceAddressFile  = null;         //   File Name used to store device temperature Data
        public static string ctblePassword      = null;			//Y: Password del ctble aka DTC
        public static string userPassword       = null;			//Y: Password enered by the user
        public static string universalPassword  = "7524368914";	//	 Universal Password to prevent the lost of password.
        public static bool uCF                  = Fharenheit;   //U: Centigrados=false (0), Fahrenheit=true (1)
        public static double fSpc               = 0;			//A: Set Point de Camara
        public static double fLi_Spc            = 0;            //B: Limite inferior set point de camara
        public static double fLs_Spc            = 0;            //C: Limite superior set point de camara
        public static int iSid                  = 0;			//D:                			
        public static int iStd                  = 0;			//E:
        public static int iSdif                 = 0;            //F:	
        public static int iTdi                  = 0;			//G:
        public static int iTeso                 = 0;			//H:
        public static int iTdud                 = 0;			//I:
        public static int iTdesh                = 0;			//J:
        public static int iTrad                 = 0;			//K:
        public static int iTtcc                 = 0;			//L:
        public static int iSpsl                 = 0;			//M:
        public static int iCalmv                = 0;			//N:
        public static int iTdesh2               = 0;			//O:
        public static int uCdesh                = 0;			//P:
        public static int iTmax                 = 0;			//Q:
        public static int iTDUDM                = 0;			//R:
        public static int iTud                  = 0;			//S:
        public static bool uHFD                 = false;	    //T:
        public static bool uSPBtnEnable         = false;		//f: SP Buttons Enable
        public static double fz                 = 0;			//a:
        public static int iTgoteo               = 0;  			//b:
        public static char TypeControl          = '0';		    //X: Tipo de Controlador (1 = CTI  |  2 = CTF  |  3 = CTR)
        public static string FirmwareVersion    = null;			//	 Version del Frimware
        public static string BLEVersion         = null;			//	 Version del Frimware 
        public static double userfSpc           = 0;			//	Set Point del Usuario
        public static float fTc                 = 0;            //	 Temperatura de la Camara
        public static float fTmd                = 0;            //	 Temperatura Monitoreo Deshielo
        public static float fTerm3              = 0;            //	 Temperatura Tercer Termistor
        public static string relays             = null;         //	> Contiene de B0 a B7
        public static char uMode                = '0';          //	B7. Modo de operacion : 0-> Focos On, 1 -> Fcos Off, 2 -> Automatico (Eco)
        public static bool uDoor                = false;        //	B6. Estado Puerta :  (true) 1 -> Encendido, (false) 0 -> Apagado
        public static bool uCompresor           = false;        //	B5. Compresor :  (true) 1 -> Encendido, (false) 0 -> Apagado 
        public static bool uFoco                = false;        //	B4. Foco :  (true) 1 -> Encendido, (false) 0 -> Apagado 
        public static bool uResistencia         = false;		//	B3. Resistencia :  (true) 1 -> Encendido, (false) 0 -> Apagado 
        public static bool uVentilador          = false;        //	B2. Vetilador :  (true) 1 -> Encendido, (false) 0 -> Apagado 
        public static bool uEngine              = false;        //	B1. Engine :  (true) 1 -> Encendido, (false) 0 -> Apagado
        public static bool uDefrost             = false;        //	B0. Defrost :  (true) 1 -> Encendido, (false) 0 -> Apagado
        public static int sampleTime            = 2;			//  Valor recibido por sample time
        public static bool controlModel         = false;		//  Modelo del Control (false: DTC, true: LDTC)

        /* DTC Program Flags */
        public static bool flagExpectingOKMessage   = false;
        public static bool flagOnChangePassword     = false;
        public static bool flagInvalidPassword      = false;
        public static bool flagPasswordWasRetrievedFromDB = false;
        public static bool flagInSetSampleTime      = false;
        public static bool flagCTBLEisBlinking      = false;
        public static bool flagInGetFactorySettings = false;
        public static bool flagInSetFactorySettings = false;
        public static bool flagInGetHistoricLogs    = false;
        public static bool flagSearchButtonClick    = false;
        public static bool flagBleDeviceConnected   = false;
        public static bool flagRollBackSampleTime   = false;
        public static bool flagInEraseEprom         = false;
        public static bool flagChangingSetpoint     = false;

        /** Historic Log Frame Variables*/
        public static DateTime logDate;
        public static int logTimerLogs;

        /* USB Comunication Specific Global Varibales */
        public static MyBgApi usbDongle         = null;
        public static string usbPort            = null;
        public static bool usbFound             = false;
        public static bool usbReady             = false;

        /* Threads */
        public Thread scanThread;
        public ScanList objScanList = new ScanList();

        /* Timer */
        System.Windows.Threading.DispatcherTimer temperatureTimer 
            = new System.Windows.Threading.DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            InitSchiChartLicense();

            //Add User Controls
            MainWindowReference = this;
            SearchControl = new SearchControl(MainWindowReference);
            DeviceListControl = new DeviceListControl(MainWindowReference);
            TemperatureControl = new TemperatureControl(MainWindowReference);
            ModesControl = new ModesControl(MainWindowReference);
            SettingsControl = new SettingsControl(MainWindowReference);
            LogsControl = new LogsControl(MainWindowReference);
            ServicesControl = new ServicesControl(MainWindowReference);
            RelaysControl = new RelaysControl(MainWindowReference);
            ChangesControl = new ChangesControl(MainWindowReference);
            FactorySettingsControl = new FactorySettingsControl(MainWindowReference);
            HelpControl = new HelpControl(MainWindowReference);
            PwdFactorySettingsControl = new PwdFactorySettingsControl(MainWindowReference);
            PwdForDeviceControl = new PwdForDeviceControl(MainWindowReference);
            MSGControl = new MSGControl(MainWindowReference);
            OkCancelControl = new OkCancelControl(MainWindowReference);
            WaitBarControl = new WaitBarControl(MainWindowReference);
            CustomSampleTimeControl = new CustomSampleTimeControl(MainWindowReference);

            //Inital User Control 
            ContentArea.Content = SearchControl;

            //Timer Setup
            temperatureTimer.Tick += new EventHandler(temperatureTimer_Tick);
            temperatureTimer.Interval = new TimeSpan(0, 0, 4);

            //Tabs Setup
            Hide_Tabs();

            //Search for bluettoth devices
            scanBluetoothLE();
        }

        public void InitSchiChartLicense()
        {

            //Official SciChart Licence
            SciChartSurface.SetLicenseKey(@"<LicenseContract>
              <Customer>FABRICANTES DE BASCULAS TORREY SA DE CV</Customer>
              <OrderId>ABT140312-9978-69113</OrderId>
              <LicenseCount>1</LicenseCount>
              <IsTrialLicense>false</IsTrialLicense>
              <SupportExpires>06/10/2014 00:00:00</SupportExpires>
              <KeyCode>lwAAAAEAAAAiypc6LT7PAW0AQ3VzdG9tZXI9RkFCUklDQU5URVMgREUgQkFTQ1VMQVMgVE9SUkVZIFNBIERFIENWO09yZGVySWQ9QUJUMTQwMzEyLTk5NzgtNjkxMTM7U3Vic2NyaXB0aW9uVmFsaWRUbz0xMC1KdW4tMjAxNEqCHY8muBmm09p6fojCGm1q6JDeIudn7sJtS8Rm7RuT2J6qs2ekNI1yPn7UTeAupQ==</KeyCode>
            </LicenseContract>");

            /*
            //Extended Supprot SciChart License
            SciChartSurface.SetLicenseKey(@"<LicenseContract>
            <Customer>Jorge Garza, electronica2@fabatsa.com.mx</Customer>
            <OrderId>Trial Extension</OrderId>
            <LicenseCount>1</LicenseCount>
            <IsTrialLicense>true</IsTrialLicense>
            <SupportExpires>04/06/2014 00:00:00</SupportExpires>
            <KeyCode>tgIAABowKIjYOc8BAAAxJytRzwEeAGkAQ3VzdG9tZXI9Sm9yZ2UgR2FyemEsIGVsZWN0cm9uaWNhMkBmYWJhdHNhLmNvbS5teDtPcmRlcklkPVRyaWFsIEV4dGVuc2lvbjtTdWJzY3JpcHRpb25WYWxpZFRvPTA2LUFwci0yMDE0Zsn8ZnyRVOQCG7WlQLIpUC+pqy5w014I1xwi81/Q4ovdYjGu7TzXiNLcIrcPMF1k</KeyCode>
            </LicenseContract>");
             */
        }

        /* Bluegigia Api Override functions*/
        public class MyBgApi : BgApi
        {
            public MyBgApi(string comPort) : base(comPort) { }

            public override void bluetoothLeConnected(ble_msg_connection_status_evt_t deviceInfo)
            {

                MainWindow.conntectionNumber = deviceInfo.connection;

                //Function called from a thread, thus use Invoke
                MainWindowReference.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    //Enable Log Notifications
                    MainWindowReference.enableLogNotifications();

                    //Update device Address
                    SearchControl.updateDeviceAddress(MainWindow.deviceAddress);

                    //Change Window to Search Control
                    MainWindowReference.ContentArea.Content = SearchControl;
                    MainWindowReference.MessagesControl_Back();

                    //Get password
                    MainWindowReference.sendCommandCTBLE("" + CTBLE_PSS);

                    //Renew Some Flags
                    MainWindow.flagSearchButtonClick = false;
                    MainWindow.flagBleDeviceConnected = true;
                }));

            }

            public override void bluetoothLeDisconnected(ble_msg_connection_disconnected_evt_t deviceInfo)
            {
                //Function called from a thread, thus use Invoke
                MainWindowReference.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    //Release Semaphore and Timer
                    releaseBLE();

                    //Stop Wait Bar
                    MainWindowReference.WaitBarControl_Hide();

                    //Stop Temperature Timer
                    MainWindowReference.temperatureTimer.Stop();

                    if (MainWindow.flagInvalidPassword)
                    {
                        if (MainWindow.flagPasswordWasRetrievedFromDB == true)
                        {
                            //Erase from the registry, because it is an invalid password
                            try
                            {
                                Microsoft.Win32.RegistryKey Key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\TORREY\\CoolManager", true);
                                Key.DeleteValue(MainWindow.deviceAddress);
                                Key.Close();
                            }

                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }

                            //Ask for Password
                            MainWindowReference.ShowDialog_PwdForDeviceControl(); //Ask for password
                            MainWindow.flagPasswordWasRetrievedFromDB = false;
                        }
                        else
                        {
                            //Normal invalida password routine
                            MainWindowReference.MSG("ALERT", "Invalid Password!", MSGControl.OPTION.NOTHING);
                           
                        }
                                              
                    }
                    else if (MainWindow.flagSearchButtonClick)
                    {
                        Console.WriteLine("Disconnected by Search Button Click");
                    }
                    else if (MainWindow.flagInEraseEprom)
                    {
                        MainWindowReference.MSG(
                            "MESSAGE",
                            "Please RESTART the device after the count to 99.9 has finish and the temperature shows again.",
                            MSGControl.OPTION.WAIT_UNITL_FINISH);
                    }
                    else
                    {
                        MainWindowReference.MSG("DISCONNECTED!", "Could not establish connection", MSGControl.OPTION.BLE_DISCONNECT);
                    }

                    //Clear Scan List and Hide Tabs
                    DeviceListControl.clearScanList();
                    MainWindowReference.Hide_Tabs();

                    //Renew Flag states
                    MainWindow.flagExpectingOKMessage = false;
                    MainWindow.flagOnChangePassword = false;
                    MainWindow.flagInvalidPassword = false;
                    MainWindow.flagInSetSampleTime = false;
                    MainWindow.flagCTBLEisBlinking = false;
                    MainWindow.flagInGetFactorySettings = false;
                    MainWindow.flagInSetFactorySettings = false;
                    MainWindow.flagSearchButtonClick = false;
                    MainWindow.flagBleDeviceConnected = false;
                    MainWindow.flagRollBackSampleTime = false;
                    MainWindow.flagInEraseEprom = false;

                    //Update Help Control, Version Tab
                    HelpControl.updateControlVersion();
                    HelpControl.updateControlBLEVersion();
                }));
            }

            public override void bluetoothLeReceivedData(ble_msg_attclient_attribute_value_evt_t deviceInfo)
            {
                //Function called from a thread, thus use Invoke
                MainWindowReference.Dispatcher.BeginInvoke(new Action(delegate()
                {

                    string receivedMsg = System.Text.Encoding.Default.GetString(deviceInfo.value);

                    if (deviceInfo.atthandle == CT_BLE_TMP_UUID)
                    {
                        if (!receivedCommandCheckSum(receivedMsg))
                        {
                            Console.WriteLine("!!!Error, checksum error in MSG ");
                            return;
                        }

                        //Obtain Temperatures
                        MainWindow.uCF = (receivedMsg[0] == '1') ? true : false;
                        string auxData = obtainDataFromCommand(receivedMsg);
                        string[] splitData = auxData.Split('|');
                        MainWindow.fTc = float.Parse(splitData[0].Replace("\\s+", ""));
                        MainWindow.fTmd = float.Parse(splitData[1].Replace("\\s+", ""));
                        MainWindow.fTerm3 = float.Parse(splitData[2].Replace("\\s+", ""));

                        //Update Temperatures
                        TemperatureControl.updateDegrees(MainWindow.uCF);
                        TemperatureControl.updateChamberTemperature(MainWindow.fTc);
                        FactorySettingsControl.updateDegreeButton(MainWindow.uCF);
                        //RelaysControl.updateChamberTemperature(MainWindow.fTc);
                        //RelaysControl.updateEvaporatorTemperature(MainWindow.fTmd);
                        MainWindowReference.updateChamberTemperature(MainWindow.fTc);
                        MainWindowReference.updateEvaporatorTemperature(MainWindow.fTmd);

                        //Get Relay State
                        MainWindowReference.getRawRelaysCTBLE();
                    }
                    else if (deviceInfo.atthandle == CT_BLE_GRL_UUID)
                    {
                        if (!receivedCommandCheckSum(receivedMsg))
                        {
                            Console.WriteLine("!!!Error, checksum error in MSG ");
                            return;
                        }

                        //Obtain Relays States
                        MainWindow.relays = receivedMsg.Substring(0, receivedMsg.Length - 1);

                        //Update Relay States
                        TemperatureControl.updateControlRelays(MainWindow.relays);

                        //Obtain Input Vlotage
                        MainWindowReference.sendCommandCTBLE("" + CTBLE_VOLTAGE);
                        //Obtain Light Sensor
                        MainWindowReference.sendCommandCTBLE("" + CTBLE_LIGHTSENSOR);
                    }
                    else if (deviceInfo.atthandle == CT_BLE_VERSION_UUID)
                    { 

                        MainWindow.BLEVersion = (new Regex("[^a-zA-Z0-9. -]")).Replace(receivedMsg, "");

                        MainWindowReference.MSG("SUCCESS", "You are now connected!!", MSGControl.OPTION.CONNECTED);

                        //Register correct password for this address
                        Microsoft.Win32.RegistryKey key;
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\TORREY\\CoolManager");
                        key.SetValue(MainWindow.deviceAddress, MainWindow.userPassword);
                        key.Close();

                        //Set Blinking On
                        MainWindowReference.sendCommandCTBLE("" + CTBLE_BLK + ON);
                        MainWindow.flagCTBLEisBlinking = true;
                        //Get Temperature
                        MainWindowReference.getRawTemperatureCTBLE();
                        //Get Limite Inferior
                        MainWindowReference.sendCommandCTBLE("" + CTBLE_LISPC);
                        //Get Limite Superiror
                        MainWindowReference.sendCommandCTBLE("" + CTBLE_LSSPC);
                        //Get Set Point
                        MainWindowReference.sendCommandCTBLE("" + CTBLE_SPC);
                        //Start Temperature Timer
                        MainWindowReference.temperatureTimer.Start();
                        //Show Tabs, depends on DTC or LDTC control model
                        MainWindowReference.Show_Tabs(controlModel);
                        //Stop Wait Bar
                        MainWindowReference.WaitBarControl_Hide();
                        //Update Sample Time
                        MainWindowReference.rollBackSampleTime();

                    }
                    else if (deviceInfo.atthandle == CT_BLE_LOGS_UUID)
                    {
                        if (!flagInGetHistoricLogs)
                        {
                            Console.WriteLine("config, checksum error or flagInGetHistoricLogs false");
                            return;
                        }


                        if (receivedMsg.Length <= LOG_MAX_FRAME_LENGTH)
                        {
                            /* Its a Type Revision Frame */

                            byte[] frame = System.Text.Encoding.Default.GetBytes(receivedMsg);

                            /*
                            //Debug
                            foreach (var i in frame)
                            {
                                Console.WriteLine(String.Format("{0:X}", (int) i));
                            }
                            */

                            if (frame[0] == 0x30)
                            {
                                /* Data 0 (Frame Init). 0x30 -> This frame contains data */
                                MainWindowReference.acknowledgeLogDataRecived();

                                if (MainWindowReference.ContentWaitBar.Content == null)
                                {
                                    MainWindowReference.WaitBarControl_Show(true);
                                }

                            }
                            else if (frame[0] == 0x31)
                            {
                                /* Data 0 (Frame Init). 0x31 -> This frame contains data,
                                 *  and its de end of historic values */
                                flagInGetHistoricLogs = false;
                                MainWindowReference.acknowledgeLogDataRecived();
                                Console.WriteLine("Historic Lecture finish");

                                //Stop Progres Dialog
                                MainWindowReference.WaitBarControl_Hide();
                                MainWindowReference.MSG("MESSAGE", "Data has been updated!", MSGControl.OPTION.NOTHING);
                                //Show Chart
                                LogsControl.startChart(LogsControl.currentChartTime, LogsControl.currentRelay, MainWindow.deviceAddressFile);

                            }
                            else if (frame[0] == 0x32)
                            {
                                /* Data 0 (Frame Init). 0x32 -> There is no more data to be 
                                 * recieved from the CTBLE Controller */
                                flagInGetHistoricLogs = false;
                                MainWindowReference.MSG("MESSAGE", "Data is up-to-date! There is no more recent data" +
                                    " avilable from the connected device.", MSGControl.OPTION.NOTHING);
                                return;
                            }
                            else
                            {
                                Console.WriteLine("LOG DATA, ERROR. FRAME (DATA 0)" +
                                        " IS INVALID FOR VALUE = 0x" + string.Format("%H", frame[0]));
                                return;
                            }

                            /* Check Frame Type */
                            if ((frame[1] & 0x80) == 0x00)
                            {
                                /* Its a Type Revision Frame */

                                if (receivedMsg.Length < 8)
                                {
                                    Console.WriteLine("Frame data, unknown error, Frame =" + String.Format("0x{0:X2} ", (int)frame[1]));
                                    return;
                                }

                                /* Data 1 (Type Logger & Timer Logs)*/
                                logTimerLogs = (int)(frame[1] & 0x3F);
                                /* Data 2 (Year)*/
                                string logYear = Convert.ToString(frame[2]);
                                /* Data 3 (Month)*/
                                string logMonth = Convert.ToString(frame[3]);
                                /* Data 4 (Day)*/
                                string logDay = Convert.ToString(frame[4]);
                                /* Data 5 (Hour)*/
                                string logHour = Convert.ToString(frame[5]);
                                /* Data 6 (Minute)*/
                                string logMinute = Convert.ToString(frame[6]);
                                /* Data 7 (Units & Status)*/
                                string logUnits = Convert.ToString((byte)(((frame[7] & 0x40) >> 6)));
                                string logStatus = Convert.ToString((byte)((frame[7] & 0x1F)));
                                /* Data 8 (Temperature Index)*/
                                string logIndex = Convert.ToString((byte)((frame[8] & 0xFF)));

                                /* Obtain Time */
                                string recievedDate = logYear + "-" + logMonth + "-" + logDay + "-" + logHour + "-" + logMinute;

                                if (!(DateTime.TryParseExact(recievedDate, "y-M-d-H-m", null, DateTimeStyles.None, out logDate)))
                                {
                                    Console.WriteLine("Fatal Error, Invalid Date & Time format ");
                                    return;
                                }

                                String revisionDate = logDate.ToString("yy-MM-dd-HH-mm", CultureInfo.InvariantCulture);
                                String formatedLogData = revisionDate + "-" + logUnits + "-" + logStatus + "-" + logIndex;
                                Console.WriteLine("Log, Revision Data = " + formatedLogData);

                                /* Update data in Wait Bar and reset Time Out counter*/
                                WaitBarControl.updateReceviedData(formatedLogData);
                                /* Save Data */
                                LogsControl.writeData(formatedLogData, MainWindow.deviceAddressFile);

                            }
                            else
                            {
                                /* Its a Type Logger Frame */

                                for (int i = 1; i < (receivedMsg.Length - 2); i += 2)
                                {
                                    /* Data 1 (Type Logger, Units & Status)*/
                                    string logUnits = Convert.ToString(((frame[i] & 0x40) >> 6));
                                    string logStatus = Convert.ToString((frame[i] & 0x1F));
                                    /* Data 8 (Temperature Index)*/
                                    string logIndex = Convert.ToString((frame[i + 1] & 0xFF));

                                    /* Obtain Time */
                                    if (logDate != null)
                                    {
                                        logDate = logDate.AddMinutes(logTimerLogs); // adds logTimerLog minutes
                                        string loggerDate = logDate.ToString("yy-MM-dd-HH-mm", CultureInfo.InvariantCulture);
                                        string formatedLogData = loggerDate + "-" + logUnits + "-" + logStatus + "-" + logIndex;
                                        Console.WriteLine(" - Log, Time Added = "+ logTimerLogs + "Logger Data = " + formatedLogData);

                                        /* Update data in Wait Bar and reset Time Out counter */
                                        WaitBarControl.updateReceviedData(formatedLogData);
                                        /* Save Data */
                                        LogsControl.writeData(formatedLogData, MainWindow.deviceAddressFile);

                                    }
                                    else
                                    {
                                        Console.WriteLine("Log, Error logDate null");
                                    }
                                }
                            }

                        }
                        else
                        {
                            Console.WriteLine("LOG DATA, FRAME LENGTH ERROR");
                        }
                    }
                    else if (deviceInfo.atthandle == CT_BLE_CONFIG_UUID)
                    {
                        if (!receivedCommandCheckSum(receivedMsg))
                        {
                            Console.WriteLine("!!!Error, checksum error in MSG ");
                            return;
                        }

                        switch (receivedMsg[0])
                        {

                            /* 
                             *  Password
                             */
                            case CTBLE_PSS:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (auxData.Equals("0"))
                                    {
                                        MainWindowReference.sendCommandCTBLE("" + CTBLE_PSS);
                                    }
                                    else if (auxData.Equals("1"))
                                    {
                                        Console.WriteLine("!!! Error, CTBLE_PSS");
                                    }
                                    else
                                    {
                                        MainWindow.ctblePassword = auxData;

                                        if (flagOnChangePassword)
                                        {
                                            MainWindowReference.MSG("MESSAGE", "Changes Saved!", MSGControl.OPTION.NOTHING);
                                            flagOnChangePassword = false;
                                        }
                                        else
                                        {
                                            if (userPassword.Equals(ctblePassword))
                                            {
                                                //Get Control Type
                                                MainWindowReference.sendCommandCTBLE("" + CTBLE_TPC);
                                            }
                                            else if (userPassword.Equals(universalPassword))
                                            {
                                                ChangesControl.updateDevicePassword(MainWindow.ctblePassword);
                                                //Get Control Type
                                                MainWindowReference.sendCommandCTBLE("" + CTBLE_TPC);
                                            }
                                            else
                                            {
                                                //Invalid password, disconnect device ...
                                                flagInvalidPassword = true;
                                                MainWindowReference.disconnectDevice();
                                            }
                                        }
                                    }

                                }
                                break;

                            /* 
                             *  Control Type
                             */
                            case CTBLE_TPC:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    TypeControl = auxData[0];
                                    FirmwareVersion = auxData.Substring(5).Replace("\\s+", "");

                                    /* Add rest of the tabs depending on Control Model */
                                    if (TypeControl == '4' || TypeControl == '5' || TypeControl == '6')
                                    {
                                        controlModel = MODEL_LDTC;
                                        TypeControl = (char)(TypeControl - 3);
                                    }
                                    else
                                    {
                                        controlModel = MODEL_DTC;
                                    }

                                    TemperatureControl.updateTypeControl(MainWindow.deviceName);
                                    MainWindowReference.updateTypeControlName(MainWindow.deviceName);
                                    SearchControl.updateDeviceName(MainWindow.deviceName);
                                    ChangesControl.updateDeviceName(MainWindow.deviceName);

                                    if (controlModel == MODEL_DTC)
                                    {
                                        // Send Calendar Time
                                        DateTime date = DateTime.Now;
                                        string dayAndTime = date.ToString("yyMMddHHmmss", CultureInfo.InvariantCulture);
                                        MainWindowReference.sendCommandCTBLE("" + CTBLE_SETTIME + dayAndTime);
                                    }
                                    else
                                    {
                                        // Read uHF Filter State
                                        MainWindowReference.sendCommandCTBLE("" + CTBLE_HFD);
                                        // Read BLE Firmware
                                        MainWindowReference.getBLEVersion();
                                    }

                                }
                                break;

                            /* 
                             *  Time
                             */
                            case CTBLE_SETTIME:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (auxData.Equals("0"))
                                    {
                                        // Read Sample Time
                                        MainWindowReference.sendCommandCTBLE("" + CTBLE_TLOGGER);
                                    }
                                }
                                break;

                            /* 
                             *  Logger Sample Time
                             */
                            case CTBLE_TLOGGER:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (auxData.Equals("0"))
                                    {
                                        MainWindowReference.sendCommandCTBLE("" + CTBLE_TLOGGER);
                                    }
                                    else
                                    {
                                        MainWindow.sampleTime = Convert.ToInt32(auxData);
                                        if (flagInSetSampleTime)
                                        {
                                            MainWindowReference.MSG("MESSAGE", "Your change was successful", MSGControl.OPTION.NOTHING);

                                            //Update Sample Time
                                            LogsControl.updateSampleTime(MainWindow.sampleTime);

                                            flagInSetSampleTime = false;
                                        }
                                        else
                                        {
                                            // Read uHF Filter State
                                            MainWindowReference.sendCommandCTBLE("" + CTBLE_HFD);
                                            // Read BLE Firmware
                                            MainWindowReference.getBLEVersion();
                                        }


                                    }
                                }
                                break;

                            /* 
                             *  Blink
                             */
                            case CTBLE_BLK:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (auxData.Equals("0"))
                                    {
                                    }
                                }
                                break;

                            /* 
                             *  Control Mode
                             */
                            case CTBLE_ECOMODE:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (auxData.Equals("0"))
                                    {
                                        MainWindowReference.getRawRelaysCTBLE();
                                    }
                                }
                                break;

                            /* 
                             *  Control Unit Type
                             */
                            case CTBLE_CUT:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (auxData.Equals("0"))
                                    {
                                        if (MainWindow.flagInSetFactorySettings == true)
                                        {
                                            //Get Temprerature
                                            MainWindowReference.getRawTemperatureCTBLE();
                                            //Get Factory Settings Values
                                            MainWindowReference.getFactorySettings();
                                        }
                                        else
                                        {
                                            //Get Temprerature
                                            MainWindowReference.getRawTemperatureCTBLE();
                                            //Get Limite Inferior
                                            MainWindowReference.sendCommandCTBLE("" + CTBLE_LISPC);
                                            //Get Limite Superiror
                                            MainWindowReference.sendCommandCTBLE("" + CTBLE_LSSPC);
                                            //Get Set Point
                                            MainWindowReference.sendCommandCTBLE("" + CTBLE_SPC);
                                        }
                                    }
                                }
                                break;

                            /* 
                             *  Ralays
                             */
                            case CTBLE_CFRV:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (auxData.Equals("0"))
                                    {
                                        RelaysControl.updateServiceRelays();
                                    }
                                }
                                break;

                            /* 
                             *  Set Point
                             */
                            case CTBLE_SPC:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (auxData.Equals("0"))
                                    {
                                        MainWindowReference.sendCommandCTBLE("" + CTBLE_SPC);
                                        if (flagInSetFactorySettings)
                                        {
                                            flagInSetFactorySettings = false;
                                        }

                                        MainWindowReference.MSG("MESSAGE", "Changes Saved!", MSGControl.OPTION.NOTHING);
                                    }
                                    else if (auxData.Equals("1"))
                                    {
                                        Console.WriteLine("!!! Error, CTBLE_SPC");
                                    }
                                    else
                                    {
                                        MainWindow.fSpc = Double.Parse(auxData);
                                        TemperatureControl.renwSetPoint();
                                    }
                                }
                                break;

                            /* 
                             *  Set Point, Limite Inferior
                             */
                            case CTBLE_LISPC:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (auxData.Equals("0"))
                                    {
                                        if (flagInSetFactorySettings)
                                        {
                                            flagInSetFactorySettings = false;
                                            MainWindowReference.MSG("MESSAGE", "Changes Saved!", MSGControl.OPTION.NOTHING);
                                        }
                                    }
                                    else if (auxData.Equals("1"))
                                    {
                                        Console.WriteLine("!!! Error, CTBLE_LISPC");
                                    }
                                    else
                                    {
                                        MainWindow.fLi_Spc = Double.Parse(auxData);
                                    }
                                }
                                break;

                            /* 
                             *  Set Point, Limite Superiro
                             */
                            case CTBLE_LSSPC:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (auxData.Equals("0"))
                                    {
                                        if (flagInSetFactorySettings)
                                        {
                                            flagInSetFactorySettings = false;
                                            MainWindowReference.MSG("MESSAGE", "Changes Saved!", MSGControl.OPTION.NOTHING);
                                        }
                                    }
                                    else if (auxData.Equals("1"))
                                    {
                                        Console.WriteLine("!!! Error, CTBLE_LSSPC");
                                    }
                                    else
                                    {
                                        MainWindow.fLs_Spc = Double.Parse(auxData);
                                    }
                                }
                                break;

                            case CTBLE_SID:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (flagInGetFactorySettings)
                                    {
                                        MainWindow.iSid = Convert.ToInt32(auxData);
                                    }
                                    else
                                    {
                                        if (auxData.Equals("0"))
                                        {
                                            if (flagInSetFactorySettings)
                                            {
                                                flagInSetFactorySettings = false;
                                                MainWindowReference.MSG("MESSAGE", "Changes Saved!", MSGControl.OPTION.NOTHING);
                                            }
                                        }
                                    }
                                }
                                break;

                            case CTBLE_STD:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (flagInGetFactorySettings)
                                    {
                                        MainWindow.iStd = Convert.ToInt32(auxData);
                                    }
                                    else
                                    {
                                        if (auxData.Equals("0"))
                                        {
                                            if (flagInSetFactorySettings)
                                            {
                                                flagInSetFactorySettings = false;
                                                MainWindowReference.MSG("MESSAGE", "Changes Saved!", MSGControl.OPTION.NOTHING);
                                            }
                                        }
                                    }
                                }
                                break;

                            case CTBLE_SDIF:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (flagInGetFactorySettings)
                                    {
                                        MainWindow.iSdif = Convert.ToInt32(auxData);
                                    }
                                    else
                                    {
                                        if (auxData.Equals("0"))
                                        {
                                            if (flagInSetFactorySettings)
                                            {
                                                flagInSetFactorySettings = false;
                                                MainWindowReference.MSG("MESSAGE", "Changes Saved!", MSGControl.OPTION.NOTHING);
                                            }
                                        }
                                    }
                                }
                                break;

                            case CTBLE_tDI:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (flagInGetFactorySettings)
                                    {
                                        MainWindow.iTdi = Convert.ToInt32(auxData);
                                    }
                                    else
                                    {
                                        if (auxData.Equals("0"))
                                        {
                                            if (flagInSetFactorySettings)
                                            {
                                                flagInSetFactorySettings = false;
                                                MainWindowReference.MSG("MESSAGE", "Changes Saved!", MSGControl.OPTION.NOTHING);
                                            }
                                        }
                                    }
                                }
                                break;

                            case CTBLE_tES0:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (flagInGetFactorySettings)
                                    {
                                        MainWindow.iTeso = Convert.ToInt32(auxData);
                                    }
                                    else
                                    {
                                        if (auxData.Equals("0"))
                                        {
                                            if (flagInSetFactorySettings)
                                            {
                                                flagInSetFactorySettings = false;
                                                MainWindowReference.MSG("MESSAGE", "Changes Saved!", MSGControl.OPTION.NOTHING);
                                            }
                                        }
                                    }
                                }
                                break;

                            case CTBLE_tDUD:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (flagInGetFactorySettings)
                                    {
                                        MainWindow.iTdud = Convert.ToInt32(auxData);
                                    }
                                    else
                                    {
                                        if (auxData.Equals("0"))
                                        {
                                            if (flagInSetFactorySettings)
                                            {
                                                flagInSetFactorySettings = false;
                                                MainWindowReference.MSG("MESSAGE", "Changes Saved!", MSGControl.OPTION.NOTHING);
                                            }
                                        }
                                    }
                                }
                                break;

                            case CTBLE_tDESH:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (flagInGetFactorySettings)
                                    {
                                        MainWindow.iTdesh = Convert.ToInt32(auxData);
                                    }
                                    else
                                    {
                                        if (auxData.Equals("0"))
                                        {
                                            if (flagInSetFactorySettings)
                                            {
                                                flagInSetFactorySettings = false;
                                                MainWindowReference.MSG("MESSAGE", "Changes Saved!", MSGControl.OPTION.NOTHING);
                                            }
                                        }
                                    }
                                }
                                break;

                            case CTBLE_tRAD:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (flagInGetFactorySettings)
                                    {
                                        MainWindow.iTrad = Convert.ToInt32(auxData);
                                    }
                                    else
                                    {
                                        if (auxData.Equals("0"))
                                        {
                                            if (flagInSetFactorySettings)
                                            {
                                                flagInSetFactorySettings = false;
                                                MainWindowReference.MSG("MESSAGE", "Changes Saved!", MSGControl.OPTION.NOTHING);
                                            }
                                        }
                                    }
                                }
                                break;

                            case CTBLE_tTCC:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (flagInGetFactorySettings)
                                    {
                                        MainWindow.iTtcc = Convert.ToInt32(auxData);
                                    }
                                    else
                                    {
                                        if (auxData.Equals("0"))
                                        {
                                            if (flagInSetFactorySettings)
                                            {
                                                flagInSetFactorySettings = false;
                                                MainWindowReference.MSG("MESSAGE", "Changes Saved!", MSGControl.OPTION.NOTHING);
                                            }
                                        }
                                    }
                                }
                                break;

                            case CTBLE_SPSL:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (flagInGetFactorySettings)
                                    {
                                        MainWindow.iSpsl = Convert.ToInt32(auxData);
                                    }
                                    else
                                    {
                                        if (auxData.Equals("0"))
                                        {
                                            if (flagInSetFactorySettings)
                                            {
                                                flagInSetFactorySettings = false;
                                                MainWindowReference.MSG("MESSAGE", "Changes Saved!", MSGControl.OPTION.NOTHING);
                                            }
                                        }
                                    }
                                }
                                break;

                            case CTBLE_CALMV:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (flagInGetFactorySettings)
                                    {
                                        MainWindow.iCalmv = Convert.ToInt32(auxData);
                                    }
                                    else
                                    {
                                        if (auxData.Equals("0"))
                                        {
                                            if (flagInSetFactorySettings)
                                            {
                                                flagInSetFactorySettings = false;
                                                MainWindowReference.MSG("MESSAGE", "Changes Saved!", MSGControl.OPTION.NOTHING);
                                            }
                                        }
                                    }
                                }
                                break;

                            case CTBLE_tDESH2:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (flagInGetFactorySettings)
                                    {
                                        MainWindow.iTdesh2 = Convert.ToInt32(auxData);
                                    }
                                    else
                                    {
                                        if (auxData.Equals("0"))
                                        {
                                            if (flagInSetFactorySettings)
                                            {
                                                flagInSetFactorySettings = false;
                                                MainWindowReference.MSG("MESSAGE", "Changes Saved!", MSGControl.OPTION.NOTHING);
                                            }
                                        }
                                    }
                                }
                                break;

                            case CTBLE_CDESH:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (flagInGetFactorySettings)
                                    {
                                        MainWindow.uCdesh = Convert.ToInt32(auxData);
                                    }
                                    else
                                    {
                                        if (auxData.Equals("0"))
                                        {
                                            if (flagInSetFactorySettings)
                                            {
                                                flagInSetFactorySettings = false;
                                                MainWindowReference.MSG("MESSAGE", "Changes Saved!", MSGControl.OPTION.NOTHING);
                                            }
                                        }
                                    }
                                }
                                break;

                            case CTBLE_TMAX:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (flagInGetFactorySettings)
                                    {
                                        MainWindow.iTmax = Convert.ToInt32(auxData);
                                    }
                                    else
                                    {
                                        if (auxData.Equals("0"))
                                        {
                                            if (flagInSetFactorySettings)
                                            {
                                                flagInSetFactorySettings = false;
                                                MainWindowReference.MSG("MESSAGE", "Changes Saved!", MSGControl.OPTION.NOTHING);
                                            }
                                        }
                                    }
                                }
                                break;

                            case CTBLE_tDUDM:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (flagInGetFactorySettings)
                                    {
                                        MainWindow.iTDUDM = Convert.ToInt32(auxData);
                                    }
                                    else
                                    {
                                        if (auxData.Equals("0"))
                                        {
                                            if (flagInSetFactorySettings)
                                            {
                                                flagInSetFactorySettings = false;
                                                MainWindowReference.MSG("MESSAGE", "Changes Saved!", MSGControl.OPTION.NOTHING);
                                            }
                                        }
                                    }
                                }
                                break;

                            case CTBLE_tUD:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (flagInGetFactorySettings)
                                    {
                                        MainWindow.iTud = Convert.ToInt32(auxData);
                                    }
                                    else
                                    {
                                        if (auxData.Equals("0"))
                                        {
                                            if (flagInSetFactorySettings)
                                            {
                                                flagInSetFactorySettings = false;
                                                MainWindowReference.MSG("MESSAGE", "Changes Saved!", MSGControl.OPTION.NOTHING);
                                            }
                                        }
                                    }
                                }
                                break;

                            case CTBLE_Z:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (flagInGetFactorySettings)
                                    {
                                        MainWindow.fz = Convert.ToDouble(auxData);
                                    }
                                    else
                                    {
                                        if (auxData.Equals("0"))
                                        {
                                            if (flagInSetFactorySettings)
                                            {
                                                flagInSetFactorySettings = false;
                                                MainWindowReference.MSG("MESSAGE", "Changes Saved!", MSGControl.OPTION.NOTHING);
                                            }
                                        }
                                    }
                                }
                                break;

                            case CTBLE_tGOTEO:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (flagInGetFactorySettings)
                                    {
                                        MainWindow.iTgoteo = Convert.ToInt32(auxData);
                                    }
                                    else
                                    {
                                        if (auxData.Equals("0"))
                                        {
                                            if (flagInSetFactorySettings)
                                            {
                                                flagInSetFactorySettings = false;
                                                MainWindowReference.MSG("MESSAGE", "Changes Saved!", MSGControl.OPTION.NOTHING);
                                            }
                                        }
                                    }
                                }
                                break;


                            case CTBLE_BTN:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);

                                    if (flagExpectingOKMessage)
                                    {
                                        if (auxData.Equals("0"))
                                        {
                                            MainWindowReference.sendCommandCTBLE("" + CTBLE_BTN);
                                        }
                                        else if (auxData.Equals("1"))
                                        {
                                            Console.WriteLine("!!! Error, CTBLE_BTN");
                                        }
                                        else if (auxData.Equals("s0"))
                                        {
                                            MainWindow.uSPBtnEnable = false;
                                            MainWindowReference.MSG("SP BUTTONS", "The external Setpoint Buttons has been disabled", MSGControl.OPTION.NOTHING);
                                            flagExpectingOKMessage = false;
                                        }
                                        else if (auxData.Equals("s1"))
                                        {
                                            MainWindow.uSPBtnEnable = true;
                                            MainWindowReference.MSG("SP BUTTONS", "The external Setpoint Buttons has been enabled", MSGControl.OPTION.NOTHING);
                                            flagExpectingOKMessage = false;
                                        }
                                        else
                                        {
                                            Console.WriteLine("!!! Error, CTBLE_BTN, MSG = " + auxData);
                                        }
                                        
                                    }
                                    else
                                    {
                                        if (auxData.Equals("0"))
                                        {
                                            MainWindow.uSPBtnEnable = false;
                                        }
                                        else
                                        {
                                            MainWindow.uSPBtnEnable = true;
                                        }
                                        

                                       
                                    }
                                    if (MainWindow.uSPBtnEnable) SettingsControl.SPEnableButton.Content = "Disable SP Buttons";
                                    else SettingsControl.SPEnableButton.Content = "Enable SP Buttons";
                                }
                                break;

                            /* 
                             *  Filter
                             */
                            case CTBLE_HFD:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);

                                    if (flagExpectingOKMessage)
                                    {
                                        if (auxData.Equals("0"))
                                        {
                                            MainWindowReference.sendCommandCTBLE("" + CTBLE_HFD);
                                        }
                                        else if (auxData.Equals("1"))
                                        {
                                            Console.WriteLine("!!! Error, CTBLE_HFD");
                                        }
                                        else
                                        {
                                            Console.WriteLine("!!! Error, CTBLE_HFD, MSG = " + auxData);
                                        }
                                        flagExpectingOKMessage = false;
                                    }
                                    else
                                    {
                                        if (auxData.Equals("0"))
                                        {
                                            MainWindow.uHFD = false;
                                        }
                                        else
                                        {
                                            MainWindow.uHFD = true;
                                        }
                                        RelaysControl.updateuHDFButton();
                                        FactorySettingsControl.updateuHDFButton();

                                        //Get SPButton status
                                        MainWindowReference.sendCommandCTBLE("" + CTBLE_BTN);
                                    }

                                    if (flagInGetFactorySettings)
                                    {
                                        flagInGetFactorySettings = false;
                                        //Update Factry Settings Values
                                        FactorySettingsControl.updateFactorySettings();
                                        //Hide WaitBar
                                        MainWindowReference.WaitBarControl_Hide();
                                        //Message
                                        MainWindowReference.MSG("MESSAGE", "Factory Setting Values Updated!", MSGControl.OPTION.NOTHING);
                                    }
                                }
                                break;

                            /* 
                             *  Factory Reset
                             */
                            case CTBLE_RSF:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (auxData.Equals("0"))
                                    {
                                    }
                                    else
                                    {
                                        Console.WriteLine("!!! Error, CTBLE_RSF");
                                    }
                                }
                                break;

                            /* 
                             * Historic Logs Commands
                             */
                            case CTBLE_SETLOG:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (auxData.Equals("0"))
                                    {
                                        MainWindowReference.activateLogLecture();
                                        flagInGetHistoricLogs = true;
                                    }
                                    break;
                                }

                            /* 
                             * Name Change
                             */
                            case CTBLE_NAME:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (auxData.Equals("0"))
                                    {
                                        MainWindowReference.sendCommandCTBLE("" + CTBLE_NAME);
                                    }
                                    else if (auxData.Equals("1"))
                                    {
                                        Console.WriteLine("!!! Error, CTBLE_NAME");
                                    }
                                    else
                                    {
                                        MainWindow.deviceName = auxData;

                                        TemperatureControl.updateTypeControl(MainWindow.deviceName);
                                        MainWindowReference.updateTypeControlName(MainWindow.deviceName);
                                        SearchControl.updateDeviceName(MainWindow.deviceName);
                                        ChangesControl.updateDeviceName(MainWindow.deviceName);

                                        MainWindowReference.MSG(
                                            "MESSAGE",
                                            "New device name \"" + auxData + "\" saved!",
                                            MSGControl.OPTION.NOTHING);
                                    }
                                }
                                break;

                            /* 
                             * Input Voltage Counts
                             */
                            case CTBLE_VOLTAGE:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    string[] auxArray = auxData.Split('-');
                                    RelaysControl.updateInputVoltage(auxArray[0].Replace("\\s+", ""));
                                }
                                break;

                            /* 
                             * Input Light Sensor Counts
                             */
                            case CTBLE_LIGHTSENSOR:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    RelaysControl.updateLightSensor(auxData.Replace("\\s+", ""));
                                }
                                break;

                            /*
                             * Erease EEprom
                             */
                            case CTBLE_DEL:
                                {
                                    string auxData = obtainDataFromCommand(receivedMsg);
                                    if (auxData.Equals("0"))
                                    {
                                        MainWindow.flagInEraseEprom = true;
                                        LogsControl.eraseLogsFile(MainWindow.deviceAddressFile);
                                        MainWindowReference.disconnectDevice();
                                    }
                                }
                                break;
                            
                            /*
                             * Test Mode
                             */
                            case CTBLE_TESTMODE:
                                {
                                }
                                break;
                            

                            default:
                                Console.WriteLine("!!!Error, Unknow Command = " + receivedMsg);
                                break;
                        }
                    }

                }));
            }

            public override void bluetoothLeTimeExceed()
            {
                //Function called from a thread, thus use Invoke
                MainWindowReference.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    //Time exceed in command, disconnect
                    MainWindowReference.disconnectDevice();
                }));
            }

        }

        public static bool receivedCommandCheckSum(string command)
        {
            /* Checks the checksum of the received data */

            if (command.Length < 3)
            {
                Console.WriteLine("!!!Error, Command length invalid");
                return false;
            }

            char datachecksum = command[command.Length - 1];
            char realchecksum = checkSum(command.Substring(0, command.Length - 1));

            if (datachecksum != realchecksum)
            {
                return false;
            }

            return true;
        }

        public static char checkSum(string pT)
        {
            /* Checksum of the data that is going to be sent to the CTBLE */
            int c = 2;
            int k = 0;
            long s = 0;
            long r = 0;
            char temp = '\0';

            for (k = 0; k < pT.Length; k++)
            {
                temp = pT[k];
                r = temp * c;			//el valor de la posición por el contador de posición
                s += r;				//se suma los valores
                c++;					//se incrementa la posición
                if (c == 9) c = 2;		//9 es el máximo contador de posición
            }

            r = s * 10;				// mod 11  residuo               
            r = r % 11;
            r = r + 48;				//solo caracteres visibles

            return (char)r;
        }

        public static string obtainDataFromCommand(string command)
        {
            /* Obtains the data from the command chain (<command letter><data><checksum>) */
            String data = command.Substring(1, command.Length - 2);
            return data;
        }

        /** 
         * Tabs Selection
         */

        private void Search_Click(object sender, MouseButtonEventArgs e)
        {
            title.Content = "Devices";
            ContentArea.Content = SearchControl;

            LineSearch.Visibility = Visibility.Visible;
            LineTemperature.Visibility = Visibility.Hidden;
            LineLogs.Visibility = Visibility.Hidden;
            LineSettings.Visibility = Visibility.Hidden;
            LineServices.Visibility = Visibility.Hidden;
            //LineDisplay.Visibility = Visibility.Hidden;
        }

        
        public void GotoTemperatureScreen()
        {
            MainWindowReference.title.Content = "Temperature";
            MainWindowReference.ContentArea.Content = TemperatureControl;

            MainWindowReference.LineSearch.Visibility = Visibility.Hidden;
            MainWindowReference.LineTemperature.Visibility = Visibility.Visible;
            MainWindowReference.LineLogs.Visibility = Visibility.Hidden;
            MainWindowReference.LineSettings.Visibility = Visibility.Hidden;
            MainWindowReference.LineServices.Visibility = Visibility.Hidden;
            //LineDisplay.Visibility = Visibility.Hidden;

            if (MainWindow.flagCTBLEisBlinking)
            {
                MainWindowReference.sendCommandCTBLE("" + CTBLE_BLK + '2');
                MainWindow.flagCTBLEisBlinking = false;
            }
            
        }

        private void Temperature_Click(object sender, MouseButtonEventArgs e)
        {
            GotoTemperatureScreen();
        }

        private void Logs_Click(object sender, MouseButtonEventArgs e)
        {

            if (controlModel == MODEL_DTC)
            {
                title.Content = "Logs";
                ContentArea.Content = LogsControl;
            }
            else
            {
                // Cambiar el contenido del Area al de Servicios en caso del LDTC
                title.Content = "Services";
                ContentArea.Content = ServicesControl;
            }
            

            LineSearch.Visibility = Visibility.Hidden;
            LineTemperature.Visibility = Visibility.Hidden;
            LineLogs.Visibility = Visibility.Visible;
            LineSettings.Visibility = Visibility.Hidden;
            LineServices.Visibility = Visibility.Hidden;
            //LineDisplay.Visibility = Visibility.Hidden;

            if (MainWindow.flagCTBLEisBlinking)
            {
                MainWindowReference.sendCommandCTBLE("" + CTBLE_BLK + '2');
                MainWindow.flagCTBLEisBlinking = false;
            }

            LogsControl.tabLogs_Selected();
        }

        private void Services_Click(object sender, MouseButtonEventArgs e)
        {
            title.Content = "Service";
            ContentArea.Content = ServicesControl;

            LineSearch.Visibility = Visibility.Hidden;
            LineTemperature.Visibility = Visibility.Hidden;
            LineLogs.Visibility = Visibility.Hidden;
            LineSettings.Visibility = Visibility.Hidden;
            LineServices.Visibility = Visibility.Visible;
            //LineDisplay.Visibility = Visibility.Hidden;

            if (MainWindow.flagCTBLEisBlinking)
            {
                MainWindowReference.sendCommandCTBLE("" + CTBLE_BLK + '2');
                MainWindow.flagCTBLEisBlinking = false;
            }
        }

        private void Settings_Click(object sender, MouseButtonEventArgs e)
        {
            title.Content = "Settings";
            ContentArea.Content = SettingsControl;

            LineSearch.Visibility = Visibility.Hidden;
            LineTemperature.Visibility = Visibility.Hidden;
            LineLogs.Visibility = Visibility.Hidden;
            LineServices.Visibility = Visibility.Hidden;
            LineSettings.Visibility = Visibility.Visible;
            //LineDisplay.Visibility = Visibility.Hidden;

            if (MainWindow.flagCTBLEisBlinking)
            {
                MainWindowReference.sendCommandCTBLE("" + CTBLE_BLK + '2');
                MainWindow.flagCTBLEisBlinking = false;
            }
        }

        private void Display_Click(object sender, MouseButtonEventArgs e)
        {
            title.Content = "Display";

            LineSearch.Visibility = Visibility.Hidden;
            LineTemperature.Visibility = Visibility.Hidden;
            LineLogs.Visibility = Visibility.Hidden;
            LineServices.Visibility = Visibility.Hidden;
            //LineDisplay.Visibility = Visibility.Visible;

            if (MainWindow.flagCTBLEisBlinking)
            {
                MainWindowReference.sendCommandCTBLE("" + CTBLE_BLK + '2');
                MainWindow.flagCTBLEisBlinking = false;
            }
        }

        private void HelpIcon_Click(object sender, MouseButtonEventArgs e)
        {
            ShowDialgo_HelpControl();    
        }

        public void Hide_Tabs()
        {
            MainWindowReference.ContentArea.Content = SearchControl;

            LineSearch.Visibility = Visibility.Visible;
            LineTemperature.Visibility = Visibility.Hidden;
            LineLogs.Visibility = Visibility.Hidden;
            LineServices.Visibility = Visibility.Hidden;
            LineSettings.Visibility = Visibility.Hidden;
            //LineDisplay.Visibility = Visibility.Hidden;

            GridTemperature.Visibility = Visibility.Hidden;
            GridLogs.Visibility = Visibility.Hidden;
            GridServices.Visibility = Visibility.Hidden;
            GridSettings.Visibility = Visibility.Hidden;
            //GridDisplay.Visibility = Visibility.Hidden;

            ConnectionDataLayer.Visibility = Visibility.Hidden;

            SearchControl.Hide_GridConnectionInfo();
        }

        public void Show_Tabs(bool controlModelType)
        {
            MainWindowReference.ContentArea.Content = SearchControl;

            LineSearch.Visibility = Visibility.Visible;
            LineTemperature.Visibility = Visibility.Hidden;
            LineLogs.Visibility = Visibility.Hidden;
            LineServices.Visibility = Visibility.Hidden;
            //LineDisplay.Visibility = Visibility.Hidden;

            if (controlModelType == MODEL_DTC)
            {
                GridTemperature.Visibility = Visibility.Visible;
                GridLogs.Visibility = Visibility.Visible;
                GridServices.Visibility = Visibility.Visible;
                GridSettings.Visibility = Visibility.Visible;
                GridLogsText.Content = "Logs";
                //GridDisplay.Visibility = Visibility.Visible;
            }
            else
            {
                // Cambiar el Texto del boton de Logs por el de Service
                GridTemperature.Visibility = Visibility.Visible;
                GridLogs.Visibility = Visibility.Visible;
                GridSettings.Visibility = Visibility.Visible;
                GridLogsText.Content = "Service";
                //GridDisplay.Visibility = Visibility.Visible;
            }

            ConnectionDataLayer.Visibility = Visibility.Visible;

            SearchControl.Show_GridConnectionInfo();
        }

        /**
         * MainWindow Updates
         */

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

        public void updateTypeControlName(string deviceName)
        {
            /* Updates the type of control to CTI or CTF, enabling or not the EchoMode */
            if (MainWindow.TypeControl == '1')
            {
                /* CTI Mode, Show Echo Mode */
                if (deviceName != null)
                    ValueDeviceName.Content = deviceName + " - DTCR";
                else
                    ValueDeviceName.Content = "(no name) - DCTR";
            }
            else if (MainWindow.TypeControl == '2')
            {
                /* CTF Mode, Hide Echo Mode  */
                if (deviceName != null)
                    ValueDeviceName.Content = deviceName + " - DTCF";
                else
                    ValueDeviceName.Content = "(no name) - DTCF";
            }
            else
            {
                /* Unknown Mode, Show Echo Mode */
                ValueDeviceName.Content = deviceName + " - DTC?";
                Console.WriteLine("!!!Error, Undefined Type of control");
            }
        }

        public void rollBackSampleTime()
        {
            flagRollBackSampleTime = true;
            LogsControl.updateSampleTime(MainWindow.sampleTime);
            flagRollBackSampleTime = false;
        }

        /**
         * Functions Called From PwdFactorySettings.xaml.cs
         */
        public void Show_FactorySettingsControl()
        {
            MainWindowReference.getFactorySettings();
            ContentMessages.Content = FactorySettingsControl;
            ContentMessages.IsHitTestVisible = true;
        }

        /**
         * Functions Called From ServicesControl.xaml.cs
         */
        public void Show_RelaysControl(bool backFromFactory)
        {
            if(!backFromFactory)
                MainWindowReference.setTestMode(true);

            //Update Relays State
            RelaysControl.setServiceRelays(MainWindow.relays);

            ContentMessages.Content = RelaysControl;
            ContentMessages.IsHitTestVisible = true;   
        }

        /**
         * Functions Called From SearcControl.xaml.cs
         */
        public void Show_ChangesControl()
        {
            ContentMessages.Content = ChangesControl;
            ContentMessages.IsHitTestVisible = true;
        }

        /**
         * Functions Called From DeviceListControl.xaml.cs
         */
        public void Show_DeviceListControl()
        {
            ContentMessages.Content = DeviceListControl;
            ContentMessages.IsHitTestVisible = true;
        }

        /**
         * Functions Called From TemperatureControl.xaml.cs
         */
        public void Show_ModesControl()
        {
            ContentMessages.Content = ModesControl;
            ContentMessages.IsHitTestVisible = true;
        }

        /**
         * Functions Called From ModesControl.xaml.cs
         */
        public void Change_ImgModes_to_Foco()
        {
            TemperatureControl.ImgModes_Foco();
            ContentMessages.Content = null;
            ContentMessages.IsHitTestVisible = false;
        }

        public void Change_ImgModes_to_FocoOff()
        {
            TemperatureControl.ImgModes_FocoOff();
            ContentMessages.Content = null;
            ContentMessages.IsHitTestVisible = false;
        }

        public void Change_ImgModes_to_Eco()
        {
            TemperatureControl.ImgModes_Eco();
            ContentMessages.Content = null;
            ContentMessages.IsHitTestVisible = false;
        }

        /**
         * Show Content
         */

        public void ShowDialgo_HelpControl()
        {
            HelpControl.updateControlVersion();
            HelpControl.updateControlBLEVersion();
            ContentDialogs.Content = HelpControl;
            ContentDialogs.IsHitTestVisible = true;
        }

        public void ShowDialog_PwdFactorySettingsControl()
        {
            ContentDialogs.Content = PwdFactorySettingsControl;
            ContentDialogs.IsHitTestVisible = true;
        }

        public void ShowDialog_PwdForDeviceControl()
        {
            ContentDialogs.Content = PwdForDeviceControl;
            ContentDialogs.IsHitTestVisible = true;
        }

        public void ShowDialog_OkCancelControl(string Title,string Message, OkCancelControl.OPTION option)
        {
            OkCancelControl.opt = option;
            OkCancelControl.updateContent(Title, Message);
            ContentDialogs.Content = OkCancelControl;
            ContentDialogs.IsHitTestVisible = true;
        }

        public void MSG(string Title, string msg, MSGControl.OPTION option)
        {
            MSGControl.opt = option;
            MSGControl.updateContent(Title, msg);
            ContentDialogs.Content = MSGControl;
            ContentDialogs.IsHitTestVisible = true;
        }

        public void WaitBarControl_Show(bool inConnection)
        {
            WaitBarControl.Start();
            WaitBarControl.flagInConnection = inConnection;
            ContentWaitBar.Content = WaitBarControl;
            ContentWaitBar.IsHitTestVisible = true;
        }

        public void ShowDialog_CustomSampleTimeControl()
        {
            ContentDialogs.Content = CustomSampleTimeControl;
            ContentDialogs.IsHitTestVisible = true;
        }

        /**
         * Hide Content
         */

        public void MessagesControl_Back()
        {
            ContentMessages.Content = null;
            ContentMessages.IsHitTestVisible = false;
        }

        public void DialogControl_Back()
        {
            ContentDialogs.Content = null;
            ContentDialogs.IsHitTestVisible = false;
        }

        public void WaitBarControl_Hide()
        {
            WaitBarControl.Stop();
            ContentWaitBar.Content = null;
            ContentWaitBar.IsHitTestVisible = false;
        }


        /*
         * Versions
         */

        public string getControlVersion()
        {
            /* Returns the firmware version to be displayed in Help List */
            if (MainWindow.flagBleDeviceConnected)
            {
                if (MainWindow.TypeControl == '1')
                {
                    /* CTI Mode, Show Echo Mode */
                    return "CTI - " + FirmwareVersion;
                }
                else if (MainWindow.TypeControl == '2')
                {
                    /* CTF Mode, Hide Echo Mode  */
                    return "CTF - " + FirmwareVersion;
                }
                else
                {
                    /* Unknown Mode, Show Echo Mode */
                    return "CT? - " + FirmwareVersion;
                }
            }
            else
            {
                return null;
            }
        }

        public string getControlBLEVersion()
        {
            /* Returns the firmware version to be displayed in Help List */
            if (MainWindow.flagBleDeviceConnected)
            {
                return MainWindow.BLEVersion;
            }
            else
            {
                return null;
            }
        }


        /*
         * Bluetooth BLE Core Comunication funtions
         */
        public void sendCommandCTBLE(string command)
        {
            string value = command + checkSum(command);
            byte[] data = System.Text.Encoding.ASCII.GetBytes(value.Replace("\0", String.Empty));
            if(usbDongle != null)
                usbDongle.ble_cmd_attclient_attribute_write(conntectionNumber, CT_BLE_CONFIG_UUID, data, true);
        }

        public void getRawTemperatureCTBLE()
        {
            usbDongle.ble_cmd_attclient_read_by_handle(conntectionNumber, CT_BLE_TMP_UUID);
        }

        public void getRawRelaysCTBLE()
        {
            usbDongle.ble_cmd_attclient_read_by_handle(conntectionNumber, CT_BLE_GRL_UUID);
        }

        public void getBLEVersion()
        {
            usbDongle.ble_cmd_attclient_read_by_handle(conntectionNumber, CT_BLE_VERSION_UUID);
        }

        public void activateLogLecture()
        {
            byte[] value = new byte[2];
            value[0] = 1;
            value[1] = 1;
            usbDongle.ble_cmd_attclient_attribute_write(conntectionNumber, CT_BLE_ACTIVATELOG_UUID, value, true);          
        }

        public void acknowledgeLogDataRecived()
        {
            byte[] value = new byte[2];
            value[0] = 2;
            value[1] = 1;
            usbDongle.ble_cmd_attclient_attribute_write(conntectionNumber, CT_BLE_ACTIVATELOG_UUID, value, false);
        }

        public void enableLogNotifications()
        {
            byte[] value2 = new byte[1];
            value2[0] = 1;
            usbDongle.ble_cmd_attclient_write_command(conntectionNumber, CT_BLE_LOGS_NOTIFY_UUID, value2);
        }

        /*
         * Functions of MainWindow
         */

        public void scanBluetoothLE()
        {
            //Stop scan list if running
            objScanList.stop();

            if (usbReady == false)
            {

                //Search Bluegigia USB device
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort"))
                {
                    usbFound = false;
                    string[] portnames = SerialPort.GetPortNames();
                    var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();
                    var tList = (from n in portnames
                                 join p in ports on n equals p["DeviceID"].ToString()
                                 select n + "-" + p["Caption"]).ToList();

                    foreach (string s in tList)
                    {
                        if (s.Contains("Bluegiga Bluetooth Low Energy"))
                        {
                            Console.WriteLine(s);
                            usbFound = true;
                            usbPort = s.Substring(0, s.IndexOf("-"));
                            Console.WriteLine(usbPort);
                        }
                    }
                }

                if (usbFound == false)
                {
                    Console.WriteLine("Error, Bluetooth Low Energy USB is not connected !!!");
                    MainWindowReference.MSG("ERROR",
                            "Bluetooth USB dongle is not connected to the PC. If it is then device driver was not properly instlled.",
                            MSGControl.OPTION.NOTHING);
                    return;
                }

                //Open COM Port
                usbDongle = new MyBgApi(usbPort);
                if (usbDongle.Open() == false)
                {
                    Console.WriteLine("Error, Could not open device Port !!!");

                    //Display Connection error
                    if (usbDongle.CONN_BUSY)
                    {
                        MainWindowReference.MSG("ERROR",
                            "Could not connect to Bluetooth USB dongle. This device is been used by another application.",
                            MSGControl.OPTION.NOTHING);
                    }
                    else
                    {
                        MainWindowReference.MSG("ERROR",
                            "Could not connect to Bluetooth USB dongle. Try reconnecting the device if the problem persists.",
                            MSGControl.OPTION.NOTHING);
                    }

                    usbDongle.Close();
                    return;
                }
                usbReady = true;
            }

            //Scan
            MainWindow.flagSearchButtonClick = true;
            usbDongle.ble_cmd_connection_disconnect(0);
            usbDongle.ble_cmd_gap_set_scan_parameters(200, 200, 1);
            usbDongle.ble_cmd_gap_discover((int)BgApi.gap_discover_mode.gap_discover_generic);

            //Start updating scan List
            Thread scanThread = new Thread(objScanList.thread);
            scanThread.IsBackground = true;
            scanThread.Start();
            while (!scanThread.IsAlive) ;

            //Show DeviceListControl Layer
            Show_DeviceListControl();
        }

        public void connectBluetoothLE(bd_addr sender)
        {
            //Stop scan list if running
            objScanList.stop();

            //Start Wait Bar
            WaitBarControl_Show(true);

            if (usbReady)
                usbDongle.ble_cmd_gap_connect_direct(sender, 0, 12, 12, 200, 0);
        }

        public void setDegreeUnits(bool unit)
        {
            /* Sets Degree to Farenheits or Celsius */
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_CUT + (unit ? '1' : '0'));
        }

        public void setSetPoint(double userSetPoint)
        {
            /* Sets the user Set Point */
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_SPC + userSetPoint);
        }

        public void setMode(string mode)
        {
            /* Sets the Mode */
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_ECOMODE + mode);

            ContentMessages.Content = null;
            ContentMessages.IsHitTestVisible = false;
        }

        public void setFactorySettingHFD(char state)
        {
            /* Set Filter State */
            flagExpectingOKMessage = true;
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_HFD + state);
        }

        public void setFactorySettingSPButton(char state)
        {
            /* Set Values to Factory */
            flagExpectingOKMessage = true;
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_BTN + state);
        }

        public void eraseEEprom()
        {
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_DEL + MainWindow.ON);
        }

        public void resetFactorySettings()
        {
            /* Reset Values to Factory Settings*/
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_RSF + MainWindow.ON);
        }

        public void setFactorySettingSPC(string data)
        {
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_SPC + data);
        }

        public void setFactorySettingLISPC(string data)
        {
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_LISPC + data);
        }

        public void setFactorySettingLSSPC(string data)
        {
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_LSSPC + data);
        }

        public void setFactorySettingSID(string data)
        {
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_SID + data);
        }

        public void setFactorySettingSTD(string data)
        {
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_STD + data);
        }

        public void setFactorySettingSDIF(string data)
        {
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_SDIF + data);
        }

        public void setFactorySettingtDI(string data)
        {
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_tDI + data);
        }

        public void setFactorySettingtESO(string data)
        {
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_tES0 + data);
        }

        public void setFactorySettingtDUD(string data)
        {
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_tDUD + data);
        }

        public void setFactorySettingtDESH(string data)
        {
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_tDESH + data);
        }

        public void setFactorySettingtRAD(string data)
        {
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_tRAD + data);
        }

        public void setFactorySettingtTCC(string data)
        {
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_tTCC + data);
        }

        public void setFactorySettingSPSL(string data)
        {
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_SPSL + data);
        }

        public void setFactorySettingCALMV(string data)
        {
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_CALMV + data);
        }

        public void setFactorySettingtDESH2(string data)
        {
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_tDESH2 + data);
        }

        public void setFactorySettingCDESH(string data)
        {
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_CDESH + data);
        }

        public void setFactorySettingTMAX(string data)
        {
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_TMAX + data);
        }

        public void setFactorySettingtDUDM(string data)
        {
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_tDUDM + data);
        }

        public void setFactorySettingtUD(string data)
        {
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_tUD + data);
        }

        public void setFactorySettingZ(string data)
        {
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_Z + data);
        }

        public void setFactorySettingtGOTEO(string data)
        {
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_tGOTEO + data);
        }

        public void setReadAllLog()
        {
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_SETLOG + OFF);
        }

        public void setReadPartialLog(string lastline)
        {
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_SETLOG + ON + lastline);
        }

        public void setSampleTime(int sampleTime)
        {
            flagInSetSampleTime = true;
            if (usbReady)
                sendCommandCTBLE("" + CTBLE_TLOGGER + sampleTime);
        }


        public void setDeviceName(string name)
        {
            if (name.Length < MIN_DEVICE_NAME_LENGTH)
            {
                MainWindowReference.MSG("ALERT", "Name must be from 3 to 15 characters long", MSGControl.OPTION.NOTHING);
            }
            else
            {
                if (usbReady)
                    sendCommandCTBLE("" + CTBLE_NAME + name);

                MessagesControl_Back();
            }

        }

        public void setDevicePassword(string newPassword)
        {

            if (usbReady)
            {
                flagOnChangePassword = true;
                sendCommandCTBLE("" + CTBLE_PSS + newPassword);
            }

            MessagesControl_Back();

        }

        public void setTestMode(bool Mode)
        {

            if (usbReady)
            {
                if (Mode)
                {
                    sendCommandCTBLE("" + CTBLE_TESTMODE + ON);
                }
                else
                {
                    sendCommandCTBLE("" + CTBLE_TESTMODE + OFF);
                }
            }
        }

        public void getFactorySettings()
        {
            flagInGetFactorySettings = true;

            MainWindowReference.WaitBarControl_Show(false);

            Console.WriteLine("In get Factory Settings");

            //Get Limite Inferior
            MainWindowReference.sendCommandCTBLE("" + CTBLE_LISPC);
            //Get Limite Superiror
            MainWindowReference.sendCommandCTBLE("" + CTBLE_LSSPC);
            //Get Set Point
            MainWindowReference.sendCommandCTBLE("" + CTBLE_SPC);
            //Get SID
            MainWindowReference.sendCommandCTBLE("" + CTBLE_SID);
            //Get STD
            MainWindowReference.sendCommandCTBLE("" + CTBLE_STD);
            //Get SDIF
            MainWindowReference.sendCommandCTBLE("" + CTBLE_SDIF);
            //Get tDI
            MainWindowReference.sendCommandCTBLE("" + CTBLE_tDI);
            //Get tES0
            MainWindowReference.sendCommandCTBLE("" + CTBLE_tES0);
            //Get tDUD
            MainWindowReference.sendCommandCTBLE("" + CTBLE_tDUD);
            //Get tDESH
            MainWindowReference.sendCommandCTBLE("" + CTBLE_tDESH);
            //Get tRAD
            MainWindowReference.sendCommandCTBLE("" + CTBLE_tRAD);
            //Get tTCC
            MainWindowReference.sendCommandCTBLE("" + CTBLE_tTCC);
            //Get SPSL
            MainWindowReference.sendCommandCTBLE("" + CTBLE_SPSL);
            //Get CALMV
            MainWindowReference.sendCommandCTBLE("" + CTBLE_CALMV);
            //Get tDESH2
            MainWindowReference.sendCommandCTBLE("" + CTBLE_tDESH2);
            //Get CDESH
            MainWindowReference.sendCommandCTBLE("" + CTBLE_CDESH);
            //Get TMAX
            MainWindowReference.sendCommandCTBLE("" + CTBLE_TMAX);
            //Get tDUDM
            MainWindowReference.sendCommandCTBLE("" + CTBLE_tDUDM);
            //Get tUD
            MainWindowReference.sendCommandCTBLE("" + CTBLE_tUD);
            //Get Z
            MainWindowReference.sendCommandCTBLE("" + CTBLE_Z);
            //Get tGOTEO
            MainWindowReference.sendCommandCTBLE("" + CTBLE_tGOTEO);
            //Get Filter State
            MainWindowReference.sendCommandCTBLE("" + CTBLE_HFD);

        }

        public void toogleServiceRelays(int nRelay)
        {
            StringBuilder auxString = new StringBuilder("0000");

            /* Obtain Actual State */
            auxString[0] = MainWindow.uCompresor ? '1' : '0';
            auxString[1] = MainWindow.uFoco ? '1' : '0';
            auxString[2] = MainWindow.uResistencia ? '1' : '0';
            auxString[3] = MainWindow.uVentilador ? '1' : '0';

            /* Send Toogled compresor state */
            switch (nRelay)
            {
                case MainWindow.RELAY_COMPRESOR:
                    auxString[0] = MainWindow.uCompresor ? '0' : '1';
                    break;
                case MainWindow.RELAY_FOCO:
                    auxString[1] = MainWindow.uFoco ? '0' : '1';
                    break;
                case MainWindow.RELAY_RESISTENCIA:
                    auxString[2] = MainWindow.uResistencia ? '0' : '1';
                    break;
                case MainWindow.RELAY_VENTILADOR:
                    auxString[3] = MainWindow.uVentilador ? '0' : '1';
                    break;
                default:
                    break;
            }

            if (usbReady)
                sendCommandCTBLE("" + CTBLE_CFRV + auxString);

        }

        public void disconnectDevice()
        {
            if (usbReady == true)
            {
                //Disconnect and close any connection
                usbDongle.ble_cmd_gap_end_procedure();
                usbDongle.ble_cmd_connection_disconnect(0);
            }
        }


        // Thread Class
        public class ScanList
        {
            private volatile bool varStop = false;

            public void thread()
            {
                varStop = false;

                while (!varStop)
                {
                    //Update Scan List
                    DeviceListControl.updateScanList(usbDongle.deviceList);
                    for (int i = 0; i < 50; i++)
                    {
                        if (varStop) break;
                        Thread.Sleep(10);
                    }
                }
            }

            public void stop()
            {
                varStop = true;
                if (usbReady == true) usbDongle.ble_cmd_gap_end_procedure();
                Thread.Sleep(50);
            }


        }

        //Timer funtion
        private void temperatureTimer_Tick(object sender, EventArgs e)
        {
            if (flagInGetFactorySettings == false && flagInGetHistoricLogs == false)
            {
                if (usbReady)
                {
                    getRawTemperatureCTBLE();

                    if (MainWindow.uSPBtnEnable && flagChangingSetpoint == false) MainWindowReference.sendCommandCTBLE("" + CTBLE_SPC); //Por si se cambió manualmente
                }
            }
        }

        private void Closing_App(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (usbDongle != null && usbReady == true)
            {
                //Stop scan list if running
                objScanList.stop();

                //Disconnect and close any connection
                usbDongle.ble_cmd_gap_end_procedure();
                usbDongle.ble_cmd_connection_disconnect(0);
                usbDongle.Close();
                usbDongle = null;
            }
            usbReady = false;
        }

     }
}
