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
        private Label lblSubtitulo;
        private Label lblUsuario;
        private Label lblPassword;
        private Label lblOlvidoPassword;
        private Label lblMensaje;

        private Panel pnlTelefono;
        private TextBox txtTelefono;
        private Button btnValidarTel;
        private Label lblOtroMetodo;

        private Panel pnlCambiarPass;
        private TextBox txtPass1;
        private TextBox txtPass2;
        private Button btnCambiarPass;

        private Panel pnlCorreo;
        private TextBox txtCorreo;
        private Button btnEnviarCorreo;
        private Label lblVolverLogin;

        private readonly UsuarioController controller;

        public LoginForm()
        {
            controller = new UsuarioController();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "SIGERE - Punto de Venta (Login)";
            this.Size = new Size(480, 360);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 244, 248);

            lblTitulo = new Label
            {
                Text = "SIGERE",
                Font = new Font("Trebuchet MS", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 40, 85),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(440, 32),
                Location = new Point(10, 15)
            };

            lblSubtitulo = new Label
            {
                Text = "Electrónica Aranda",
                Font = new Font("Segoe UI", 10.5f, FontStyle.Italic),
                ForeColor = Color.FromArgb(70, 90, 120),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(440, 22),
                Location = new Point(10, 47)
            };

            lblUsuario = new Label { Text = "Usuario:", Font = new Font("Segoe UI", 10), Location = new Point(40, 95), AutoSize = true };
            txtUsuario = new TextBox { Location = new Point(170, 92), Size = new Size(240, 26), Font = new Font("Segoe UI", 10) };

            lblPassword = new Label { Text = "Contraseña:", Font = new Font("Segoe UI", 10), Location = new Point(40, 135), AutoSize = true };
            txtPassword = new TextBox { Location = new Point(170, 132), Size = new Size(240, 26), Font = new Font("Segoe UI", 10), PasswordChar = '*' };

            btnIngresar = new Button
            {
                Text = "Ingresar",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(170, 172),
                Size = new Size(240, 36),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnIngresar.FlatAppearance.BorderSize = 0;
            btnIngresar.Click += BtnIngresar_Click;

            lblOlvidoPassword = new Label
            {
                Text = "¿Olvidaste la contraseña?",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Underline),
                ForeColor = Color.FromArgb(0, 102, 204),
                Cursor = Cursors.Hand,
                Location = new Point(170, 218),
                AutoSize = true
            };
            lblOlvidoPassword.Click += (s, e) => MostrarVistaTelefono();

            pnlTelefono = new Panel
            {
                Location = new Point(10, 85),
                Size = new Size(440, 170),
                Visible = false,
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.None
            };

            pnlTelefono.Controls.Add(new Label { Text = "Teléfono:", Font = new Font("Segoe UI", 10), Location = new Point(30, 12), AutoSize = true });
            txtTelefono = new TextBox { Location = new Point(160, 9), Size = new Size(140, 26), Font = new Font("Segoe UI", 10) };
            
            btnValidarTel = new Button
            {
                Text = "Validar",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(310, 7),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnValidarTel.FlatAppearance.BorderSize = 0;
            btnValidarTel.Click += BtnValidarTel_Click;

            lblOtroMetodo = new Label
            {
                Text = "Elegir otro método",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Underline),
                ForeColor = Color.FromArgb(0, 102, 204),
                Cursor = Cursors.Hand,
                Location = new Point(30, 46),
                AutoSize = true
            };
            lblOtroMetodo.Click += (s, e) => MostrarVistaCorreo();

            pnlTelefono.Controls.Add(txtTelefono);
            pnlTelefono.Controls.Add(btnValidarTel);
            pnlTelefono.Controls.Add(lblOtroMetodo);

            pnlCambiarPass = new Panel
            {
                Location = new Point(20, 75),
                Size = new Size(400, 90),
                Visible = false,
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.None
            };
            pnlCambiarPass.Controls.Add(new Label { Text = "Nueva contraseña:", Font = new Font("Segoe UI", 9.5f), Location = new Point(10, 8), AutoSize = true });
            txtPass1 = new TextBox { Location = new Point(140, 5), Size = new Size(140, 26), Font = new Font("Segoe UI", 10), PasswordChar = '*' };

            pnlCambiarPass.Controls.Add(new Label { Text = "Repetir contraseña:", Font = new Font("Segoe UI", 9.5f), Location = new Point(10, 45), AutoSize = true });
            txtPass2 = new TextBox { Location = new Point(140, 42), Size = new Size(140, 26), Font = new Font("Segoe UI", 10), PasswordChar = '*' };

            btnCambiarPass = new Button
            {
                Text = "Cambiar\ncontraseña",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Location = new Point(290, 5),
                Size = new Size(90, 63),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCambiarPass.FlatAppearance.BorderSize = 0;
            btnCambiarPass.Click += BtnCambiarPass_Click;

            pnlCambiarPass.Controls.Add(txtPass1);
            pnlCambiarPass.Controls.Add(txtPass2);
            pnlCambiarPass.Controls.Add(btnCambiarPass);
            pnlTelefono.Controls.Add(pnlCambiarPass);

            pnlCorreo = new Panel
            {
                Location = new Point(10, 85),
                Size = new Size(440, 140),
                Visible = false,
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.None
            };

            pnlCorreo.Controls.Add(new Label { Text = "Correo electrónico:", Font = new Font("Segoe UI", 10), Location = new Point(30, 15), AutoSize = true });
            txtCorreo = new TextBox { Location = new Point(160, 12), Size = new Size(140, 26), Font = new Font("Segoe UI", 10) };
            
            btnEnviarCorreo = new Button
            {
                Text = "Enviar",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(310, 10),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnEnviarCorreo.FlatAppearance.BorderSize = 0;
            btnEnviarCorreo.Click += BtnEnviarCorreo_Click;

            lblVolverLogin = new Label
            {
                Text = "Volver al inicio de sesión",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Underline),
                ForeColor = Color.FromArgb(0, 102, 204),
                Cursor = Cursors.Hand,
                Location = new Point(30, 52),
                AutoSize = true
            };
            lblVolverLogin.Click += (s, e) => MostrarVistaLogin();

            pnlCorreo.Controls.Add(txtCorreo);
            pnlCorreo.Controls.Add(btnEnviarCorreo);
            pnlCorreo.Controls.Add(lblVolverLogin);

            lblMensaje = new Label
            {
                Location = new Point(10, 270),
                Size = new Size(440, 35),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Red,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblSubtitulo);
            this.Controls.Add(lblUsuario);
            this.Controls.Add(txtUsuario);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnIngresar);
            this.Controls.Add(lblOlvidoPassword);
            this.Controls.Add(pnlTelefono);
            this.Controls.Add(pnlCorreo);
            this.Controls.Add(lblMensaje);
        }

        private void MostrarVistaLogin()
        {
            lblUsuario.Visible = true;
            txtUsuario.Visible = true;
            lblPassword.Visible = true;
            txtPassword.Visible = true;
            btnIngresar.Visible = true;
            lblOlvidoPassword.Visible = true;

            pnlTelefono.Visible = false;
            pnlCorreo.Visible = false;
            pnlCambiarPass.Visible = false;

            txtUsuario.Clear();
            txtPassword.Clear();
            txtTelefono.Clear();
            txtPass1.Clear();
            txtPass2.Clear();
            txtCorreo.Clear();
        }

        private void MostrarVistaTelefono()
        {
            lblUsuario.Visible = false;
            txtUsuario.Visible = false;
            lblPassword.Visible = false;
            txtPassword.Visible = false;
            btnIngresar.Visible = false;
            lblOlvidoPassword.Visible = false;

            pnlTelefono.Visible = true;
            pnlCorreo.Visible = false;
            pnlCambiarPass.Visible = false;

            txtUsuario.Clear();
            txtPassword.Clear();
            txtTelefono.Clear();
            txtPass1.Clear();
            txtPass2.Clear();
            txtCorreo.Clear();
            lblMensaje.Text = "";
        }

        private void MostrarVistaCorreo()
        {
            pnlTelefono.Visible = false;
            pnlCorreo.Visible = true;
            pnlCambiarPass.Visible = false;

            txtUsuario.Clear();
            txtPassword.Clear();
            txtTelefono.Clear();
            txtPass1.Clear();
            txtPass2.Clear();
            txtCorreo.Clear();
            lblMensaje.Text = "";
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
                txtUsuario.Clear();
                txtPassword.Clear();
            }
            else
            {
                lblMensaje.ForeColor = Color.Red;
                lblMensaje.Text = resultado.Mensaje;
            }
        }

        private void BtnValidarTel_Click(object sender, EventArgs e)
        {
            var resultado = controller.ValidarTelefonoRecuperacion(txtTelefono.Text.Trim());
            if (resultado.Exito)
            {
                lblMensaje.ForeColor = Color.Green;
                lblMensaje.Text = resultado.Mensaje;
                pnlCambiarPass.Visible = true;
                txtPass1.Clear();
                txtPass2.Clear();
            }
            else
            {
                lblMensaje.ForeColor = Color.Red;
                lblMensaje.Text = resultado.Mensaje;
                pnlCambiarPass.Visible = false;
            }
        }

        private void BtnCambiarPass_Click(object sender, EventArgs e)
        {
            var resultado = controller.ReestablecerPassword(txtTelefono.Text.Trim(), txtPass1.Text.Trim(), txtPass2.Text.Trim());
            if (resultado.Exito)
            {
                MostrarVistaLogin();
                lblMensaje.ForeColor = Color.Green;
                lblMensaje.Text = resultado.Mensaje;
            }
            else
            {
                lblMensaje.ForeColor = Color.Red;
                lblMensaje.Text = resultado.Mensaje;
            }
        }

        private void BtnEnviarCorreo_Click(object sender, EventArgs e)
        {
            var resultado = controller.EnviarCorreoVerificacion(txtCorreo.Text.Trim());
            if (resultado.Exito)
            {
                lblMensaje.ForeColor = Color.Green;
                lblMensaje.Text = resultado.Mensaje;
            }
            else
            {
                lblMensaje.ForeColor = Color.Red;
                lblMensaje.Text = resultado.Mensaje;
            }
        }
    }
}