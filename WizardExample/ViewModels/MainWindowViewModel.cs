using Avalonia.Interactivity;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using CommunityToolkit.Mvvm.Input;

namespace WizardExample.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    { 
        int selectedIndex = 0;
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedIndex, value);
                NextAdvancedCommand.NotifyCanExecuteChanged();
            }
        }

        string requiredText = "";
        public string RequiredText
        {
            get => requiredText;
            set
            {
                this.RaiseAndSetIfChanged(ref requiredText, value);
                NextAdvancedCommand.NotifyCanExecuteChanged();
            }
        }

        public IObservable<bool> SomeParameter;

        public MainWindowViewModel()
        {
            SomeParameter = this.WhenAnyValue(x => x.RequiredText, y => y.Length > 5).Throttle(new TimeSpan(5000));
            
            BackCommand = ReactiveCommand.Create<RoutedEventArgs>(Back);
            NextCommand = ReactiveCommand.Create<RoutedEventArgs>(Next);
        }

        public ReactiveCommand<RoutedEventArgs, Unit> BackCommand { get; }
        public ReactiveCommand<RoutedEventArgs, Unit> NextCommand { get; }

        const int PageCount = 3;
        public void Back(RoutedEventArgs e)
        {
            //no check here because this button should be disabled on page index 0
            SelectedIndex--;
        }
        public void Next(RoutedEventArgs e)
        {
            if (SelectedIndex < PageCount - 1)
            {
                //pretend there's advanced logic that might skip more than one page at a time
                SelectedIndex++;
            }
            else
            {
                //pretend there's code here that closes the dialog
            }
        }
        [RelayCommand (CanExecute = nameof(CanGoNext))]
        private void NextAdvanced()
        {
            if (selectedIndex == 1 && RequiredText.Length > 5)
            {
                SelectedIndex++;
            }
            else
            {
                SelectedIndex++;
            }
        }

        private bool CanGoNext => selectedIndex != 1 || RequiredText.Length > 5;
    }
}
