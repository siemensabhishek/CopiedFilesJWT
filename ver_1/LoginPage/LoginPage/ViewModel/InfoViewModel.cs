using CustomerEntities.Models;
using LoginPage.Model;
using Newtonsoft.Json;
using System.Net.Http;
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


        /*

        void UpdateCustomer(int Username)
        {
            var url = $"https://localhost:7172/customer/UpdateFullCustomerDetailsById/{Username}";
            var custModel = new CustomerUpdateDetails
            {
                Id = Username,
                FirstName = Firstname,
                Address = Address
            };
            using (var client = new HttpClient())
            {
                var msg = new HttpRequestMessage(HttpMethod.Post, url);
                msg.Headers.Add("User-Agent", "C# Program");
                var res = client.SendAsync(msg).Result;
            }
        }

        */

        public void ExecuteSaveCommand(object obj)
        {
            // UpdateCustomer(Username);
            var url = $"https://localhost:7172/customer/UpdateFullCustomerDetailsById/{Username}";
            var custModel = new CustomerUpdateDetails
            {
                Id = Username,
                FirstName = Firstname,
                Address = Address
            };
            using (var client = new HttpClient())
            {
                var msg = new HttpRequestMessage(HttpMethod.Post, url);
                msg.Headers.Add("User-Agent", "C# Program");
                var res = client.SendAsync(msg).Result;
            }
        }



        private void GetDetails(CustomerLogin customer)
        {
            Username = customer.id;
            Firstname = customer.firstName;
            Address = customer.address;
        }


        public async void CustomerEditDetails()
        {

            var url = $"https://localhost:7172/customer/GetFullCustomerDetailById/{MainWindow.UserId}";

            using (var client = new HttpClient())
            {
                var msg = new HttpRequestMessage(HttpMethod.Get, url);
                msg.Headers.Add("User-Agent", "C# Program");
                var res = client.SendAsync(msg).Result;

                var content = await res.Content.ReadAsStringAsync();
                //  var stringContent = Convert.ToString(content);
                var contentResponse = JsonConvert.DeserializeObject<CustomerLogin>(content);
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
