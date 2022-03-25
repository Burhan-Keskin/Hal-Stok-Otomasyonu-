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
    public partial class frmUrunListeleme : Form
    {
        public frmUrunListeleme()
        {
            InitializeComponent();
        }

        SqlConnection baglantı = new SqlConnection("Data Source=DESKTOP-ICU6LR7;Initial Catalog=Stok_Takip;Integrated Security=True");
        DataSet daset = new DataSet();
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
        private void frmUrunListeleme_Load(object sender, EventArgs e)
        {
            ürünListele();
            kategorigetir();
            
        }

        private void ürünListele()
        {
            baglantı.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from urun", baglantı);
            adtr.Fill(daset, "urun");
            dataGridView1.DataSource = daset.Tables["urun"];
            baglantı.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            BarkodNotxt.Text = dataGridView1.CurrentRow.Cells["barkodno"].Value.ToString();
            Katagoritxt.Text = dataGridView1.CurrentRow.Cells["katagori"].Value.ToString();
            ÜrünAdıtxt.Text = dataGridView1.CurrentRow.Cells["urunadi"].Value.ToString();
            Miktarıtxt.Text = dataGridView1.CurrentRow.Cells["miktarı"].Value.ToString();
            AlışFiyatıtxt.Text = dataGridView1.CurrentRow.Cells["alisfıyati"].Value.ToString();
            SatışFiyatıtxt.Text = dataGridView1.CurrentRow.Cells["satisfiyati"].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglantı.Open();
            SqlCommand komut = new SqlCommand("update urun set urunadi=@urunadi,miktarı=@miktarı,alisfıyati=@alisfıyati,satisfiyati=@satisfıyati where barkodno=@barkodno",baglantı);
            komut.Parameters.AddWithValue("@barkodno",BarkodNotxt.Text);
            komut.Parameters.AddWithValue("@urunadi", ÜrünAdıtxt.Text);
            komut.Parameters.AddWithValue("@miktarı", int.Parse(Miktarıtxt.Text));
            komut.Parameters.AddWithValue("@alisfıyati",double.Parse(AlışFiyatıtxt.Text));
            komut.Parameters.AddWithValue("@satisfıyati",double.Parse(SatışFiyatıtxt.Text));
            komut.ExecuteNonQuery();
            baglantı.Close();
            daset.Tables["urun"].Clear();
            ürünListele();
            MessageBox.Show("Güncelleme Yapıldı");
            foreach(Control item in this.Controls)
            {
                if(item is TextBox)
                {
                    item.Text = "";
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(BarkodNotxt.Text!="")
            {
                baglantı.Open();
                SqlCommand komut = new SqlCommand("update urun set katagori=@katagori,urunadi=@urunadi where barkodno=@barkodno", baglantı);
                komut.Parameters.AddWithValue("@barkodno", BarkodNotxt.Text);
                komut.Parameters.AddWithValue("@katagori", comboBox1.Text);
                komut.Parameters.AddWithValue("@urunadi", comboBox2.Text);
                komut.ExecuteNonQuery();
                baglantı.Close();
                MessageBox.Show("Güncelleme Yapıldı");
                daset.Tables["urun"].Clear();
                ürünListele();

            }
            else
            {
                MessageBox.Show("BarkodNo yazılı değil");
            }
            foreach (Control item in this.Controls)
            {
                if (item is ComboBox)
                {
                    item.Text = "";
                }
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            comboBox2.Text = "";
            baglantı.Open();
            SqlCommand komut = new SqlCommand("select *from markabilgileri where katogori='" + comboBox1.SelectedItem + "'", baglantı);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboBox2.Items.Add(read["marka"].ToString());
            }
            baglantı.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglantı.Open();
            SqlCommand komıut = new SqlCommand("delete from urun where barkodno='" + dataGridView1.CurrentRow.Cells["barkodno"].Value.ToString() + "'", baglantı);
            komıut.ExecuteNonQuery();
            baglantı.Close();
            daset.Tables["urun"].Clear();
            ürünListele();
            MessageBox.Show("urun silindi");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DataTable tablo = new DataTable();
            baglantı.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select *from urun where barkodno like '%" + textBox1.Text + "%'", baglantı);
            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglantı.Close();
        }
    }
}
