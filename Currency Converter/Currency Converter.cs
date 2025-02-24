using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Currency_Converter
{
    public partial class Currency_Converter : Form
    {
        private DatabaseHelper _dbHelper = new DatabaseHelper();

        public Currency_Converter()
        {
            InitializeComponent();
            loadPeriod();
            loadCurrencyOrigin();
            loadCurrencyConvert();
        }

        private void loadPeriod()
        {
            try
            {
                string query = "SELECT * FROM [Period]";

                DataTable periodTable = _dbHelper.getData(query);
                cbPeriod.DisplayMember = "name";
                cbPeriod.ValueMember = "id";
                cbPeriod.DataSource = periodTable;
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

        private void loadCurrencyOrigin()
        {
            try
            {
                string query = "SELECT * FROM [Currency]";

                DataTable currencyTable = _dbHelper.getData(query);
                cbOriginAmount.DisplayMember = "abbreviation";
                cbOriginAmount.ValueMember = "id";
                cbOriginAmount.DataSource = currencyTable;
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

        private void loadCurrencyConvert()
        {
            try
            {
                string query = "SELECT * FROM [Currency]";

                DataTable currencyTable = _dbHelper.getData(query);
                cbConvertTo.DisplayMember = "abbreviation";
                cbConvertTo.ValueMember = "id";
                cbConvertTo.DataSource = currencyTable;
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

        private decimal convertCurrency(decimal amount, int fromCurrency, int toCurrency, int periodId)
        {
            if (fromCurrency == 28)
            {
                decimal toRate = getExchangeRate(toCurrency, periodId);
                return amount * toRate;
            } 
            else if (toCurrency == 28)
            {
                decimal fromRate = getExchangeRate(fromCurrency, periodId);
                return amount / fromRate;
            }
            {
                decimal fromRate = getExchangeRate(fromCurrency, periodId);
                decimal toRate = getExchangeRate(toCurrency, periodId);
                decimal crossRate = toRate / fromRate;
                return amount * crossRate;
            }
        }

        private decimal getExchangeRate(int currencyId, int periodId)
        {
            decimal rate = 0;

            try
            {
                string query = "SELECT [rate] FROM [USDExchangeRate] WHERE [period_id] = @periodId AND [currency_id] = @currencyId";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@periodId", periodId),
                    new SqlParameter("@currencyId", currencyId)
                };

                rate = (decimal)_dbHelper.excuteScalar(query, parameters);

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Terjadi Kesalahan Database : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi Kesalahan : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return rate;
        }

       private void btnConverter_Click_1(object sender, EventArgs e)
        {

            if (decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                int fromCurrency = (int)cbOriginAmount.SelectedValue;
                int toCurrency = (int)cbConvertTo.SelectedValue;
                int periodId = (int)cbPeriod.SelectedValue;

                decimal result = convertCurrency(amount, fromCurrency, toCurrency, periodId);
                txtConvert.Text = result.ToString("N3");
            }
            else
            {
                MessageBox.Show("Masukan Angka Yang Valid", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cbOriginAmount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbOriginAmount.SelectedItem != null)
            {
                

                int currencyId = (int)cbOriginAmount.SelectedValue;

                try
                {
                    string query = "SELECT [country] FROM [Currency] WHERE id = @currencyId";

                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@currencyId", currencyId)
                    };

                    string country = _dbHelper.excuteScalar(query, parameters).ToString();

                    lbContryOrigin.Text = country;

                    string selectedCountry = country;
                    string imgPath = "C:/Users/Advan/Documents/LKS Provinsi DKI Jakarta 2024/Currency Converter/assets/";

                    if (selectedCountry == country)
                    {
                        imgPath = Path.Combine(imgPath, country + ".png");
                    }
                    else
                    {
                        imgPath = "";
                    }

                    if (!string.IsNullOrEmpty(imgPath))
                    {
                        pbCountry1.SizeMode = PictureBoxSizeMode.StretchImage;
                        pbCountry1.Image = Image.FromFile(imgPath);
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
        }

        private void cbConvertTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbConvertTo.SelectedItem != null)
            {
                int currencyId = (int)cbConvertTo.SelectedValue;

                try
                {
                    string query = "SELECT [country] FROM [Currency] WHERE id = @currencyId";

                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@currencyId", currencyId)
                    };

                    string country = _dbHelper.excuteScalar(query, parameters).ToString();

                    lbContryConvert.Text = country;

                    string selectedCountry = country;
                    string imgPath = "C:/Users/Advan/Documents/LKS Provinsi DKI Jakarta 2024/Currency Converter/assets/";

                    if (selectedCountry == country)
                    {
                        imgPath = Path.Combine(imgPath, country + ".png");
                    }
                    else
                    {
                        MessageBox.Show("Bendara Tidak Tersedia", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    if (!string.IsNullOrEmpty(imgPath))
                    {
                        pbCountry2.SizeMode = PictureBoxSizeMode.StretchImage;
                        pbCountry2.Image = Image.FromFile(imgPath);
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

        }    
    }
}