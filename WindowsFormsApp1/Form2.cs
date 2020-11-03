using Nest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2(string text)
        {
            InitializeComponent();
            Searchbar.Text = text;
        }

        private async void SearchFile()
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
                .DefaultIndex("filesmanager");
            var client = new ElasticClient(settings);
            string textsearch = Searchbar.Text.ToString();

            var list = await Support.SearchFile(textsearch, client);

            //Console.WriteLine(list[0].url);
            SearchView.Invoke(new MethodInvoker(delegate ()
            {
                SearchView.DataSource = list;
            }
            ));
        }

        private void SearchView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var url = SearchView.Rows[e.RowIndex].Cells[2].Value.ToString();
            Process.Start(url);
        }

        private void Form2_Load(object sender, EventArgs e)

        {
            Thread thread = new Thread(new ThreadStart(this.SearchFile));
            // Thread thread = new Thread( Support.DirSearch("D:\\test\\", client));
            thread.IsBackground = true;
            thread.Start();
            SearchView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}