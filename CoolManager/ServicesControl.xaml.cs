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
    /// Interaction logic for ServicesControl.xaml
    /// </summary>
    public partial class ServicesControl : UserControl
    {

        MainWindow MainWindowReference;
        private const string PASSWORD = "frt2013CT";

        public ServicesControl(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindowReference = mainWindow;
        }

        private void password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)  
            {
                if (passwordBox.Password.Equals(PASSWORD))
                {
                    Console.WriteLine("Password Correct");
                    MainWindowReference.Show_RelaysControl(false);                    
                }
                else
                {
                    MainWindowReference.MSG("ALERT", "Invalid Password", MSGControl.OPTION.NOTHING); 
                }
                passwordBox.Password = "";
            }  
        }
    }
}
