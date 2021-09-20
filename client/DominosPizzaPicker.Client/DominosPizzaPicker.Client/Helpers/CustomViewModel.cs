using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;

namespace DominosPizzaPicker.Client.Helpers
{
    public class CustomViewModel : Disposable, INotifyPropertyChanged
    {
        #region Properties and Backing Fields

        public ActivityIndicator ActivityIndicator { get; set; }

        public INavigation Navigation { get; set; }

        private Dictionary<string, Func<bool>> _validations;
        public Dictionary<string, Func<bool>> Validations => _validations = _validations ?? new Dictionary<string, Func<bool>>();


        private List<string> _errors;
        public List<string> Errors => _errors = _errors ?? new List<string>();

        //private List<Command> _commands;
        //public List<Command> Commands => _commands = _commands ?? new List<Command>();

        private Dictionary<Command, List<string>> _commandCanExecuteProperties;
        public Dictionary<Command, List<string>> CommandCanExecuteProperties => _commandCanExecuteProperties = _commandCanExecuteProperties ?? new Dictionary<Command, List<string>>();

        // Result of most recent validation
        public bool IsValid => Errors != null && !Errors.Any();

        private bool _isBusy; 
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private CancellationHelper _cancellation;
        protected CancellationHelper Cancellation => _cancellation = _cancellation ?? new CancellationHelper();

        protected bool EnableViewModelPropertyChanged
        {
            set
            {
                PropertyChanged -= ViewModel_PropertyChanged;

                if (!value)
                    return;

                PropertyChanged += ViewModel_PropertyChanged;
            }
        }

        #endregion

        #region Constructor

        public CustomViewModel()
        {
            PropertyChanged += ViewModel_PropertyChanged;
            AddValidations();
            InitializeCommands();
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                EnableViewModelPropertyChanged = false;
                OnViewModelPropertyChanged(e);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                EnableViewModelPropertyChanged = true;
            }

        }
        #endregion

        #region Overrides
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                PropertyChanged -= ViewModel_PropertyChanged;
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Methods

        // run all validations, add/remove to errors list as needed
        protected virtual bool Validate()
        {
            foreach (var v in Validations)
            {
                if (v.Value())
                    Errors.Remove(v.Key);

                else if (!Errors.Contains(v.Key))
                    Errors.Add(v.Key);
            }
            if(Validations.Count > 0)
                OnPropertyChanged(nameof(IsValid));
            return IsValid;
        }

        // run validation of one property. add/remove to errors list as needed
        protected virtual bool Validate(string propertyName)
        {
            if (Validations.ContainsKey(propertyName))
            {
                if (Validations[propertyName]())
                    Errors.Remove(propertyName);

                else if (!Errors.Contains(propertyName))
                    Errors.Add(propertyName);
            }
            OnPropertyChanged(nameof(IsValid));
            return !Errors.Contains(propertyName);
        }

        protected virtual void AddValidations()
        {
        }

        protected virtual void InitializeCommands()
        {
        }

        protected virtual void OnViewModelPropertyChanged(PropertyChangedEventArgs e)
        {
            foreach (var c in CommandCanExecuteProperties)
            {
                if (c.Value.Contains(e.PropertyName))
                    c.Key.ChangeCanExecute();
            }
        }

        // Set a property and notify property changed if necessary
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string name = null)
        {
            if (object.Equals(storage, value))
                return false;            

            storage = value;
            OnPropertyChanged(name);
            return true;
        }

        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

    }
}