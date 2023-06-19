using System;
using System.Timers;
using System.Windows.Controls;

namespace LoginPage.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        static int m_counter = 0;
        public LoginView()
        {
            InitializeComponent();

            var timer = new System.Timers.Timer();
            timer.Interval = 100;
            //   timer.Elapsed += OnTimerElapsed;
            timer.Start();
            //      timer.Elapsed += OnTimerElapsed;
            if (m_counter == 20)
            {
                timer.Stop();
            }
            timer.Elapsed += OnTimerElapsed;

        }



        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            m_counter++;
            Console.WriteLine("Counter" + m_counter);
            text1.Dispatcher.Invoke(() =>
            {
                text1.Text = "counter: " + m_counter;

            });
        }






    }
}
