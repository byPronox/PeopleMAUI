using People.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace People.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Person> People { get; set; }
        private string _newPersonName;
        private string _statusMessage;
        private Person _selectedPerson;

        public string NewPersonName
        {
            get => _newPersonName;
            set
            {
                if (_newPersonName != value)
                {
                    _newPersonName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public Person SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                if (_selectedPerson != value)
                {
                    _selectedPerson = value;
                    OnPropertyChanged();
                    if (_selectedPerson != null)
                        DeletePerson();
                }
            }
        }

        public ICommand AddPersonCommand { get; }
        public ICommand GetPeopleCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainPageViewModel()
        {
            People = new ObservableCollection<Person>();
            AddPersonCommand = new Command(AddPerson);
            GetPeopleCommand = new Command(GetPeople);
        }

        private void AddPerson()
        {
            if (!string.IsNullOrWhiteSpace(NewPersonName))
            {
                App.PersonRepo.AddNewPerson(NewPersonName);
                StatusMessage = App.PersonRepo.StatusMessage;
                NewPersonName = string.Empty; 
                GetPeople(); 
            }
            else
            {
                StatusMessage = "Name cannot be empty!";
            }
        }

        private void GetPeople()
        {
            People.Clear();
            foreach (var person in App.PersonRepo.GetAllPeople())
            {
                People.Add(person);
            }
        }

        private async void DeletePerson()
        {
            if (SelectedPerson == null)
                return;

            
            App.PersonRepo.DeletePerson(SelectedPerson.Id);

            
            await App.Current.MainPage.DisplayAlert(
                "Registro Eliminado",
                "Stefan Jativa acaba de eliminar un registro.",
                "OK");

            
            GetPeople();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
