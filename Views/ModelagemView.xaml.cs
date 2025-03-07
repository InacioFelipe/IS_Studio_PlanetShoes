using ISStudioCorel;
using System;
using System.Globalization;
using System.Windows;

namespace IS_Studio_PlanetShoes.Views
{
    /// <summary>
    /// Interação lógica para ModelagemView.xam
    /// </summary>
    public partial class ModelagemView : System.Windows.Controls.Page
    {
        private UtilitariosCorel utilitarios = new UtilitariosCorel();
        public ModelagemView()
        {
            InitializeComponent();
            cbxPartOfString.Items.Clear();
            cbxPartOfString.Items.Add(1);
            cbxPartOfString.Items.Add(2);
            cbxPartOfString.Items.Add(3);
            cbxPartOfString.SelectedIndex = 1;
            chkReverse.IsChecked = true;
        }

        private void btnGetPropertiesOfSourceObject_Click(object sender, RoutedEventArgs e)
        {
            double witdh = utilitarios.GetSizeWidth();
            if (witdh != 0)
            {
                double heigth = utilitarios.GetSizeHeight();
                if (heigth != 0)
                {
                    txtSizeWidthSource.Text = witdh.ToString();
                    txtSizeHeightSource.Text = heigth.ToString();
                }
            }
        }

        private void btnGetPropertiesOfDestinyObject_Click(object sender, RoutedEventArgs e)
        {
            double witdh = utilitarios.GetSizeWidth();
            if (witdh != 0)
            {
                double heigth = utilitarios.GetSizeHeight();
                if (heigth != 0)
                {
                    txtSizeWidthTarget.Text = witdh.ToString();
                    txtSizeHeightTarget.Text = heigth.ToString();
                }
            }
        }

        private void btnReplacePropertyOfObject_Click(object sender, RoutedEventArgs e)
        {
            double w = Convert.ToDouble(txtSizeWidthSource.Text);
            double h = Convert.ToDouble(txtSizeHeightSource.Text);
            double toW = Convert.ToDouble(txtSizeWidthTarget.Text);
            double toH = Convert.ToDouble(txtSizeHeightTarget.Text);

            utilitarios.ReplaceVectorsWithSize(w,h,toW,toH);

        }

        private void btnIdentifiesAndCalculatesTheValueOfProgressionX_Click(object sender, RoutedEventArgs e)
        {
            txtProgressionValueX.Text = utilitarios.GetValueForProgressionFactor("width").ToString("F3");
            //cbxListOfShapesX.Items.Clear();
            cbxListOfShapesX.ItemsSource = utilitarios.GetListProgressionsFactors("width");
            cbxListOfShapesX.SelectedIndex = 0;
        }

        private void btnIdentifiesAndCalculatesTheValueOfProgressionY_Click(object sender, RoutedEventArgs e)
        {
            txtProgressionValueY.Text = utilitarios.GetValueForProgressionFactor("height").ToString("F3");
            //cbxListOfShapesY.Items.Clear();
            cbxListOfShapesY.ItemsSource = utilitarios.GetListProgressionsFactors("height");
            cbxListOfShapesY.SelectedIndex = 0;
        }

        private void btnProgressApply_Click(object sender, RoutedEventArgs e)
        {

            //global::System.Windows.MessageBox.Show("Tentando efetuar escala ...");

            int numScale = 0;
            int numDownProgress=0;
            int numUpperProgress=0;
            double idWidth = 0;
            double idHeight = 0;

            bool successNumScale = int.TryParse(txtPiloto.Text, out numScale);
            bool successDown = int.TryParse(txtNumberOfProgressionsToDown.Text, out numDownProgress);
            bool successUpper = int.TryParse(txtNumberOfProgressionsToUpper.Text, out numUpperProgress);
            bool successIdWidth = double.TryParse(txtProgressionValueX.Text, out idWidth);
            bool successIdHeight = double.TryParse(txtProgressionValueY.Text, out idHeight);

            if (!successNumScale)
            {
                global::System.Windows.MessageBox.Show("Não foi possível obter o valor para o piloto da escala");
            }

            if (!successDown)
            {
                global::System.Windows.MessageBox.Show("Não foi possível obter o valor Inferior da escala");
            }

            if (!successUpper)
            {
                global::System.Windows.MessageBox.Show("Não foi possível obter o valor Superior da escala");
            }

            if (!successIdWidth)
            {
                global::System.Windows.MessageBox.Show("Não foi possível obter o valor de indice para largura");
            }

            if (!successIdHeight)
            {
                global::System.Windows.MessageBox.Show("Não foi possível obter o valor de indice para a altura");
            }

            utilitarios.CreateScaleWithValues(numScale, numDownProgress, numUpperProgress, idWidth, idHeight);

        }

        private void btnPutMark34_Click(object sender, RoutedEventArgs e)
        {
            global::System.Windows.MessageBox.Show("Put the pike 34 in a select shape");
        }

        private void btnPutMark35_Click(object sender, RoutedEventArgs e)
        {
            global::System.Windows.MessageBox.Show("Put the pike 35 in a select shape");
        }

        private void btnCreateAndMoveObjectToLayerWithTheNameOfObject_Click(object sender, RoutedEventArgs e)
        {
            utilitarios.MoveToLayerWithTheShapeName(Convert.ToInt16(cbxPartOfString.Text),Convert.ToBoolean(chkReverse.IsChecked));
            cbxListOfLayers.ItemsSource = utilitarios.GetAllLayers();
            cbxListOfLayers.SelectedIndex = 0;
        }

        private void btnPopulaListaComCamadas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cbxListOfLayers.ItemsSource = utilitarios.GetAllLayers();
                cbxListOfLayers.SelectedIndex = 0;
            }
            catch (Exception ex)
            {

                global::System.Windows.MessageBox.Show($"Unable to update layer list\n{ex}");
            }
            
        }

        private void btnEnviaSelecaoParaCamada_Click(object sender, RoutedEventArgs e)
        {
           
            utilitarios.SendSelectionToLayer(cbxListOfLayers.Text);
            //global::System.Windows.MessageBox.Show($"Send to layer");
        }

        private void btnTeste_03(object sender, RoutedEventArgs e)
        {
            global::System.Windows.MessageBox.Show("Botao de teste_03");
        }
    }
}
