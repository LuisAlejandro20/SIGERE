using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SIGERE.Controllers;
using SIGERE.Models;

namespace SIGERE.Views
{
    public class InventarioVista : Form
    {
        private Label lblTitulo;
        private Label lblSubtitulo;
        private Label lblBuscar;
        private TextBox txtCriterio;
        private Button btnBuscar;
        private Button btnMostrarTodos;
        private Button btnCerrar;
        private DataGridView dgvProductos;
        private Label lblMensaje;

        private readonly InventarioControl controller;

        public InventarioVista()
        {
            controller = new InventarioControl();
            InitializeComponent();
            CargarCatalogoInicial();
        }

        private void InitializeComponent()
        {
            this.Text = "SIGERE - Consulta de Disponibilidad (CU3)";
            this.Size = new Size(920, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 246, 248);

            lblTitulo = new Label
            {
                Text = "CONSULTA DE DISPONIBILIDAD",
                Font = new Font("Trebuchet MS", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(13, 27, 64), // #0D1B40
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(880, 40),
                Location = new Point(10, 15)
            };

            lblSubtitulo = new Label
            {
                Text = "Electrónica Aranda - Catálogo e Inventario en Tiempo Real",
                Font = new Font("Segoe UI", 12f, FontStyle.Italic),
                ForeColor = Color.FromArgb(70, 90, 120),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(880, 28),
                Location = new Point(10, 55)
            };

            lblBuscar = new Label
            {
                Text = "&Buscar producto (Nombre o Clave):",
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59), // #1E293B
                Location = new Point(30, 105),
                AutoSize = true
            };

            txtCriterio = new TextBox
            {
                Location = new Point(30, 132),
                Size = new Size(440, 34),
                Font = new Font("Segoe UI", 12f),
                TabIndex = 0,
                AccessibleName = "Criterio de búsqueda",
                AccessibleDescription = "Ingrese la clave exacta o parte del nombre del producto"
            };

            btnBuscar = new Button
            {
                Text = "&Buscar",
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                Location = new Point(485, 130),
                Size = new Size(130, 38),
                BackColor = Color.FromArgb(31, 111, 178), // #1F6FB2
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TabIndex = 1,
                AccessibleName = "Botón Buscar"
            };
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.Click += BtnBuscar_Click;

            btnMostrarTodos = new Button
            {
                Text = "&Ver Todos",
                Font = new Font("Segoe UI", 11.5f),
                Location = new Point(625, 130),
                Size = new Size(130, 38),
                BackColor = Color.FromArgb(71, 85, 105),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TabIndex = 2,
                AccessibleName = "Botón Ver Todos los productos"
            };
            btnMostrarTodos.FlatAppearance.BorderSize = 0;
            btnMostrarTodos.Click += (s, e) => { txtCriterio.Clear(); CargarCatalogoInicial(); };

            btnCerrar = new Button
            {
                Text = "&Volver",
                Font = new Font("Segoe UI", 11.5f),
                Location = new Point(765, 130),
                Size = new Size(110, 38),
                BackColor = Color.FromArgb(185, 28, 28),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TabIndex = 3,
                AccessibleName = "Botón Volver"
            };
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Click += (s, e) => this.Close();

            // Tabla de resultados (DataGridView)
            dgvProductos = new DataGridView
            {
                Location = new Point(30, 185),
                Size = new Size(845, 310),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 10.5f),
                TabIndex = 4,
                AccessibleName = "Tabla de disponibilidad de productos"
            };

            dgvProductos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(13, 27, 64);
            dgvProductos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProductos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            dgvProductos.EnableHeadersVisualStyles = false;

            ConfigurarColumnas();

            lblMensaje = new Label
            {
                Location = new Point(30, 505),
                Size = new Size(845, 35),
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.FromArgb(30, 41, 59),
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                AccessibleName = "Mensaje de estado de consulta"
            };

            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblSubtitulo);
            this.Controls.Add(lblBuscar);
            this.Controls.Add(txtCriterio);
            this.Controls.Add(btnBuscar);
            this.Controls.Add(btnMostrarTodos);
            this.Controls.Add(btnCerrar);
            this.Controls.Add(dgvProductos);
            this.Controls.Add(lblMensaje);

            this.AcceptButton = btnBuscar;
            this.CancelButton = btnCerrar;
        }

        private void ConfigurarColumnas()
        {
            dgvProductos.Columns.Clear();
            dgvProductos.Columns.Add("Clave", "Clave");
            dgvProductos.Columns.Add("Nombre", "Nombre del Producto");
            dgvProductos.Columns.Add("Categoria", "Categoría");
            dgvProductos.Columns.Add("Precio", "Precio Unitario");
            dgvProductos.Columns.Add("Existencia", "Existencia");
            dgvProductos.Columns.Add("Ubicacion", "Ubicación en Almacén");
            dgvProductos.Columns.Add("Disponibilidad", "Estado Stock");

            dgvProductos.Columns["Clave"].FillWeight = 12;
            dgvProductos.Columns["Nombre"].FillWeight = 28;
            dgvProductos.Columns["Categoria"].FillWeight = 15;
            dgvProductos.Columns["Precio"].FillWeight = 13;
            dgvProductos.Columns["Existencia"].FillWeight = 10;
            dgvProductos.Columns["Ubicacion"].FillWeight = 15;
            dgvProductos.Columns["Disponibilidad"].FillWeight = 15;
        }

        private void LlenarTabla(List<Producto> productos)
        {
            dgvProductos.Rows.Clear();
            foreach (var p in productos)
            {
                string estadoStock = p.Existencia > 0 ? "Disponible" : "Agotado";
                int rowIndex = dgvProductos.Rows.Add(
                    p.Clave,
                    p.Nombre,
                    p.NombreCategoria,
                    p.Precio.ToString("C2"),
                    p.Existencia,
                    p.Ubicacion,
                    estadoStock
                );

                // Escenario alterno 5a: Resaltar visualmente con texto e indicador si está agotado
                if (p.Existencia == 0)
                {
                    dgvProductos.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    dgvProductos.Rows[rowIndex].DefaultCellStyle.Font = new Font("Segoe UI", 10.5f, FontStyle.Bold);
                }
            }
        }

        private void CargarCatalogoInicial()
        {
            var resultado = controller.CargarTodosLosProductos();
            if (resultado.Exito)
            {
                LlenarTabla(resultado.Productos);
                lblMensaje.ForeColor = Color.FromArgb(30, 41, 59);
                lblMensaje.Text = resultado.Mensaje;
            }
            else
            {
                lblMensaje.ForeColor = Color.Red;
                lblMensaje.Text = resultado.Mensaje;
            }
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            string criterio = txtCriterio.Text;
            var resultado = controller.ConsultarProductos(criterio);

            if (resultado.Exito)
            {
                LlenarTabla(resultado.Productos);
                lblMensaje.ForeColor = Color.DarkGreen;
                lblMensaje.Text = resultado.Mensaje;
            }
            else
            {
                dgvProductos.Rows.Clear();
                lblMensaje.ForeColor = Color.Red;
                lblMensaje.Text = resultado.Mensaje;
            }
        }
    }
}