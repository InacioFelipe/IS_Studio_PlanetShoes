using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using corel = Corel.Interop.VGCore;

namespace IS_Studio_PlanetShoes
{
    public partial class DockerUI : UserControl
    {
        private corel.Application corelApp;
        private Styles.StylesController stylesController;
        public DockerUI(object app)
        {
            InitializeComponent();
            try
            {
                CultureInfo.CurrentCulture = new CultureInfo("pt-BR");
                CultureInfo.CurrentUICulture = new CultureInfo("pt-BR");

                this.corelApp = app as corel.Application;
                stylesController = new Styles.StylesController(this.Resources, this.corelApp);
            }
            catch
            {
                global::System.Windows.MessageBox.Show("VGCore Erro");
            }

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            stylesController.LoadThemeFromPreference();
            ContainerAbas.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEFE00"));
        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Uri destinationPage = null;

            // Lógica para lidar com a mudança de seleção da aba
            if (e.Source is System.Windows.Controls.TabControl tabControl)
            {
                if (tabControl.SelectedItem is System.Windows.Controls.TabItem selectedTab)
                {
                    // Aqui você pode atualizar o conteúdo com base na aba selecionada
                    if (selectedTab.Header.ToString() == "Modelagem")
                    {
                        //destinationPage = new Uri("Views\\ModelagemView.xaml", UriKind.Relative);
                    }
                    else if (selectedTab.Header.ToString() == "Desenho")
                    {
                        //destinationPage = new Uri("Views\\DesenhoView.xaml", UriKind.Relative);
                    }
                    else if (selectedTab.Header.ToString() == "Catalogo")
                    {
                        destinationPage = new Uri("Views\\CatalogoView.xaml", UriKind.Relative);
                    }
                    else if (selectedTab.Header.ToString() == "Config")
                    {
                        destinationPage = new Uri("Views\\ConfiguracoesView.xaml", UriKind.Relative);
                    }

                    if (destinationPage != null)
                    {
                        ContainerAbas.Source = destinationPage;
                        ContainerAbas.Navigate(ContainerAbas.Source, corelApp);
                    }
                }
            }
        }
    }
}
