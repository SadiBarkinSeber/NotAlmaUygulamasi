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

namespace NotAlmaUygulamasi
{
    public partial class Notlar : Form
    {
        private bool changesMade = false;
        private string kullaniciAdi; // kullaniciAdi değişkenini sınıf seviyesinde tanımla

        public Notlar(string kullaniciAdi) // Constructor ile kullaniciAdi'ni al
        {
            InitializeComponent();
            this.kullaniciAdi = kullaniciAdi;
            

            // Kullanıcının klasörünü oluştur
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string klasorPath = Path.Combine(desktopPath, "NotUygulamasi", kullaniciAdi);

            if (!Directory.Exists(klasorPath))
            {
                Directory.CreateDirectory(klasorPath);
            }

            // Kullanıcının klasöründeki notları yükle
            string[] notlar = Directory.GetFiles(klasorPath, "*.txt").Select(Path.GetFileNameWithoutExtension).ToArray();
            lbNotlar.Items.AddRange(notlar);
        }

        private void btnNotEkle_Click(object sender, EventArgs e)
        {
            changesMade = true;

            string kullaniciAdi = (Application.OpenForms["GirisEkrani"] as GirisEkrani)?.KullaniciAdi;


            if (!string.IsNullOrEmpty(kullaniciAdi))
            {
                string notAdi = tbNotBaslik.Text;
                string notIcerik = rtbNot.Text;

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string klasorPath = Path.Combine(desktopPath, "NotUygulamasi", kullaniciAdi);
                string filePath = Path.Combine(klasorPath, notAdi + ".txt");

                try
                {
                    // Kullanıcının klasörünü oluştur
                    if (!Directory.Exists(klasorPath))
                    {
                        Directory.CreateDirectory(klasorPath);
                    }

                    // Dosya yoksa yeni notu oluştur
                    File.WriteAllText(filePath, notIcerik);
                    MessageBox.Show("Not başarılı bir şekilde eklendi.");

                    // ListBox'a not adını ekle (eğer daha önce eklenmemişse)
                    if (!lbNotlar.Items.Contains(notAdi))
                    {
                        lbNotlar.Items.Add(notAdi);
                    }

                    // Başarılı ekleme sonrası temizleme
                    tbNotBaslik.Clear();
                    rtbNot.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluştu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Not başlığı boş olamaz.");
            }
        }

        private void lbNotlar_SelectedIndexChanged(object sender, EventArgs e)
        {
            changesMade = true;
            if (lbNotlar.SelectedIndex != -1)
            {
                string secilenNotAdi = lbNotlar.SelectedItem.ToString();
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string klasorPath = Path.Combine(desktopPath, "NotUygulamasi", kullaniciAdi);
                string filePath = Path.Combine(klasorPath, secilenNotAdi + ".txt");

                if (File.Exists(filePath))
                {
                    string notIcerik = File.ReadAllText(filePath);
                    tbNotBaslik.Text = secilenNotAdi;
                    rtbNot.Text = notIcerik;
                }
                else
                {
                    MessageBox.Show("Seçilen notun dosyası bulunamadı.");
                }
            }
            else
            {
                // ListBox'ta herhangi bir öğe seçili değilse, tbNotBaslik ve rtbNot kontrollerini temizle
                tbNotBaslik.Clear();
                rtbNot.Clear();
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            changesMade = true;
            string notAdi = tbNotBaslik.Text;
            string notIcerik = rtbNot.Text;

            if (!string.IsNullOrEmpty(notAdi))
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string klasorPath = Path.Combine(desktopPath, "NotUygulamasi", kullaniciAdi);

                string filePath = Path.Combine(klasorPath, notAdi + ".txt");

                try
                {
                    // Kullanıcının klasörünü oluştur
                    if (!Directory.Exists(klasorPath))
                    {
                        Directory.CreateDirectory(klasorPath);
                    }

                    // Dosya varsa içeriği güncelle, yoksa yeni notu oluştur
                    if (File.Exists(filePath))
                    {
                        File.WriteAllText(filePath, notIcerik);
                        MessageBox.Show("Not güncellendi.");
                    }
                    else
                    {
                        File.WriteAllText(filePath, notIcerik);
                        MessageBox.Show("Not başarılı bir şekilde kaydedildi.");
                        lbNotlar.Items.Add(notAdi); // ListBox'a not adını ekle
                    }

                    // Başarılı kaydetme veya güncelleme sonrası temizleme
                    tbNotBaslik.Clear();
                    rtbNot.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluştu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Not başlığı boş olamaz.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            changesMade = true;

            if (lbNotlar.SelectedIndex != -1)
            {
                string secilenNotAdi = lbNotlar.SelectedItem.ToString();

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string klasorPath = Path.Combine(desktopPath, "NotUygulamasi", kullaniciAdi); // kullaniciAdi değişkenini kullan

                string filePath = Path.Combine(klasorPath, secilenNotAdi + ".txt");

                try
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        lbNotlar.Items.RemoveAt(lbNotlar.SelectedIndex);
                    }
                    else
                    {
                        MessageBox.Show("Seçilen notun dosyası bulunamadı.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluştu: " + ex.Message);
                }
            }
        }

        private void Notlar_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (changesMade)
            {
                // Eğer değişiklik yapıldıysa kullanıcıya kaydetmeyi unutma uyarısı göster
                DialogResult result = MessageBox.Show("Çıkış Yapmak İstiyor musunuz ?", "Uyarı", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    Environment.Exit(0);
                }
                else if (result == DialogResult.No)
                {
                    // Eğer "Hayır" seçilirse uygulamadan çık
                    e.Cancel = true; // Bu satırı ekleyerek çıkışı onayla
                    
                }
            }

        }
    }
}