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
    public partial class frmsatislistele : Form
    {
        public frmsatislistele()
        {
            InitializeComponent();
        }
        SqlConnection baglantı = new SqlConnection("Data Source=DESKTOP-ICU6LR7;Initial Catalog=Stok_Takip;Integrated Security=True");
        DataSet daset = new DataSet();
        private void satislistele()
        {
            baglantı.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from satis", baglantı);
            adtr.Fill(daset, "satis");
            dataGridView1.DataSource = daset.Tables["satis"];
            baglantı.Close();
        }
        private void frmsatislistele_Load(object sender, EventArgs e)
        {
            satislistele();
        }
    }
}
