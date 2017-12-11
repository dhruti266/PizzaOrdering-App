using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Full_Moon_Pizza
{
    public partial class FullMoonPizza : Form
    {
        private string order;
        private double totalCost;
        private double lblTotalCost = 0.0;
        private double lblPopCost = 0.0;
        private double lblJuiceCost = 0.0;
        private double lblSizeCost = 0.0;
        public FullMoonPizza()
        {
            InitializeComponent();
            // home tab - make all lables transperant.
            welcome.Parent = pictureBoxHome;
            welcome.BackColor = Color.Transparent;

            // custome pizza tab 
            Size.Parent = pictureBoxCustomize;
            Size.BackColor = Color.Transparent;
            Crust.Parent = pictureBoxCustomize;
            Crust.BackColor = Color.Transparent;
            Sauce.Parent = pictureBoxCustomize;
            Sauce.BackColor = Color.Transparent;
            Toppings.Parent = pictureBoxCustomize;
            Toppings.BackColor = Color.Transparent;
            SpecialPizza.Parent = pictureBoxCustomize;
            SpecialPizza.BackColor = Color.Transparent;

            //Beverage tab
            Pop.Parent = pictureBoxBeverages;
            Pop.BackColor = Color.Transparent;
            Juice.Parent = pictureBoxBeverages;
            Juice.BackColor = Color.Transparent;
            DippingSauce.Parent = pictureBoxBeverages;
            DippingSauce.BackColor = Color.Transparent;

            //other item tab
            BuyOtherItems.Parent = pictureBoxOtherItem;
            BuyOtherItems.BackColor = Color.Transparent;

            // customer info tab
            FullName.Parent = pictureBoxCustInfo;
            FullName.BackColor = Color.Transparent;
            Address.Parent = pictureBoxCustInfo;
            Address.BackColor = Color.Transparent;
            Phone1.Parent = pictureBoxCustInfo;
            Phone1.BackColor = Color.Transparent;
            Fname.Parent = pictureBoxCustInfo;
            Fname.BackColor = Color.Transparent;
            Lname.Parent = pictureBoxCustInfo;
            Lname.BackColor = Color.Transparent;

            //contact us tab
            Phone.Parent = pictureBoxContact;
            Phone.BackColor = Color.Transparent;
            Location.Parent = pictureBoxContact;
            Location.BackColor = Color.Transparent;
            LocationName.Parent = pictureBoxContact;
            LocationName.BackColor = Color.Transparent;
            PhoneNumber.Parent = pictureBoxContact;
            PhoneNumber.BackColor = Color.Transparent;
            Email.Parent = pictureBoxContact;
            Email.BackColor = Color.Transparent;
            EmailName.Parent = pictureBoxContact;
            EmailName.BackColor = Color.Transparent;
        }
        private void FullMoonPizza_Load(object sender, EventArgs e)
        {
            DisplayCurrentTotal(lblTotalCost);

            comboBoxSize.SelectedIndex = 1;
            comboBoxCrust.SelectedIndex = 0;
            comboBoxSauce.SelectedIndex = 0;
            comboBoxSpecialPizza.SelectedIndex = 0;
            comboBoxPop.SelectedIndex = 0;
            comboBoxJuice.SelectedIndex = 0;
            order = "";
            totalCost = 0;
        }

        private void ResetOrder()
        {
            comboBoxSize.SelectedIndex = 1;
            comboBoxCrust.SelectedIndex = 0;
            comboBoxSauce.SelectedIndex = 0;
            comboBoxSpecialPizza.SelectedIndex = 0;
            comboBoxPop.SelectedIndex = 0;
            comboBoxJuice.SelectedIndex = 0;
            DisplayCurrentTotal(lblTotalCost);

            textBoxFName.Clear();
            textBoxLName.Clear();
            textBoxAddress.Clear();
            textBoxPhone.Clear();


            // clear all checked items from checkedListBox
            foreach (int i in checkedListBoxDipping.CheckedIndices)
            {
                checkedListBoxDipping.SetItemCheckState(i, CheckState.Unchecked);
            }

            foreach (int i in checkedListBoxToppings.CheckedIndices)
            {
                checkedListBoxToppings.SetItemCheckState(i, CheckState.Unchecked);
            }

            foreach (int i in checkedListBoxOtherItems.CheckedIndices)
            {
                checkedListBoxOtherItems.SetItemCheckState(i, CheckState.Unchecked);
            }
            order = "";
            totalCost = 0.0;
            CustomCurrentTotal.Text = "Current Total : " + totalCost.ToString("C");
        }
        private void buttonResetOrder_Click(object sender, EventArgs e)
        {
            ResetOrder();
        }

        private void buttonPlaceOrder_Click(object sender, EventArgs e)
        {
            double pizzaCost, beveragesCost, otherItemCost;
            order = "";

            order += "Full Moon Pizza Order" + "\n\n";
            order += "\n Name    : " + textBoxFName.Text.ToString() + " " + textBoxLName.Text.ToString();
            order += "\n Address : " + textBoxAddress.Text.ToString();
            order += "\n Phone    : " + textBoxPhone.Text.ToString();
            order += "\n Date       : " + System.DateTime.Now.ToString();
            order += "\n\n" + comboBoxCrust.SelectedItem.ToString();
            order += "(" + comboBoxSize.SelectedItem.ToString() + ")" + "\n";
            order += "Sauce : " + comboBoxSauce.SelectedItem.ToString() + "\n";
            if (checkedListBoxToppings.CheckedItems.Count > 0)
            {
                order += "Toppings : ";
                foreach (string toppingList in checkedListBoxToppings.CheckedItems)
                {
                    order += "\n" + toppingList;
                }
            }
            else if ((comboBoxSpecialPizza.SelectedIndex == 0) && (checkedListBoxToppings.CheckedItems.Count == 0))
            {
                MessageBox.Show("Please either choose toppings or buy special pizza with added toppings!");
                return;
            }

            if (textBoxFName.Text == "" || textBoxLName.Text == "" || textBoxAddress.Text == "" )
            {
                MessageBox.Show("Before placing an order, first fill up customer information.");
                return;
            }
            if (comboBoxSpecialPizza.SelectedIndex > 0)
                order += "\nSpecial Pizza : " + comboBoxSpecialPizza.SelectedItem.ToString() + "\n";

            pizzaCost = CalculatePizzaCost();
            order += "\nPizza Cost : " + pizzaCost.ToString("C") + "\n\n";


            order += "Beverages : ";
            order += "\n" + comboBoxPop.SelectedIndex + " Pops";
            order += "\n" + comboBoxJuice.SelectedIndex + " Juice";
            order += "\n" + checkedListBoxDipping.CheckedItems.Count + " Dippings";

            beveragesCost = CalculateBeverageCost();
            order += "\n" + "Beverage Cost : " + beveragesCost.ToString("C") + "\n\n";

            order += "Other Items : ";
            otherItemCost = CalculateOtherItemCost();
            order += "\nOther Items Cost : " + otherItemCost.ToString("C");

            order += "\n\n-----------------------------------";
            totalCost = pizzaCost + beveragesCost + otherItemCost;
            order += "\n\nTotal Amount : " + totalCost.ToString("C");
            order += "\n\n Thank You \n\n";

          // MessageBox.Show(order);

            
            if (MessageBox.Show(order, MessageBoxButtons.OK.ToString()) == DialogResult.OK)
            {
                ResetOrder();
                MessageBox.Show("Start Ordering Another Pizza.");

            }

        }

        public double CalculatePizzaCost()
        {
            double pizzaCost = 0.0;
            // check pizza size price
            if (comboBoxSize.SelectedIndex == 0)
            {
                pizzaCost += 9.99;
            }

            else if (comboBoxSize.SelectedIndex == 1)
            {
                pizzaCost += 11.99;
            }

            else if (comboBoxSize.SelectedIndex == 2)
            {
                pizzaCost += 14.99;
            }

            else if (comboBoxSize.SelectedIndex == 3)
            {
                pizzaCost += 16.99;
            }


            //check topping price

            pizzaCost += (checkedListBoxToppings.CheckedItems.Count * 1.50);
            return pizzaCost;
        }

        public double CalculateBeverageCost()
        {
            double bevragesCost = 0.0;
            bevragesCost += (comboBoxPop.SelectedIndex * 1.25) + (comboBoxJuice.SelectedIndex * 1.99) + (checkedListBoxDipping.CheckedItems.Count * 0.69);

            return bevragesCost;
        }

        public double CalculateOtherItemCost()
        {
            double otherItemCost = 0.0;

            foreach (Object otherItemList in checkedListBoxOtherItems.CheckedItems)
            {
                int index = checkedListBoxOtherItems.Items.IndexOf(otherItemList);

                switch (index)
                {
                    case 0:
                        otherItemCost += 5;
                        order += "\n" + otherItemList.ToString();
                        DisplayCurrentTotal(lblTotalCost);
                        break;

                    case 1:
                        otherItemCost += 9.75;
                        order += "\n" + otherItemList.ToString();
                        DisplayCurrentTotal(lblTotalCost);
                        break;

                    case 2:
                        otherItemCost += 8.49;
                        order += "\n" + otherItemList.ToString();
                        DisplayCurrentTotal(lblTotalCost);
                        break;

                    case 3:
                    case 4:
                    case 5:
                    case 6:
                        otherItemCost += 3.99;
                        order += "\n" + otherItemList.ToString();
                        DisplayCurrentTotal(lblTotalCost);
                        break;

                }

            }

            return otherItemCost;
        }

        // validation code
        private void FullMoonPizza_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                MessageBox.Show("Right key is not allowed.");
            }
        }
        private void CustForm_Validating(object sender, CancelEventArgs e)
        {
            if ((textBoxFName.Text.Trim() == String.Empty))
            {
                MessageBox.Show(textBoxFName, "First Name is required");
                return;
            }
            else
             if ((textBoxLName.Text.Trim() == String.Empty))
            {
                MessageBox.Show(textBoxLName, "Last Name is required");
                return;
            }
            else if ((textBoxAddress.Text.Trim() == String.Empty))
            {
                MessageBox.Show(textBoxAddress, "Address is required");
                return;
            }
            else if ((textBoxPhone.Text.Length < 10))
            {
                MessageBox.Show(textBoxPhone, "Valid phone number is required!");
                return;

            }
           
        }
      
        private void textBoxFName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= 65 && e.KeyChar <= 90) && !(e.KeyChar >= 97 && e.KeyChar <= 122) && e.KeyChar != (char)Keys.Back)
            {
                MessageBox.Show("Enter only alphabetic characters");
                e.Handled = true;
            }
        }
       
        private void textBoxLName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= 65 && e.KeyChar <= 90) && !(e.KeyChar >= 97 && e.KeyChar <= 122) && e.KeyChar != (char)Keys.Back)
            {
                MessageBox.Show("Enter only alphabetic characters.");
                e.Handled = true;
            }
        }

        private void textBoxAddress_KeyPress(object sender, KeyPressEventArgs e)
        {                                                                                                                                    // allows white space
            if (!(e.KeyChar >= 65 && e.KeyChar <= 90) && !(e.KeyChar >= 97 && e.KeyChar <= 122) && !(e.KeyChar >= 48 && e.KeyChar <= 57) && e.KeyChar != 32 && e.KeyChar != (char)Keys.Back)
            {
                MessageBox.Show("Please enter valid address.");
                e.Handled = true;
            }
        }

        private void textBoxPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!(e.KeyChar >= 48 && e.KeyChar <= 57) && e.KeyChar != (char)Keys.Back)
            {
                MessageBox.Show("Invalid Phone Number.");
                e.Handled = true;
            }
        }
       

        private void comboBoxSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblTotalCost -= lblSizeCost;
            switch (comboBoxSize.SelectedIndex)
            {
                case 0: 
                     lblSizeCost = 9.99;
                     break;

                case 1:
                    lblSizeCost = 11.99;
                    break;

                case 2:
                    lblSizeCost = 14.99;
                    break;

                case 3:
                    lblSizeCost = 16.99;
                    break;
            }
            lblTotalCost += lblSizeCost;
            DisplayCurrentTotal(lblTotalCost);
        }

        // add price to current total lable

        private void DisplayCurrentTotal(double lblTotalCost)
        {
            CustomCurrentTotal.Text = "Current Total : " + lblTotalCost.ToString("C");
            BevCurrentTotal.Text = "Current Total : " + lblTotalCost.ToString("C");
            OtherCurrentTotal.Text = "Current Total : " + lblTotalCost.ToString("C");

        }
        private void checkedListBoxToppings_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
                lblTotalCost += 1.50;

            if(e.NewValue == CheckState.Unchecked)
                lblTotalCost -= 1.50;

            DisplayCurrentTotal(lblTotalCost);
        }

        private void checkedListBoxDipping_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
                lblTotalCost += 0.69;

            if (e.NewValue == CheckState.Unchecked)
                lblTotalCost -= 0.69;

            DisplayCurrentTotal(lblTotalCost);
        }

        private void comboBoxPop_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblTotalCost -= lblPopCost;
            lblPopCost = (comboBoxPop.SelectedIndex * 1.25);
            lblTotalCost += lblPopCost;

            DisplayCurrentTotal(lblTotalCost);
        }


        private void comboBoxJuice_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblTotalCost -= lblJuiceCost;
            lblJuiceCost = (comboBoxJuice.SelectedIndex * 1.99);
            lblTotalCost += lblJuiceCost;

            DisplayCurrentTotal(lblTotalCost);
        }
        private void checkedListBoxOtherItems_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            switch (e.Index)
            {
                case 0:
                    if (e.NewValue == CheckState.Checked)  lblTotalCost += 5;
                    else if (e.NewValue == CheckState.Unchecked) lblTotalCost -= 5;
                    DisplayCurrentTotal(lblTotalCost);
                    break;
                case 1:
                    if (e.NewValue == CheckState.Checked) lblTotalCost += 9.75;
                    else if (e.NewValue == CheckState.Unchecked) lblTotalCost -= 9.75;
                    DisplayCurrentTotal(lblTotalCost);
                    break;
                case 2:
                    if (e.NewValue == CheckState.Checked) lblTotalCost += 8.49;
                    else if (e.NewValue == CheckState.Unchecked) lblTotalCost -= 8.49;
                    DisplayCurrentTotal(lblTotalCost);
                    break;
                case 3:
                case 5:
                case 4:
                case 6:
                    if (e.NewValue == CheckState.Checked) lblTotalCost += 3.99;
                    else if (e.NewValue == CheckState.Unchecked) lblTotalCost -= 3.99;
                    DisplayCurrentTotal(lblTotalCost);
                    break;
                
            }
            
        }

        private void textBoxFName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
