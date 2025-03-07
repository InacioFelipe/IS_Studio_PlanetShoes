using System.Windows;
using System.Windows.Controls;
using IS_Studio_PlanetShoes.ViewModels;
using IS_Studio_PlanetShoes.Services;

namespace IS_Studio_PlanetShoes.Views
{
    /// <summary>
    /// Interação lógica para CatalogoView.xam
    /// </summary>
    public partial class CatalogoView : Page
    {

        private CatalogoViewModel viewModel = new CatalogoViewModel();
        private CorelService _corelService = new CorelService();
        public CatalogoView()
        {
            InitializeComponent();
        }

        private void btnOrcamentoMateus(object sender, RoutedEventArgs e)
        {
            //double lengthResult = _corelService.GetVectorsLength();
            //double budgetResult = lengthResult * 0.07;
            //txtLengthResult.Text = lengthResult.ToString("F3");
            //txtBudgetResult.Text = budgetResult.ToString("F3");
            _corelService.CreateCardOfBudget();
        }

        private void btnOrcamentoUsinova(object sender, RoutedEventArgs e)
        {
            double lengthResult = _corelService.GetVectorsLength();
            double budgetResult = lengthResult * 0.25;
            txtLengthResult.Text = lengthResult.ToString("F3");
            txtBudgetResult.Text = budgetResult.ToString("F3");
        }
    }
}
