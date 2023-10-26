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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace NotAlmaUygulamasi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            btnKayitEkle.Enabled = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            btnKayitEkle.Enabled = cbSozlesme.Checked;
        }

        private void btnKayitEkle_Click(object sender, EventArgs e)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string folderPath = Path.Combine(desktopPath, "NotUygulamasi");

            int minimumUzunluk = 6;

            if (txtKullaniciAdi.Text.Length < minimumUzunluk)
            {
                MessageBox.Show("Kullanıcı adı en az 6 karakter olmalıdır.");
                return; 
            }
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = Path.Combine(folderPath, "Password.txt");

                
                if (!File.Exists(filePath))
                {
                    using (StreamWriter sw = File.CreateText(filePath))
                    {
                        
                        sw.WriteLine(txtKullaniciAdi.Text + ";" + txtSifre.Text);
                    }
                }
                else
                {
                    
                    using (StreamWriter sw = File.AppendText(filePath))
                    {
                        sw.WriteLine(txtKullaniciAdi.Text + ";" + txtSifre.Text);
                    }
                }

                MessageBox.Show("Veriler başarıyla kaydedildi.");

                GirisEkrani girisEkrani = new GirisEkrani();
                this.Hide();
                girisEkrani.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
        }
    }
}
