using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.IO;
using System.Windows.Controls;

namespace Pochta
{
    public partial class CreatingMessageInterface : Window
    {
        string path = @"C:\Users\ender\source\repos\Bases of Algorythm and Coding\Pochta\bin\Debug\chernoviki\";

        string mail;
        string pass;
        MailMessage message = new MailMessage();
        SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

        public CreatingMessageInterface(string mail, string pass)
        {
            InitializeComponent();

            this.mail = mail;
            this.pass = pass;
            otkovoTB.Text = mail;

            string[] files = Directory.GetFiles(path);
            for (int i = 0; i < files.Length; i++)
            chernovikiCB.Items.Add(files[i].Substring(path.Length));


            smtp.Credentials = new NetworkCredential(mail, pass);
            smtp.EnableSsl = true;
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //проверяем формат почты
                Helper.CheckMail(komuTB.Text);

                //адрес отправителя
                message.From = new MailAddress(mail);

                //адреса получателей из листбокса адресов (или адрес получателя из текстбокса, если листбокс пустой)
                if (rassylkaLB.Items.Count == 0)
                    message.To.Add(new MailAddress(komuTB.Text));
                else
                    foreach (MailAddress mailAddress in rassylkaLB.Items)
                        message.To.Add(mailAddress);

                //если текстбокс темы сообщения не пуст
                if (!string.IsNullOrEmpty(temaTB.Text) && !string.IsNullOrWhiteSpace(temaTB.Text))
                    message.Subject = temaTB.Text;

                //содержимое сообщения
                message.Body = textTB.Text;

                //если листбокс вложений не пуст
                if (attachmentsLB.Items.Count != 0)
                    for (int i = 0; i < attachmentsLB.Items.Count; i++)
                        message.Attachments.Add(new Attachment(attachmentsLB.Items[i].ToString()));



                smtp.Send(message);

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

        }

        private void AddAddress_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Helper.CheckMail(komuTB.Text);

                if (rassylkaLB.Items.Count == 0)
                {
                    rassylka.Visibility = Visibility.Visible;
                    rassylkaLB.Visibility = Visibility.Visible;
                    DeleteAddress.Visibility = Visibility.Visible;
                }

                if (!rassylkaLB.Items.Contains(komuTB.Text))
                    rassylkaLB.Items.Add(new MailAddress(komuTB.Text));
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void DeleteAddress_Click(object sender, RoutedEventArgs e)
        {
            if (rassylkaLB.SelectedItem != null)
                rassylkaLB.Items.Remove(rassylkaLB.SelectedItem);

            if (rassylkaLB.Items.Count == 0)
            {
                rassylka.Visibility = Visibility.Hidden;
                rassylkaLB.Visibility = Visibility.Hidden;
                DeleteAddress.Visibility = Visibility.Hidden;
            }
        }

        private void attachmentsLB_Drop(object sender, DragEventArgs e)
        {
            //список файлов
            List<string> FileList = new List<string>();
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                FileList.AddRange((string[])e.Data.GetData(DataFormats.FileDrop));

            for (int i = 0; i < FileList.Count; i++)
            {
                //проверка файла на доступное расширение
                if (Path.GetExtension(FileList[i]) == ".jpg"
                || Path.GetExtension(FileList[i]) == ".png"
                || Path.GetExtension(FileList[i]) == ".xlsx"
                || Path.GetExtension(FileList[i]) == ".docx"
                || Path.GetExtension(FileList[i]) == ".zip"
                || Path.GetExtension(FileList[i]) == ".rar"
                || Path.GetExtension(FileList[i]) == ".txt")
                    attachmentsLB.Items.Add(FileList[i]);
                else if (Path.GetExtension(FileList[i]) == "")
                    MessageBox.Show("Невозможно добавить папку, как вложение");
                else MessageBox.Show($@"Невозможно добавить файл по адресу {FileList[i]}, так как он имеет неверное расширение");
            }
        }

        private void DeleteAttachment_Click(object sender, RoutedEventArgs e)
        {
            if (attachmentsLB.SelectedItem != null)
                attachmentsLB.Items.Remove(attachmentsLB.SelectedItem);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var mainWindow = (Application.Current.MainWindow as MainWindow);
            if (mainWindow != null)
            {
                mainWindow.Close();
            }
        }

        private void AddChernovik_Click(object sender, RoutedEventArgs e)
        {
            Directory.CreateDirectory(path);
            int filesCount = Directory.GetFiles(path).Length + 1;

            string fileName;
            if (!string.IsNullOrEmpty(temaTB.Text) && !string.IsNullOrWhiteSpace(temaTB.Text))
                fileName = temaTB.Text + ".chern";
            else fileName = "Новый черновик" + filesCount.ToString() + ".chern";

            string file = path + fileName;
            using (BinaryWriter writer = new BinaryWriter(File.Open(file, FileMode.OpenOrCreate)))
            {
                writer.Write(rassylkaLB.Items.Count);

                if (rassylkaLB.Items.Count == 0)
                    writer.Write(komuTB.Text);
                else
                    for (int i = 0; i < rassylkaLB.Items.Count; i++)
                        writer.Write(rassylkaLB.Items[i].ToString());

                ///здесь ошибка, криво записывается вложение
                writer.Write(attachmentsLB.Items.Count);
                for (int i = 0; i < attachmentsLB.Items.Count; i++)
                    writer.Write(attachmentsLB.Items[i].ToString());

                writer.Write(textTB.Text);

            }

            chernovikiCB.Items.Add(fileName);

            MessageBox.Show("Черновик добавлен.");
        }

        private void OpenChernovik_Click(object sender, RoutedEventArgs e)
        {
            string file;
            if (chernovikiCB.SelectedItem != null)
            {
                file = chernovikiCB.SelectedItem.ToString();
                using (BinaryReader reader = new BinaryReader(File.OpenRead(path + file)))
                {
                    int addressCount = reader.ReadInt32();
                    if (addressCount == 0)
                        komuTB.Text = reader.ReadString();
                    else
                    {
                        rassylka.Visibility = Visibility.Visible;
                        rassylkaLB.Visibility = Visibility.Visible;
                        DeleteAddress.Visibility = Visibility.Visible;
                        rassylkaLB.ItemsSource = null;
                        for (int i = 0; i < addressCount; i++)
                            rassylkaLB.Items.Add(reader.ReadString());
                    }

                    ///здесь ошибка, криво считывается вложение
                    int attachmentCount = reader.ReadInt32();
                    if (attachmentCount != 0)
                    {
                        for (int i = 0; i < attachmentCount; i++)
                            attachmentsLB.Items.Add(reader.ReadInt32());
                    }
                }
            }
            else MessageBox.Show("Не выбран черновик");
        }

        private void DeleteChernovik_Click(object sender, RoutedEventArgs e)
        {
            if (chernovikiCB.SelectedItem != null)
            {
                string file = path + chernovikiCB.Text;
                File.Delete(file);
                chernovikiCB.Items.Remove(chernovikiCB.SelectedItem);
                chernovikiCB.SelectedIndex = -1;
            }
            else MessageBox.Show("Не выбран черновик");
        }
    }
}