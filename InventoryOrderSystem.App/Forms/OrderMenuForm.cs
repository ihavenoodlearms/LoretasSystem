using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace InventoryOrderingSystem
{
    public partial class OrderMenuForm : Form
    {
        private Dictionary<string, List<Product>> categoryProducts;
        private List<Product> selectedProducts;

        public OrderMenuForm()
        {
            InitializeComponent();
            InitializeCategories();
            selectedProducts = new List<Product>();

            btnCoffeesTeas.Click += (s, e) => LoadCategory("COFFEES & TEAS");
            btnSnacks.Click += (s, e) => LoadCategory("SNACKS");
            btnFrappes.Click += (s, e) => LoadCategory("FRAPPES, FRUIT MILKS, & MORE");

            // Load default category
            LoadCategory("COFFEES & TEAS");
        }

        private void InitializeCategories()
        {
            string imagePath = Path.Combine(Application.StartupPath, "Resources");
            categoryProducts = new Dictionary<string, List<Product>>
    {
        {"COFFEES & TEAS", new List<Product>
            {
                new Product("Americano", 98.00m, Path.Combine(imagePath, "americano.jpeg")),
                new Product("Cappuccino", 98.00m, Path.Combine(imagePath, "cappucino.jpeg")),
                new Product("Cafe Mocha", 98.00m, Path.Combine(imagePath, "cafe_mocha.jpeg")),
                new Product("Caramel Macchiato", 98.00m, Path.Combine(imagePath, "caramel_macchiato.jpeg"))
            }
        },
        {"SNACKS", new List<Product>()},
        {"FRAPPES, FRUIT MILKS, & MORE", new List<Product>()}
    };
        }

        private void LoadCategory(string category)
        {
            labelIcedCoffees.Text = category.ToUpper();
            flowLayoutPanelProducts.Controls.Clear();

            foreach (var product in categoryProducts[category])
            {
                var productControl = new ProductUserControl(product);
                productControl.OnProductSelected += ProductControl_OnProductSelected;
                flowLayoutPanelProducts.Controls.Add(productControl);
            }
        }

        private void ProductControl_OnProductSelected(object sender, ProductSelectedEventArgs e)
        {
            if (e.IsSelected)
            {
                selectedProducts.Add(e.Product);
            }
            else
            {
                selectedProducts.Remove(e.Product);
            }
            UpdateOrderSummary();
        }

        private void UpdateOrderSummary()
        {
            // Implement order summary update logic here
            // This could update a ListBox or Label with the current order details
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }

        public Product(string name, decimal price, string imagePath)
        {
            Name = name;
            Price = price;
            ImagePath = imagePath;
        }
    }

    public class ProductUserControl : UserControl
    {
        public event EventHandler<ProductSelectedEventArgs> OnProductSelected;
        private Product _product;

        public ProductUserControl(Product product)
        {
            _product = product;
            InitializeComponent(product);
        }

        private void InitializeComponent(Product product)
        {
            this.Size = new Size(180, 250);

            var pictureBox = new PictureBox
            {
                Image = Image.FromFile(product.ImagePath),
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(160, 160),
                Location = new Point(10, 10)
            };

            var nameLabel = new Label
            {
                Text = product.Name,
                AutoSize = true,
                Location = new Point(10, 180)
            };

            var priceLabel = new Label
            {
                Text = $"₱{product.Price:F2}",
                AutoSize = true,
                Location = new Point(10, 200)
            };

            var checkBox = new CheckBox
            {
                Text = "Add to Order",
                AutoSize = true,
                Location = new Point(10, 220)
            };

            checkBox.CheckedChanged += (s, e) =>
            {
                OnProductSelected?.Invoke(this, new ProductSelectedEventArgs(_product, checkBox.Checked));
            };

            this.Controls.Add(pictureBox);
            this.Controls.Add(nameLabel);
            this.Controls.Add(priceLabel);
            this.Controls.Add(checkBox);
        }
    }

    public class ProductSelectedEventArgs : EventArgs
    {
        public Product Product { get; }
        public bool IsSelected { get; }

        public ProductSelectedEventArgs(Product product, bool isSelected)
        {
            Product = product;
            IsSelected = isSelected;
        }
    }
}