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

            ConstruirInterfaz();

        }

        private void ConstruirInterfaz()
        {
            // Contenedor principal
            TableLayoutPanel principal = new TableLayoutPanel();
            principal.Dock = DockStyle.Fill;
            principal.Padding = new Padding(12);
            principal.ColumnCount = 1;
            principal.RowCount = 3;
            principal.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100));
            principal.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 105));
            principal.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100));
            principal.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 75));

            // Encabezado azul
            Panel encabezado = new Panel();
            encabezado.Dock = DockStyle.Fill;
            encabezado.BackColor = Color.FromArgb(35, 59, 91);

            Label titulo = new Label();
            titulo.Text = "◉  Simulador de Autómatas";
            titulo.ForeColor = Color.White;
            titulo.Font = new Font("Segoe UI", 23, FontStyle.Bold);
            titulo.Location = new Point(25, 18);
            titulo.AutoSize = true;

            Label subtitulo = new Label();
            subtitulo.Text = "AFD    •    AFN    •    Conversión    •    Minimización";
            subtitulo.ForeColor = Color.WhiteSmoke;
            subtitulo.Font = new Font("Segoe UI", 11);
            subtitulo.Location = new Point(30, 65);
            subtitulo.AutoSize = true;

            encabezado.Controls.Add(titulo);
            encabezado.Controls.Add(subtitulo);

            // Área central: cuatro paneles
            TableLayoutPanel contenido = new TableLayoutPanel();
            contenido.Dock = DockStyle.Fill;
            contenido.Padding = new Padding(0, 10, 0, 10);
            contenido.ColumnCount = 2;
            contenido.RowCount = 2;

            contenido.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 58));
            contenido.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 42));
            contenido.RowStyles.Add(
                new RowStyle(SizeType.Percent, 48));
            contenido.RowStyles.Add(
                new RowStyle(SizeType.Percent, 52));

            // Panel de configuración
            GroupBox configuracion = new GroupBox();
            configuracion.Text = "Configuración del Autómata";
            configuracion.Dock = DockStyle.Fill;
            configuracion.Padding = new Padding(12);
            configuracion.Margin = new Padding(5);

            TableLayoutPanel datos = new TableLayoutPanel();
            datos.Dock = DockStyle.Fill;
            datos.ColumnCount = 2;
            datos.RowCount = 5;
            datos.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 155));
            datos.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100));

            for (int i = 0; i < 5; i++)
            {
                datos.RowStyles.Add(
                    new RowStyle(SizeType.Percent, 20));
            }

            TextBox txtNombre = new TextBox();
            txtNombre.Name = "txtNombreAutomata";
            txtNombre.Text = "Mi autómata";
            txtNombre.Dock = DockStyle.Fill;

            ComboBox cmbTipo = new ComboBox();
            cmbTipo.Name = "cmbTipoAutomata";
            cmbTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipo.Items.AddRange(new object[] { "AFN", "AFD" });
            cmbTipo.SelectedIndex = 0;
            cmbTipo.Dock = DockStyle.Fill;

            TextBox txtAlfabeto = new TextBox();
            txtAlfabeto.Name = "txtAlfabeto";
            txtAlfabeto.Text = "0,1";
            txtAlfabeto.Dock = DockStyle.Fill;

            TextBox txtEstados = new TextBox();
            txtEstados.Name = "txtEstados";
            txtEstados.PlaceholderText = "q0,q1,q2";
            txtEstados.Dock = DockStyle.Fill;

            Button btnAgregarEstados = new Button();
            btnAgregarEstados.Name = "btnAgregarEstados";
            btnAgregarEstados.Text = "Agregar estados";
            btnAgregarEstados.Dock = DockStyle.Fill;
            btnAgregarEstados.BackColor = Color.FromArgb(38, 126, 220);
            btnAgregarEstados.ForeColor = Color.White;
            btnAgregarEstados.FlatStyle = FlatStyle.Flat;

            datos.Controls.Add(new Label
            {
                Text = "Nombre del autómata:",
                AutoSize = true,
                Anchor = AnchorStyles.Left
            }, 0, 0);
            datos.Controls.Add(txtNombre, 1, 0);

            datos.Controls.Add(new Label
            {
                Text = "Tipo de autómata:",
                AutoSize = true,
                Anchor = AnchorStyles.Left
            }, 0, 1);
            datos.Controls.Add(cmbTipo, 1, 1);

            datos.Controls.Add(new Label
            {
                Text = "Alfabeto (separado por comas):",
                AutoSize = true,
                Anchor = AnchorStyles.Left
            }, 0, 2);
            datos.Controls.Add(txtAlfabeto, 1, 2);

            datos.Controls.Add(new Label
            {
                Text = "Estados:",
                AutoSize = true,
                Anchor = AnchorStyles.Left
            }, 0, 3);
            datos.Controls.Add(txtEstados, 1, 3);

            datos.Controls.Add(btnAgregarEstados, 1, 4);

            configuracion.Controls.Add(datos);

            // Panel para simular cadenas
            GroupBox simulacion = new GroupBox();
            simulacion.Text = "Simular cadena";
            simulacion.Dock = DockStyle.Fill;
            simulacion.Padding = new Padding(12);
            simulacion.Margin = new Padding(5);

            TableLayoutPanel areaSimulacion = new TableLayoutPanel();
            areaSimulacion.Dock = DockStyle.Fill;
            areaSimulacion.ColumnCount = 1;
            areaSimulacion.RowCount = 4;

            areaSimulacion.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 30));
            areaSimulacion.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 42));
            areaSimulacion.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 48));
            areaSimulacion.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100));

            TextBox txtCadena = new TextBox();
            txtCadena.Name = "txtCadena";
            txtCadena.Dock = DockStyle.Fill;

            Button btnSimular = new Button();
            btnSimular.Name = "btnSimular";
            btnSimular.Text = "▶   Simular";
            btnSimular.Dock = DockStyle.Fill;
            btnSimular.BackColor = Color.FromArgb(38, 126, 220);
            btnSimular.ForeColor = Color.White;
            btnSimular.FlatStyle = FlatStyle.Flat;

            Label lblResultado = new Label();
            lblResultado.Name = "lblResultado";
            lblResultado.Text = "Aquí aparecerá el resultado de la simulación.";
            lblResultado.Dock = DockStyle.Fill;
            lblResultado.AutoSize = false;
            lblResultado.Padding = new Padding(8);
            lblResultado.BackColor = Color.White;

            areaSimulacion.Controls.Add(new Label
            {
                Text = "Cadena:",
                AutoSize = true
            }, 0, 0);
            areaSimulacion.Controls.Add(txtCadena, 0, 1);
            areaSimulacion.Controls.Add(btnSimular, 0, 2);
            areaSimulacion.Controls.Add(lblResultado, 0, 3);

            simulacion.Controls.Add(areaSimulacion);

            // Panel de transiciones
            GroupBox transiciones = new GroupBox();
            transiciones.Text = "Transiciones";
            transiciones.Dock = DockStyle.Fill;
            transiciones.Padding = new Padding(10);
            transiciones.Margin = new Padding(5);

            DataGridView tabla = new DataGridView();
            tabla.Name = "dgvTransiciones";
            tabla.Dock = DockStyle.Fill;
            tabla.AllowUserToAddRows = false;
            tabla.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            tabla.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            tabla.MultiSelect = false;
            tabla.RowHeadersVisible = false;

            tabla.Columns.Add("Origen", "Origen");
            tabla.Columns.Add("Simbolo", "Símbolo");
            tabla.Columns.Add("Destino", "Destino");

            transiciones.Controls.Add(tabla);

            // Panel para representar el autómata
            GroupBox representacion = new GroupBox();
            representacion.Text = "Representación del autómata";
            representacion.Dock = DockStyle.Fill;
            representacion.Padding = new Padding(10);
            representacion.Margin = new Padding(5);

            Panel lienzo = new Panel();
            lienzo.Name = "panelAutomata";
            lienzo.Dock = DockStyle.Fill;
            lienzo.BackColor = Color.White;

            Label lblLienzo = new Label();
            lblLienzo.Text =
                "La representación gráfica aparecerá aquí.";
            lblLienzo.Dock = DockStyle.Fill;
            lblLienzo.TextAlign = ContentAlignment.MiddleCenter;
            lblLienzo.ForeColor = Color.DimGray;

            lienzo.Controls.Add(lblLienzo);
            representacion.Controls.Add(lienzo);

            contenido.Controls.Add(configuracion, 0, 0);
            contenido.Controls.Add(simulacion, 1, 0);
            contenido.Controls.Add(transiciones, 0, 1);
            contenido.Controls.Add(representacion, 1, 1);

            // Barra inferior
            FlowLayoutPanel barra = new FlowLayoutPanel();
            barra.Dock = DockStyle.Fill;
            barra.FlowDirection = FlowDirection.LeftToRight;
            barra.WrapContents = false;
            barra.Padding = new Padding(0, 10, 0, 0);

            Button btnConvertir = new Button();
            btnConvertir.Name = "btnConvertir";
            btnConvertir.Text = "Convertir AFN → AFD";
            btnConvertir.Size = new Size(245, 48);

            Button btnMinimizar = new Button();
            btnMinimizar.Name = "btnMinimizar";
            btnMinimizar.Text = "Minimizar AFD";
            btnMinimizar.Size = new Size(220, 48);

            Button btnLimpiar = new Button();
            btnLimpiar.Name = "btnLimpiarTodo";
            btnLimpiar.Text = "Limpiar todo";
            btnLimpiar.Size = new Size(160, 48);

            barra.Controls.Add(btnConvertir);
            barra.Controls.Add(btnMinimizar);
            barra.Controls.Add(btnLimpiar);

            principal.Controls.Add(encabezado, 0, 0);
            principal.Controls.Add(contenido, 0, 1);
            principal.Controls.Add(barra, 0, 2);

            this.Controls.Add(principal);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

       

    }
}
