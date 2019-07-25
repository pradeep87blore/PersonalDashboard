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
using UserManager;

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
            EnableMFA(false);
        }

        private void Button_signup_Click(object sender, RoutedEventArgs e)
        {
            if(bUsedForSignIn == false) // Sign up process
            {
                if(UserSignupHelper.SignupUser(textBox_userName.Text, textBox_password.Text))
                {
                    MessageBox.Show(Strings.USER_SIGN_UP_SUCCESS);

                    EnableMFA(true);
                }
                else
                {
                    MessageBox.Show(Strings.USER_SIGN_UP_FAILED);
                    EnableMFA(false);
                }
            }
            else
            {

            }
        }

        private async void Button_Verify(object sender, RoutedEventArgs e)
        {
            if(UserSignupHelper.ValidateUser(textBox_userName.Text, textBox_MFA.Text))
            {
                MessageBox.Show(Strings.USER_VALIDATION_SUCCESS);

                //string bucketsforuser = await UserSignupHelper.ListUserBuckets(textBox_userName.Text, textBox_password.Text);
                //MessageBox.Show(bucketsforuser, "Buckets for the users");
            }
            else
            {
                MessageBox.Show(Strings.USER_VALIDATION_FAILED);
            }
        }

        private void EnableMFA(bool bEnable)
        {
            button_verify.IsEnabled = bEnable;
            textBox_MFA.IsEnabled = bEnable;

            textBox_userName.IsReadOnly = bEnable;
            textBox_password.IsReadOnly = bEnable;
        }
    }
}
