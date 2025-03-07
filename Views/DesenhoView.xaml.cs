using IS_Studio_Corel;
using ISStudioCorel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace IS_Studio_PlanetShoes.Views
{
    /// <summary>
    /// Interação lógica para DesenhoView.xam
    /// </summary>
    public partial class DesenhoView : Page
    {
        private UtilitariosCorel utilitarios = new UtilitariosCorel();
        

        public DesenhoView()
        {
            InitializeComponent();
        }

        private void btnGetCurveLength_Click(object sender, RoutedEventArgs e)
        {
            //global::System.Windows.MessageBox.Show("Get Curves Length");
            txtCurveLength.Text= utilitarios.GetVectorsLength().ToString("F3");
        }

        private void btnCreateQuoteForNavalhaUniao_Click(object sender, RoutedEventArgs e)
        {
            //global::System.Windows.MessageBox.Show("Create Quote for Navalha União");
            //utilitarios.CreateCardWithTheBudgetRazors(utilitarios.GetRazorsLength().ToString());
        }

        private void btnCreatePinoMola_Click(object sender, RoutedEventArgs e)
        {
            //global::System.Windows.MessageBox.Show("Create Quote for Navalha União");
            utilitarios.PlacePinoMolaByClicking();
        }

        private void btnCreateVazador1mm_Click(object sender, RoutedEventArgs e)
        {
            utilitarios.PlaceVazadorByClicking(1);
        }

        private void btnCreateVazador15mm_Click(object sender, RoutedEventArgs e)
        {
            utilitarios.PlaceVazadorByClicking(1.5);
        }

        private void btnCreateVazador2mm_Click(object sender, RoutedEventArgs e)
        {
            utilitarios.PlaceVazadorByClicking(2);
        }

        private void btnRenameShapes_Click(object sender, RoutedEventArgs e)
        {
            //global::System.Windows.MessageBox.Show("Rename Shapes of the Selection");

            char strSeparator='\0'; // vazio
            if (radUnderscore.IsChecked == true) strSeparator = '_';
            if (radHifen.IsChecked == true) strSeparator = '-';
            if (radSpace.IsChecked == true) strSeparator = ' ';

            string strEnum = string.Empty; // vazio
            if (radAuto.IsChecked == true) strEnum = "auto";
            if (radId.IsChecked == true) strEnum = "id";
            if (radSemEnumeracao.IsChecked == true) strEnum = "sem";

            utilitarios.NameShapesWithParameters(txtPrefix.Text, txtName.Text, txtSuffix.Text, strSeparator, strEnum, txtInicioEnum.Text, txtPassoEnum.Text);
        }

        private void txtPrefix_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPrefix.Text = "";
        }

        private void txtName_GotFocus(object sender, RoutedEventArgs e)
        {
            txtName.Text = "";
        }

        private void txtSuffix_GotFocus(object sender, RoutedEventArgs e)
        {
            txtSuffix.Text = "";
        }

        private void txtPrefix_LostFocus(object sender, RoutedEventArgs e)
        {
            if(txtPrefix.Text == "") {
                txtPrefix.Text = "Prefix";
            }
            else
            {
                txtPrefix.Text = txtPrefix.Text;
            }
        }

        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {
            if( txtName.Text == "") {
                txtName.Text = "Name";
            }
            else
            {
                txtName.Text = txtName.Text;
            }
        }

        private void txtSuffix_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtSuffix.Text == "") {
                txtSuffix.Text = "Suffix";
            }
            else
            {
                txtSuffix.Text = txtSuffix.Text;
            }
        }

        private void btnFrameAroundSelection_Click(object sender, RoutedEventArgs e)
        {
            utilitarios.CreateFrameArroundTheSelection();
        }

        private void txtInicioEnum_GotFocus(object sender, RoutedEventArgs e)
        {
            txtInicioEnum.Text = "";
            radHifen.IsChecked = false;
            radId.IsChecked = false;
            radAuto.IsChecked = true;
        }

        private void txtInicioEnum_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtInicioEnum.Text == "")
            {
                txtInicioEnum.Text = "0";
            }
            else
            {
                txtInicioEnum.Text = txtInicioEnum.Text;
            }
        }

        private void txtPassoEnum_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPassoEnum.Text = "";
            radHifen.IsChecked = false;
            radId.IsChecked = false;
            radAuto.IsChecked = true;
        }

        private void txtPassoEnum_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtPassoEnum.Text == "")
            {
                txtPassoEnum.Text = "0";
            }
            else
            {
                txtPassoEnum.Text = txtPassoEnum.Text;
            }
        }

        private void btnCreateFrameForSilkScreen_Click(object sender, RoutedEventArgs e)
        {
            //global::System.Windows.MessageBox.Show("Iniciando procedimento para criar telas de Silk ...");
            double frameWidth;
            if (!double.TryParse(txtFrameSizeWidth.Text, out frameWidth))
            {
                global::System.Windows.MessageBox.Show("Digite um valor numérico para a largura do quadro");
                return;
            }
            double frameHeight;
            if (!double.TryParse(txtFrameSizeHeight.Text, out frameHeight))
            {
                global::System.Windows.MessageBox.Show("Digite um valor numérico para a altura do quadro");
                return;
            }
            double frameBorder;
            if (!double.TryParse(txtFrameBorder.Text, out frameBorder))
            {
                global::System.Windows.MessageBox.Show("Digite um valor numérico para a largura da borda do quadro");
                return;
            }
            double printHeight;
            if (!double.TryParse(txtPrintHeight.Text, out printHeight))
            {
                global::System.Windows.MessageBox.Show("Digite um valor numérico para a altura da gravação");
                return;
            }

            utilitarios.CreateSilkScreen(frameWidth, frameHeight, frameBorder, printHeight);
        }

        private void btnCreateOffset_Click(object sender, RoutedEventArgs e)
        {
            int numberOffset;
            if (!int.TryParse(txtNumberOffset.Text, out numberOffset))
            {
                global::System.Windows.MessageBox.Show("Digite um valor numérico inteiro para o numero de offsets");
                return;
            }
            double distanceOffset;
            if (!double.TryParse(txtDistanceOffset.Text, out distanceOffset))
            {
                global::System.Windows.MessageBox.Show("Digite um valor numérico para o numero de offsets");
                return;
            }

            utilitarios.CreatOffseArroundTheSelection(numberOffset, distanceOffset, 0.01, "");
        }

        private void txtNumberOffset_GotFocus(object sender, RoutedEventArgs e)
        {
            txtNumberOffset.Text = "";
        }

        private void txtNumberOffset_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtNumberOffset.Text == "")
            {
                txtNumberOffset.Text = "1";
            }
            else
            {
                txtNumberOffset.Text = txtNumberOffset.Text;
            }
        }

        private void txtDistanceOffset_GotFocus(object sender, RoutedEventArgs e)
        {
            txtDistanceOffset.Text = "";
        }

        private void txtDistanceOffset_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtDistanceOffset.Text == "")
            {
                txtDistanceOffset.Text = "5";
            }
            else
            {
                txtDistanceOffset.Text = txtDistanceOffset.Text;
            }
        }

        private void radAuto_Checked(object sender, RoutedEventArgs e)
        {
            txtInicioEnum.Text = "1";
            txtPassoEnum.Text = "1";
        }

        private void btnOrderByDownTop_Click(object sender, RoutedEventArgs e)
        {
            //utilitarios.OrderObjectsOnLayer("y","desc");
        }

        private void btnOrderByLeftRight_Click(object sender, RoutedEventArgs e)
        {
           //utilitarios.OrderObjectsOnLayer("x", "desc");
        }

        private void btnOrderByTopDown_Click(object sender, RoutedEventArgs e)
        {
            //utilitarios.OrderObjectsOnLayer("y", "asc");
        }

        private void btnOrderByRightLeft_Click(object sender, RoutedEventArgs e)
        {
            //utilitarios.OrderObjectsOnLayer("x", "asc");
        }

        private void btnCreateBoundaryMarks_Click(object sender, RoutedEventArgs e)
        {
            utilitarios.CreateBoundaryMarks(10);
        }
    }
}
