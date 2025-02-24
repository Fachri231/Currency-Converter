using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Currency_Converter
{
    public partial class Register : Form
    {
        private DatabaseHelper _dbHelper = new DatabaseHelper();
        public Register()
        {
            InitializeComponent();
        }

        private void lbShowPassword_Click(object sender, EventArgs e)
        {

            if (cbShow.Checked)
            {
                cbShow.Checked = false;
            }
            else
            {
                cbShow.Checked = true;
            }
        }

        private void cbShow_CheckedChanged(object sender, EventArgs e)
        {
           if (cbShow.Checked)
            {
                txtPassword.PasswordChar = '\0';
                txtConfirmPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '*';
                txtConfirmPassword.PasswordChar = '*';
            }
        }

        private void llbLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login loginform = new Login();
            loginform.StartPosition = FormStartPosition.CenterScreen;
            this.Hide();
            loginform.ShowDialog();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Semua Field Harus Terisi", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Password Yang Anda Masukan Tidak Sesuai", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string HashedPassword = HashPassword(password);

            try
            {
                string cekQuery = "SELECT username FROM [users] WHERE username = @username";

                SqlParameter[] paramCek =
                {
                    new SqlParameter("@username", username)
                };

                object result = _dbHelper.excuteScalar(cekQuery, paramCek);
                int count = result != null ? Convert.ToInt32(result) : 0;

                if (count > 0)
                {
                    MessageBox.Show("Username Yang Anda Masukan Telah Di Gunakan", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string query = "INSERT INTO [users] ([username], [password]) VALUES (@username, @password)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@username", username),
                    new SqlParameter("@password", HashedPassword)
                };

                int affectedRow = _dbHelper.executeNonQuery(query, parameters);

                if (affectedRow > 0)
                {
                    MessageBox.Show("Register Berhasil!, Silakan Login", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Login loginform = new Login();
                    loginform.StartPosition = FormStartPosition.CenterScreen;
                    this.Hide();
                    loginform.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Register Gagal!, Silakan Login", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Terjadi Kesalahan Database : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi Kesalahan : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string HashPassword(string password)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha512.ComputeHash(bytes);

                StringBuilder hashString = new StringBuilder();

                foreach(byte b in hashBytes)
                {
                    hashString.Append(b.ToString("x2"));
                }
                return hashString.ToString();
            }
           
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
           
        }
    }
}
