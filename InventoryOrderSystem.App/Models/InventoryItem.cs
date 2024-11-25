using System.ComponentModel;

namespace InventoryOrderSystem.Models
{
    public class InventoryItem : INotifyPropertyChanged
    {
        private int _itemId;
        private string _name;
        private int _quantity;
        private string _category;
        private string _unit;

        public int ItemId
        {
            get => _itemId;
            set
            {
                if (_itemId != value)
                {
                    _itemId = value;
                    OnPropertyChanged(nameof(ItemId));
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        public string Category
        {
            get => _category;
            set
            {
                if (_category != value)
                {
                    _category = value;
                    OnPropertyChanged(nameof(Category));
                }
            }
        }

        public string Unit
        {
            get => _unit;
            set
            {
                if (_unit != value)
                {
                    _unit = value;
                    OnPropertyChanged(nameof(Unit));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public InventoryItem() { }

        public InventoryItem(int itemId, string name, int quantity, string category, string unit)
        {
            ItemId = itemId;
            Name = name;
            Quantity = quantity;
            Category = category;
            Unit = unit;
        }

        public override string ToString()
        {
            return $"Item ID: {ItemId}, Name: {Name}, Quantity: {Quantity} {Unit}, Category: {Category}";
        }
    }
}