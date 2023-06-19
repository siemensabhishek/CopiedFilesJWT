using System.Timers;
using System.Windows.Controls;


namespace LoginPage.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        //static int m_counter = 0;
        //private Timer timer;
        public LoginView()
        {
            InitializeComponent();
            /*

            timer = new System.Timers.Timer();

            timer.Interval = 100;
            timer.Elapsed += OnTimerElapsed;
            timer.Start();
            timer.Elapsed += OnTimerElapsed;


            */

        }



        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            /*
            m_counter++;
            if (m_counter == 20)
            {
                Console.WriteLine("Time stopped");
                timer.Stop();
            }
            Console.WriteLine("Counter" + m_counter);
            */
            /*
            text1.Dispatcher.Invoke(() =>
            {
                text1.Text = "counter: " + m_counter;

            });
            */

        }






    }
}
