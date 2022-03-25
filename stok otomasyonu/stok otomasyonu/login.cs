using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace stok_otomasyonu
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }
       
        SqlConnection baglantı = new SqlConnection("Data Source=DESKTOP-ICU6LR7;Initial Catalog=Stok_Takip;Integrated Security=True");
        private void button1_Click(object sender, EventArgs e)
        {
            MüsteriYönetim f3 = new MüsteriYönetim();
            baglantı.Open();
            SqlCommand komut = new SqlCommand("SELECT * FROM Müşteri where adsoyad = '" + textBox1.Text + "' AND tc = '" + textBox2.Text + "'",baglantı);
            SqlDataReader read = komut.ExecuteReader();
            if (read.Read())
            {
                f3.Show();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı ya da şifre yanlış");
            }

            baglantı.Close();
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmMüşteriEkle ekle = new frmMüşteriEkle();
            ekle.ShowDialog();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
        }

        private void login_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            baglantı.Open();
            SqlCommand komut = new SqlCommand("SELECT * FROM Müşteri where adsoyad = '" + textBox1.Text + "' AND tc = '" + textBox2.Text + "'", baglantı);
            SqlDataReader read = komut.ExecuteReader();
            if (read.Read())
            {
                frmsatış f2 = new frmsatış();
                f2.Show();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı ya da şifre yanlış");
            }

            baglantı.Close();

        }
    }
    }

