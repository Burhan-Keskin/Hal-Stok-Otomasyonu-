using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace stok_otomasyonu
{
    public partial class frmKatogori : Form
    {
        public frmKatogori()
        {
            InitializeComponent();
        }
        SqlConnection baglantı = new SqlConnection("Data Source=DESKTOP-ICU6LR7;Initial Catalog=Stok_Takip;Integrated Security=True");
        bool durum;
        private void kategoriengelle()
        {
            durum = true;
            baglantı.Open();
            SqlCommand komut = new SqlCommand("select * from kategoribilgileri",baglantı);
            SqlDataReader read = komut.ExecuteReader();
            while(read.Read())
            {
                if (textBox1.Text == read["katagori"].ToString()||textBox1.Text=="")
                {
                    durum = false;
                }
            }
            baglantı.Close();

        }
        private void frmKatogori_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            kategoriengelle();
            if(durum==true)
            {
                baglantı.Open();
                SqlCommand komut = new SqlCommand("insert into kategoribilgileri(katagori)values(@kategori)", baglantı);
                komut.Parameters.AddWithValue("@kategori", textBox1.Text);
                komut.ExecuteNonQuery();
                baglantı.Close();
                
                MessageBox.Show("Kategori Eklendi");
            }
            else
            {
                MessageBox.Show("Böyle Bir kategori var");
            }
            textBox1.Text = "";

        }
    }
}
