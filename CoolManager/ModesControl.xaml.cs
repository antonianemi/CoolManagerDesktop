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
    /// Interaction logic for ModesControl.xaml
    /// </summary>
    public partial class ModesControl : UserControl
    {
        MainWindow MainWindowReference;

        public ModesControl(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindowReference = mainWindow;
        }

        /* Foco */

        private void Foco_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindowReference.setMode("0");
            GridFoco.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
        }

        private void Foco_ButtonDown(object sender, MouseButtonEventArgs e)
        {
            GridFoco.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFB8900"));
        }

        private void Foco_MouseLeave(object sender, MouseEventArgs e)
        {
            GridFoco.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
        }

        /* Foco Off */

        private void FocoOff_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindowReference.setMode("1");
            GridFocoOff.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
        }

        private void FocoOff_ButtonDown(object sender, MouseButtonEventArgs e)
        {
            GridFocoOff.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFB8900"));
        }

        private void FocoOff_MouseLeave(object sender, MouseEventArgs e)
        {
            GridFocoOff.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
        }

        /* Eco */

        private void Eco_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindowReference.setMode("2");
            GridEco.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
        }

        private void Eco_ButtonDown(object sender, MouseButtonEventArgs e)
        {
            GridEco.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFB8900"));
        }

        private void Eco_MouseLeave(object sender, MouseEventArgs e)
        {
            GridEco.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
        }

        /* Other */

        private void Back_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindowReference.MessagesControl_Back();
        }
    }
}
