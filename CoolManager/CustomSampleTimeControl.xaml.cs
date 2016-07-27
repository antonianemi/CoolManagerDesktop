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
    /// Interaction logic for CustomSampleTimeControl.xaml
    /// </summary>
    public partial class CustomSampleTimeControl : UserControl
    {

        MainWindow MainWindowReference;

        public CustomSampleTimeControl(MainWindow MainWindow)
        {
            InitializeComponent();
            MainWindowReference = MainWindow;
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                checkInput();
            }
        }

        public void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            checkInput();
        }

        public void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindowReference.rollBackSampleTime();
            MainWindowReference.DialogControl_Back();         
        }

        public void checkInput()
        {
            int result;

            MainWindowReference.DialogControl_Back();

            if (int.TryParse(SampleTime.Text, out result))
            {
                if (result > 0 && result <= 60)
                {
                    MainWindowReference.setSampleTime(result);
                }
                else
                {
                    MainWindowReference.MSG("ERROR", "Input must be between 1 and 60, try again", MSGControl.OPTION.NOTHING);
                    MainWindowReference.rollBackSampleTime();
                }
            }
            else
            {
                MainWindowReference.MSG("ERROR", "Input must be only numbers, try again", MSGControl.OPTION.NOTHING);
                MainWindowReference.rollBackSampleTime();
            }
            SampleTime.Text = "";
            
        }
    }
}
