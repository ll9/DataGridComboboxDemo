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

            var comboboxColum = new DataGridViewComboBoxColumn();
            comboboxColum.Items.AddRange("Hans", "Peter");
            comboboxColum.DataPropertyName = "Name";
            comboboxColum.HeaderText = "Name";

            dataGridView1.Columns.Add(comboboxColum);
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
    }
}
