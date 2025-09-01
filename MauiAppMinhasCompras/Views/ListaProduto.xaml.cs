using MauiAppMinhasCompras.Models;
using MauiAppMinhasCompras.ViewModels;

namespace MauiAppMinhasCompras.Views
{
    public partial class ListaProduto : ContentPage
    {
        private ProdutoViewModel ViewModel => BindingContext as ProdutoViewModel;

        public ListaProduto()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel?.CarregarProdutos();
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new Views.NovoProduto());
            }
            catch (Exception ex)
            {
                DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            double soma = ViewModel?.Total ?? 0;
            string msg = $"O total é {soma:C}";
            DisplayAlert("Total dos Produtos", msg, "OK");
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var produto = menuItem?.CommandParameter as Produto;

            if (produto != null)
            {
                bool confirmar = await DisplayAlert("Confirmar", $"Deseja remover {produto.Descricao}?", "Sim", "Não");

                if (confirmar)
                {
                    try
                    {
                        await ViewModel.RemoverProduto(produto.Id);
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Erro", $"Falha ao remover: {ex.Message}", "OK");
                    }
                }
            }
        }
    }
}