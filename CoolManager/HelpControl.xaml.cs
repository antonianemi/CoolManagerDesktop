using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CoolManager
{

    public enum Tabs
    {
        LABEL_BU,
        LABEL_AU,
        LABEL_LS,
        LABEL_A1,
        LABEL_A2,
        LABEL_A3,
        LABEL_DF,
        LABEL_VERSION
    };


    /// <summary>
    /// Interaction logic for HelpControl.xaml
    /// </summary>
    public partial class HelpControl : UserControl
    {
        MainWindow MainWindowReference;

        public static string firmwareVersion = "??";
        public static string bleVersion = "??";

        public HelpControl(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindowReference = mainWindow;
            tab_select(Tabs.LABEL_BU);
        }

        private void Bu_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            tab_select(Tabs.LABEL_BU);
        }

        private void Au_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            tab_select(Tabs.LABEL_AU);
        }

        private void LS_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            tab_select(Tabs.LABEL_LS);
        }

        private void A1_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            tab_select(Tabs.LABEL_A1);
        }

        private void A2_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            tab_select(Tabs.LABEL_A2);
        }

        private void A3_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            tab_select(Tabs.LABEL_A3);
        }

        private void dF_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            tab_select(Tabs.LABEL_DF);
        }

        private void Version_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            tab_select(Tabs.LABEL_VERSION);
        }


        private void tab_select(Tabs tab)
        {
          
            if (tab == Tabs.LABEL_BU)
            {
                labelBu.Foreground = new SolidColorBrush(Colors.Black);
                labelBu.Background = new SolidColorBrush(Colors.White);

                richTextBox1.Document = (FlowDocument)FindResource("FlowDocumentBu");
            }
            else
            {
                labelBu.Foreground = new SolidColorBrush(Colors.White);
                labelBu.Background = new SolidColorBrush(Colors.SeaGreen);
            }

            if (tab == Tabs.LABEL_AU)
            {
                labelAu.Foreground = new SolidColorBrush(Colors.Black);
                labelAu.Background = new SolidColorBrush(Colors.White);

                richTextBox1.Document = (FlowDocument)FindResource("FlowDocumentAu");
            }
            else
            {
                labelAu.Foreground = new SolidColorBrush(Colors.White);
                labelAu.Background = new SolidColorBrush(Colors.SeaGreen);
            }

            if (tab == Tabs.LABEL_LS)
            {
                labelLS.Foreground = new SolidColorBrush(Colors.Black);
                labelLS.Background = new SolidColorBrush(Colors.White);

                richTextBox1.Document = (FlowDocument)FindResource("FlowDocumentLS");
            }
            else
            {
                labelLS.Foreground = new SolidColorBrush(Colors.White);
                labelLS.Background = new SolidColorBrush(Colors.SeaGreen);
            }

            if (tab == Tabs.LABEL_A1)
            {
                labelA1.Foreground = new SolidColorBrush(Colors.Black);
                labelA1.Background = new SolidColorBrush(Colors.White);

                richTextBox1.Document = (FlowDocument)FindResource("FlowDocumentA1");
            }
            else
            {
                labelA1.Foreground = new SolidColorBrush(Colors.White);
                labelA1.Background = new SolidColorBrush(Colors.SeaGreen);
            }

            if (tab == Tabs.LABEL_A2)
            {
                labelA2.Foreground = new SolidColorBrush(Colors.Black);
                labelA2.Background = new SolidColorBrush(Colors.White);

                richTextBox1.Document = (FlowDocument)FindResource("FlowDocumentA2");
            }
            else
            {
                labelA2.Foreground = new SolidColorBrush(Colors.White);
                labelA2.Background = new SolidColorBrush(Colors.SeaGreen);
            }

            if (tab == Tabs.LABEL_A3)
            {
                labelA3.Foreground = new SolidColorBrush(Colors.Black);
                labelA3.Background = new SolidColorBrush(Colors.White);

                richTextBox1.Document = (FlowDocument)FindResource("FlowDocumentA3");
            }
            else
            {
                labelA3.Foreground = new SolidColorBrush(Colors.White);
                labelA3.Background = new SolidColorBrush(Colors.SeaGreen);
            }

            if (tab == Tabs.LABEL_DF)
            {
                labeldF.Foreground = new SolidColorBrush(Colors.Black);
                labeldF.Background = new SolidColorBrush(Colors.White);

                richTextBox1.Document = (FlowDocument)FindResource("FlowDocumentDF");
            }
            else
            {
                labeldF.Foreground = new SolidColorBrush(Colors.White);
                labeldF.Background = new SolidColorBrush(Colors.SeaGreen);
            }

            if (tab == Tabs.LABEL_VERSION)
            {
                labelVersion.Foreground = new SolidColorBrush(Colors.Black);
                labelVersion.Background = new SolidColorBrush(Colors.White);

                FlowDocument flowDocument = new FlowDocument();
                Table table = new Table();
                TableRowGroup group = new TableRowGroup();

                flowDocument.Blocks.Add(table);
                table.RowGroups.Add(group);

                TableRow Row1 = new TableRow();
                TableRow Row2 = new TableRow();
                TableRow Row3 = new TableRow();
                TableRow Row4 = new TableRow();
                TableRow Row5 = new TableRow();
                TableRow Row6 = new TableRow();
                TableRow Row7 = new TableRow();

                Row1.FontWeight = FontWeights.Bold;
                Row1.Foreground = new SolidColorBrush(Colors.White);
                Row1.Background = new SolidColorBrush(Colors.SeaGreen);

                Row5.FontWeight = FontWeights.Bold;
                Row5.Foreground = new SolidColorBrush(Colors.White);
                Row5.Background = new SolidColorBrush(Colors.SeaGreen);

                Row1.Cells.Add(new TableCell(new Paragraph(new Run("DEFINITION")) { FontSize = 16 }));
                Row2.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                Row3.Cells.Add(new TableCell(new Paragraph(new Run("FRIMWARE CONTROL: " + firmwareVersion +
                    "\nFRIMWARE BLUETOOTH: " + bleVersion + "\nSOFTWARE PC: " + getSoftwareVersion())) { FontSize = 14 }));
                Row4.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                Row5.Cells.Add(new TableCell(new Paragraph(new Run("PROCEDURE")) { FontSize = 16 }));
                Row6.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                Row7.Cells.Add(new TableCell(new Paragraph(new Run("Last updated: " + 
                    String.Format(new System.Globalization.CultureInfo(""), "{0:yyyy-MMMM-d}", RetrieveLastUpdate()))) { FontSize = 14 }));

                group.Rows.Add(Row1);
                group.Rows.Add(Row2);
                group.Rows.Add(Row3);
                group.Rows.Add(Row4);
                group.Rows.Add(Row5);
                group.Rows.Add(Row6);
                group.Rows.Add(Row7);

                richTextBox1.Document = flowDocument;
            }
            else
            {
                labelVersion.Foreground = new SolidColorBrush(Colors.White);
                labelVersion.Background = new SolidColorBrush(Colors.SeaGreen);
            }

        }

        /* Other */

        private void Back_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindowReference.DialogControl_Back();
            tab_select(Tabs.LABEL_BU);
        }

        public void updateControlVersion()
        {
            string auxData = MainWindowReference.getControlVersion();
            firmwareVersion = auxData ?? "??";
        }

        public void updateControlBLEVersion()
        {
            string auxData = MainWindowReference.getControlBLEVersion();
            bleVersion = auxData ?? "??";
        }

        public string getSoftwareVersion()
        {
            string[] items = MainWindowReference.Title.Split(' ');
            if (items.Length != 4)
                return null;
            else
                return items[3];
        }

        private DateTime RetrieveLastUpdate()
        {
            //Gets the last build from the linker
            string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            byte[] b = new byte[2048];
            System.IO.Stream s = null;

            try
            {
                s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally
            {
                if (s != null)
                {
                    s.Close();
                }
            }

            int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
            return dt;
        }

        
    }


}
