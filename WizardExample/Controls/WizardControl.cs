using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Styling;
using System;
using System.Windows.Input;

namespace WizardExample.Controls
{
    public class WizardControl : Carousel, IStyleable
    {
        public const string PART_Carousel = nameof(PART_Carousel);
        public const string PART_BackButton = nameof(PART_BackButton);
        public const string PART_NextButton = nameof(PART_NextButton);

        Type IStyleable.StyleKey => typeof(WizardControl);

        #region NextEnabled Attached Property
        const string NextEnabled = nameof(NextEnabled);
        public static readonly AttachedProperty<bool> NextEnabledProperty =
            AvaloniaProperty.RegisterAttached<WizardControl, Control, bool>(NextEnabled, true);

        public static bool GetNextEnabled(Control element)
        {
            return element.GetValue(NextEnabledProperty);
        }

        public static void SetNextEnabled(Control element, bool value)
        {
            element.SetValue(NextEnabledProperty, value);
        }
        #endregion

        //This could probably be replaced with code that detects when you're on a page > 0
        #region BackEnabled Attached Property
        const string BackEnabled = nameof(BackEnabled);
        public static readonly AttachedProperty<bool> BackEnabledProperty =
            AvaloniaProperty.RegisterAttached<WizardControl, Control, bool>(BackEnabled, true);

        public static bool GetBackEnabled(Control element)
        {
            return element.GetValue(BackEnabledProperty);
        }

        public static void SetBackEnabled(Control element, bool value)
        {
            element.SetValue(BackEnabledProperty, value);
        }
        #endregion

        //This could probably be replaced with code that just detects when we're on the last page
        //and changes the text of the button to say "Finish"
        #region NextContent Attached Property
        const string NextContent = nameof(NextContent);
        public static readonly AttachedProperty<object> NextContentProperty =
            AvaloniaProperty.RegisterAttached<WizardControl, Control, object>(NextContent, "Next");

        public static object GetNextContent(Control element)
        {
            return element.GetValue(NextContentProperty);
        }

        public static void SetNextContent(Control element, object value)
        {
            element.SetValue(NextContentProperty, value);
        }
        #endregion

        //Maybe these two commands could be made to have default implementations if there's no binding given...?
        #region Back Command
        public static readonly StyledProperty<ICommand> BackCommandProperty =
            AvaloniaProperty.Register<WizardControl, ICommand>(nameof(BackCommand));
        public ICommand BackCommand
        {
            get => GetValue(BackCommandProperty);
            set => SetValue(BackCommandProperty, value);
        }
        #endregion

        #region Next Command
        public static readonly StyledProperty<ICommand> NextCommandProperty =
            AvaloniaProperty.Register<WizardControl, ICommand>(nameof(NextCommand));
        public ICommand NextCommand
        {
            get => GetValue(NextCommandProperty);
            set => SetValue(NextCommandProperty, value);
        }
        #endregion

        Carousel Carousel;
        Button BackButton, NextButton;
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            Carousel = e.NameScope.Find<Carousel>(PART_Carousel);
            BackButton = e.NameScope.Find<Button>(PART_BackButton);
            NextButton = e.NameScope.Find<Button>(PART_NextButton);
            Carousel.SelectionChanged += Carousel_SelectionChanged;
            Carousel_SelectionChanged(Carousel, null);
        }

        IDisposable? nextContentBinding = null, nextEnabledBinding = null, backEnabledBinding = null;
        void ReplaceBinding(ref IDisposable? binding, Control item, AvaloniaProperty property, Control source, string path)
        {
            if (binding != null)
                binding.Dispose();
            binding = item.Bind(property, new Binding { Source = source, Path = path });
        }
        private void Carousel_SelectionChanged(object? sender, SelectionChangedEventArgs? e)
        {
            // What should this do? If it's just to enable / disable the buttons, we can do this more simple. 
            // Either do it via command or by doing it here:

            BackButton.IsEnabled = SelectedIndex > 0;
            NextButton.IsEnabled = SelectedIndex < ItemCount - 1;

            // if (sender is Carousel carousel)
            // {
            //     if (carousel.SelectedItem is Control control)
            //     {
            //         ReplaceBinding(ref backEnabledBinding, BackButton, Button.IsEnabledProperty, control, BackEnabled);
            //         ReplaceBinding(ref nextEnabledBinding, NextButton, Button.IsEnabledProperty, control, NextEnabled);
            //         ReplaceBinding(ref nextContentBinding, NextButton, Button.ContentProperty, control, NextContent);
            //     }
            // }
        }
    }
}
