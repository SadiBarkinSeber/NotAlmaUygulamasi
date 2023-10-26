using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NotAlmaUygulamasi
{
    public partial class GirisEkrani : Form
    {
        public GirisEkrani()
        {
            InitializeComponent();
        }

        private void lnkKayit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void btnGirisYap_Click(object sender, EventArgs e)
        {

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string filePath = Path.Combine(desktopPath, "NotUygulamasi", "Password.txt");

            try
            {
                // Dosya var mı
                if (File.Exists(filePath))
                {
                    // Dosyayı oku
                    string[] lines = File.ReadAllLines(filePath);

                    foreach (string line in lines)
                    {
                        // kullanıcı adı ve parola kısmına ayır
                        string[] parts = line.Split(';');

                        if (parts.Length == 2)
                        {
                            if (txtAdSoyad.Text == parts[0] && txtSifre.Text == parts[1])
                            {
                                MessageBox.Show("Giriş başarılı!");

                                // Notlar formunu başlatırken kullaniciAdi parametresini ver
                                string kullaniciAdi = txtAdSoyad.Text;
                                Notlar notlar = new Notlar(txtAdSoyad.Text);
                                notlar.Show();
                                this.Hide();

                                return;
                            }
                        }
                    }
                    MessageBox.Show("Geçersiz kullanıcı adı veya parola!");
                }
                else
                {
                    MessageBox.Show("Password.txt dosyası bulunamadı. Lütfen kayıt olunuz");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
        }
        public string KullaniciAdi
        {
            get { return txtAdSoyad.Text; }
        }
    }
}
