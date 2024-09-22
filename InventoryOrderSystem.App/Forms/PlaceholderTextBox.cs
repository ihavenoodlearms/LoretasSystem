using System;
using System.Drawing;
using System.Windows.Forms;

namespace InventoryOrderSystem
{
    public class PlaceholderTextBox : TextBox
    {
        private string _placeholderText;
        private Color _placeholderColor = Color.Gray;
        private bool _isPassword;

        public string PlaceholderText
        {
            get { return _placeholderText; }
            set
            {
                _placeholderText = value;
                SetPlaceholder();
            }
        }

        public bool IsPassword
        {
            get { return _isPassword; }
            set
            {
                _isPassword = value;
                if (_isPassword)
                {
                    PasswordChar = '•';
                }
            }
        }

        public PlaceholderTextBox()
        {
            GotFocus += RemovePlaceholder;
            LostFocus += SetPlaceholder;
        }

        private void RemovePlaceholder(object sender, EventArgs e)
        {
            if (Text == _placeholderText)
            {
                Text = "";
                ForeColor = SystemColors.WindowText;
                if (_isPassword)
                {
                    PasswordChar = '•';
                }
            }
        }

        private void SetPlaceholder(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Text))
            {
                Text = _placeholderText;
                ForeColor = _placeholderColor;
                if (_isPassword)
                {
                    PasswordChar = '\0';
                }
            }
        }

        private void SetPlaceholder()
        {
            if (string.IsNullOrEmpty(Text) || Text == _placeholderText)
            {
                Text = _placeholderText;
                ForeColor = _placeholderColor;
                if (_isPassword)
                {
                    PasswordChar = '\0';
                }
            }
        }
    }
}