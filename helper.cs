using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace Pochta
{
    class Helper
    {
        ///Генерирует исключение при неправильном вводе почты
        public static void CheckMail(string mail)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(mail) || string.IsNullOrEmpty(mail))
                    throw new Exception("Поле почты не заполнено");

                string mail_pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";                

                if (!Regex.IsMatch(mail, mail_pattern, RegexOptions.Singleline))
                    throw new Exception("Неверный формат почты");
            }
            catch
            {
                throw;
            }
        }
    }
}
