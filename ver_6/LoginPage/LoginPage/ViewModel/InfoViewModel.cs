using CrudModels;
using LoginPage.View;
using Newtonsoft.Json;
using Petnet.ActivityTracker;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace LoginPage.ViewModel
{
    public class InfoViewModel : ViewModelBase
    {
        public InfoViewModel()
        {
            //  var idleTime = IdleTimeDetector.GetIdleTimeInfo();
            Console.WriteLine(DateTime.Now);
            Console.WriteLine("Info view Started");
            _activityTracker = new ActivityTracker();
            TrackActivity();
            /*
            if (idleTime.IdleTime.TotalSeconds >= 20)
            {
                // They are idle!
                MessageBox.Show("Updated successfully", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                //  Console.WriteLine("The user is idle form 20 seconds");
            }
            */
            CustomerEditDetails();
            EditCommand = new ViewModelCommand(ExecuteEditCommand, CanExecuteEditCommand);
            SaveCommand = new ViewModelCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
            //   ExecuteEditCommand()

        }







        private DispatcherTimer _dispatcherTimer;
        private int _username;
        private string _firstname;
        private string _address;
        private bool _isReadOnly = true;
        private int _addressId;
        private string _errorMessage;
        public IActivityTracker _activityTracker { get; set; }
        private DateTime timerStart;


        // check the activity
        private void TrackActivity()
        {
            if (_dispatcherTimer != null)
            {
                StopDispatcherTimer();
            }
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += CheckActivity;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            _dispatcherTimer.Start();
            timerStart = DateTime.Now;
        }



        private void CheckActivity(object sender, EventArgs e)
        {
            var lastActivity = _activityTracker.LastActivity;
            var logoutTime = lastActivity.AddSeconds(10);
            var maxlogoutTime = lastActivity.AddSeconds(15);

            //  Console.WriteLine(lastActivity + " " + logoutTime + " " + DateTime.Now + " Conter " + InfoView.m_counter);
            //  Console.WriteLine(DateTime.Now > logoutTime && (InfoView.m_counter > 8 && InfoView.m_counter < 10));

            if (DateTime.Now > maxlogoutTime)
            {
                Console.WriteLine("Token has Expired");
                Console.WriteLine(DateTime.Now);
            }
            else if (DateTime.Now < logoutTime)
            {
                Console.WriteLine("Token is valid");
                Console.WriteLine(DateTime.Now);
            }

            else if (DateTime.Now > logoutTime && DateTime.Now < maxlogoutTime && (DateTime.Now - timerStart).TotalSeconds > 10)
            {
                //  RestartApp();
                InfoView.m_counter = 0;
                Console.WriteLine(DateTime.Now);
                SendToken();
                Console.WriteLine("Token is regenetated");
                _dispatcherTimer.Stop();
                _dispatcherTimer.Start();
                timerStart = DateTime.Now;


                //MessageBox.Show("Activity Tracker is working", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);

                //   _activityTracker.Reset();
            }



        }



        private void StopDispatcherTimer()
        {
            _dispatcherTimer.Tick -= CheckActivity;
            _dispatcherTimer.Stop();
        }




        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public int Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                OnPropertyChanged("Username");
            }
        }
        public string Firstname
        {
            get
            {
                return _firstname;
            }
            set
            {
                _firstname = value;
                OnPropertyChanged("Firstname");
            }

        }
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                OnPropertyChanged("Address");
            }
        }




        /*
        public bool IsReadOnly
        {
            get
            {
                return true;
            }
            set
            {
                _isReadOnly = value;
                OnPropertyChanged("IsReadOnly");

            }

        }
        */


        public bool IsReadOnly
        {
            get
            {
                return _isReadOnly;
            }
            set
            {
                _isReadOnly = value;
                OnPropertyChanged(nameof(IsReadOnly));
            }
        }

        public int AddressId
        {
            get
            {
                return _addressId;
            }
            set
            {
                _addressId = value;

            }
        }







        /*------------------------------------------------------------------------------
       Code for the timer
        -------------------------------------------------------------------------------*/

        /*
        public static class IdleTimeDetector
        {
            [DllImport("user32.dll")]
            static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

            public static IdleTimeInfo GetIdleTimeInfo()
            {
                int systemUptime = Environment.TickCount,
                    lastInputTicks = 0,
                    idleTicks = 0;

                LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
                lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
                lastInputInfo.dwTime = 0;

                if (GetLastInputInfo(ref lastInputInfo))
                {
                    lastInputTicks = (int)lastInputInfo.dwTime;

                    idleTicks = systemUptime - lastInputTicks;
                }

                return new IdleTimeInfo
                {
                    LastInputTime = DateTime.Now.AddMilliseconds(-1 * idleTicks),
                    IdleTime = new TimeSpan(0, 0, 0, 0, idleTicks),
                    SystemUptimeMilliseconds = systemUptime,
                };
            }
        }

        public class IdleTimeInfo
        {
            public DateTime LastInputTime { get; internal set; }

            public TimeSpan IdleTime { get; internal set; }

            public int SystemUptimeMilliseconds { get; internal set; }
        }

        internal struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }

        */















        // Commands
        public ICommand SaveCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand RecoverPasswordCommand { get; }

        public ICommand ShowPasswordCommand { get; }

        public ICommand RememberPasswordCommand { get; }


        /*
        public void LoginViewModel()
        {
            LoginCommand = new ViewModelCommand(ExecuteEditCommand, CanExecuteEditCommand);
            /// RecoverPasswordCommand = new ViewModelCommand(p => ExecuteRecoverPassword("", ""));

        }

        */











        public void ExecuteEditCommand(object obj)
        {
            IsReadOnly = false;
            /*
            if (InfoView.m_counter < 10)
            {
                InfoView.m_counter = 0;
                // http get method to get the new token 
                SendToken();
                //   RegenerateAccessToken();
            }

            */

            /*
            if (!IsReadOnly)
            {
                InfoView.m_counter = 0;

            }

            if (InfoView.m_counter == 10)
            {
                Console.WriteLine("Token Expired can't be updated");
            }
            */

        }








        public async void UpdateCustomer(int Username)
        {
            //    var url = $"https://localhost:7172/customer/EditCustomerById/{Username}";
            var custModel = new CustomerEditDetails
            {
                Id = Username,
                FirstName = Firstname,
                AddressId = AddressId,
                Address = new AddressDetails
                {
                    AddressId = AddressId,
                    Address = Address
                }

            };


            bool checkExpiredToken(string ReceivedAccessToken, string ReceivedRefreshToken)
            {
                if (ReceivedRefreshToken == MainWindow.refreshToken && ReceivedAccessToken == MainWindow.token)
                {
                    return true;
                }
                return false;
            }




            async Task<bool> IsValidToken()
            {

                var url = "https://localhost:7172/customer/SendAccessAndRefreshToken";

                using (var client1 = new HttpClient())
                {

                    var msg = new HttpRequestMessage(HttpMethod.Get, url);
                    msg.Headers.Add("User-Agent", "C# Program");
                    var res1 = client1.SendAsync(msg).Result;

                    var content1 = await res1.Content.ReadAsStringAsync();
                    var stringContent1 = Convert.ToString(content1);
                    var contentResponse = JsonConvert.DeserializeObject<List<string>>(stringContent1);
                    var responseOne = contentResponse[0];
                    var responseTwo = contentResponse[1];
                    var receivedAccessToken = Convert.ToString(responseOne);
                    var receivedRefreshToken = Convert.ToString(responseTwo);
                    if (checkExpiredToken(receivedAccessToken, receivedRefreshToken))
                    {

                        var newUrl = "https://localhost:7172/customer/generate-Tokens";
                        using (var client2 = new HttpClient())
                        {
                            var msg1 = new HttpRequestMessage(HttpMethod.Post, newUrl);
                            msg1.Headers.Add("User-Agent", "C# Program");
                            var res2 = client2.SendAsync(msg1).Result;
                            var content2 = await res2.Content.ReadAsStringAsync();
                            var stringContent2 = Convert.ToString(content2);
                            MainWindow.token = stringContent2;
                            UpdateCustomer(Username);
                        }
                    }

                }
                return false;

            }






            using (HttpClient client = new HttpClient())
            {

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + MainWindow.token);


                // string text = File.ReadAllText("payload.json");
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(custModel);
                //    var stringContent = new StringContent(jsonString);  


                var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");

                var response = await client.PutAsync($"https://localhost:7172/customer/EditCustomerById/{Username}", stringContent);

                var result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {

                    MessageBox.Show("Updated successfully", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                    //  MessageBoxResult result1 = MessageBox.Show("Customer Details Updated Successfully !!");

                }
                else
                {
                    // Console.WriteLine("Error " + response.StatusCode);




                    /*
                    var url = "https://localhost:7172/customer/SendAccessAndRefreshToken";

                    using (var client1 = new HttpClient())
                    {

                        var msg = new HttpRequestMessage(HttpMethod.Get, url);
                        msg.Headers.Add("User-Agent", "C# Program");
                        var res1 = client1.SendAsync(msg).Result;

                        var content1 = await res1.Content.ReadAsStringAsync();
                        var stringContent1 = Convert.ToString(content1);
                        var contentResponse = JsonConvert.DeserializeObject<List<string>>(stringContent1);
                        var responseOne = contentResponse[0];
                        var responseTwo = contentResponse[1];
                        var receivedAccessToken = Convert.ToString(responseOne);
                        var receivedRefreshToken = Convert.ToString(responseTwo);
                        if (checkExpiredToken(receivedAccessToken, receivedRefreshToken))
                        {
                            var newUrl = "https://localhost:7172/customer/generate-Tokens";
                            using (var client2 = new HttpClient())
                            {
                                var msg1 = new HttpRequestMessage(HttpMethod.Get, newUrl);
                                msg1.Headers.Add("User-Agent", "C# Program");
                                var res2 = client2.SendAsync(msg1).Result;
                                var content2 = await res2.Content.ReadAsStringAsync();
                                var stringContent2 = Convert.ToString(content2);
                                MainWindow.token = stringContent2;
                                UpdateCustomer(Username);
                            }
                        }

                    }
                    */
                    //return false;


                    //   ErrorMessage = "Token Expired. Please login again.";
                    MessageBox.Show("Token expired please press ok to login again", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);

                    MainWindow.ViewIndex = 3;
                }

                //if (result != null)
                //{
                //    Console.WriteLine("Updated Successfully");
                //}
                //else
                //{
                //    Console.WriteLine("Error" + response.StatusCode);
                //}



            }



            //  var client = new HttpClient();
            //   client.BaseAddress = new Uri("https://localhost:7172/customer/}");
            //  var jsonString = JsonSerializer.Serialize(custModel);
            //var content = new StringContent(custModel, Encoding.UTF8,"application/json");

            //var jsonString = /*Encoding.UTF8.GetBytes(*/Newtonsoft.Json.JsonConvert.SerializeObject(custModel);
            /*
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(custModel);
            var stringContent = new StringContent(jsonString);
            var response =  client.PutAsync($"https://localhost:7172/customer/EditCustomerById/{Username}", stringContent).Result;
        //    var response = client.PutAsync(new Uri(https://localhost:7172/customer/EditCustomerById/{Username}), stringContent).Result;


            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Updated Successfully");
            }
            else
            {
                Console.WriteLine("Error " + response.StatusCode);
            }
            */




        }


        public async void RegenerateAccessToken()
        {
            var newUrl = "https://localhost:7172/customer/generate-Tokens";
            using (var client2 = new HttpClient())
            {
                var msg1 = new HttpRequestMessage(HttpMethod.Get, newUrl);
                msg1.Headers.Add("User-Agent", "C# Program");
                var res2 = client2.SendAsync(msg1).Result;
                var content2 = await res2.Content.ReadAsStringAsync();
                var stringContent2 = Convert.ToString(content2);
                MainWindow.token = stringContent2;

            }
        }


        // post method to send the previous refresh token to the server side to check the validity


        public async void SendToken()
        {

            var newUrl = "https://localhost:7172/customer/ReceivedOldToken";
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Add("Custom", MainWindow.refreshToken);

                var msg = new HttpRequestMessage(HttpMethod.Get, newUrl);
                msg.Headers.Add("User-Agent", "C# Program");
                var res = client.SendAsync(msg).Result;
                var content = await res.Content.ReadAsStringAsync();
                var stringContent = Convert.ToString(content);

                var contentResponse = JsonConvert.DeserializeObject<List<string>>(stringContent);
                var firstResponse = contentResponse[0];
                var secondResponse = contentResponse[1];
                var AccessToken = Convert.ToString(firstResponse);
                var RefreshToken = Convert.ToString(secondResponse);
                MainWindow.token = AccessToken;
                MainWindow.refreshToken = RefreshToken;

            }
        }




        public void ExecuteSaveCommand(object obj)
        {

            /*
            if (InfoView.m_counter < 10)
            {
                InfoView.m_counter = 0;
                RegenerateAccessToken();
                UpdateCustomer(Username);
            }
            else
            {
                MessageBox.Show("Token expired please press ok to login again", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                InfoView.m_counter = 0;
                MainWindow.ViewIndex = 3;

                // Console.WriteLine("Token Expired please login Again");
            }
            */
            UpdateCustomer(Username);

        }



        private void GetDetails(CustomerEditDetails customer)
        {
            Username = customer.Id;
            Firstname = customer.FirstName;
            Address = customer.Address?.Address;
            AddressId = customer.AddressId;
        }


        public async void CustomerEditDetails()
        {
            var url = $"https://localhost:7172/customer/GetFullCustomerDetailById/{MainWindow.UserId}";

            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + MainWindow.token);
                var msg = new HttpRequestMessage(HttpMethod.Get, url);

                msg.Headers.Add("User-Agent", "C# Program");

                var res = client.SendAsync(msg).Result;

                var content = await res.Content.ReadAsStringAsync();

                /*

                using (var newclient = new HttpClient())
                {
                    var msg1 = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7172/customer/generate-Tokens");
                    msg1.Headers.Add("User-Agent", "C# Program");
                    var res1 = newclient.SendAsync(msg1).Result;
                    var content1 = await res1.Content.ReadAsStringAsync();
                    _token = content1;

                }
                */
                //  var stringContent = Convert.ToString(content);
                var contentResponse = JsonConvert.DeserializeObject<CustomerEditDetails>(content);
                //Username = contentResponse.id;
                //Firstname = contentResponse.firstName;
                GetDetails(contentResponse);

                // Console.WriteLine(content);
                // return contentResponse;
            }
            // return 

        }




        private bool CanExecuteEditCommand(object obj)
        {
            bool canExecute;
            if ((Username.GetType() != typeof(int)) || (Firstname == "" || Address == ""))
            {
                canExecute = false;
            }
            else
            {
                canExecute = true;
            }
            return canExecute;
        }



        private bool CanExecuteSaveCommand(object obj)
        {
            bool canExecute;
            if (Firstname == "" || Address == "")
            {
                canExecute = false;
            }
            else
            {
                canExecute = true;
            }
            return canExecute;
        }


        /*
        private bool CanExecuteEditCommand(object obj)
        {
            bool canExecute;
            if (Username != 0 && (Firstname != null && Address != null))
            {
                canExecute = true;
            }
            else
            {
                canExecute = false;
            }
            return canExecute;
        }
        */

    }
}
