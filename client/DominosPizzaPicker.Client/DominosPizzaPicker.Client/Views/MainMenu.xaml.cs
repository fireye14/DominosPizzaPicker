using DominosPizzaPicker.Client.Helpers.CustomViews;
using DominosPizzaPicker.Client.Models.Managers;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DominosPizzaPicker.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainMenu : CustomMainMenu
	{
        public Command ConnectCommand { get; private set; }

        public MainMenu() : base()
        {
            lblURL.Text = "Current: " + (Constants.ApplicationURL == Constants.LocalIISURL ? "Local IIS URL" : (Constants.ApplicationURL == Constants.ConveyorURL ? "Conveyor URL" : "Not Connected"));

            ConnectCommand = new Command<string>(
                execute: async (string type) =>
                {
                    IsBusy = true;
                    NavigateCommand.ChangeCanExecute();
                    ConnectCommand.ChangeCanExecute();
                    var before = Constants.ApplicationURL;
                    Constants.ApplicationURL = type.ToUpper().Equals("LOCAL") ? Constants.LocalIISURL : Constants.ConveyorURL;
                    if (before != Constants.ApplicationURL)
                    {
                        lblURL.Text = "Current: ...";
                        if (await CachedData.ResetConnection())
                            lblURL.Text = "Current: " + (Constants.ApplicationURL == Constants.LocalIISURL ? "Local IIS URL" : (Constants.ApplicationURL == Constants.ConveyorURL ? "Conveyor URL" : "Not Connected"));
                        else
                        {
                            Constants.ApplicationURL = "NOT CONNECTED";
                            lblURL.Text = "Current: Not Connected";
                        }
                    }
                    IsBusy = false;
                    NavigateCommand.ChangeCanExecute();
                    ConnectCommand.ChangeCanExecute();
                },
                canExecute: (string a) =>
                {
                    return !IsBusy;
                });

            BindingContext = null;
            BindingContext = this;

        }

    }
}