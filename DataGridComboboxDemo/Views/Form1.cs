using DataGridComboboxDemo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataGridComboboxDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var people = new List<Person>
            {
                new Person("Hans", 33),
                new Person("Peter", 27)
            };
            dataGridView1.DataSource = people;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                Console.WriteLine(column.Name);
                Console.WriteLine(column.DataPropertyName);
                Console.WriteLine(column.HeaderText);
            }

            var comboBoxColumn = new DataGridViewComboBoxColumn();
            comboBoxColumn.DataSource = people;
            comboBoxColumn.DisplayMember = nameof(Person.Name);
            comboBoxColumn.ValueMember = nameof(Person.Name);
            comboBoxColumn.DataPropertyName = nameof(Person.Name);

            dataGridView1.Columns.Add(comboBoxColumn);
        }
    }
}
