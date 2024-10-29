using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab0401_Shop_Application
{
    public partial class Form1 : Form
    {
        APD66_64011212155Entities context = new APD66_64011212155Entities();
        public Form1()
        {
            InitializeComponent();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            customerBindingSource.DataSource = context.Customers.ToList();
            productBindingSource.DataSource = context.Products.ToList();

            var results = context.Suppliers.OrderBy(s => s.CompanyName)
                .Select(s => new { s.Id, s.CompanyName });
            foreach(var result in results)
            {
                comboBox1.Items.Add(new ComboBoxItem(result.Id.ToString(),result.CompanyName));
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Customer customer = new Customer();
            customer.FirstName = textBox11.Text;
            customer.LastName = textBox10.Text;
            customer.City = textBox9.Text;
            customer.Country = textBox8.Text;
            customer.Phone = textBox7.Text;

            context.Customers.Add(customer);
            int change = context.SaveChanges();
            MessageBox.Show("Change " + change + "records");

            customerBindingSource.DataSource = context.Customers.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            customerBindingSource.AddNew();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            customerBindingSource.EndEdit();
            int change = context.SaveChanges();
            MessageBox.Show("Change " + change + "records");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string id = textBox12.Text;
            int idInt = int.Parse(id);
            var toDel = context.Customers
                .Where(c => c.Id == idInt)
                .First();
            context.Customers.Remove(toDel);
            int change = context.SaveChanges();
            MessageBox.Show("Change " + change + "records");

            customerBindingSource.DataSource = context.Customers.ToList();
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = int.Parse(dataGridView3.SelectedRows[0]
                .Cells[0].Value.ToString());

            var result = context.Products
                .Where(p => p.Id == id)
                .First();
            textBox18.Text = result.Id.ToString();
            textBox17.Text = result.ProductName;
            textBox16.Text = result.UnitPrice.ToString();
            textBox15.Text = result.Package;
            textBox14.Text = result.IsDiscontinued.ToString();
            comboBox1.Text = result.Supplier.CompanyName;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine(((ComboBoxItem)(comboBox1.SelectedItem)).Value);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Product product = new Product();
            product.ProductName = textBox17.Text;
            product.UnitPrice = decimal.Parse(textBox16.Text);
            product.Package = textBox15.Text;
            product.IsDiscontinued = bool.Parse(textBox14.Text);
            product.SupplierId = int.Parse(((ComboBoxItem)(comboBox1.SelectedItem)).Value);

            context.Products.Add(product);
            int change = context.SaveChanges();
            MessageBox.Show("Change " + change + "records");
        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            int id = int.Parse(textBox18.Text);
            var result = context.Products
                .Where(p => p.Id == id)
                .First();
            result.ProductName = textBox17.Text;
            result.UnitPrice = decimal.Parse(textBox16.Text);
            result.Package = textBox15.Text;
            result.IsDiscontinued = bool.Parse(textBox14.Text);
            result.SupplierId = int.Parse(((ComboBoxItem)(comboBox1.SelectedItem)).Value);

            int change = context.SaveChanges();
            if (change > 0)
            {
                MessageBox.Show("Update Ok");
                productBindingSource.DataSource = context.Products.ToList();
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            int id = int.Parse(textBox18.Text);

            var result = context.Products
                .Where(p => p.Id == id)
                .First();
            context.Products.Remove(result);
            int change = context.SaveChanges();
            MessageBox.Show("Change " + change + "records");
            productBindingSource.DataSource = context.Products.ToList();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                numericUpDown1.Focus();
            }
        }

        private void numericUpDown1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                int id = int.Parse(textBox1.Text);
                var result = context.Products.Where(p => p.Id == id)
                    .First();
                string[] item = new string[]
                {
                    result.Id.ToString(),
                    result.ProductName,
                numericUpDown1.Value.ToString(),
                result.UnitPrice.ToString(),
                (result.UnitPrice * numericUpDown1.Value).ToString()
                };
                listView1.Items.Add(new ListViewItem(item));
                double sum = calculateTotal(listView1.Items);
                label2.Text = sum.ToString();
            }
        }

        private double calculateTotal(ListView.ListViewItemCollection items)
        {
            double sum = 0;
            foreach (ListViewItem item in items) 
            {
                sum += double.Parse(item.SubItems[4].Text);
            }
            return sum;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Order order = new Order();
            order.OrderDate = DateTime.Now;
            order.OrderNumber = "123456";
            order.CustomerId = 2;
            order.TotalAmount = decimal.Parse(label2.Text);

            context.Orders.Add(order);
            int change = context.SaveChanges();
            MessageBox.Show("Change " + change + "records");
            if (change == 1)
            {
                foreach(ListViewItem item in listView1.Items)
                {
                    OrderItem orderItem = new OrderItem();
                    orderItem.OrderId = order.Id;
                    orderItem.ProductId = int.Parse(item.SubItems[0].Text);
                    orderItem.UnitPrice = decimal.Parse(item.SubItems[3].Text);
                    orderItem.Quantity = int.Parse(item.SubItems[2].Text);

                    context.OrderItems.Add(orderItem);
                    context.SaveChanges();
                }
                MessageBox.Show("Save completed");
            }

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
    }

    internal class ComboBoxItem
    {
        public string Value { get; set; } //Supplier ID
        public string Text { get; set; } //Supplier Companyname
        public ComboBoxItem(string val, string text)
        {
            Value = val;
            Text = text;
        }
        public override string ToString()
        {
            return Text;
        }
    }

