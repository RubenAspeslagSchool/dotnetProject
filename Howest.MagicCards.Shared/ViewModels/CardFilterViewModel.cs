using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Howest.MagicCards.Shared.ViewModels
{
    public class CardFilterViewModel : INotifyPropertyChanged
    {
        public string? CardName { get; set; }
        public string? CardText { get; set; }
        public string? ArtistName { get; set; }
        public string? SetCode { get; set; }
        public string? RarityCode { get; set; }
        public int? PageNumber { get; set; } = 1;
        public int? PageSize { get; set; } = 50;
        public int? MaxPageSize { get; set;} = 150;
        public int? orderBy { get; set;}

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
