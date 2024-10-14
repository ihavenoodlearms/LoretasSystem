using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace InventoryOrderingSystem
{
    public partial class AddOnEditForm : Form
    {
        private List<string> currentAddOns;
        public List<string> UpdatedAddOns => currentAddOns;

        public AddOnEditForm(List<string> addOns)
        {
            InitializeComponent();
            currentAddOns = new List<string>(addOns);
            PopulateAddOnsList();
        }

        private void PopulateAddOnsList()
        {
            listBoxAddOns.Items.Clear();
            foreach (var addOn in currentAddOns)
            {
                listBoxAddOns.Items.Add(addOn);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string newAddOn = textBoxNewAddOn.Text.Trim();
            if (!string.IsNullOrEmpty(newAddOn) && !currentAddOns.Contains(newAddOn))
            {
                currentAddOns.Add(newAddOn);
                PopulateAddOnsList();
                textBoxNewAddOn.Clear();
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listBoxAddOns.SelectedItem != null)
            {
                currentAddOns.Remove(listBoxAddOns.SelectedItem.ToString());
                PopulateAddOnsList();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}