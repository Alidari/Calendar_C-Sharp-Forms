using System.Collections;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data.Common;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

//************************* ALÝ DARI - 20360859010 *********************************
namespace WinFormsApp3
{
    public partial class Form1 : Form
    {


        
        private List<string> saatler = new List<string> { "00:00", "01:00", "02:00", "03:00", "04:00", "05:00", "06:00", "07:00", "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00", "22:00", "23:00", };
        private List<string> colors = new List<string> { "#EF3E5B", "#F26270", "#F68FA0", "#6F5495", "#A09ED6", "#3F647E", "#688FAD", "#9FC1D3" };
        int activeWeek = 0;
        int[] seciliCell = new int[2];
        System.Windows.Forms.Button nextButton;
        System.Windows.Forms.Button backButton;
        System.Windows.Forms.Button deleteButton;
        Random random = new Random();
        String activeNot = "";
        Form2 f2;
        System.Windows.Forms.TextBox search;
        System.Windows.Forms.ListView aramaSonucListesi;
        List<Not> notlar = new List<Not>();
        int calendarYear;
        int calenarMonth;
        int[] calendar_days = new int[7];
        Label year_month;

        public static int secilmisSaatIndex;

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            InitializeDataGridView();
            InitializeNextButton();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int ekranWidth = Screen.PrimaryScreen.Bounds.Width;
            int ekranHeight = Screen.PrimaryScreen.Bounds.Height;

            this.Width = ekranWidth / 2;
            this.Height = ekranHeight;

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(this.Width, 0);
            nextButton = new System.Windows.Forms.Button();
            
        }

        private void InitializeDataGridView()
        {
            year_month = new Label();
            year_month.Text = "";
            year_month.Size = new Size(200, 30);
            year_month.Location = new Point(this.Width /2 , Top);

            dataGridView1.ScrollBars = ScrollBars.Both;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Width = this.Width + 100;
            dataGridView1.Height = this.Height;
            dataGridView1.Location = new Point(0, year_month.Bottom);


            dataGridView1.Columns.Add("saat", "Saatler");
            for (int i = 0; i < saatler.Count; i++)
            {
                //i += activeWeek;
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = saatler[i];
            }
            int index = 0;
            for (int i = 0; i < 7; i++)
            {
                
                DateTime currentDay = DateTime.Now.AddDays(i);
                string dayName = currentDay.ToString("dddd");
                string monthName = currentDay.ToString("MMMM");
                calendar_days[index] = currentDay.Day;
                index++;
                calenarMonth = currentDay.Month;
                calendarYear = currentDay.Year;
                year_month.Text = calendarYear + " - " + monthName;

                int dayOfMonth = currentDay.Day;

                dataGridView1.Columns.Add("Gun", $"{dayName} \n {dayOfMonth}");

            }
            dataGridView1.Columns["Gun"].HeaderCell.Style.BackColor = Color.RebeccaPurple;
            dataGridView1.EnableHeadersVisualStyles = false;

            dataGridView1.CellEndEdit += DataGridView1_CellEndEdit;
            dataGridView1.SelectionChanged += DataGridViewCellStateChangedEventHandler;

            this.Controls.Add(dataGridView1);
        }
        private void DataGridViewCellStateChangedEventHandler(object sender, EventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;

            if (dataGridView != null)
            {
                // Seçili hücre bilgisini al
                DataGridViewCell selectedCell = dataGridView.CurrentCell;

                if (selectedCell != null)
                {
                    seciliCell[1] = selectedCell.RowIndex;
                    seciliCell[0] = selectedCell.ColumnIndex;
                    
                    // Seçili hücrenin satýr ve sütun indeksleri
                   
                }
                
            }
        }
        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex > 0 && e.RowIndex >= 0)
            {
                activeNot = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();

                using (f2 = new Form2(activeNot, e.RowIndex))
                {
                    f2.AlinanNot = activeNot;
                    f2.ShowDialog();
                }
                
               
                int colorIndex = random.Next(0, colors.Count);

                string tarih = calendar_days[e.ColumnIndex-1] + "." + calenarMonth + "." + calendarYear;
                Not new_not = new Not(e.ColumnIndex, e.RowIndex, activeNot, activeWeek,secilmisSaatIndex,e.RowIndex, colorIndex, tarih);
                List<Not> not_control = Bul(new_not,"Index");
                if (not_control.Count != 0)
                {
                    not_control[0].text = new_not.text;
                }
                else
                {
                    notlar.Add(new_not);
                }

               

                gridColorUpdate();
                UpdateListView(search.Text);


            }
        }

        private void InitializeNextButton()
        {
            

            nextButton = new System.Windows.Forms.Button();
            nextButton.Text = "Ýleri";
            nextButton.Location = new Point(dataGridView1.Right - 100, dataGridView1.Bottom + 10);
            nextButton.Size = new Size(100, 30);
            nextButton.BackColor = Color.Turquoise;
            nextButton.ForeColor = Color.Black;
            nextButton.Visible = true;
            nextButton.BringToFront();
            nextButton.Click += NextEvent;
            this.Controls.Add(nextButton);

            backButton = new System.Windows.Forms.Button();
            backButton.Text = "Geri";
            backButton.Location = new Point(dataGridView1.Left, dataGridView1.Bottom + 10);
            backButton.Size = new Size(100, 30);
            backButton.BackColor = Color.Turquoise;
            backButton.ForeColor = Color.Black;
            backButton.Visible = true;
            backButton.BringToFront();
            backButton.Click += NextEvent;
            this.Controls.Add(backButton);

            deleteButton = new System.Windows.Forms.Button();
            deleteButton.Text = "Sil";
            deleteButton.Location = new Point((dataGridView1.Width/2)-20, dataGridView1.Bottom + 10);
            deleteButton.Size = new Size(100, 30);
            deleteButton.BackColor = Color.Turquoise;
            deleteButton.ForeColor = Color.Black;
            deleteButton.Visible = true;
            deleteButton.BringToFront();
            deleteButton.Click += NextEvent;
            this.Controls.Add(deleteButton);

            search = new System.Windows.Forms.TextBox();

            search.Text = "Arama Yapmak Ýstediðiniz Olayýn Ýsmini Giriniz";
            search.Location = new Point(deleteButton.Left-120, deleteButton.Bottom + 50);
            search.Size = new Size(350, 40);
            search.Multiline = false;
            search.BackColor = Color.White;
            search.TextChanged += textBox1_TextChanged;
            this.Controls.Add(search);


            aramaSonucListesi = new System.Windows.Forms.ListView();
            aramaSonucListesi.Location = new Point(deleteButton.Left - 120, search.Bottom + 10);
            aramaSonucListesi.Size = new Size(350, 300);
            aramaSonucListesi.View = View.Details;
            aramaSonucListesi.Columns.Add("Etkinlik", 120);
            aramaSonucListesi.Columns.Add("Tarih", 230);
            aramaSonucListesi.SelectedIndexChanged += ListView1_SelectedIndexChanged;

            

            this.Controls.Add(year_month);

            //aramaSonucListesi.BackColor = Color.Red;


            this.Controls.Add(aramaSonucListesi);
        }
        private void NextEvent(object sender, EventArgs e)
        {
            System.Windows.Forms.Button button = (System.Windows.Forms.Button)sender;

            if (button.Text == "Ýleri")
            {
                activeWeek += 7;
            }
            else if(button.Text == "Geri")
            {
                activeWeek -= 7;
            }
            else if (button.Text == "Sil")
            {
                
                List<Not> kontrol = Bul(new Not(seciliCell[0], seciliCell[1],"",activeWeek,0,0), "Index");

                if(kontrol.Count > 0)
                {
                    DialogResult result = MessageBox.Show("'" + dataGridView1.Rows[seciliCell[1]].Cells[seciliCell[0]].Value.ToString() + "' isimli etkinliði silmek istediðinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    // Kullanýcýnýn seçimine göre iþlem yapabilirsiniz.
                    if (result == DialogResult.Yes)
                    {
                        dataGridView1.Rows[seciliCell[1]].Cells[seciliCell[0]].Value = "";
                        foreach (Not item in notlar)
                        {
                            if (item.rowIndex == seciliCell[1] && item.columnIndex == seciliCell[0])
                            {
                                notlar.Remove(item);
                                updateGrid();
                                break;
                            }
                        }
                        
                    }
                    
                    


                }
                else
                {
                    MessageBox.Show("Seçtiðiniz Tarih Boþ");
                }

            }


            updateGrid();

        }

        public void updateGrid(object sender=null, string e=null)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("saat", "Saatler");
            for (int i = 0; i < saatler.Count; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = saatler[i];
            }


            int index = 0;
            // Yeni haftanýn gün sütunlarý ekleniyor
            for (int i = 0; i < 7; i++)
            {
                DateTime currentDay = DateTime.Now.AddDays(i + activeWeek);
                string dayName = currentDay.ToString("dddd");
                int dayOfMonth = currentDay.Day;
                string monthName = currentDay.ToString("MMMM");


                calendar_days[index++] = currentDay.Day;
                calenarMonth = currentDay.Month;
                calendarYear = currentDay.Year;
                year_month.Text = calendarYear + " - " + monthName;


                dataGridView1.Columns.Add($"Gun{i}", $"{dayName} \n {dayOfMonth}");

                if (currentDay.Date == DateTime.Today)
                {
                    dataGridView1.Columns[i + 1].HeaderCell.Style.BackColor = Color.RebeccaPurple;
                }
            }
            // Header hücrelerini renklendirme
            dataGridView1.EnableHeadersVisualStyles = false;
            gridColorUpdate();
        }

        
        public void gridColorUpdate()
        {
            Not control_not = new Not(0, 0, "", activeWeek, 0, 0);
            List<Not> aktif_notlar = Bul(control_not, "Week");
            
            
            foreach (Not item in aktif_notlar)
            {
                dataGridView1.Rows[item.rowIndex].Cells[item.columnIndex].Value = item.text;
                for (int i = 0; i <= item.sonSaat; i++)
                {;

                    string selectedColorHex = colors[item.colorIndex];

                    Color selectedColor = ColorTranslator.FromHtml(selectedColorHex);

                    dataGridView1.Rows[item.rowIndex + i].Cells[item.columnIndex].Style.BackColor = selectedColor;

                }


            }
        }

        public List<Not> Bul(Not not, string tur)
        {
            List<Not> bulunanNotlar = new List<Not>();

            if (tur == "Index")
            {
                foreach (Not item in notlar)
                {
                    if ((not.rowIndex == item.rowIndex) && (not.columnIndex == item.columnIndex) && (not.activeWeek == item.activeWeek))
                    {
                        bulunanNotlar.Add(item);
                    }
                }
            }
            else if(tur == "Week")
            {
                foreach (Not item in notlar)
                {
                    if(not.activeWeek == item.activeWeek)
                    {
                        bulunanNotlar.Add(item);
                    }
                }
            }
            else if(tur == "Text")
            {
                foreach (Not item in notlar)
                {
                    if (not.text == item.text)
                    {
                        bulunanNotlar.Add(item);
                    }
                }
            }
            else if (tur == "direct")
            {
                foreach (Not item in notlar)
                {
                    if (not.text == item.text && not.activeWeek == item.activeWeek)
                    {
                        bulunanNotlar.Add(item);
                    }
                }
            }
            return bulunanNotlar;
        }



        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // TextBox içeriði deðiþtiðinde burada çalýþacak kodu ekleyebilirsiniz.
            // Örneðin:
            System.Windows.Forms.TextBox textbox = (System.Windows.Forms.TextBox)sender;
            UpdateListView(textbox.Text);

        }
        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ListView listView = (System.Windows.Forms.ListView)sender;

            // ListView'dan seçili öðeleri al
            foreach (ListViewItem selectedItem in listView.SelectedItems)
            {
                string text = selectedItem.Text;
                
                activeWeek = Int16.Parse(selectedItem.SubItems[2].Text);
                List<Not> notlar = Bul(new Not(0, 0, text, activeWeek, 0, 0),"direct");
                updateGrid();
                dataGridView1.CurrentCell = dataGridView1.Rows[notlar[0].rowIndex].Cells[notlar[0].columnIndex];
            }
        }
        public void UpdateListView(string esleme)
        {
            aramaSonucListesi.Clear();
            aramaSonucListesi.Columns.Add("Etkinlik", 120);
            aramaSonucListesi.Columns.Add("Tarih", 230);
            if (esleme != null)
            {
                foreach (Not item in notlar)
            {
                
                    if (item.text.Substring(0, Math.Min(esleme.Length, item.text.Length)) == esleme)
                    {
                        ListViewItem item1 = new ListViewItem(item.text);
                        item1.SubItems.Add(item.tarih);
                        item1.SubItems.Add(item.activeWeek +"");
                        aramaSonucListesi.Items.AddRange(new ListViewItem[] { item1 });

                    }
                }
            }

        }
        

    }

    public class Not
    {
        public int columnIndex;
        public int rowIndex;
        public int activeWeek;
        public string text;
        public int sonSaat;
        public int ilkSaat;
        public int colorIndex;
        public string tarih;
        public Not(int columnIndex, int rowIndex, string text, int activeWeek, int Saatler,int ilkSaat,int colorIndex = 0,string tarih = "")
        {
            this.columnIndex = columnIndex;
            this.rowIndex = rowIndex;
            this.text = text;
            this.activeWeek = activeWeek;
            this.tarih = tarih;

            if(Saatler == null)
            {
                this.sonSaat = 0;
            }
            else
            {
                this.sonSaat = Saatler;
            }
            this.ilkSaat = ilkSaat;
            this.colorIndex = colorIndex;
        }

        
    }





   
   

}


