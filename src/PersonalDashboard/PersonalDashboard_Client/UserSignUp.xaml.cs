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
using System.Windows.Shapes;

namespace PersonalDashboard_Client
{
    /// <summary>
    /// Interaction logic for UserSignUp.xaml
    /// </summary>
    public partial class UserSignUp : Window
    {
        bool bUsedForSignIn = false; // If true, this window will be used for sign up instead

        public UserSignUp(bool bSignIn = false)
        {
            InitializeComponent();

            bUsedForSignIn = bSignIn;

            if (bSignIn == true)
            {
                ConfigureWindowForSignIn();
            }
            else
            {
                ConfigureWindowForSignUp();
            }
        }

        private void ConfigureWindowForSignUp()
        {
            this.Title = Strings.SIGN_UP;
            button_signup.Content = Strings.SIGN_UP;
        }

        private void ConfigureWindowForSignIn()
        {
            this.Title = Strings.SIGN_IN;
            button_signup.Content = Strings.SIGN_IN;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {

        }

        private void Button_signup_Click(object sender, RoutedEventArgs e)
        {
            if(bUsedForSignIn)
            {

            }
            else
            {

            }
        }
    }
}
