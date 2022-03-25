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
    public partial class frmsatış : Form
    {
        public frmsatış()
        {
            InitializeComponent();
        }
        SqlConnection baglantı = new SqlConnection("Data Source=DESKTOP-ICU6LR7;Initial Catalog=Stok_Takip;Integrated Security=True");
        DataSet daset = new DataSet();
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
        private void Form1_Load(object sender, EventArgs e)
        {
            sepetlistele();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmMüşteriEkle ekle = new frmMüşteriEkle();
            ekle.ShowDialog();
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmMüşteriListeleme listele = new frmMüşteriListeleme();
            listele.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            frmürünekle ekle = new frmürünekle();
            ekle.ShowDialog();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmKatogori kategori = new frmKatogori();
            kategori.ShowDialog();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmMarka marka = new frmMarka();
            marka.ShowDialog();

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

        private void button8_Click(object sender, EventArgs e)
        {
            frmUrunListeleme listele = new frmUrunListeleme();
            listele.ShowDialog();
        }

        private void txtTc_TextChanged(object sender, EventArgs e)
        {
            if(txtTc.Text=="")
            {
                txtAdSoyad.Text = "";
                txtTelefon.Text = "";

            }
            baglantı.Open();
            SqlCommand komut = new SqlCommand("select * from Müşteri where tc like '"+txtTc.Text+"'", baglantı);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                txtAdSoyad.Text=read["adsoyad"].ToString();
                txtTelefon.Text = read["adsoyad"].ToString();
            }
            baglantı.Close();
        }

        private void txtBarkodNo_TextChanged(object sender, EventArgs e)
        {
            if(txtBarkodNo.Text=="")
            {
                temizle();
            }
            baglantı.Open();
            SqlCommand komut = new SqlCommand("select * from urun where barkodno like '" + txtBarkodNo.Text + "'", baglantı);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                txtÜrünMiktarı.Text = read["urunadi"].ToString();
                txtSatışFiyatı.Text = read["satisfiyati"].ToString();
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
        private void btnEkle_Click(object sender, EventArgs e)
        {
            barkodkontrol();
            if(durum==true)
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
                SqlCommand komut2 = new SqlCommand("update sepet set miktari=miktari+'"+int.Parse(txtMiktar.Text)+ "' where barkodno='" + txtBarkodNo.Text + "'", baglantı);
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

        private void txtMiktar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToplamFiyat.Text =(double.Parse(txtMiktar.Text) * double.Parse(txtSatışFiyatı.Text)).ToString();
            }
            catch (Exception)
            {

                ;
            }
        }

        private void txtSatışFiyatı_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToplamFiyat.Text = (double.Parse(txtMiktar.Text) * double.Parse(txtSatışFiyatı.Text)).ToString();
            }
            catch (Exception)
            {

                ;
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            baglantı.Open();
            SqlCommand komut = new SqlCommand("delete from sepet where barkodno='"+dataGridView1.CurrentRow.Cells["barkodno"].Value.ToString()+"'", baglantı);
            komut.ExecuteNonQuery();
            baglantı.Close();
            
            MessageBox.Show("Ürün Sepetten Çıkarıldı");
            daset.Tables["sepet"].Clear();
            sepetlistele();
            hesapla();


        }

        private void btnSatışİptal_Click(object sender, EventArgs e)
        {
            baglantı.Open();
            SqlCommand komut = new SqlCommand("delete from sepet", baglantı);
            komut.ExecuteNonQuery();
            baglantı.Close();
            
            MessageBox.Show("Ürünler Sepetten Çıkarıldı");
            daset.Tables["sepet"].Clear();
            sepetlistele();
            hesapla();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            frmsatislistele listele = new frmsatislistele();
            listele.ShowDialog();
        }

        private void btnSatışYap_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
            {
                baglantı.Open();
                SqlCommand komut = new SqlCommand("insert into satis(tc,adsoyad,telefon,barkodno,urunadi,miktari,satisfiyati,toplamfiyati,tarih) values(@tc,@adsoyad,@telefon,@barkodno,@urunadi,@miktari,@satisfiyati,@toplamfiyati,@tarih)", baglantı);
                komut.Parameters.AddWithValue("@tc", txtTc.Text);
                komut.Parameters.AddWithValue("@adsoyad", txtAdSoyad.Text);
                komut.Parameters.AddWithValue("@telefon", txtTelefon.Text);
                komut.Parameters.AddWithValue("@barkodno", dataGridView1.Rows[i].Cells["barkodno"].Value.ToString());
                komut.Parameters.AddWithValue("@urunadi", dataGridView1.Rows[i].Cells["urunadi"].Value.ToString());
                komut.Parameters.AddWithValue("@miktari", int.Parse(dataGridView1.Rows[i].Cells["miktari"].Value.ToString()));
                komut.Parameters.AddWithValue("@satisfiyati", double.Parse(dataGridView1.Rows[i].Cells["satisfiyati"].Value.ToString()));
                komut.Parameters.AddWithValue("@toplamfiyati", double.Parse(dataGridView1.Rows[i].Cells["toplamfiyati"].Value.ToString()));
                komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                SqlCommand komut2 = new SqlCommand("update urun set miktarı=miktarı-'" + int.Parse(dataGridView1.Rows[i].Cells["miktari"].Value.ToString()) + "'where barkodno='" + dataGridView1.Rows[i].Cells["barkodno"].Value.ToString() + "'", baglantı);
                komut2.ExecuteNonQuery();
                baglantı.Close();
            }
            
            baglantı.Open();
            SqlCommand komut3 = new SqlCommand("delete from sepet", baglantı);
            komut3  .ExecuteNonQuery();
            baglantı.Close();
            daset.Tables["sepet"].Clear();
            sepetlistele();
            hesapla();
        }
    }
}
