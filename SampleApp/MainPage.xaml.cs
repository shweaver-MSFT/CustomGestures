// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using CustomGestures;
using CustomGestures.Gestures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SampleApp
{
    /// <summary>
    /// This sample demonstrates the implementation of the CustomGestureRecognizer. 
    /// The original Windows GestureRecognizer is also implemented to demonstrate
    /// their function side-by-side and in conjunction with each other.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ManipulationInputProcessor _processor;
        private CustomManipulationInputProcessor _customProcessor;
        private List<Color> _colors;

        public MainPage()
        {
            InitializeComponent();

            SizeChanged += MainPage_SizeChanged;

            // Convert the colors class to a list of colors.
            // This is used to update the rectangle color.
            var props = typeof(Colors).GetProperties();
            var colorNames = props.Select((p) => p.Name).ToList();
            _colors = colorNames.Select((name) => (Color)typeof(Colors).GetProperty(name).GetValue(typeof(Colors))).ToList();

            // Windows Gesture Recognizer
            SetupElementForGestureRecognition(TargetElement, ReferenceElement);

            // Custom Gesture Recognizer
            SetupElementForCustomGestureRecognition(TargetElement, ReferenceElement);
        }

        private async void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                //Center the Content
                var left = (ReferenceElement.ActualWidth - Content.ActualWidth) / 2;
                Canvas.SetLeft(Content, left);

                var top = (ReferenceElement.ActualHeight - Content.ActualHeight) / 2;
                Canvas.SetTop(Content, top);
            });
        }

        private void SetupElementForGestureRecognition(UIElement target, UIElement reference)
        {
            var gestureRecognizer = new GestureRecognizer();
            gestureRecognizer.GestureSettings = GestureSettings.Tap | GestureSettings.DoubleTap;
            gestureRecognizer.Tapped += GestureRecognizer_Tapped;

            _processor = new ManipulationInputProcessor(gestureRecognizer, target, reference);
        }

        private void SetupElementForCustomGestureRecognition(UIElement target, UIElement reference)
        {
            var gestureRecognizer = new CustomGestureRecognizer();
            gestureRecognizer.GestureSettings = CustomGestureSettings.MultiTap;
            gestureRecognizer.MultiTapped += GestureRecognizer_MultiTapped;

            _customProcessor = new CustomManipulationInputProcessor(gestureRecognizer, target, reference);
        }

        private void GestureRecognizer_Tapped(GestureRecognizer sender, TappedEventArgs args)
        {
            // The GestureRecognizer only supports Tap and DoubleTap. 
            // TapCount will never be geater than 2.
            var eventName = args.TapCount == 1 ? "Tap" : "DoubleTap";
            UpdateDisplay(eventName, 1);
        }

        private void GestureRecognizer_MultiTapped(CustomGestureRecognizer sender, MultiTappedEventArgs args)
        {
            UpdateDisplay("MultiTap", args.TouchCount);
        }

        private async void UpdateDisplay(string eventName, uint touchCount)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                var random = new Random();
                var bgColor = _colors[random.Next(_colors.Count())];
                var fgColor = (bgColor == Colors.Black) ? Colors.White : Colors.Black;

                TargetElement.Background = new SolidColorBrush(bgColor);
                TouchCount.Foreground = new SolidColorBrush(fgColor);
                EventName.Foreground = new SolidColorBrush(fgColor);

                EventName.Text = eventName;
                TouchCount.Text = touchCount.ToString();
            });
        }
    }
}
