using DataGridComboboxDemo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataGridComboboxDemo
{
    public partial class Form1 : Form
    {
        private DataTable table;

        public Form1()
        {
            InitializeComponent();


            var query = "SELECT * FROM PERSON";
            table = new DataTable();
            using (var connection = new SQLiteConnection("Data Source=db.sqlite"))
            using (var adapter = new SQLiteDataAdapter(query, connection))
            {
                adapter.Fill(table);
            }

            dataGridView1.DataSource = table;
            dataGridView1.Columns.Remove("Name");

            var comboboxColum = new DataGridViewComboBoxColumn
            {
                Name = "Name",
                DataPropertyName = "Name",
                HeaderText = "Name"
            };
            foreach (var item in table.Rows.Cast<DataRow>().Select(r => r["Name"]))
            {
                comboboxColum.Items.Add(item);
            }

            dataGridView1.Columns.Add(comboboxColum);

            dataGridView1.CellValidating += dataGridView1_CellValidating;
            dataGridView1.EditingControlShowing += dataGridView1_EditingControlShowing;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var query = "SELECT * FROM PERSON";

            using (var connection = new SQLiteConnection("Data Source=db.sqlite"))
            using (var adapter = new SQLiteDataAdapter(query, connection))
            {
                var builder = new SQLiteCommandBuilder(adapter);

                adapter.InsertCommand = builder.GetInsertCommand();
                adapter.UpdateCommand = builder.GetUpdateCommand();
                adapter.DeleteCommand = builder.GetDeleteCommand();
                adapter.Update(table);
            }
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridView1.Columns["Name"] is DataGridViewComboBoxColumn comboBoxColumn)
            {
                if (e.ColumnIndex == comboBoxColumn.Index)
                {
                    if (!comboBoxColumn.Items.Contains(e.FormattedValue))
                    {
                        comboBoxColumn.Items.Add(e.FormattedValue);
                    }
                }
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView1.Columns["Name"] is DataGridViewComboBoxColumn comboBoxColumn)
            {
                if (dataGridView1.CurrentCellAddress.X == comboBoxColumn.Index)
                {
                    if (e.Control is ComboBox cb)
                    {
                        cb.DropDownStyle = ComboBoxStyle.DropDown;
                    }
                }
            }
        }
    }
}
