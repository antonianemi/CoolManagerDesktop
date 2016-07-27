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
    /// Interaction logic for FactorySettingsControl.xaml
    /// </summary>
    public partial class FactorySettingsControl : UserControl
    {
        /* Global Variables */
        MainWindow MainWindowReference;

        //SPC
        double valueSPC;
        //Only SPC limits are obtained from the controller

        //LISPC
        double valueLISPC;
        static int limitMaxLISPC;
        static int limitMinLISPC;
        //LSSPC
        double valueLSSPC;
        static int limitMaxLSSPC;
        static int limitMinLSSPC;
        //SID
        double valueSID;
        static int limitMaxSID;
        static int limitMinSID;
        //STD
        double valueSTD;
        static int limitMaxSTD;
        static int limitMinSTD;
        //SDIF
        double valueSDIF;
        static int limitMaxSDIF;
        static int limitMinSDIF;
        //tDI
        double valuetDI;
        static int limitMaxtDI;
        static int limitMintDI;
        //tESO
        double valuetESO;
        static int limitMaxtESO;
        static int limitMintESO;
        //tDUD
        double valuetDUD;
        static int limitMaxtDUD;
        static int limitMintDUD;
        //tDESH
        double valuetDESH;
        static int limitMaxtDESH;
        static int limitMintDESH;
        //tRAD
        double valuetRAD;
        static int limitMaxtRAD;
        static int limitMintRAD;
        //tTCC
        double valuetTCC;
        static int limitMaxtTCC;
        static int limitMintTCC;
        //SPSL
        double valueSPSL;
        static int limitMaxSPSL;
        static int limitMinSPSL;
        //CALMV
        double valueCALMV;
        static int limitMaxCALMV;
        static int limitMinCALMV;
        //tDESH2
        double valuetDESH2;
        static int limitMaxtDESH2;
        static int limitMintDESH2;
        //cDESH
        double valuecDESH;
        static int limitMaxcDESH;
        static int limitMincDESH;
        //TMAX
        double valueTMAX;
        static int limitMaxTMAX;
        static int limitMinTMAX;
        //tDUDM
        double valuetDUDM;
        static int limitMaxtDUDM;
        static int limitMintDUDM;
        //tUD
        double valuetUD;
        static int limitMaxtUD;
        static int limitMintUD;
        //Z
        double valueZ;        
        static int limitMaxZ;
        static int limitMinZ;
        //tGOTEO
        double valuetGOTEO;
        static int limitMaxtGOTEO;
        static int limitMintGOTEO;

        public FactorySettingsControl(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindowReference = mainWindow;

            limitCentigrades();
        }

        public void limitCentigrades()
        {

            limitMaxLISPC = 24;
            limitMinLISPC = -35;

            limitMaxLSSPC = 24;
            limitMinLSSPC = -35;

            limitMaxSID = 24;
            limitMinSID = -35;

            limitMaxSTD = 24;
            limitMinSTD = -35;

            limitMaxSDIF = 24;
            limitMinSDIF = -35;

            limitMaxtDI = 180;
            limitMintDI = 10;

            limitMaxtESO = 10;
            limitMintESO = 0;

            limitMaxtDUD = 1500;
            limitMintDUD = 0;

            limitMaxtDESH = 200;
            limitMintDESH = 0;

            limitMaxtRAD = 20;
            limitMintRAD = 0;

            limitMaxtTCC = 200;
            limitMintTCC = 0;

            limitMaxSPSL = 1000;
            limitMinSPSL = 0;

            limitMaxCALMV = 1000;
            limitMinCALMV = 0;

            limitMaxtDESH2 = 200;
            limitMintDESH2 = 0;

            limitMaxcDESH = 20;
            limitMincDESH = 0;

            limitMaxTMAX = 24;
            limitMinTMAX = -35;

            limitMaxtDUDM = 1500;
            limitMintDUDM = 0;

            limitMaxtUD = 1500;
            limitMintUD = 0;

            limitMaxZ = 10;
            limitMinZ = 0;

            limitMaxtGOTEO = 1500;
            limitMintGOTEO = 0;

        }

        public static void limitFahrenheit()
        {

            limitMaxLISPC = 80;
            limitMinLISPC = -30;

            limitMaxLSSPC = 80;
            limitMinLSSPC = -30;

            limitMaxSID = 80;
            limitMinSID = -30;

            limitMaxSTD = 80;
            limitMinSTD = -30;

            limitMaxSDIF = 80;
            limitMinSDIF = -30;

            limitMaxtDI = 180;
            limitMintDI = 10;

            limitMaxtESO = 10;
            limitMintESO = 0;

            limitMaxtDUD = 1500;
            limitMintDUD = 0;

            limitMaxtDESH = 200;
            limitMintDESH = 0;

            limitMaxtRAD = 20;
            limitMintRAD = 0;

            limitMaxtTCC = 200;
            limitMintTCC = 0;

            limitMaxSPSL = 1000;
            limitMinSPSL = 0;

            limitMaxCALMV = 1000;
            limitMinCALMV = 0;

            limitMaxtDESH2 = 200;
            limitMintDESH2 = 0;

            limitMaxcDESH = 20;
            limitMincDESH = 0;

            limitMaxTMAX = 80;
            limitMinTMAX = -30;

            limitMaxtDUDM = 1500;
            limitMintDUDM = 0;

            limitMaxtUD = 1500;
            limitMintUD = 0;

            limitMaxZ = 18;
            limitMinZ = 0;

            limitMaxtGOTEO = 1500;
            limitMintGOTEO = 0;

        }


        //SPC

        private void ButtonSPC_Set_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.flagInSetFactorySettings = true;
            MainWindowReference.setFactorySettingSPC(txtSPC.Text);
        }

        private void ButtonSPC_Plus_Click(object sender, RoutedEventArgs e)
        {
            double limitMaxSPC = Double.Parse(txtLSSPC.Text);
            valueSPC = Double.Parse(txtSPC.Text);

            if (MainWindow.uCF)
            {
                if (limitMaxSPC > valueSPC)
                {
                    valueSPC++;
                    txtSPC.Text = ("" + (int)valueSPC);
                }
            }
            else
            {
                if (limitMaxSPC > valueSPC)
                {
                    valueSPC += 0.5;
                    txtSPC.Text = ("" + valueSPC);
                }
            }
        }

        private void ButtonSPC_Minus_Click(object sender, RoutedEventArgs e)
        {
            double limitMinSPC = Double.Parse(txtLISPC.Text);
            valueSPC = Double.Parse(txtSPC.Text);

            if (MainWindow.uCF)
            {
                if (valueSPC > limitMinSPC)
                {
                    valueSPC--;
                    txtSPC.Text = ("" + (int)valueSPC);
                }
            }
            else
            {
                if (valueSPC > limitMinSPC)
                {
                    valueSPC -= 0.5;
                    txtSPC.Text = ("" + valueSPC);
                }
            }

        }


        //LI SPC

        private void ButtonLISPC_Set_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.flagInSetFactorySettings = true;
            MainWindowReference.setFactorySettingLISPC(txtLISPC.Text);	
        }

        private void ButtonLISPC_Plus_Click(object sender, RoutedEventArgs e)
        {
            valueLISPC = Double.Parse(txtLISPC.Text);

            if (MainWindow.uCF)
            {
                if (limitMaxLISPC > valueLISPC)
                {
                    valueLISPC++;
                    txtLISPC.Text = ("" + (int)valueLISPC);
                }
            }
            else
            {
                if (limitMaxLISPC > valueLISPC)
                {
                    valueLISPC += 0.5;
                    txtLISPC.Text = ("" + valueLISPC);
                }
            }

        }

        private void ButtonLISPC_Minus_Click(object sender, RoutedEventArgs e)
        {
            valueLISPC = Double.Parse(txtLISPC.Text);
            if (MainWindow.uCF)
            {
                if (valueLISPC > limitMinLISPC)
                {
                    valueLISPC--;
                    txtLISPC.Text = ("" + (int)valueLISPC);
                }
            }
            else
            {
                if (valueLISPC > limitMinLISPC)
                {
                    valueLISPC -= 0.5;
                    txtLISPC.Text = ("" + valueLISPC);
                }
            }
        }

        //LS SPC

        private void ButtonLSSPC_Set_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.flagInSetFactorySettings = true;
            MainWindowReference.setFactorySettingLSSPC(txtLSSPC.Text);		
        }

        private void ButtonLSSPC_Plus_Click(object sender, RoutedEventArgs e)
        {
            valueLSSPC = Double.Parse(txtLSSPC.Text);
            if (MainWindow.uCF)
            {
                if (limitMaxLSSPC > valueLSSPC)
                {
                    valueLSSPC++;
                    txtLSSPC.Text = ("" + (int)valueLSSPC);
                }
            }
            else
            {
                if (limitMaxLSSPC > valueLSSPC)
                {
                    valueLSSPC += 0.5;
                    txtLSSPC.Text = ("" + valueLSSPC);
                }
            }
        }

        private void ButtonLSSPC_Minus_Click(object sender, RoutedEventArgs e)
        {
            valueLSSPC = Double.Parse(txtLSSPC.Text);
            if (MainWindow.uCF)
            {
                if (valueLSSPC > limitMinLSSPC)
                {
                    valueLSSPC--;
                    txtLSSPC.Text = ("" + (int)valueLSSPC);
                }
            }
            else
            {
                if (valueLSSPC > limitMinLSSPC)
                {
                    valueLSSPC -= 0.5;
                    txtLSSPC.Text = ("" + valueLSSPC);
                }
            }
        }

        //SID

        private void ButtonSID_Set_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.flagInSetFactorySettings = true;
            MainWindowReference.setFactorySettingSID(txtSID.Text);	
        }

        private void ButtonSID_Plus_Click(object sender, RoutedEventArgs e)
        {
            valueSID = Double.Parse(txtSID.Text);

            if (limitMaxSID > valueSID)
            {
                valueSID++;
                txtSID.Text = ("" + (int)valueSID);
            }	
        }

        private void ButtonSID_Minus_Click(object sender, RoutedEventArgs e)
        {
            valueSID = Double.Parse(txtSID.Text);
            if (valueSID > limitMinSID)
            {
                valueSID--;
                txtSID.Text = ("" + (int)valueSID);
            }
        }


        //STD

        private void ButtonSTD_Set_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.flagInSetFactorySettings = true;
            MainWindowReference.setFactorySettingSTD(txtSTD.Text);	
        }

        private void ButtonSTD_Plus_Click(object sender, RoutedEventArgs e)
        {
            valueSTD = Double.Parse(txtSTD.Text);
            if (limitMaxSTD > valueSTD)
            {
                valueSTD++;
                txtSTD.Text = ("" + (int)valueSTD);
            }
        }

        private void ButtonSTD_Minus_Click(object sender, RoutedEventArgs e)
        {
            valueSTD = Double.Parse(txtSTD.Text);
            if (valueSTD > limitMinSTD)
            {
                valueSTD--;
                txtSTD.Text = ("" + (int)valueSTD);
            }
        }

        //SDIF

        private void ButtonSDIF_Set_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.flagInSetFactorySettings = true;
            MainWindowReference.setFactorySettingSDIF(txtSDIF.Text);	
        }

        private void ButtonSDIF_Plus_Click(object sender, RoutedEventArgs e)
        {
            valueSDIF = Double.Parse(txtSDIF.Text);
            if (limitMaxSDIF > valueSDIF)
            {
                valueSDIF++;
                txtSDIF.Text = ("" + (int)valueSDIF);
            }
        }

        private void ButtonSDIF_Minus_Click(object sender, RoutedEventArgs e)
        {
            valueSDIF = Double.Parse(txtSDIF.Text);
            if (valueSDIF > limitMinSDIF)
            {
                valueSDIF--;
                txtSDIF.Text  = ("" + (int)valueSDIF);
            }	
        }


        //tDI

        private void ButtontDI_Set_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.flagInSetFactorySettings = true;
            MainWindowReference.setFactorySettingtDI(txttDI.Text);	
        }

        private void ButtontDI_Plus_Click(object sender, RoutedEventArgs e)
        {
            valuetDI = Double.Parse(txttDI.Text);
            if (limitMaxtDI > valuetDI)
            {
                valuetDI++;
                txttDI.Text = ("" + (int)valuetDI);
            }
        }

        private void ButtontDI_Minus_Click(object sender, RoutedEventArgs e)
        {
            valuetDI = Double.Parse(txttDI.Text);
            if (valuetDI > limitMintDI)
            {
                valuetDI--;
                txttDI.Text = ("" + (int)valuetDI);
            }
        }

        //tESO

        private void ButtontESO_Set_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.flagInSetFactorySettings = true;
            MainWindowReference.setFactorySettingtESO(txttESO.Text);
        }

        private void ButtontESO_Plus_Click(object sender, RoutedEventArgs e)
        {
            valuetESO = Double.Parse(txttESO.Text);
            if (limitMaxtESO > valuetESO)
            {
                valuetESO++;
                txttESO.Text = ("" + (int)valuetESO);
            }
        }

        private void ButtontESO_Minus_Click(object sender, RoutedEventArgs e)
        {
            valuetESO = Double.Parse(txttESO.Text);
            if (valuetESO > limitMintESO)
            {
                valuetESO--;
                txttESO.Text  = ("" + (int)valuetESO);
            }	
        }

        //tDUD

        private void ButtontDUD_Set_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.flagInSetFactorySettings = true;
            MainWindowReference.setFactorySettingtDUD(txttDUD.Text);
        }

        private void ButtontDUD_Plus_Click(object sender, RoutedEventArgs e)
        {
            valuetDUD = Double.Parse(txttDUD.Text);
            if (limitMaxtDUD > valuetDUD)
            {
                valuetDUD++;
                txttDUD.Text = ("" + (int)valuetDUD);
            }
        }

        private void ButtontDUD_Minus_Click(object sender, RoutedEventArgs e)
        {
            valuetDUD = Double.Parse(txttDUD.Text);
            if (valuetDUD > limitMintDUD)
            {
                valuetDUD--;
                txttDUD.Text  = ("" + (int)valuetDUD);
            }	
        }


        //tDESH

        private void ButtontDESH_Set_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.flagInSetFactorySettings = true;
            MainWindowReference.setFactorySettingtDESH(txttDESH.Text);
        }

        private void ButtontDESH_Plus_Click(object sender, RoutedEventArgs e)
        {
            valuetDESH = Double.Parse(txttDESH.Text);
            if (limitMaxtDESH > valuetDESH)
            {
                valuetDESH++;
                txttDESH.Text = ("" + (int)valuetDESH);
            }	
        }

        private void ButtontDESH_Minus_Click(object sender, RoutedEventArgs e)
        {
            valuetDESH = Double.Parse(txttDESH.Text);
            if (valuetDESH > limitMintDESH)
            {
                valuetDESH--;
                txttDESH.Text = ("" + (int)valuetDESH);
            }	
        }

        //tDESH2

        private void ButtontDESH2_Set_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.flagInSetFactorySettings = true;
            MainWindowReference.setFactorySettingtDESH2(txttDESH2.Text);
        }

        private void ButtontDESH2_Plus_Click(object sender, RoutedEventArgs e)
        {
            valuetDESH2 = Double.Parse(txttDESH2.Text);
            if (limitMaxtDESH2 > valuetDESH2)
            {
                valuetDESH2++;
                txttDESH2.Text = ("" + (int)valuetDESH2);
            }
        }

        private void ButtontDESH2_Minus_Click(object sender, RoutedEventArgs e)
        {
            valuetDESH2 = Double.Parse(txttDESH2.Text);
            if (valuetDESH2 > limitMintDESH2)
            {
                valuetDESH2--;
                txttDESH2.Text = ("" + (int)valuetDESH2);
            }
        }

        
        //cDESH2
        private void ButtoncDESH_Set_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.flagInSetFactorySettings = true;
            MainWindowReference.setFactorySettingCDESH(txtcDESH.Text);	
        }

        private void ButtoncDESH_Plus_Click(object sender, RoutedEventArgs e)
        {
            valuecDESH = Double.Parse(txtcDESH.Text);
            if (limitMaxcDESH > valuecDESH)
            {
                valuecDESH++;
                txtcDESH.Text = ("" + (int)valuecDESH);
            }
        }

        private void ButtoncDESH_Minus_Click(object sender, RoutedEventArgs e)
        {
            valuecDESH = Double.Parse(txtcDESH.Text);
            if (valuecDESH > limitMincDESH)
            {
                valuecDESH--;
                txtcDESH.Text = ("" + (int)valuecDESH);
            }	
        }

        //TMAX

        private void ButtonTMAX_Set_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.flagInSetFactorySettings = true;
            MainWindowReference.setFactorySettingTMAX(txtTMAX.Text);	
        }

        private void ButtonTMAX_Plus_Click(object sender, RoutedEventArgs e)
        {
            valueTMAX = Double.Parse(txtTMAX.Text);
            if (limitMaxTMAX > valueTMAX)
            {
                valueTMAX++;
                txtTMAX.Text = ("" + (int)valueTMAX);
            }
        }

        private void ButtonTMAX_Minus_Click(object sender, RoutedEventArgs e)
        {
            valueTMAX = Double.Parse(txtTMAX.Text);
            if (valueTMAX > limitMinTMAX)
            {
                valueTMAX--;
                txtTMAX.Text = ("" + (int)valueTMAX);
            }
        }

        // tRAD

        private void ButtontRAD_Set_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.flagInSetFactorySettings = true;
            MainWindowReference.setFactorySettingtRAD(txttRAD.Text);
        }

        private void ButtontRAD_Plus_Click(object sender, RoutedEventArgs e)
        {
            valuetRAD = Double.Parse(txttRAD.Text);
            if (limitMaxtRAD > valuetRAD)
            {
                valuetRAD++;
                txttRAD.Text = ("" + (int)valuetRAD);
            }
        }

        private void ButtontRAD_Minus_Click(object sender, RoutedEventArgs e)
        {
            valuetRAD = Double.Parse(txttRAD.Text);
            if (valuetRAD > limitMintRAD)
            {
                valuetRAD--;
                txttRAD.Text = ("" + (int)valuetRAD);
            }
        }

        //tDUDM

        private void ButtontDUDM_Set_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.flagInSetFactorySettings = true;
            MainWindowReference.setFactorySettingtDUDM(txttDUDM.Text);	
        }

        private void ButtontDUDM_Plus_Click(object sender, RoutedEventArgs e)
        {
            valuetDUDM = Double.Parse(txttDUDM.Text);
            if (limitMaxtDUDM > valuetDUDM)
            {
                valuetDUDM++;
                txttDUDM.Text = ("" + (int)valuetDUDM);
            }
        }

        private void ButtontDUDM_Minus_Click(object sender, RoutedEventArgs e)
        {
            valuetDUDM = Double.Parse(txttDUDM.Text);
            if (valuetDUDM > limitMintDUDM)
            {
                valuetDUDM--;
                txttDUDM.Text = ("" + (int)valuetDUDM);
            }
        }

        //tTCC

        private void ButtontTCC_Set_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.flagInSetFactorySettings = true;
            MainWindowReference.setFactorySettingtTCC(txttTCC.Text);	
        }

        private void ButtontTCC_Plus_Click(object sender, RoutedEventArgs e)
        {
            valuetTCC = Double.Parse(txttTCC.Text);
            if (limitMaxtTCC > valuetTCC)
            {
                valuetTCC++;
                txttTCC.Text = ("" + (int)valuetTCC);
            }
        }

        private void ButtontTCC_Minus_Click(object sender, RoutedEventArgs e)
        {
            valuetTCC = Double.Parse(txttTCC.Text);
            if (valuetTCC > limitMintTCC)
            {
                valuetTCC--;
                txttTCC.Text = ("" + (int)valuetTCC);
            }
        }

        //tUD

        private void ButtontUD_Set_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.flagInSetFactorySettings = true;
            MainWindowReference.setFactorySettingtUD(txttUD.Text);	
        }

        private void ButtontUD_Plus_Click(object sender, RoutedEventArgs e)
        {
            valuetUD = Double.Parse(txttUD.Text);
            if (limitMaxtUD > valuetUD)
            {
                valuetUD++;
                txttUD.Text = ("" + (int)valuetUD);
            }
        }

        private void ButtontUD_Minus_Click(object sender, RoutedEventArgs e)
        {
            valuetUD = Double.Parse(txttUD.Text);
            if (valuetUD > limitMintUD)
            {
                valuetUD--;
                txttUD.Text = ("" + (int)valuetUD);
            }
        }

        //SPSL

        private void ButtonSPSL_Set_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.flagInSetFactorySettings = true;
            MainWindowReference.setFactorySettingSPSL(txtSPSL.Text);
        }

        private void ButtonSPSL_Plus_Click(object sender, RoutedEventArgs e)
        {
            valueSPSL = Double.Parse(txtSPSL.Text);
            if (limitMaxSPSL > valueSPSL)
            {
                valueSPSL++;
                txtSPSL.Text = ("" + (int)valueSPSL);
            }
        }

        private void ButtonSPSL_Minus_Click(object sender, RoutedEventArgs e)
        {
            valueSPSL = Double.Parse(txtSPSL.Text);
            if (valueSPSL > limitMinSPSL)
            {
                valueSPSL--;
                txtSPSL.Text = ("" + (int)valueSPSL);
            }
        }

        //CALMV

        private void ButtonCALMV_Set_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.flagInSetFactorySettings = true;
            MainWindowReference.setFactorySettingCALMV(txtCALMV.Text);
        }

        private void ButtonCALMV_Plus_Click(object sender, RoutedEventArgs e)
        {
            valueCALMV = Double.Parse(txtCALMV.Text);
            if (limitMaxCALMV > valueCALMV)
            {
                valueCALMV++;
                txtCALMV.Text = ("" + (int)valueCALMV);
            }
        }

        private void ButtonCALMV_Minus_Click(object sender, RoutedEventArgs e)
        {
            valueCALMV = Double.Parse(txtCALMV.Text);
            if (valueCALMV > limitMinCALMV)
            {
                valueCALMV--;
                txtCALMV.Text = ("" + (int)valueCALMV);
            }
        }

        //Z

        private void ButtonZ_Set_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.flagInSetFactorySettings = true;
            MainWindowReference.setFactorySettingZ(txtZ.Text);
        }

        private void ButtonZ_Plus_Click(object sender, RoutedEventArgs e)
        {
            valueZ = Double.Parse(txtZ.Text);
            if (MainWindow.uCF)
            {
                if (limitMaxZ > valueZ)
                {
                    valueZ++;
                    txtZ.Text = ("" + (int)valueZ);
                }
            }
            else
            {
                if (limitMaxZ > valueZ)
                {
                    valueZ += 0.5;
                    txtZ.Text = ("" + valueZ);
                }
            }
        }

        private void ButtonZ_Minus_Click(object sender, RoutedEventArgs e)
        {
            valueZ = Double.Parse(txtZ.Text);
            if (MainWindow.uCF)
            {
                if (valueZ > limitMinZ)
                {
                    valueZ--;
                    txtZ.Text = ("" + (int)valueZ);
                }
            }
            else
            {
                if (valueZ > limitMinZ)
                {
                    valueZ -= 0.5;
                    txtZ.Text  = ("" + valueZ);
                }
            }
        }

        //tGOTEO

        private void ButtontGOTEO_Set_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.flagInSetFactorySettings = true;
            MainWindowReference.setFactorySettingtGOTEO(txttGOTEO.Text);
        }

        private void ButtontGOTEO_Plus_Click(object sender, RoutedEventArgs e)
        {
            valuetGOTEO = Double.Parse(txttGOTEO.Text);
            if (limitMaxtGOTEO > valuetGOTEO)
            {
                valuetGOTEO++;
                txttGOTEO.Text = ("" + (int)valuetGOTEO);
            }
        }

        private void ButtontGOTEO_Minus_Click(object sender, RoutedEventArgs e)
        {
            valuetGOTEO = Double.Parse(txttGOTEO.Text);
            if (valuetGOTEO > limitMintGOTEO)
            {
                valuetGOTEO--;
                txttGOTEO.Text = ("" + (int)valuetGOTEO);
            }
        }

        private void ButtonHdfOff_Click(object sender, RoutedEventArgs e)
        {
            MainWindowReference.setFactorySettingHFD(MainWindow.OFF);
        }

        private void ButtonHdfOn_Click(object sender, RoutedEventArgs e)
        {
            MainWindowReference.setFactorySettingHFD(MainWindow.ON);
        }

        private void ButtonUnitC_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.flagInSetFactorySettings = true;
            MainWindowReference.setDegreeUnits(MainWindow.Celsius);
        }

        private void ButtonUnitF_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.flagInSetFactorySettings = true;
            MainWindowReference.setDegreeUnits(MainWindow.Fharenheit);
        }

        private void ButtontFactoryReset_Click(object sender, RoutedEventArgs e)
        {
            MainWindowReference.ShowDialog_OkCancelControl(
                "CAUTION", 
                "Are you sure you want to reset values to factory settings?",
                OkCancelControl.OPTION.FACOTRY_RESET);
        }

        private void ButtontReadValues_Click(object sender, RoutedEventArgs e)
        {
            MainWindowReference.getFactorySettings();
        }

        /** Other **/ 

        private void Back_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindowReference.Show_RelaysControl(true);
        }

        public void updateFactorySettings()
        {
            if (MainWindow.uCF) txtSPC.Text = ("" + (int)MainWindow.fSpc);
            else txtSPC.Text = ("" + MainWindow.fSpc);
            if (MainWindow.uCF) txtLISPC.Text = ("" + (int)MainWindow.fLi_Spc);
            else txtLISPC.Text = ("" + MainWindow.fLi_Spc);
            if (MainWindow.uCF) txtLSSPC.Text = ("" + (int)MainWindow.fLs_Spc);
            else txtLSSPC.Text = ("" + MainWindow.fLs_Spc);

            txtSID.Text = ("" + MainWindow.iSid);
            txtSTD.Text = ("" + MainWindow.iStd);
            txtSDIF.Text = ("" + MainWindow.iSdif);
            txttDI.Text = ("" + MainWindow.iTdi);
            txttESO.Text = ("" + MainWindow.iTeso);
            txttDUD.Text = ("" + MainWindow.iTdud);
            txttDESH.Text = ("" + MainWindow.iTdesh);
            txttRAD.Text = ("" + MainWindow.iTrad);
            txttTCC.Text = ("" + MainWindow.iTtcc);
            txtSPSL.Text = ("" + MainWindow.iSpsl);
            txtCALMV.Text = ("" + MainWindow.iCalmv);
            txttDESH2.Text = ("" + MainWindow.iTdesh2);
            txtcDESH.Text = ("" + MainWindow.uCdesh);
            txtTMAX.Text = ("" + MainWindow.iTmax);
            txttDUDM.Text = ("" + MainWindow.iTDUDM);
            txttUD.Text = ("" + MainWindow.iTud);
            if (MainWindow.uCF) txtZ.Text = ("" + (int)MainWindow.fz);
            else txtZ.Text = ("" + MainWindow.fz);
            txttGOTEO.Text = ("" + MainWindow.iTgoteo);
        }

       
        public void updateuHDFButton()
        {
            if (MainWindow.uHFD)
            {
                btnHfdOn.Background = new SolidColorBrush(Colors.DodgerBlue);
                btnHfdOff.Background = new SolidColorBrush(Colors.Silver);

                btnHfdOn.IsHitTestVisible = false;
                btnHfdOff.IsHitTestVisible = true;
            }
            else
            {
                btnHfdOn.Background = new SolidColorBrush(Colors.Silver);
                btnHfdOff.Background = new SolidColorBrush(Colors.DodgerBlue);

                btnHfdOn.IsHitTestVisible = true;
                btnHfdOff.IsHitTestVisible = false;
            }
        }

        public void updateDegreeButton(bool unit)
        {
            if (unit)
            {
                /*Fahrenheit*/
                btnUnitC.Background = new SolidColorBrush(Colors.Silver);
                btnUnitF.Background = new SolidColorBrush(Colors.DodgerBlue);

                btnUnitC.IsHitTestVisible = true;
                btnUnitF.IsHitTestVisible = false;

                limitFahrenheit();
            }
            else
            {
                /*Celsius*/
                btnUnitC.Background = new SolidColorBrush(Colors.DodgerBlue);
                btnUnitF.Background = new SolidColorBrush(Colors.Silver);

                btnUnitC.IsHitTestVisible = false;
                btnUnitF.IsHitTestVisible = true;

                limitCentigrades();
            }
        }
        
        

        

        

        

        

        

        

        

        
        

        

        

        

        

        

        

       

        

        

        

        

        

        

        

        

        

        

        

        

        

        

        

        

        

        

        

        

        

        

        

        

        

    }
}
