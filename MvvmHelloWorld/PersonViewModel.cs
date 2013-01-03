using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelloWorld.Annotations;

namespace MvvmHelloWorld
{
    class PersonViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<Person> _personDataSource;

        public ObservableCollection<Person> PersonDataSource
        {
            get
            {
                if (_personDataSource == null)
                {
                    _personDataSource = new ObservableCollection<Person>();
                }
                return _personDataSource;
            }
        }

        #region SelectedPerson, SelectedName, SelectedAge
        private Person _selectedPerson;
        public Person SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                if (_selectedPerson != value)
                {
                    _selectedPerson = value;
                    if (_selectedPerson != null)
                    {
                        SelectedName = _selectedPerson.Name;
                        SelectedAge = _selectedPerson.Age;
                    }

                    OnPropertyChanged("SelectedName");
                    OnPropertyChanged("SelectedAge");


                }
            }
        }

        private string _name;

        public string SelectedName
        {
            get
            {
                if (this.SelectedPerson != null)
                {
                    return this.SelectedPerson.Name;
                }
                return string.Empty;
            }
            set
            {
                this._name = value;
            }
        }

        private int _age;

        public int SelectedAge
        {
            get
            {
                if (this.SelectedPerson != null)
                {
                    return this.SelectedPerson.Age;
                }
                return 0;
            }
            set
            {
                this._age = value;
            }
        }
        #endregion

        #region Commands
        public ICommand LoadDataCommand { get; private set; }

        public ICommand SaveChangesCommand { get; private set; }

        public ICommand AddPersonCommand { get; set; }

        #endregion

        public PersonViewModel()
        {
            SaveChangesCommand = new DelegateCommand(SaveChangesAction);
            LoadDataCommand = new DelegateCommand(LoadDataAction);
            AddPersonCommand = new DelegateCommand(AddPersonAction);
        }

        private void LoadDataAction(object obj)
        {
            PersonDataSource.Add(new Person() {Name = "John"});
            PersonDataSource.Add(new Person() {Name = "Kate"});
            PersonDataSource.Add(new Person() { Name = "Sam"});
        }

        private void SaveChangesAction(object obj)
        {
            if (SelectedPerson != null)
            {
                SelectedPerson.Name = _name;
                SelectedPerson.Age = _age;
            }

            IEnumerable<Person> match = _personDataSource.Where((item) => item == SelectedPerson);
            if (match.FirstOrDefault() == null)
            {
                Debug.Assert(SelectedPerson != null, "SelectedPerson != null");
                PersonDataSource.Add(SelectedPerson);
            }
        }

        private void AddPersonAction(object obj)
        {
            SelectedPerson = new Person();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
