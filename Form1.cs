using SimuladorAutomatas.Automatas;
using SimuladorAutomatas.Modelos;
using SimuladorAutomatas.Servicios;
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using System.Drawing;


namespace SimuladorAutomatas
{
    public partial class Form1 : Form
    {
        // Autómata con el que estamos trabajando.
        private Automata automataActual = new AFN();

        // Tabla de transiciones.
        private DataGridView dgvTransiciones;

        public Form1()
        {q0
            InitializeComponent();

            // Configuración de la ventana.
            this.Text = "Simulador de Autómatas";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1300, 850);
            this.MinimumSize = new Size(1100, 750);
            this.BackColor = Color.FromArgb(242, 246, 251);
            this.Font = new Font("Segoe UI", 10);

            // Construimos la interfaz.
            ConstruirInterfaz();
        }

        private void ConstruirInterfaz()
        {
            // ==========================================
            // CONTENEDOR PRINCIPAL
            // ==========================================

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
                new RowStyle(SizeType.Absolute, 65));


            // ==========================================
            // ENCABEZADO
            // ==========================================

            Panel encabezado = new Panel();

            encabezado.Dock = DockStyle.Fill;
            encabezado.BackColor = Color.FromArgb(35, 59, 91);

            Label titulo = new Label();

            titulo.Text = "◉  Simulador de Autómatas";
            titulo.ForeColor = Color.White;
            titulo.Font = new Font(
                "Segoe UI", 23, FontStyle.Bold);

            titulo.Location = new Point(25, 18);
            titulo.AutoSize = true;

            Label subtitulo = new Label();

            subtitulo.Text =
                "AFD    •    AFN    •    Conversión    •    Minimización";

            subtitulo.ForeColor = Color.WhiteSmoke;
            subtitulo.Font = new Font("Segoe UI", 11);
            subtitulo.Location = new Point(30, 65);
            subtitulo.AutoSize = true;

            encabezado.Controls.Add(titulo);
            encabezado.Controls.Add(subtitulo);


            // ==========================================
            // ÁREA CENTRAL
            // ==========================================

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


            // ==========================================
            // PANEL 1: CONFIGURACIÓN
            // ==========================================

            GroupBox configuracion = new GroupBox();

            configuracion.Text = "Configuración del Autómata";
            configuracion.Dock = DockStyle.Fill;
            configuracion.Padding = new Padding(12);
            configuracion.Margin = new Padding(5);

            TableLayoutPanel datos = new TableLayoutPanel();

            datos.Dock = DockStyle.Fill;
            datos.ColumnCount = 2;
            datos.RowCount = 7;

            datos.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 155));

            datos.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100));

            for (int i = 0; i < 7; i++)
            {
                datos.RowStyles.Add(
                    new RowStyle(SizeType.Percent, 100f / 7));
            }


            // Nombre del autómata.
            TextBox txtNombre = new TextBox();

            txtNombre.Name = "txtNombreAutomata";
            txtNombre.Text = "Mi autómata";
            txtNombre.Dock = DockStyle.Fill;


            // Tipo de autómata.
            ComboBox cmbTipo = new ComboBox();

            cmbTipo.Name = "cmbTipoAutomata";
            cmbTipo.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbTipo.Items.AddRange(
                new object[] { "AFN", "AFD" });

            cmbTipo.SelectedIndex = 0;
            cmbTipo.Dock = DockStyle.Fill;


            // Alfabeto.
            TextBox txtAlfabeto = new TextBox();

            txtAlfabeto.Name = "txtAlfabeto";
            txtAlfabeto.Text = "0,1";
            txtAlfabeto.Dock = DockStyle.Fill;


            // Lista de estados.
            TextBox txtEstados = new TextBox();

            txtEstados.Name = "txtEstados";
            txtEstados.Text = "q0,q1,q2";
            txtEstados.Dock = DockStyle.Fill;


            // Estado inicial.
            TextBox txtInicial = new TextBox();

            txtInicial.Name = "txtEstadoInicial";
            txtInicial.Text = "q0";
            txtInicial.Dock = DockStyle.Fill;


            // Estados finales.
            TextBox txtFinales = new TextBox();

            txtFinales.Name = "txtEstadosFinales";
            txtFinales.Text = "q2";
            txtFinales.Dock = DockStyle.Fill;


            // Botón para crear el autómata.
            Button btnAgregarEstados = new Button();

            btnAgregarEstados.Name = "btnAgregarEstados";
            btnAgregarEstados.Text = "Crear autómata";
            btnAgregarEstados.Dock = DockStyle.Fill;

            btnAgregarEstados.BackColor =
                Color.FromArgb(38, 126, 220);

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


            datos.Controls.Add(new Label
            {
                Text = "Estado inicial:",
                AutoSize = true,
                Anchor = AnchorStyles.Left
            }, 0, 4);

            datos.Controls.Add(txtInicial, 1, 4);


            datos.Controls.Add(new Label
            {
                Text = "Estados finales:",
                AutoSize = true,
                Anchor = AnchorStyles.Left
            }, 0, 5);

            datos.Controls.Add(txtFinales, 1, 5);

            datos.Controls.Add(btnAgregarEstados, 1, 6);

            configuracion.Controls.Add(datos);


            // ==========================================
            // PANEL 2: SIMULAR CADENA
            // ==========================================

            GroupBox simulacion = new GroupBox();

            simulacion.Text = "Simular cadena";
            simulacion.Dock = DockStyle.Fill;
            simulacion.Padding = new Padding(12);
            simulacion.Margin = new Padding(5);

            TableLayoutPanel areaSimulacion =
                new TableLayoutPanel();

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

            btnSimular.BackColor =
                Color.FromArgb(38, 126, 220);

            btnSimular.ForeColor = Color.White;
            btnSimular.FlatStyle = FlatStyle.Flat;


            Label lblResultado = new Label();

            lblResultado.Name = "lblResultado";
            lblResultado.Text =
                "Aquí aparecerá el resultado de la simulación.";

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


            // ==========================================
            // PANEL 3: TRANSICIONES
            // ==========================================

            GroupBox transiciones = new GroupBox();

            transiciones.Text = "Transiciones";
            transiciones.Dock = DockStyle.Fill;
            transiciones.Padding = new Padding(10);
            transiciones.Margin = new Padding(5);


            TableLayoutPanel areaTransiciones =
                new TableLayoutPanel();

            areaTransiciones.Dock = DockStyle.Fill;
            areaTransiciones.ColumnCount = 1;
            areaTransiciones.RowCount = 2;

            areaTransiciones.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 45));

            areaTransiciones.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100));


            FlowLayoutPanel entradasTransicion =
                new FlowLayoutPanel();

            entradasTransicion.Dock = DockStyle.Fill;
            entradasTransicion.WrapContents = false;


            TextBox txtOrigen = new TextBox();

            txtOrigen.Name = "txtOrigen";
            txtOrigen.PlaceholderText = "Origen";
            txtOrigen.Width = 90;


            TextBox txtSimbolo = new TextBox();

            txtSimbolo.Name = "txtSimbolo";
            txtSimbolo.PlaceholderText = "Símbolo";
            txtSimbolo.Width = 90;


            TextBox txtDestino = new TextBox();

            txtDestino.Name = "txtDestino";
            txtDestino.PlaceholderText = "Destino";
            txtDestino.Width = 90;


            Button btnAgregarTransicion = new Button();

            btnAgregarTransicion.Name = "btnAgregarTransicion";
            btnAgregarTransicion.Text = "Agregar";
            btnAgregarTransicion.Width = 100;


            entradasTransicion.Controls.Add(txtOrigen);
            entradasTransicion.Controls.Add(txtSimbolo);
            entradasTransicion.Controls.Add(txtDestino);
            entradasTransicion.Controls.Add(btnAgregarTransicion);


            dgvTransiciones = new DataGridView();

            dgvTransiciones.Name = "dgvTransiciones";
            dgvTransiciones.Dock = DockStyle.Fill;

            dgvTransiciones.AllowUserToAddRows = false;

            dgvTransiciones.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvTransiciones.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvTransiciones.MultiSelect = false;
            dgvTransiciones.RowHeadersVisible = false;

            dgvTransiciones.Columns.Add("Origen", "Origen");
            dgvTransiciones.Columns.Add("Simbolo", "Símbolo");
            dgvTransiciones.Columns.Add("Destino", "Destino");


            areaTransiciones.Controls.Add(
                entradasTransicion, 0, 0);

            areaTransiciones.Controls.Add(
                dgvTransiciones, 0, 1);

            transiciones.Controls.Add(areaTransiciones);


            // ==========================================
            // PANEL 4: REPRESENTACIÓN DEL AUTÓMATA
            // ==========================================

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

            lblLienzo.TextAlign =
                ContentAlignment.MiddleCenter;

            lblLienzo.ForeColor = Color.DimGray;

            lienzo.Controls.Add(lblLienzo);
            representacion.Controls.Add(lienzo);


            // Incorporar los cuatro paneles.
            contenido.Controls.Add(configuracion, 0, 0);
            contenido.Controls.Add(simulacion, 1, 0);
            contenido.Controls.Add(transiciones, 0, 1);
            contenido.Controls.Add(representacion, 1, 1);


            // ==========================================
            // BARRA INFERIOR
            // ==========================================

            FlowLayoutPanel barra = new FlowLayoutPanel();

            barra.Dock = DockStyle.Fill;
            barra.FlowDirection = FlowDirection.LeftToRight;
            barra.WrapContents = false;
            barra.Padding = new Padding(0, 8, 0, 0);


            Button btnConvertir = new Button();

            btnConvertir.Name = "btnConvertir";
            btnConvertir.Text = "Convertir AFN → AFD";
            btnConvertir.Size = new Size(245, 45);


            Button btnMinimizar = new Button();

            btnMinimizar.Name = "btnMinimizar";
            btnMinimizar.Text = "Minimizar AFD";
            btnMinimizar.Size = new Size(220, 45);


            Button btnLimpiar = new Button();

            btnLimpiar.Name = "btnLimpiarTodo";
            btnLimpiar.Text = "Limpiar todo";
            btnLimpiar.Size = new Size(160, 45);


            barra.Controls.Add(btnConvertir);
            barra.Controls.Add(btnMinimizar);
            barra.Controls.Add(btnLimpiar);


            principal.Controls.Add(encabezado, 0, 0);
            principal.Controls.Add(contenido, 0, 1);
            principal.Controls.Add(barra, 0, 2);

            this.Controls.Add(principal);


            // ==========================================
            // EVENTO: CREAR AUTÓMATA
            // ==========================================

            btnAgregarEstados.Click += (s, e) =>
            {
                try
                {
                    string nombre = txtNombre.Text.Trim();

                    if (string.IsNullOrWhiteSpace(nombre))
                    {
                        MessageBox.Show(
                            "Escribe un nombre para el autómata.");

                        return;
                    }


                    string[] nombresEstados = txtEstados.Text
                        .Split(',')
                        .Select(n => n.Trim())
                        .Where(n => n.Length > 0)
                        .ToArray();


                    if (nombresEstados.Length == 0)
                    {
                        MessageBox.Show(
                            "Debes ingresar al menos un estado.");

                        return;
                    }


                    if (nombresEstados.Distinct(
                        StringComparer.OrdinalIgnoreCase).Count()
                        != nombresEstados.Length)
                    {
                        MessageBox.Show(
                            "No puedes repetir los nombres de los estados.");

                        return;
                    }


                    string nombreInicial = txtInicial.Text.Trim();


                    string[] finales = txtFinales.Text
                        .Split(',')
                        .Select(n => n.Trim())
                        .Where(n => n.Length > 0)
                        .ToArray();


                    if (!nombresEstados.Contains(
                        nombreInicial,
                        StringComparer.OrdinalIgnoreCase))
                    {
                        MessageBox.Show(
                            "El estado inicial debe estar en la lista de estados.");

                        return;
                    }


                    if (finales.Any(f => !nombresEstados.Contains(
                        f, StringComparer.OrdinalIgnoreCase)))
                    {
                        MessageBox.Show(
                            "Todos los estados finales deben existir.");

                        return;
                    }


                    // Crear el tipo de autómata seleccionado.
                    if (cmbTipo.SelectedItem?.ToString() == "AFD")
                    {
                        automataActual = new AFD();
                    }
                    else
                    {
                        automataActual = new AFN();
                    }


                    // Crear los estados.
                    foreach (string n in nombresEstados)
                    {
                        automataActual.AgregarEstado(new Estado(n));
                    }


                    // Configurar el estado inicial.
                    Estado inicial = automataActual.Estados.First(
                        x => x.Nombre.Equals(
                            nombreInicial,
                            StringComparison.OrdinalIgnoreCase));

                    automataActual.EstablecerEstadoInicial(inicial);


                    // Configurar los estados finales.
                    foreach (string f in finales)
                    {
                        Estado estadoFinal =
                            automataActual.Estados.First(
                                x => x.Nombre.Equals(
                                    f,
                                    StringComparison.OrdinalIgnoreCase));

                        automataActual.AgregarEstadoFinal(estadoFinal);
                    }


                    // Configurar el alfabeto.
                    string[] simbolos = txtAlfabeto.Text
                        .Split(',')
                        .Select(a => a.Trim())
                        .Where(a => a.Length > 0)
                        .ToArray();


                    foreach (string simbolo in simbolos)
                    {
                        if (simbolo != "ε")
                        {
                            automataActual.Alfabeto.Add(simbolo);
                        }
                    }


                    // Limpiar las transiciones de la interfaz.
                    dgvTransiciones.Rows.Clear();


                    lblResultado.ForeColor = Color.DarkGreen;

                    lblResultado.Text =
                        "Autómata creado correctamente: " + nombre;


                    MessageBox.Show(
                        "El autómata se creó correctamente.",
                        "Simulador de Autómatas",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Error al crear el autómata",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            };


            // ==========================================
            // EVENTO: AGREGAR TRANSICIÓN
            // ==========================================

            btnAgregarTransicion.Click += (s, e) =>
            {
                try
                {
                    string origenNombre = txtOrigen.Text.Trim();
                    string simbolo = txtSimbolo.Text.Trim();
                    string destinoNombre = txtDestino.Text.Trim();


                    if (string.IsNullOrWhiteSpace(origenNombre) ||
                        string.IsNullOrWhiteSpace(simbolo) ||
                        string.IsNullOrWhiteSpace(destinoNombre))
                    {
                        MessageBox.Show(
                            "Completa el origen, símbolo y destino.");

                        return;
                    }


                    // Este simulador procesa un carácter por símbolo.
                    if (simbolo.Length != 1)
                    {
                        MessageBox.Show(
                            "Ingresa un solo carácter como símbolo.");

                        return;
                    }


                    Estado origen = automataActual.Estados.FirstOrDefault(
                        x => x.Nombre.Equals(
                            origenNombre,
                            StringComparison.OrdinalIgnoreCase));


                    Estado destino = automataActual.Estados.FirstOrDefault(
                        x => x.Nombre.Equals(
                            destinoNombre,
                            StringComparison.OrdinalIgnoreCase));


                    if (origen == null || destino == null)
                    {
                        MessageBox.Show(
                            "El origen y el destino deben ser estados existentes.");

                        return;
                    }


                    // Validar que el símbolo pertenezca al alfabeto.
                    bool esEpsilon = simbolo == "ε";

                    if (!automataActual.Alfabeto.Contains(simbolo)
                        && !(esEpsilon && automataActual is AFN))
                    {
                        MessageBox.Show(
                            "El símbolo no pertenece al alfabeto del autómata.");

                        return;
                    }


                    // Agregar la transición según el tipo.
                    if (automataActual is AFD afd)
                    {
                        afd.AgregarTransicionAFD(
                            origen, simbolo, destino);
                    }
                    else if (automataActual is AFN afn)
                    {
                        afn.AgregarTransicionAFN(
                            origen, simbolo, destino);
                    }


                    // Mostrar la transición en la tabla.
                    MostrarTransiciones();


                    txtOrigen.Clear();
                    txtSimbolo.Clear();
                    txtDestino.Clear();

                    lblResultado.ForeColor = Color.DarkGreen;

                    lblResultado.Text =
                        "Transición agregada correctamente.";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Error al agregar transición",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            };


            // ==========================================
            // EVENTO: SIMULAR CADENA
            // ==========================================

            btnSimular.Click += (s, e) =>
            {
                try
                {
                    if (automataActual.EstadoInicial == null)
                    {
                        MessageBox.Show(
                            "Primero debes crear el autómata.");

                        return;
                    }


                    string cadena = txtCadena.Text;

                    Simulador simulador = new Simulador();

                    bool aceptada = simulador.SimularAFN(
                        automataActual, cadena);


                    if (aceptada)
                    {
                        lblResultado.Text =
                            "✓ CADENA ACEPTADA\n\n" +
                            "La cadena pertenece al lenguaje del autómata.";

                        lblResultado.ForeColor = Color.DarkGreen;
                        lblResultado.BackColor =
                            Color.FromArgb(225, 247, 235);
                    }
                    else
                    {
                        lblResultado.Text =
                            "✗ CADENA RECHAZADA\n\n" +
                            "La cadena no pertenece al lenguaje del autómata.";

                        lblResultado.ForeColor = Color.DarkRed;
                        lblResultado.BackColor =
                            Color.FromArgb(255, 232, 232);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Error de simulación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            };


            // ==========================================
            // EVENTO: CONVERTIR AFN A AFD
            // ==========================================

            btnConvertir.Click += (s, e) =>
            {
                try
                {
                    if (automataActual is not AFN afn)
                    {
                        MessageBox.Show(
                            "El autómata actual no es un AFN.");

                        return;
                    }


                    if (afn.EstadoInicial == null)
                    {
                        MessageBox.Show(
                            "El AFN no tiene estado inicial.");

                        return;
                    }


                    ConversorAFNaAFD conversor =
                        new ConversorAFNaAFD();

                    AFD convertido = conversor.Convertir(afn);

                    automataActual = convertido;

                    cmbTipo.SelectedItem = "AFD";

                    MostrarTransiciones();

                    lblResultado.ForeColor = Color.DarkGreen;

                    lblResultado.Text =
                        "AFN convertido a AFD correctamente.";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Error de conversión",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            };


            // ==========================================
            // EVENTO: MINIMIZAR AFD
            // ==========================================

            btnMinimizar.Click += (s, e) =>
            {
                try
                {
                    if (automataActual is not AFD afd)
                    {
                        MessageBox.Show(
                            "Debes crear un AFD o convertir un AFN primero.");

                        return;
                    }


                    if (afd.EstadoInicial == null)
                    {
                        MessageBox.Show(
                            "El AFD no tiene estado inicial.");

                        return;
                    }


                    MinimizadorAFD minimizador =
                        new MinimizadorAFD();

                    automataActual = minimizador.Minimizar(afd);

                    MostrarTransiciones();

                    lblResultado.ForeColor = Color.DarkGreen;

                    lblResultado.Text =
                        "AFD minimizado correctamente.";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Error de minimización",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            };


            // ==========================================
            // EVENTO: LIMPIAR TODO
            // ==========================================

            btnLimpiar.Click += (s, e) =>
            {
                DialogResult confirmacion = MessageBox.Show(
                    "¿Deseas eliminar el autómata actual y limpiar los campos?",
                    "Confirmar limpieza",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);


                if (confirmacion != DialogResult.Yes)
                {
                    return;
                }


                automataActual = new AFN();

                cmbTipo.SelectedIndex = 0;

                txtNombre.Clear();
                txtAlfabeto.Text = "0,1";
                txtEstados.Clear();
                txtInicial.Text = "q0";
                txtFinales.Text = "q2";
                txtCadena.Clear();

                txtOrigen.Clear();
                txtSimbolo.Clear();
                txtDestino.Clear();

                dgvTransiciones.Rows.Clear();


                lblResultado.Text =
                    "Crea un autómata para comenzar.";

                lblResultado.ForeColor = Color.Black;

                lblResultado.BackColor = Color.White;
            };
        }


        // ==========================================
        // ACTUALIZAR TABLA DE TRANSICIONES
        // ==========================================

        private void MostrarTransiciones()
        {
            dgvTransiciones.Rows.Clear();

            foreach (Transicion t in automataActual.Transiciones)
            {
                dgvTransiciones.Rows.Add(
                    t.EstadoOrigen.Nombre,
                    t.Simbolo,
                    t.EstadoDestino.Nombre);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Evento de carga del formulario.
        }
    }
}
