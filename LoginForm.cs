using System;
using System.Drawing;
using System.Windows.Forms;
using SIGERE.Controllers;

namespace SIGERE.Views
{
    public class LoginForm : Form
    {
        private TextBox txtUsuario;
        private TextBox txtPassword;
        private Button btnIngresar;
        private Label lblTitulo;
        private Label lblUsuario;
        private Label lblPassword;
        private Label lblMensaje;

        private readonly UsuarioController controller;

        public LoginForm()
        {
            controller = new UsuarioController();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "SIGERE - Punto de Venta (Login)";
            this.Size = new Size(400, 320);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            lblTitulo = new Label
            {
                Text = "SISTEMA SIGERE\nElectrónica Aranda",
                Font = new Font("Arial", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(360, 50),
                Location = new Point(10, 15)
            };

            lblUsuario = new Label { Text = "Usuario:", Location = new Point(40, 80), AutoSize = true };
            txtUsuario = new TextBox { Location = new Point(130, 77), Size = new Size(200, 25) };

            lblPassword = new Label { Text = "Contraseña:", Location = new Point(40, 120), AutoSize = true };
            txtPassword = new TextBox { Location = new Point(130, 117), Size = new Size(200, 25), PasswordChar = '*' };

            btnIngresar = new Button
            {
                Text = "Ingresar",
                Location = new Point(130, 160),
                Size = new Size(200, 35),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnIngresar.Click += BtnIngresar_Click;

            lblMensaje = new Label
            {
                Location = new Point(20, 210),
                Size = new Size(350, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Red
            };

            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblUsuario);
            this.Controls.Add(txtUsuario);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnIngresar);
            this.Controls.Add(lblMensaje);
        }

        private void BtnIngresar_Click(object sender, EventArgs e)
        {
            string user = txtUsuario.Text.Trim();
            string pass = txtPassword.Text.Trim();

            var resultado = controller.IniciarSesion(user, pass);

            if (resultado.Exito)
            {
                lblMensaje.ForeColor = Color.Green;
                lblMensaje.Text = resultado.Mensaje;
                MessageBox.Show(resultado.Mensaje, "Acceso Concedido", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                lblMensaje.ForeColor = Color.Red;
                lblMensaje.Text = resultado.Mensaje;
            }
        }
    }
}