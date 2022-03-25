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
    public partial class frmMarka : Form
    {
        public frmMarka()
        {
            InitializeComponent();
        }
        SqlConnection baglantı = new SqlConnection("Data Source=DESKTOP-ICU6LR7;Initial Catalog=Stok_Takip;Integrated Security=True");
        bool durum;
        private void markaengelle()
        {
            durum = true;
            baglantı.Open();
            SqlCommand komut = new SqlCommand("select * from markabilgileri", baglantı);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (comboBox1.Text==read["katogori"].ToString() && textBox1.Text == read["marka"].ToString() ||comboBox1.Text==""|| textBox1.Text == "")
                {
                    durum = false;
                }
            }
            baglantı.Close();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            markaengelle();
            if(durum==true)
            {
                baglantı.Open();
                SqlCommand komut = new SqlCommand("insert into markabilgileri(katogori,marka)values('" + comboBox1.Text + "','" + textBox1.Text + "')", baglantı);
                komut.ExecuteNonQuery();
                baglantı.Close();
               
                MessageBox.Show("Marka Eklendi");
            }
            else
            {
                MessageBox.Show("Böyle Bir Marka Mevcut");
            }
            textBox1.Text = "";
            comboBox1.Text = "";

        }
            private void kategorigetir()
        {
            baglantı.Open();
            SqlCommand komut = new SqlCommand("select *from kategoribilgileri", baglantı);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboBox1.Items.Add(read["katagori"].ToString());
            }
            baglantı.Close();
        }
        private void frmMarka_Load(object sender, EventArgs e)
        {
            kategorigetir();
             
        }
    }
}
