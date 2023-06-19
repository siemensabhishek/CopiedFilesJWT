using CrudModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LoginPage.ViewModel
{
    public class InfoViewModel : ViewModelBase
    {
        public InfoViewModel()
        {
            CustomerEditDetails();
            EditCommand = new ViewModelCommand(ExecuteEditCommand, CanExecuteEditCommand);
            SaveCommand = new ViewModelCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
            //   ExecuteEditCommand()

        }
        private int _username;
        private string _firstname;
        private string _address;
        private bool _isReadOnly = true;
        private int _addressId;
        private string _errorMessage;
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
            /*
            using (HttpClient client = new HttpClient())
            {

               // var jsonString = JsonSerializer.Serialize(custModel);
                //var content = new FormUrlEncodedContent((System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>>)custModel);
                HttpResponseMessage response = await client.PostAsync(url, custModel);
                // code to do something with response
            }
            */

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



                    //await IsValidToken();

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
                    //return false;


                    //  ErrorMessage = "Token Expired. Please login again.";
                    // MessageBox.Show("Token expired please press ok to login again", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);

                    // MainWindow.ViewIndex = 3;
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

        public void ExecuteSaveCommand(object obj)
        {

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
