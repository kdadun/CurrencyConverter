using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CurrencyExchangeRate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadCurrency();
            LoadCurrencyIntoTable();
            

        }
        public void LoadCurrency()
        {
            XmlDocument doc = new XmlDocument();
            XmlNodeList nodeList;
            doc.Load("http://api.nbp.pl/api/exchangerates/tables/A/?format=xml");
            nodeList = doc.GetElementsByTagName("Rate");
            for (int i = 1; i < nodeList.Count; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Text = nodeList[i].FirstChild.InnerText;
                item.Value = nodeList[i].ChildNodes.Item(2).InnerText;
                comboBox1.Items.Add(item);
                comboBox2.Items.Add(item);
            }
        }
        public void LoadCurrencyIntoTable()
        {
            XmlDocument doc = new XmlDocument();
            XmlNodeList nodeList;
            doc.Load("http://api.nbp.pl/api/exchangerates/tables/A/?format=xml");

            DataTable table = new DataTable();
            table.Columns.Add("Waluta", typeof(string));
            table.Columns.Add("Kod", typeof(string));
            table.Columns.Add("Wartość", typeof(string));
            
            nodeList = doc.GetElementsByTagName("Rate");
            for (int i = 1; i < nodeList.Count; i++)
            {
                DataRow row = table.NewRow();
                row[0] = nodeList[i].FirstChild.InnerText;
                row[1] = nodeList[i].ChildNodes.Item(1).InnerText;
                row[2] = nodeList[i].ChildNodes.Item(2).InnerText;

                table.Rows.Add(row);
            }
            dataGridView1.DataSource = table;
        }
       
        public void EchangeRate(double yourInput,double combo1,double combo2)
        {
            double result = Math.Round(((yourInput * combo1) / combo2),2);
            label1.Text = result.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            double combo1,combo2,textbox;
            
            try
            {
                if (radioEnglish.Checked == true || radioPolish.Checked == true)
                {
                    if (double.TryParse(((comboBox1.SelectedItem as ComboBoxItem).Value).ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out combo1) && double.TryParse(textBox1.Text, out textbox) && double.TryParse(((comboBox2.SelectedItem as ComboBoxItem).Value).ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out combo2))
                    {
                        EchangeRate(textbox, combo1, combo2);
                    }
                    else
                    {
                        MessageBox.Show("Invalid input");
                    }
                }
                else
                {

                    MessageBox.Show("Choose language");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

       

        private void radioEnglish_CheckedChanged(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            XmlNodeList nodeDate;
            doc.Load("http://api.nbp.pl/api/exchangerates/tables/A/?format=xml");
            nodeDate = doc.GetElementsByTagName("EffectiveDate");
          
                if (radioEnglish.Checked == true)
                {
                    button1.Text = "Convert ";
                    label2.Text = "Enter the amount of money:";
                    label3.Text = "Select currency:";
                    label4.Text = "Convert to:";
                    label5.Text = "Value after conversion";
                    label6.Text = "Based on quotations of foreign exchange rates of the NBP from " + nodeDate[0].InnerText;
                    groupBox1.Text = "Choose language";
                }
            
            }

        private void radioPolish_CheckedChanged(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            XmlNodeList nodeDate;
            doc.Load("http://api.nbp.pl/api/exchangerates/tables/A/?format=xml");
            nodeDate = doc.GetElementsByTagName("EffectiveDate");
           
                if (radioPolish.Checked == true)
                {
                    button1.Text = "Przelicz";
                    groupBox1.Text = "Wybierz język";
                    label2.Text = "Wprowadz kwotę pieniędzy:";
                    label3.Text = "Przelicz z:";
                    label4.Text = "Przelicz na:";
                    label5.Text = "Wartoś po konwersji";
                    label6.Text = "Na podstawie notowań kursów walut obcych NBP z " + nodeDate[0].InnerText;



                }
            
        }
    }
}
