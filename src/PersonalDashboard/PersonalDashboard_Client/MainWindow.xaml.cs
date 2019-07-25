using System.Windows;
using UserManager;
using Utils;

namespace PersonalDashboard_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Logger.Log("PersonalDashboard_Client initialized");
        }

        private void Button_signup_Click(object sender, RoutedEventArgs e)
        {
            UserSignUp signUpWindow = new UserSignUp();
            var bsignUpComplete = signUpWindow.ShowDialog();
        }

        private void Button_signin_Click(object sender, RoutedEventArgs e)
        {
            UserSignUp signUpWindow = new UserSignUp(true);
            var bsignInComplete = signUpWindow.ShowDialog();
        }

        private void Window_Initialized(object sender, System.EventArgs e)
        {
            SetStrings();

            var retVal = Initializations.InitializeUserManager();
            if (retVal == string.Empty)
            {
                // Nothing more to be done than start reading the values
            }
            else
            {
                MessageBox.Show(retVal);
            }
        }

        private void SetStrings()
        {
            this.Title = Strings.WELCOME_PAGE_TITLE;
            button_signin.Content = Strings.SIGN_IN;
            button_signup.Content = Strings.SIGN_UP;
        }
    }
}
