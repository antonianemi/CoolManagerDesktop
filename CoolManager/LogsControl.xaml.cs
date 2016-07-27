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
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.ChartModifiers;
using System.ComponentModel;
using System.IO;


namespace CoolManager
{
    /// <summary>
    /// Interaction logic for LogsControl.xaml
    /// </summary>
    public partial class LogsControl : UserControl
    {
        //Save Log Files in User LocalAppData -> C:\User\UserName\TORREY\Temperature_Logs\
        public static string APP_DATA = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) , "TORREY");
        static string DIR_LOGS = System.IO.Path.Combine(APP_DATA, "Temperature_Logs");
        static string MAIL_TABLE_FILE = System.IO.Path.Combine(DIR_LOGS, "TableLogs.txt");

        public const int SIX_MONTHS      = 0;
        public const int THREE_MONTHS    = 1;
        public const int ONE_MONTH       = 2;
        public const int ONE_WEEK        = 3;
        public const int ONE_DAY         = 4;

        public const int RLY_COMPRESSOR  = 0;
        public const int RLY_LAMP        = 1;
        public const int RLY_RESISTOR    = 2;
        public const int RLY_FAN         = 3;
        public const int RLY_DOOR        = 4;

        public const int SAMPLE_TIME_1MIN   = 0;
        public const int SAMPLE_TIME_2MIN   = 1;
        public const int SAMPLE_TIME_5MIN   = 2;
        public const int SAMPLE_TIME_10MIN  = 3;
        public const int SAMPLE_TIME_60MIN  = 4;
        public const int SAMPLE_TIME_NMIN   = 5;

        public const int T_1MIN   = 1;
        public const int T_2MIN   = 2;
        public const int T_5MIN   = 5;
        public const int T_10MIN  = 10;
        public const int T_60MIN  = 60;

        public static int currentChartTime = SIX_MONTHS;
        public static int currentRelay = RLY_COMPRESSOR;

        MainWindow MainWindowReference;

        public LogsControl(MainWindow MainWindow)
        {
            InitializeComponent();
            MainWindowReference = MainWindow;
        }

        private void Download_MouseUp(object sender, MouseButtonEventArgs e)
        {
            downloadNewLogData();
        }

        private void Download_MouseLeave(object sender, MouseEventArgs e)
        {
            DownloadButton.Background = new SolidColorBrush(Colors.DodgerBlue);
        }

        private void Download_MouseEnter(object sender, MouseEventArgs e)
        {
            DownloadButton.Background = new SolidColorBrush(Colors.LimeGreen);
        }

        private void Save_MouseUp(object sender, MouseButtonEventArgs e)
        {

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "TableLog"; // Default file name
            dlg.DefaultExt = ".text"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save File document
                try
                {
                    System.IO.File.Copy(MAIL_TABLE_FILE, dlg.FileName, true);
                    MainWindowReference.MSG("MESSAGE", "File Saved", MSGControl.OPTION.NOTHING);
                }
                catch (Exception ex)
                {
                    MainWindowReference.MSG("ERROR", "Could not save File!", MSGControl.OPTION.NOTHING);
                    Console.WriteLine("Error in Save File, Exception = " + ex.ToString());
                }

            }
        }

        private void Save_MouseLeave(object sender, MouseEventArgs e)
        {
            SaveButton.Background = new SolidColorBrush(Colors.DodgerBlue);
        }

        private void Save_MouseEnter(object sender, MouseEventArgs e)
        {
            SaveButton.Background = new SolidColorBrush(Colors.LimeGreen);
        }

        private void Erase_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MainWindowReference.ShowDialog_OkCancelControl(
                        "WARNING",
                        "Are you sure you want to EREASE all temperature values?",
                        OkCancelControl.OPTION.ERASE_EEPROM);
        }

        private void Erase_MouseLeave(object sender, MouseEventArgs e)
        {
            EraseButton.Background = new SolidColorBrush(Colors.DodgerBlue);
        }

        private void Erase_MouseEnter(object sender, MouseEventArgs e)
        {
            EraseButton.Background = new SolidColorBrush(Colors.LimeGreen);
        }

        private void RelayComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int itemNum = RelayComboBox.SelectedIndex;
            if (MainWindow.usbReady)
                startChart(currentChartTime, itemNum, MainWindow.deviceAddressFile);
        }

        private void ChartTimeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int itemNum = ChartTimeComboBox.SelectedIndex;
            if (MainWindow.usbReady)
                startChart(itemNum, currentRelay, MainWindow.deviceAddressFile);
        }

        private void SampleTimeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            int itemNum = SampleTimeComboBox.SelectedIndex;
            int selectedSampleTime = 1;

            switch (itemNum) {
                case SAMPLE_TIME_1MIN: selectedSampleTime = T_1MIN; break;
                case SAMPLE_TIME_2MIN: selectedSampleTime = T_2MIN; break;
                case SAMPLE_TIME_5MIN: selectedSampleTime = T_5MIN; break;
                case SAMPLE_TIME_10MIN: selectedSampleTime = T_10MIN; break;
                case SAMPLE_TIME_60MIN: selectedSampleTime = T_60MIN; break;
                default:
                    try
                    {
                        if (SampleTimeComboBox.SelectedValue.Equals("Custom ..."))
                            MainWindowReference.ShowDialog_CustomSampleTimeControl();
                    }
                    catch { }
                    break;
            }
            if (MainWindow.usbReady && MainWindow.flagInSetSampleTime !=true
                && MainWindow.flagRollBackSampleTime != true && itemNum <= 4)
                MainWindowReference.setSampleTime(selectedSampleTime);
        }
        

        /* Redraws Chart if there is Data available */
        public void tabLogs_Selected()
        {
            string data = readFileLastLine(MainWindow.deviceAddressFile);
            if (data == null)
                HideLayers();
            else
            {
                ShowLayers();
                startChart(currentChartTime, currentRelay, MainWindow.deviceAddressFile);
            }
        }

        /* Reads File last Line of the connected Device Mac Address, 
         * if it not exist it creates a new file */
        public string readFileLastLine(string fileName)
        {
            string line, lastline = null;
            string filePath = System.IO.Path.Combine(DIR_LOGS, fileName);

            //Create folder Logs if doesn´t exists
            Directory.CreateDirectory(DIR_LOGS);

            //Returns last data if file exists, else creates File
            if (File.Exists(filePath))
            {
                StreamReader file = new StreamReader(filePath);
                while ((line = file.ReadLine()) != null)
                {
                    lastline = line;
                }

                file.Close();

                if (lastline != null)
                {
                    string[] item = lastline.Split('-');
                    string searchDateAndTime = item[0] + item[1] + item[2] + item[3] + item[4];
                    Console.WriteLine("Last Line = {0}", searchDateAndTime);
                    return searchDateAndTime;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                File.Create(filePath);
            }

            return null;
        }

        /* Writes Data to device address file */
        public void writeData(string formatedLogData, string fileName)
        {
            string filePath = System.IO.Path.Combine(DIR_LOGS, fileName);

            //Create folder Logs if doesn´t exists
            if (!Directory.Exists(DIR_LOGS))
            {
                Console.WriteLine("Fatal error the file directory should exists");
                return;
            }

            //Returns last data if file exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Fatal error the file should exists");
                return;
            }

            try
            {
                StreamWriter file = new StreamWriter(filePath, true);
                file.WriteLine(formatedLogData);
                file.Close();
            }
            catch (IOException)
            {
                Console.WriteLine("Error, File is been used by another program");
            }
            
        }

        /* Erease the File of the Logs */
        public void eraseLogsFile(string fileName)
        {
            string filePath = System.IO.Path.Combine(DIR_LOGS, fileName);

            //Create folder Logs if doesn´t exists
            if (!Directory.Exists(DIR_LOGS))
            {
                Console.WriteLine("Fatal error the file directory should exists");
                return;
            }

            //Returns last data if file exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Fatal error the file should exists");
                return;
            }

            File.Delete(filePath);
            HideLayers();
        }

        /* Draws the chart depending on the selected range */
        public void startChart(int range, int rly, string fileName)
        {
            string line, tableLogData;
            string filePath = System.IO.Path.Combine(DIR_LOGS, fileName);
            int year, month, day, hour, minute, index, numLog = 0;
            int unit, status, Compressor, Lamp, Resistor, Fan, DoorOpen;
            string format = "y/M/d H:m";
            string dateString = null;
            double graphTemp;
            DateTime datetime = DateTime.Now;

            //Set ComboBox Selection
            ChartTimeComboBox.SelectedIndex = range;
            RelayComboBox.SelectedIndex = rly;

            //Save selection
            currentChartTime = range;
            currentRelay = rly;

            //Show Chart Layer
            ShowLayers();

            //Chart Titles
            temperatureTitle.Content = "Time vs Temperature  (" + rangeToString(range)+")";
            relaysTitle.Content = rlyToString(rly);

            //Set Temperature Degree Unit
            if (MainWindow.uCF)
                DegreeUnit.Content = "°F";
            else
                DegreeUnit.Content = "°C";

            if (File.Exists(filePath))
            {
                // Define a temperature and relays data series
                var tempSeries = new XyDataSeries<DateTime, double>();
                var relaysSeries = new XyDataSeries<DateTime, double>();
                tempSeries.SeriesName = "Temperature";
                relaysSeries.SeriesName = "Relay";

                //Get Bigining Date Range to display Data
                DateTime currentDate = DateTime.Now;

                switch (range)
                { 
                    case SIX_MONTHS: //six month
                        {
                            currentDate = currentDate.AddMonths(-6);
                            break;
                        }
                    case THREE_MONTHS: //three month
                        {
                            currentDate = currentDate.AddMonths(-3);
                            break;
                        }
                    case ONE_MONTH:  //one month 
                        {
                            currentDate = currentDate.AddMonths(-1);
                            break;
                        }
                    case ONE_WEEK: //one week
                        {
                            currentDate = currentDate.AddDays(-7);
                            break;
                        }
                    case ONE_DAY: //one day
                        {
                            currentDate = currentDate.AddHours(-24);
                            break;
                        }                     
                }

                //Text file to save data shown in graph for future use
                StreamWriter tableLogFile = new StreamWriter(MAIL_TABLE_FILE, false);

                //Read Data
                StreamReader file = new StreamReader(filePath);
                while ((line = file.ReadLine()) != null)
                {
                    //Separate data into corresponding value
                    string[] items = line.Split('-');
                    year = int.Parse(items[0]);
                    month = int.Parse(items[1]);
                    day = int.Parse(items[2]);
                    hour = int.Parse(items[3]);
                    minute = int.Parse(items[4]);
                    index = int.Parse(items[7]);
                    unit = int.Parse(items[5]);
                    status = int.Parse(items[6]);
                    DoorOpen = (status & 0x10) >> 4; 
                    Compressor = (status & 0x08) >> 3; 
                    Lamp = (status & 0x04) >> 2; 
                    Resistor = (status & 0x02) >> 1;
                    Fan = status & 0x01; 

                    //Format Date and Time
                    try
                    {
                        dateString = year + "/" + month + "/" + day + " " + hour + ":" + minute;
                        datetime = DateTime.ParseExact(dateString, format, null);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("{0} is not in the correct format.", dateString);
                    }

                    //Check if dateTime is later than currentDate - Range.
                    if ((DateTime.Compare(datetime, currentDate)) >= 0)
                    {
                        //Graph Temperature
                        graphTemp = selectUnitGradeValue(MainWindow.uCF, unit, index);
                        tempSeries.Append(datetime, graphTemp);

                        //Graph Relay Status
                        switch (rly)
                        {
                            case RLY_COMPRESSOR: relaysSeries.Append(datetime, Compressor); break;
                            case RLY_FAN: relaysSeries.Append(datetime, Fan); break;
                            case RLY_LAMP: relaysSeries.Append(datetime, Lamp); break;
                            case RLY_RESISTOR: relaysSeries.Append(datetime, Resistor); break;
                            case RLY_DOOR: relaysSeries.Append(datetime, DoorOpen); break;
                            default: relaysSeries.Append(datetime, Compressor); break;
                        }

                        //Save Formated Data in a TableLog.txt File
                        tableLogData = numLog + "\t" + String.Format("{0:d2}", year) + "/" + String.Format("{0:d2}", month) + "/" +
                        String.Format("{0:d2}", day) + "\t" + String.Format("{0:d2}", hour) + ":" +
                        String.Format("{0:d2}", minute) + "\t" + graphTemp + DegreeUnit.Content + "\tCompressor:" + Compressor +
                        "\tLamp:" + Lamp + "\tResistor:" + Resistor + "\tFan:" + Fan + "\tDoorOpen:" + DoorOpen + "\n";

                        tableLogFile.Write(tableLogData);

                        numLog++;
                    }
                   
                }

                file.Close();
                tableLogFile.Close();

                // Set Data Series Chart Values
                temperatureChart.RenderableSeries[0].DataSeries = tempSeries;
                relaysChart.RenderableSeries[0].DataSeries = relaysSeries;

                //Zoom Charts
                temperatureChart.ZoomExtents();
                relaysChart.ZoomExtents();

                //Check if there was no Data in Range
                if (temperatureChart.RenderableSeries[0].DataSeries.Count == 0)
                {
                    MainWindowReference.MSG("ALERT", "There is no data for the selected time interval." +
                                    " Click in Download to aquire new data", MSGControl.OPTION.NOTHING);
                }

            }
        }

        public string rangeToString(int range)
        {
            switch (range)
            {
                case SIX_MONTHS: return "6 months";
                case THREE_MONTHS: return "3 months";
                case ONE_MONTH: return "1 month";
                case ONE_WEEK: return "1 week";
                case ONE_DAY: return "1 day";
                default: break;
            }
            return "";
        }

        public string rlyToString(int rly)
        {
            switch (rly)
            {
                case RLY_COMPRESSOR: return "Compressor";
                case RLY_FAN: return "Fan";
                case RLY_LAMP: return "Lamp";
                case RLY_RESISTOR: return "Resistor";
                case RLY_DOOR: return "Door";
                default: break;
            }
            return "";
        }

        public void ShowLayers()
        {
            temperatureChart.Visibility = Visibility.Visible;
            relaysChart.Visibility = Visibility.Visible;
            temperatureTitle.Visibility = Visibility.Visible;
            relaysTitle.Visibility = Visibility.Visible;
            RelayLabel.Visibility = Visibility.Visible;
            RelayComboBox.Visibility = Visibility.Visible;
            CharTimeLabel.Visibility = Visibility.Visible;
            ChartTimeComboBox.Visibility = Visibility.Visible;
            SaveButtonBorder.Visibility = Visibility.Visible;
            DegreeUnit.Visibility = Visibility.Visible;
            SampleTime.Visibility = Visibility.Visible;
            SampleTimeComboBox.Visibility = Visibility.Visible;
        }

        public void HideLayers()
        {
            temperatureChart.Visibility = Visibility.Hidden;
            relaysChart.Visibility = Visibility.Hidden;
            temperatureTitle.Visibility = Visibility.Hidden;
            relaysTitle.Visibility = Visibility.Hidden;
            RelayLabel.Visibility = Visibility.Hidden;
            RelayComboBox.Visibility = Visibility.Hidden;
            CharTimeLabel.Visibility = Visibility.Hidden;
            ChartTimeComboBox.Visibility = Visibility.Hidden;
            SaveButtonBorder.Visibility = Visibility.Hidden;
            DegreeUnit.Visibility = Visibility.Hidden;
            SampleTime.Visibility = Visibility.Hidden;
            SampleTimeComboBox.Visibility = Visibility.Hidden;
        }

        public void downloadNewLogData()
        {
            //1. Read the file.
            string lastline = readFileLastLine(MainWindow.deviceAddressFile);
            if (lastline == null)
            {
                //2. Obtain all data
                MainWindowReference.setReadAllLog();
            }
            else
            {
                //2. Obtain partial data
                MainWindowReference.setReadPartialLog(lastline);
            }
        }

        /*
        * Know kind of grade values to use
        */
        public double selectUnitGradeValue(bool unitGrade, int unit, int index)
        {

            double temperature = fGet_Value_Temperature(unit, index);

            double rettemp = 0;

            if (unitGrade && unit == 1)
            {    // if �F state && unit=1
                rettemp = temperature;
                //return temperature;
            }
            if (unitGrade && unit == 0)
            {    // if �F state && unit=0
                rettemp = centigradeToF(temperature);
                //return centigradeToF(temperature);
            }
            if (!unitGrade && unit == 1)
            {	// if �C state && unit=1
                rettemp = fahrenheitToC(temperature);
                //return fahrenheitToC(temperature);
            }
            if (!unitGrade && unit == 0)
            {	// if �C state && unit=0
                rettemp = temperature;
                //return temperature;
            }

            return rettemp;

        }

        /*
         * Converts an Index to a temperature
         */
        public double fGet_Value_Temperature(int unit, int iIndex)
        {

            int iFactor;
            int iSustractor;
            double fValue_Temperature;

            if (unit == 0)
            {                       //SI SON °C(0)
                iFactor = 5;		//VALORES DE COVERSION DE INDICE a °C
                iSustractor = 350;
            }
            else
            {				        //SI SON °F(1)
                iFactor = 10;	    //VALORES DE COVERSION DE INDICE a °F
                iSustractor = 310;
            }

            if (iIndex > 171 || iIndex == 0)
            {				//INDICE NO VALIDO
                return 250;
            }

            fValue_Temperature = (double)(iIndex * iFactor - iSustractor);  //FORMULA DE CONVERSION DE INDICE A TEMP
            fValue_Temperature /= 10;                                       //FORMULA DE CONVERSION DE INDICE A TEMP

            return fValue_Temperature;
        }


        /*
         * Convert fahrenheit grades to centigrades
         */
        public double fahrenheitToC(double fahrenheit)
        {

            fahrenheit = (fahrenheit - 32.0) * (5.0 / 9.0);
            return fRedondeo_1_decimal(fahrenheit);

        }

        /*
        * Convert centigrades grades to fahrenheit
        */
        public double centigradeToF(double centigrade)
        {

            centigrade = (centigrade * (9.0 / 5.0)) + 32.0;
            return fRedondeo_0_decimal(centigrade);

        }

        /*
         * Rounds up the temperature when converting from C to F
         */
        double fRedondeo_0_decimal(double fTemperatura_Promedio)
        {
            int iPart_Int;
            byte uFlagSigno = 0;

            if (fTemperatura_Promedio < 0)
            {
                uFlagSigno = 1;
                fTemperatura_Promedio *= (-1);
            }

            iPart_Int = (int)(fTemperatura_Promedio);
            fTemperatura_Promedio -= (double)(iPart_Int);

            if (fTemperatura_Promedio >= 0.5)
            {
                fTemperatura_Promedio = 1;
            }
            else
            {
                fTemperatura_Promedio = 0;
            }

            fTemperatura_Promedio += (double)(iPart_Int);


            if (uFlagSigno == 1)
            {
                fTemperatura_Promedio *= (-1);
            }

            return fTemperatura_Promedio;
        }

        /*
         * Rounds up the temperature when converting from F to C
         */
        double fRedondeo_1_decimal(double fTemperatura_Promedio)
        {
            int iPart_Int;
            byte uFlagSigno = 0;

            if (fTemperatura_Promedio < 0)
            {
                uFlagSigno = 1;
                fTemperatura_Promedio *= (-1);
            }

            iPart_Int = (int)(fTemperatura_Promedio);
            fTemperatura_Promedio -= (double)(iPart_Int);

            if (fTemperatura_Promedio > 0.75)
            {
                fTemperatura_Promedio = 1;
            }
            else if (fTemperatura_Promedio > 0.25)
            {
                fTemperatura_Promedio = 0.5;
            }
            else
            {
                fTemperatura_Promedio = 0;
            }
            fTemperatura_Promedio += (double)(iPart_Int);


            if (uFlagSigno == 1)
            {
                fTemperatura_Promedio *= (-1);
            }

            return fTemperatura_Promedio;
        }

        public void updateSampleTime(int SampleTime)
        {

            switch (SampleTime)
            {
                case T_1MIN:                    
                    SampleTimeComboBox.Items.RemoveAt(5);
                    if (SampleTimeComboBox.Items.Count == 6) SampleTimeComboBox.Items.RemoveAt(5);
                    SampleTimeComboBox.Items.Add("Custom ...");
                    SampleTimeComboBox.SelectedIndex = SAMPLE_TIME_1MIN;
                    break;
                case T_2MIN:                     
                    SampleTimeComboBox.Items.RemoveAt(5);
                    if (SampleTimeComboBox.Items.Count == 6) SampleTimeComboBox.Items.RemoveAt(5);
                    SampleTimeComboBox.Items.Add("Custom ...");
                    SampleTimeComboBox.SelectedIndex = SAMPLE_TIME_2MIN; 
                    break;
                case T_5MIN:                   
                    SampleTimeComboBox.Items.RemoveAt(5);
                    if (SampleTimeComboBox.Items.Count == 6) SampleTimeComboBox.Items.RemoveAt(5);
                    SampleTimeComboBox.Items.Add("Custom ...");
                    SampleTimeComboBox.SelectedIndex = SAMPLE_TIME_5MIN;
                    break;
                case T_10MIN:                     
                    SampleTimeComboBox.Items.RemoveAt(5);
                    if (SampleTimeComboBox.Items.Count == 6) SampleTimeComboBox.Items.RemoveAt(5);
                    SampleTimeComboBox.Items.Add("Custom ...");
                    SampleTimeComboBox.SelectedIndex = SAMPLE_TIME_10MIN;
                    break;
                case T_60MIN:                    
                    SampleTimeComboBox.Items.RemoveAt(5);
                    if (SampleTimeComboBox.Items.Count == 6) SampleTimeComboBox.Items.RemoveAt(5);
                    SampleTimeComboBox.Items.Add("Custom ...");
                    SampleTimeComboBox.SelectedIndex = SAMPLE_TIME_60MIN;
                    break;
                default:                    
                    SampleTimeComboBox.Items.RemoveAt(5);
                    if (SampleTimeComboBox.Items.Count == 6) SampleTimeComboBox.Items.RemoveAt(5);
                    SampleTimeComboBox.Items.Add(SampleTime +" Minutes");
                    SampleTimeComboBox.Items.Add("Custom ...");
                    SampleTimeComboBox.SelectedIndex = SAMPLE_TIME_NMIN;
                    break;
            }

            SampleTimeComboBox.Items.Refresh();
        }
    }
}
