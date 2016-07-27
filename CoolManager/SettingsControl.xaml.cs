using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Lógica de interacción para SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {

        MainWindow MainWindowReference;

        public SettingsControl(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindowReference = mainWindow;
        }

        //*ChangeButton*
        private void ChangeButton_LeftMouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangeButton.Background = new ImageBrush((ImageSource)FindResource("ImgBtnAzUp"));
            MainWindowReference.Show_ChangesControl();
        }

        private void ChangeButton_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeButton.Background = new ImageBrush((ImageSource)FindResource("ImgBtnAzDown"));
        }

        private void ChangeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeButton.Background = new ImageBrush((ImageSource)FindResource("ImgBtnAzUp"));
        }


        //*SPEnableButton*
        private void SPEnableButton_LeftMouseUp(object sender, MouseButtonEventArgs e)
        {
            SPEnableButton.Background = new ImageBrush((ImageSource)FindResource("ImgBtnAzUp"));
            
            if (MainWindow.uSPBtnEnable)
            {
                MainWindowReference.setFactorySettingSPButton(MainWindow.OFF);
            }
            else
            {
                MainWindowReference.setFactorySettingSPButton(MainWindow.ON);
            }

        }

        private void SPEnableButton_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            SPEnableButton.Background = new ImageBrush((ImageSource)FindResource("ImgBtnAzDown"));
        }

        private void SPEnableButton_MouseLeave(object sender, MouseEventArgs e)
        {
            SPEnableButton.Background = new ImageBrush((ImageSource)FindResource("ImgBtnAzUp"));
        }


        //*ErasePasswordsButton*
        private void ErasePasswordsButton_LeftMouseUp(object sender, MouseButtonEventArgs e)
        {
            ErasePasswordsButton.Background = new ImageBrush((ImageSource)FindResource("ImgBtnAzUp"));

            MainWindowReference.ShowDialog_OkCancelControl(
                         "ALERT",
                         "Are you sure that you want to erase all saved passwords?, This cannot be undone.",
                         OkCancelControl.OPTION.ERASE_SAVED_PASSWORDS);
        }

        private void ErasePasswordsButton_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            ErasePasswordsButton.Background = new ImageBrush((ImageSource)FindResource("ImgBtnAzDown"));
        }

        private void ErasePasswordsButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ErasePasswordsButton.Background = new ImageBrush((ImageSource)FindResource("ImgBtnAzUp"));
        }
    }
}
