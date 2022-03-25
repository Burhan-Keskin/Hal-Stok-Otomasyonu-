using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace stok_otomasyonu
{
    public partial class MüsteriYönetim : Form
    {
        public MüsteriYönetim()
        {
            InitializeComponent();
        }
        SqlConnection baglantı = new SqlConnection("Data Source=DESKTOP-ICU6LR7;Initial Catalog=Stok_Takip;Integrated Security=True");
        DataSet daset = new DataSet();
        private void button8_Click(object sender, EventArgs e)
        {
            frmUrunListeleme listele = new frmUrunListeleme();
            listele.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmMüşteriListeleme listele = new frmMüşteriListeleme();
            listele.ShowDialog();
        }
        private void hesapla()
        {
            try
            {
                baglantı.Open();
                SqlCommand komut = new SqlCommand("select sum(toplamfiyati) from sepet", baglantı);
                label9.Text = komut.ExecuteScalar() + "TL";
                baglantı.Close();
            }
            catch (Exception)
            {


            }
        }
        private void sepetlistele()
        {
            baglantı.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from sepet", baglantı);
            adtr.Fill(daset, "sepet");
            dataGridView1.DataSource = daset.Tables["sepet"];
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;
            baglantı.Close();
        }
        bool durum;
        private void barkodkontrol()
        {
            durum = true;
            baglantı.Open();
            SqlCommand komut = new SqlCommand("select * from sepet", baglantı);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (txtBarkodNo.Text == read["barkodno"].ToString())
                {
                    durum = false;
                }
            }
            baglantı.Close();
        }
        private void temizle()
        {
            if (txtBarkodNo.Text == "")
            {
                foreach (Control item in groupBox2.Controls)
                {
                    if (item is TextBox)
                    {
                        if (item != txtMiktar)
                        {
                            item.Text = "";
                        }
                    }
                }
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            barkodkontrol();
            if (durum == true)
            {
                baglantı.Open();
                SqlCommand komut = new SqlCommand("insert into sepet(tc,adsoyad,telefon,barkodno,urunadi,miktari,satisfiyati,toplamfiyati,tarih) values(@tc,@adsoyad,@telefon,@barkodno,@urunadi,@miktari,@satisfiyati,@toplamfiyati,@tarih)", baglantı);
                komut.Parameters.AddWithValue("@tc", txtTc.Text);
                komut.Parameters.AddWithValue("@adsoyad", txtAdSoyad.Text);
                komut.Parameters.AddWithValue("@telefon", txtTelefon.Text);
                komut.Parameters.AddWithValue("@barkodno", txtBarkodNo.Text);
                komut.Parameters.AddWithValue("@urunadi", txtÜrünMiktarı.Text);
                komut.Parameters.AddWithValue("@miktari", int.Parse(txtMiktar.Text));
                komut.Parameters.AddWithValue("@satisfiyati", double.Parse(txtSatışFiyatı.Text));
                komut.Parameters.AddWithValue("@toplamfiyati", double.Parse(txtToplamFiyat.Text));
                komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                baglantı.Close();
            }
            else
            {
                baglantı.Open();
                SqlCommand komut2 = new SqlCommand("update sepet set miktari=miktari+'" + int.Parse(txtMiktar.Text) + "' where barkodno='" + txtBarkodNo.Text + "'", baglantı);
                komut2.ExecuteNonQuery();
                SqlCommand komut3 = new SqlCommand("update sepet set toplamfiyati = miktari * satisfiyati where barkodno='" + txtBarkodNo.Text + "'", baglantı);
                komut3.ExecuteNonQuery();
                baglantı.Close();
            }

            daset.Tables["sepet"].Clear();
            sepetlistele();
            hesapla();
            foreach (Control item in groupBox2.Controls)
            {
                if (item is TextBox)
                {
                    if (item != txtMiktar)
                    {
                        item.Text = "";
                    }
                }
            }
            temizle();
        }
    }
    }

