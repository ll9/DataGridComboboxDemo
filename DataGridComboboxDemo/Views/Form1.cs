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
            var uniqueItems = new HashSet<object>(
                table.Rows.Cast<DataRow>()
                .Select(r => r["Name"])
                .ToList());

            foreach (var item in uniqueItems)
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
            if (sender is DataGridView dataGridView)
            {
                var column = dataGridView.Columns[e.ColumnIndex];
                if (column is DataGridViewComboBoxColumn comboBoxColumn)
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
            if (sender is DataGridView dataGridView)
            {
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    if (column is DataGridViewComboBoxColumn comboBoxColumn)
                    {
                        if (e.Control is ComboBox cb)
                        {
                            cb.DropDownStyle = ComboBoxStyle.DropDown;
                        }
                    }
                }
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (sender is DataGridView dataGridView)
            {
                var col = dataGridView.Columns[e.ColumnIndex];



                dataGridView1.Columns.Remove(col);

                var comboboxColum = new DataGridViewComboBoxColumn
                {
                    Name = col.DataPropertyName,
                    DataPropertyName = col.DataPropertyName,
                    HeaderText = col.DataPropertyName
                };
                var uniqueItems = new HashSet<object>(
                    table.Rows.Cast<DataRow>()
                    .Select(r => r[col.DataPropertyName])
                    .ToList());

                foreach (var item in uniqueItems)
                {
                    comboboxColum.Items.Add(item);
                }

                dataGridView1.Columns.Insert(col.DisplayIndex, comboboxColum);

            }
        }
    }
}
