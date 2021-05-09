using System;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows;


namespace Pochta
{
    public partial class MainWindow : Window
    {
        string checkCode;
        public MainWindow() {
            InitializeComponent();
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Helper.CheckMail(mailTB.Text);

                CreatingMessageInterface window = new CreatingMessageInterface(mailTB.Text, passTB.Password);
                Hide();
                window.Show();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void SendCode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                Helper.CheckMail(mailTB.Text);

                CheckCode.IsEnabled = true;
                MailAddress from = new MailAddress("timkin.moxim@mail.ru", "System");
                MailAddress to = new MailAddress(mailTB.Text, "System");
                MailMessage mess = new MailMessage(from, to);

                Random r = new Random(DateTime.Now.Millisecond);
                int code = r.Next(100, 999) * 1000 + r.Next(100, 999);
                checkCode = code.ToString();

                mess.Subject = "Аутентификация входа в приложение почты";
                mess.Body = "Проверочный код для входа в приложение почты: " + checkCode;

                SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
                smtp.Credentials = new NetworkCredential("timkin.moxim@mail.ru", ")TrLePuo2Ry7");
                smtp.EnableSsl = true;
                smtp.Send(mess);
            }

            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void CheckCode_Click(object sender, RoutedEventArgs e)
        {
            if (testCodeTB.Text != checkCode)
                MessageBox.Show("Неверный проверочный код");
            else LogIn.IsEnabled = true;
        }
    }
}
