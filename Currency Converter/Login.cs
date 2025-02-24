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
    public partial class Login : Form
    {
        private DatabaseHelper _dbHelper = new DatabaseHelper();
        public Login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (cbShowPass.Checked == false)
            {
                cbShowPass.Checked = true;
            } 
            else if (cbShowPass.Checked == true)
            {
                cbShowPass.Checked = false;
            }
        }

        private void llbRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Register registerform = new Register();
            registerform.StartPosition = FormStartPosition.CenterScreen;
            this.Hide();
            registerform.ShowDialog();
        }

        private void cbShowPass_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = cbShowPass.Checked ? '\0' : '*';

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Masukan Username Dan Password", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string HashedPassword = HashPassword(password);

            try
            {
                string query = "SELECT COUNT(*) FROM [users] WHERE username = @username AND password = @password";

                SqlParameter[] parameters =
                {
                        new SqlParameter("@username", username),
                        new SqlParameter("@password", HashedPassword)
                    };

                object result = _dbHelper.excuteScalar(query, parameters);
                int count = result != null ? (int)result : 0;

                if (count > 0)
                {
                    MessageBox.Show("Login Berhasil!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Currency_Converter currencyform = new Currency_Converter();
                    currencyform.StartPosition = FormStartPosition.CenterScreen;
                    this.Hide();
                    currencyform.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Username Atau Password Salah", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                foreach (byte b in hashBytes)
                {
                    hashString.Append(b.ToString("x2"));
                }
                return hashString.ToString();
            }
        }
    }
}
