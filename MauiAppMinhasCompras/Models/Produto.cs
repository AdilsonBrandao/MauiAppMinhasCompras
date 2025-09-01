using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiAppMinhasCompras.Models
{
    public class Produto : INotifyPropertyChanged
    {
        private int _id;
        private string _descricao;
        private double _quantidade;
        private double _preco;

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public string Descricao
        {
            get => _descricao;
            set
            {
                _descricao = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Total)); // Atualiza o Total quando a descrição muda
            }
        }

        public double Quantidade
        {
            get => _quantidade;
            set
            {
                _quantidade = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Total)); // Atualiza o Total quando a quantidade muda
            }
        }

        public double Preco
        {
            get => _preco;
            set
            {
                _preco = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Total)); // Atualiza o Total quando o preço muda
            }
        }

        public double Total { get => Quantidade * Preco; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}