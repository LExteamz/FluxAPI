using FluxAPI;
using System;
using System.IO;
using System.Windows.Forms;

namespace Test
{
    public partial class FluxTest : Form
    {
        readonly Flux GetFlux = new Flux();

        public FluxTest()
        {
            InitializeComponent();
            GetFlux.DoAutoAttach = true;
            _ = GetFlux.InitializeAsync("RC7");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Text = string.Empty;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Flux | Open File";
            openFileDialog.Multiselect = false;

            var result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string filename = openFileDialog.FileName;
                string content = File.ReadAllText(filename);

                fastColoredTextBox1.Text = content;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetFlux.Inject();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GetFlux.Execute(fastColoredTextBox1.Text);
        }
    }
}
