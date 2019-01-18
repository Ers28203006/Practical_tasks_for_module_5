using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace PracticalTask5
{
    class Program
    {

        static void Main(string[] args)
        {
            User user = new User();

            user.Login = EnterLogin();

            string password;
            password=EnterPassword();
            user.Password = password;

            user.RepeatePassword = EnterRepeatPassword(password);
             

            user.Email=EnterEmail();

            user.PhoneNumber = EnterPhone();

            string adoptedCode = VerificationAccount();
            bool enterCode = false;
            while (enterCode == false) {
                Console.WriteLine("Введите полученый код");
                string verificationCode = Console.ReadLine();

                if (adoptedCode == verificationCode)
                {
                    Console.WriteLine("Поздравляю Вы прошли верификацию!!!");
                    enterCode = true;
                }

                else
                {
                    Console.WriteLine("Попробуйте снова");

                }
            }
        }
        
        
        #region Ввод логина

        static string EnterLogin()
        {
            string login="";
            bool corectEnterLogin = false;
            while (corectEnterLogin == false)
            {
                Console.WriteLine("Введите логин (не должен содержать кирилицы и его длина не менее 4 символов): ");
                 login = Console.ReadLine();

                for (int i = 0; i < login.Length; i++)
                {
                    if ((((login[i] >= 'а') || (login[i] >= 'А')) && ((login[i] >= 'я') || (login[i] >= 'Я'))) || (login.Length < 4))
                    {
                        corectEnterLogin = false;
                    }

                    else
                    {
                        corectEnterLogin = true;
                    }
                }

                if (corectEnterLogin == false)
                {
                    Console.WriteLine(" НЕ КОРРЕКТНО ВВЕДЕН ЛОГИН, ПОПРОБУЙТЕ СНОВА!!!");
                }
            }
            return login;
        }

        #endregion

        #region Ввод пароля

        static string  EnterPassword()
        {
         bool corectEnterPassword = false;
         int strength;
            string password = "";
            while (corectEnterPassword == false)
            {
                strength = 0;
                Console.WriteLine("Введите пароль(должен состоять из заглавных и прописных букв, цифр, символов и знаков припенаний, не менее 8 символов): ");
                password =  ReadPassword();

                if (ContainsDigit(password)) strength++;
                if (ContainsLowerLetter(password)) strength++;
                if (ContainsPunctuation(password)) strength++;
                if (ContainsSeparator(password)) strength++;
                if (ContainsUpperLetter(password)) strength++;

                if (strength < 4 || password.Length < 8)
                {
                    Console.WriteLine("Пароль не  достаточно надежный, придумайте и введите заново!");
                    corectEnterPassword = false;
                }
                else if (strength == 4 && password.Length >= 8)
                {
                    Console.WriteLine("Пароль средней сложности... но казахам пойдет!!!!");
                    corectEnterPassword = true;
                }
                else if (strength > 4 && password.Length >= 8)
                {
                    Console.WriteLine("Пароль надежный!!! Можно пользоваться!!!");
                    corectEnterPassword = true;
                }
            }
            return password;
        }


        static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {

                        password = password.Substring(0, password.Length - 1);

                        int pos = Console.CursorLeft;

                        Console.SetCursorPosition(pos - 1, Console.CursorTop);

                        Console.Write(" ");

                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }

            Console.WriteLine();
            return password;
        }


        static bool ContainsLowerLetter(string _password)
        {
            foreach (char c in _password)
            {
                if ((Char.IsLetter(c)) && (Char.IsLower(c)))
                    return true;
            }
            return false;
        }

        static bool ContainsUpperLetter(string _password)
        {
            foreach (char c in _password)
            {
                if ((Char.IsLetter(c)) && (Char.IsUpper(c)))
                    return true;
            }
            return false;
        }

        static bool ContainsDigit(string _password)
        {
            foreach (char c in _password)
            {
                if (Char.IsDigit(c))
                    return true;
            }
            return false;
        }

        static bool ContainsPunctuation(string _password)
        {
            foreach (char c in _password)
            {
                if (Char.IsPunctuation(c))
                    return true;
            }
            return false;
        }

        static bool ContainsSeparator(string _password)
        {
            foreach (char c in _password)
            {
                if (Char.IsSeparator(c))
                    return true;
            }
            return false;
        }

        #endregion

        #region Ввод повторного пароля

        static string EnterRepeatPassword(string password)
        {
            bool corectEnterRepeatPassword = false;
            string repeatPassword = "";

            while (corectEnterRepeatPassword == false)
            {
                Console.WriteLine("Введите пароль повторно: ");
                repeatPassword = ReadPassword();
                if (repeatPassword != password)
                {
                    Console.WriteLine("Пароль повторно введен не верно, попробуйте снова");
                    corectEnterRepeatPassword = false;
                }
                else
                {
                    Console.WriteLine("Пароль подтвержден!!!");
                    corectEnterRepeatPassword = true;
                }
            }
            return repeatPassword;
        }

        #endregion

        #region Ввод почтового адреса

        static string EnterEmail()
        {
            bool corectEnterEmail = false;
            string email="";
            while (corectEnterEmail == false)
            {
                Console.WriteLine("Введите почтовый адрес: ");
                email = Console.ReadLine();
                corectEnterEmail = IsEmail(email);

                if (corectEnterEmail == false)
                {
                    Console.WriteLine("Не верный формат почтового адреса, попробуте снова. ");
                }
                else
                {
                    Console.WriteLine("Все верно осталось зарегистрировать Ваш телефон!");
                }
            }
            return email;
        }
        static bool IsEmail(string strEmail)
        {
            Regex rgxEmail = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                       @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                       @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            return rgxEmail.IsMatch(strEmail);
        }


        #endregion

        #region Ввод телефонного номера

        static string EnterPhone()
        {
            bool corectEnterPhone = false;
            string phoneNumber = "";

            while (corectEnterPhone == false)
            {
                Console.WriteLine("Введите номер Вашего телефона в формате +X XXX XXX XX XX : ");
                phoneNumber = Console.ReadLine();
                corectEnterPhone = IsPhoneNumber(phoneNumber);
                if (corectEnterPhone == false)
                {
                    Console.WriteLine("Не верный формат номера, попробуте снова. ");
                }
                else
                {
                    Console.WriteLine("Все верно! Ждите СМС!!!");
                    corectEnterPhone = true;
                }
            }
            return phoneNumber;
        }

        static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(\+[0-9]{11})$").Success;
        }

        #endregion

        #region СМС
        static string VerificationAccount()
        {
            // Find your Account Sid and Token at twilio.com/console
            Random rnd = new Random();

            string smscode = Convert.ToString(rnd.Next(100, 1000));

            const string accountSid = "ACa0c12e6e09e1653a03d3cc33658a9580";
            const string authToken = "8a1049c615ecfa7b05231e86dab52724";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
            from: new Twilio.Types.PhoneNumber("+12622170671"),
            body: smscode,
            to: new Twilio.Types.PhoneNumber("+77013819614")
            );

            return smscode;
        }
        #endregion

    }
}