using SimuladorAutomatas.Automatas;
using SimuladorAutomatas.Modelos;
using SimuladorAutomatas.Servicios;
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms;
namespace SimuladorAutomatas

{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.Text = "Simulador de Autómatas";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1300, 850);
            this.MinimumSize = new Size(1100, 750);
            this.BackColor = Color.FromArgb(242, 246, 251);
            this.Font = new Font("Segoe UI", 10);

        }
       
        private void Form1_Load(object sender, EventArgs e)
        {

        }

       

    }
}
