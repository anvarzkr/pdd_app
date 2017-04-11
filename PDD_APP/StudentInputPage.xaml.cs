using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PDD_APP
{
    /// <summary>
    /// Логика взаимодействия для StudentInputPage.xaml
    /// </summary>
    public partial class StudentInputPage : Page
    {
        public StudentInputPage()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == (String)textBox.Tag)
                textBox.Text = "";
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "")
                textBox.Text = (String)textBox.Tag;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int id = 0;
            try
            {
                id = Int32.Parse((String)idTextBox.Text);
            }
            catch (FormatException FE) { MessageBox.Show("Номер участника может быть только числом!"); return; }

            Models.Student student = Models.Student.get(id);
            if (student == null 
                
                || !student.fname.ToLower().Equals(fnameTextBox.Text.ToLower())
                || !student.lname.ToLower().Equals(lnameTextBox.Text.ToLower()))
            {
                MessageBox.Show("Вы ввели некорректные данные.");
                return;
            }

            MainWindow.testingStudent = student;
            Utils.goToPage("StartPage");

        }
    }
}
