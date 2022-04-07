using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExpenseTracker
{
    public partial class TrackerForm : Form
    {
        public List<Expense> expenseList { get; set; } = new List<Expense>();
        public bool containsPeriod = false;
        public TrackerForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dateTimePicker.Format = DateTimePickerFormat.Custom;
            dateTimePicker.CustomFormat = "dd.MM.yyyy";
            SetupDatagrid();
        }

        //executes when button is clicked
        private void buttonAddExpense_Click(object sender, EventArgs e)
        {
            try
            {
                //Check if textboxes have values
                if (String.IsNullOrEmpty(textBoxName.Text) || String.IsNullOrEmpty(textBoxPrice.Text)) return;
                Decimal price = Decimal.Parse(textBoxPrice.Text);                                                       
                DateTime date = dateTimePicker.Value.Date;

                //add new expense to the list of expenses
                expenseList.Add(new Expense(date, textBoxName.Text, price));
                AddLastExpenseRow();
                UpdateSubtotal();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Cannot process transaction." + ex);
            }

        }

        private void UpdateSubtotal()
        {
            Decimal subtotal = 0;
            //labelSubtotal = subtotal;
            foreach (var expense in expenseList)
            {
                subtotal += expense.price;
            }
            labelSubtotal.Text = subtotal.ToString(); ;
        }

        private void SetupDatagrid()
        {
            dgvExpenses.AllowUserToAddRows = false;
            dgvExpenses.ReadOnly = true;
            dgvExpenses.RowHeadersVisible = false;

            //button for deleting expense in column
            DataGridViewButtonColumn btn_delete = new DataGridViewButtonColumn();
            {
                btn_delete.Name = "Delete";
                btn_delete.HeaderText = "Delete";
                btn_delete.Text = "X";
                btn_delete.FlatStyle = FlatStyle.Flat;
                btn_delete.DefaultCellStyle.BackColor = Color.Red;
                btn_delete.DefaultCellStyle.ForeColor = Color.White;
                btn_delete.UseColumnTextForButtonValue = true;
                btn_delete.Width = 60;
                dgvExpenses.Columns.Add(btn_delete);
            }
            //creating columns for expenses
            DataGridViewTextBoxColumn columnDate = new DataGridViewTextBoxColumn();    
            {
                columnDate.Name = "Date";
                columnDate.HeaderText = "Date";
                dgvExpenses.Columns.Add(columnDate);

            }
            DataGridViewTextBoxColumn columnName = new DataGridViewTextBoxColumn();    
            {
                columnName.Name = "Name";
                columnName.HeaderText = "Name";
                dgvExpenses.Columns.Add(columnName);
            }
            DataGridViewTextBoxColumn columnPrice = new DataGridViewTextBoxColumn();   
            {
                columnPrice.Name = "Price";
                columnPrice.HeaderText = "Price(EUR)";
                dgvExpenses.Columns.Add(columnPrice);

            }
            //column width autosizing
            dgvExpenses.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvExpenses.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvExpenses.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        //executes when user writes to price textbox
        private void textBoxPrice_TextChanged(object sender, EventArgs e)
        {
            //check if user is using only numbers and period
            if (System.Text.RegularExpressions.Regex.IsMatch(textBoxPrice.Text, "[^0-9,]"))
            {
                //if (textBoxPrice.Text.Contains(".")) textBoxPrice.Text.Replace(".", ",");
                //if (!textBoxPrice.Text.Contains("."))
                textBoxPrice.Text = textBoxPrice.Text.Remove(textBoxPrice.Text.Length - 1);
            }
        }
        //executes when user writes to name textbox
        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBoxName.Text, "[^a-zA-Z]"))
            {
                textBoxName.Text = textBoxName.Text.Remove(textBoxName.Text.Length - 1);
            }
        }

        private void AddLastExpenseRow()
        {
            // Create a new row
            int rowId = dgvExpenses.Rows.Add();
            // Grab the new row and occupy it with new data
            DataGridViewRow row = dgvExpenses.Rows[rowId];
            row.Cells["Date"].Value = expenseList[expenseList.Count()-1].date.ToString("d");
            row.Cells["Name"].Value = expenseList[expenseList.Count()-1].name;                                
            row.Cells["Price"].Value = expenseList[expenseList.Count()-1].price;
        }

        //stops on click cell highlighting
        private void dgv_expenses_SelectionChanged(object sender, EventArgs e)
        {
            dgvExpenses.SelectionChanged -= dgv_expenses_SelectionChanged;
            dgvExpenses.ClearSelection();
            dgvExpenses.SelectionChanged += dgv_expenses_SelectionChanged;
        }

        private void dgv_expenses_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                //Delete the row on which the button was clicked
                dgvExpenses.Rows.RemoveAt(e.RowIndex);
                expenseList.RemoveAt(e.RowIndex);
                //update subtotal label
                UpdateSubtotal();
            }
        }
    }
}
