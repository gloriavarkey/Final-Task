using System;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private byte[] _key;
        private byte[] _iv;

        public MainWindow()
        {
            InitializeComponent();
            _key = PasswordManager.GenerateKey();
            _iv = PasswordManager.GenerateIV();
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            string password = PasswordTextBox.Text;
            string encryptedPassword = PasswordManager.EncryptPassword(password, _key, _iv);
            EncryptedPasswordTextBox.Text = encryptedPassword;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string encryptedPassword = EncryptedPasswordTextBox.Text;
            FileHandler.SaveToFile("password.txt", encryptedPassword);
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            string encryptedPassword = FileHandler.ReadFromFile("password.txt");
            EncryptedPasswordTextBox.Text = encryptedPassword;
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            string encryptedPassword = EncryptedPasswordTextBox.Text;
            TimeSpan elapsedTime;
            string decryptedPassword = BruteForceDecryptor.BruteForceDecrypt(encryptedPassword, _key, _iv, out elapsedTime);
            DecryptedPasswordTextBox.Text = decryptedPassword;
            ElapsedTimeTextBox.Text = elapsedTime.TotalSeconds.ToString("F2") + " seconds";
        }
    }
}
