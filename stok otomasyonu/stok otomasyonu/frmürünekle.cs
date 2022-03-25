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
    public partial class frmürünekle : Form
    {
        public frmürünekle()
        {
            InitializeComponent();
        }
        SqlConnection baglantı = new SqlConnection("Data Source=DESKTOP-ICU6LR7;Initial Catalog=Stok_Takip;Integrated Security=True");
        bool durum;
        private void barkodkontrol()
        {
            durum = true;
            baglantı.Open();
            SqlCommand komut = new SqlCommand("select * from urun", baglantı);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (txtBarkodNo.Text==read["barkodno"].ToString()||txtBarkodNo.Text=="")
                {
                    durum = false;
                }
            }
            baglantı.Close();

        }
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if(BarkodNotxt.Text=="")
            {
                label15.Text = "";
                foreach(Control item in groupBox2.Controls)
                {
                    if(item is TextBox)
                    {
                        item.Text = "";
                    }
                }
            }
            baglantı.Open();
            SqlCommand komut = new SqlCommand("Select * from urun where barkodno like'"+BarkodNotxt.Text+"'",baglantı);
            SqlDataReader read = komut.ExecuteReader();
            while(read.Read())
            {
                Katagoritxt.Text = read["katagori"].ToString();
                //Markatxt.Text = read["marka"].ToString();
                ÜrünAdıtxt.Text = read["urunadi"].ToString();
                label15.Text = read["miktarı"].ToString();
                AlışFiyatıtxt.Text = read["alisfıyati"].ToString();
                SatışFiyatıtxt.Text = read["Satisfiyati"].ToString();
                
            }
            baglantı.Close();
        }

        private void btnYeniEkle_Click(object sender, EventArgs e)
        {
            barkodkontrol();
            if (durum == true)
            {
                baglantı.Open();
                SqlCommand komut = new SqlCommand("insert into urun(barkodno,katagori,urunadi,miktarı,alisfıyati,satisfiyati,tarih) values(@barkodno,@katagori,@urunadi,@miktarı,@alisfiyati,@satisfiyatı,@tarih)", baglantı);
                komut.Parameters.AddWithValue("@barkodno", txtBarkodNo.Text);
                komut.Parameters.AddWithValue("@katagori", comboKategori.Text);
                komut.Parameters.AddWithValue("@marka", comboMarka.Text);
                komut.Parameters.AddWithValue("@urunadi", txtÜrünAdı.Text);
                komut.Parameters.AddWithValue("@miktarı", int.Parse(txtMiktarı.Text));
                komut.Parameters.AddWithValue("@alisfiyati", double.Parse(txtAlışFiyatı.Text));
                komut.Parameters.AddWithValue("@satisfiyatı", double.Parse(txtSatışFiyatı.Text));
                komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                baglantı.Close();
                MessageBox.Show("Ürün Eklendi");
                

            }
            else
            {
                MessageBox.Show("Bu Barkod Numarasından Kayıt Mevcut");
            }
            comboMarka.Items.Clear();
            foreach (Control item in groupBox1.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
                if (item is ComboBox)
                {
                    item.Text = "";
                }
            }



        }
        
        private void kategorigetir()

        {
            baglantı.Open();
            SqlCommand komut = new SqlCommand("select *from kategoribilgileri", baglantı);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboKategori.Items.Add(read["katagori"].ToString());
            }
            baglantı.Close();
        }
        private void frmürünekle_Load(object sender, EventArgs e)
        {
            kategorigetir();

                
        }

        private void comboKategori_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboMarka.Items.Clear();
            comboMarka.Text = "";
            baglantı.Open();
            SqlCommand komut = new SqlCommand("select *from markabilgileri where katogori='"+comboKategori.SelectedItem+"'", baglantı);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboMarka.Items.Add(read["marka"].ToString());
            }
            baglantı.Close();
        }

        private void btnVarOlanaEkle_Click(object sender, EventArgs e)
        {
           
            baglantı.Open();
            SqlCommand komut = new SqlCommand("update urun set miktarı=miktarı+'"+int.Parse(Miktarıtxt.Text)+"'where barkodno='"+BarkodNotxt.Text+"'",baglantı);
            komut.ExecuteNonQuery();
            baglantı.Close();
            foreach (Control item in groupBox2.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
                
            }
            MessageBox.Show("Var olan ürüne ekleme yapıldı");
        }
    }
}
