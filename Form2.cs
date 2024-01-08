using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp3
{

    public partial class Form2 : Form
    {

        //String[] =
        private List<string> saatler = new List<string> { "00:00", "01:00", "02:00", "03:00", "04:00", "05:00", "06:00", "07:00", "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00", "22:00", "23:00"};
        private Label txtNot;
        private Label saatSec;
        Button btnKaydet;
        String not;
        ComboBox saatSecici;
        int saatIndex;
        
    

        
        public string AlinanNot
        {
            get { return txtNot.Text; }
            set { txtNot.Text = value; }
        }

        public Form2(string Not,int Index)
        {
            InitializeComponent();
            InitializeComponents();

            this.Width= 500;
            this.Height= 500;
            not = Not;
            saatIndex = Index;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            saatSecici = new ComboBox();
            saatSecici.Location = new Point(btnKaydet.Right - btnKaydet.Width, btnKaydet.Bottom);
            saatSecici.Name = "Etkinliğin devam edeceği saat";
            saatSecici.Size = new Size(100, 50);
            for (int i = saatIndex; i <saatler.Count; i++)
            {
                saatSecici.Items.Add($"{saatler[i]}");
            }
            saatSecici.SelectedIndex = 0;
            this.Controls.Add(saatSecici);

            saatSec = new Label();
            saatSec.Text = "Etkinlik hangi saate kadar devam edecek";
            saatSec.Location = new Point(saatSecici.Left-70, btnKaydet.Bottom+50);
            saatSec.Size = new Size(400, 50);
            this.Controls.Add(saatSec);
            
        }

        private void InitializeComponents()
        {
            txtNot = new Label();
            txtNot.Text = AlinanNot;
            txtNot.Size = new System.Drawing.Size(400, 30);
            

            btnKaydet = new Button();
            btnKaydet.Text = "Kaydet";
            btnKaydet.Click += BtnKaydet_Click;
            btnKaydet.Location = new Point((this.Width - btnKaydet.Width)/4, txtNot.Bottom + 10);
            btnKaydet.Size = new Size(100, 30);
            btnKaydet.BackColor = Color.Turquoise;
            btnKaydet.ForeColor = Color.Black;
            btnKaydet.Visible = true;
            btnKaydet.BringToFront();

            txtNot.Location = new Point(btnKaydet.Left+20,10);
            this.Controls.Add(btnKaydet);
            this.Controls.Add(txtNot);
            this.Controls.Add(btnKaydet);
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            string not =txtNot.Text;
            // Notu kullanmak için yapılacak işlemleri buraya ekleyin
            MessageBox.Show($"{not} isimli etkinlik {saatler[saatIndex]} : {saatler[saatIndex+ saatSecici.SelectedIndex]} arasına başarıyla kaydedildi");
            Form1.secilmisSaatIndex = saatSecici.SelectedIndex;
            // Formu kapatın
            this.Hide();
        }


        
    }
}
