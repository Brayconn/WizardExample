using Avalonia.Interactivity;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace WizardExample.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    { 
        int selectedIndex = 0;
        public int SelectedIndex { get => selectedIndex; set => this.RaiseAndSetIfChanged(ref selectedIndex, value); }

        string requiredText = "";
        public string RequiredText { get => requiredText; set => this.RaiseAndSetIfChanged(ref requiredText, value); }

        static IObservable<bool> ObservableAND(IEnumerable<IObservable<bool>> observables)
        {
            return Observable.CombineLatest(observables, x => x.All(y => y));
        }
        static IObservable<bool> ObservableOR(IEnumerable<IObservable<bool>> observables)
        {
            return Observable.CombineLatest(observables, x => x.Any(y => y));
        }
        IObservable<bool> MakePageConditions(Dictionary<int,IObservable<bool>[]> pageParams)
        {
            var allConditions = new List<IObservable<bool>>(PageCount);
            for (int i = 0; i < PageCount; i++)
            {
                var pageConditions = new List<IObservable<bool>>();

                //HACK need to generate functions that do the comparison
                //otherwise i will stay at PageCount for every comparison
                var f = (int j) =>
                    (int x) =>
                        x == j;
                
                //Basic page check
                pageConditions.Add(this.WhenAnyValue(x => x.SelectedIndex, f(i)));
                
                //any other requirements on this page
                if (pageParams.ContainsKey(i))
                    pageConditions.AddRange(pageParams[i]);

                //add it to the list
                if (pageConditions.Count > 1)
                    allConditions.Add(ObservableAND(pageConditions));
                else
                    allConditions.Add(pageConditions[0]);
            }
            return ObservableOR(allConditions);
        }

        public MainWindowViewModel()
        {
            BackCommand = ReactiveCommand.Create<RoutedEventArgs>(Back);

            //Manual version would've looked like this:
            //List<IObservable<bool>> PagesOK = new List<IObservable<bool>>(PageCount);
            //PagesOK.Add(this.WhenAnyValue(x => x.SelectedIndex, x => x == 0));
            //IObservable<bool> FileOK = this.WhenAnyValue(x => x.RequiredText, x => File.Exists(x))
            //                               .Throttle(TimeSpan.FromMilliseconds(500),
            //                               Avalonia.Threading.AvaloniaScheduler.Instance)
            //PagesOK.Add(ObservableAND(this.WhenAnyValue(x => x.SelectedIndex, x => x == 1), FileOK));
            //PagesOK.Add(this.WhenAnyValue(x => x.SelectedIndex, x => x == 2));
            //NextCommand = ReactiveCommand.Create<RoutedEventArgs>(Next, ObservableOR(PagesOK));

            NextCommand = ReactiveCommand.Create<RoutedEventArgs>(Next, 
                MakePageConditions(new Dictionary<int, IObservable<bool>[]>()
                {
                    { 1, new[]
                        {
                            this.WhenAnyValue(x => x.RequiredText, x => File.Exists(x))
                            .Throttle(TimeSpan.FromMilliseconds(500), Avalonia.Threading.AvaloniaScheduler.Instance)
                        }
                    }
                }));
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
    }
}
