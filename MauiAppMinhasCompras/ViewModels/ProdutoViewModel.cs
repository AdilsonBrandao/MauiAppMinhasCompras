using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiAppMinhasCompras.ViewModels
{
    public class ProdutoViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Produto> _produtos;
        private ObservableCollection<Produto> _produtosFiltrados;
        private string _termoBusca;
        private double _total;

        public ObservableCollection<Produto> Produtos
        {
            get => _produtos;
            set
            {
                _produtos = value;
                OnPropertyChanged();
                CalcularTotal();
            }
        }

        public ObservableCollection<Produto> ProdutosFiltrados
        {
            get => _produtosFiltrados;
            set
            {
                _produtosFiltrados = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ContadorProdutos));
            }
        }

        public string TermoBusca
        {
            get => _termoBusca;
            set
            {
                if (_termoBusca != value)
                {
                    _termoBusca = value;
                    OnPropertyChanged();
                    FiltrarProdutos();
                }
            }
        }

        public double Total
        {
            get => _total;
            set
            {
                _total = value;
                OnPropertyChanged();
            }
        }

        public string ContadorProdutos => $"{ProdutosFiltrados?.Count ?? 0} produto(s) encontrado(s)";

        public ProdutoViewModel()
        {
            Produtos = new ObservableCollection<Produto>();
            ProdutosFiltrados = new ObservableCollection<Produto>();
            CarregarProdutos();
        }

        public async void CarregarProdutos()
        {
            try
            {
                var produtos = await App.Db.GetAll();
                Produtos = new ObservableCollection<Produto>(produtos);
                ProdutosFiltrados = new ObservableCollection<Produto>(produtos);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Falha ao carregar produtos: {ex.Message}", "OK");
            }
        }

        private void FiltrarProdutos()
        {
            if (string.IsNullOrWhiteSpace(TermoBusca))
            {
                ProdutosFiltrados = new ObservableCollection<Produto>(Produtos);
                return;
            }

            var termo = TermoBusca.ToLower();

            var produtosFiltrados = Produtos
                .Where(p => p.Descricao.ToLower().Contains(termo))
                .ToList();

            ProdutosFiltrados = new ObservableCollection<Produto>(produtosFiltrados);
        }

        public async Task AdicionarProduto(Produto produto)
        {
            try
            {
                await App.Db.Insert(produto);
                await CarregarProdutosAsync(); // Recarrega a lista
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Falha ao adicionar produto: {ex.Message}", "OK");
            }
        }

        public async Task RemoverProduto(int id)
        {
            try
            {
                await App.Db.Delete(id);
                await CarregarProdutosAsync(); // Recarrega a lista
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Falha ao remover produto: {ex.Message}", "OK");
            }
        }

        public async Task AtualizarProduto(Produto produto)
        {
            try
            {
                await App.Db.Update(produto);
                await CarregarProdutosAsync(); // Recarrega a lista
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Falha ao atualizar produto: {ex.Message}", "OK");
            }
        }

        private async Task CarregarProdutosAsync()
        {
            var produtos = await App.Db.GetAll();
            Produtos = new ObservableCollection<Produto>(produtos);

            // Reaplica o filtro se houver um termo de busca
            if (!string.IsNullOrWhiteSpace(TermoBusca))
            {
                FiltrarProdutos();
            }
            else
            {
                ProdutosFiltrados = new ObservableCollection<Produto>(Produtos);
            }
        }

        private void CalcularTotal()
        {
            Total = ProdutosFiltrados?.Sum(i => i.Total) ?? 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}