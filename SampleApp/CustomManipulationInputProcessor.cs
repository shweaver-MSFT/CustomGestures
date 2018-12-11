// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using CustomGestures;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace SampleApp
{
    public class CustomManipulationInputProcessor
    {
        private CustomGestureRecognizer _recognizer;
        private UIElement _element;
        private readonly UIElement _reference;

        public CustomManipulationInputProcessor(CustomGestureRecognizer gestureRecognizer, UIElement target, UIElement reference)
        {
            _recognizer = gestureRecognizer;
            _element = target;
            _reference = reference;

            // Set up pointer event handlers. 
            // These receive input events that are used by the gesture recognizer.
            _element.PointerPressed += OnPointerPressed;
            _element.PointerMoved += OnPointerMoved;
            _element.PointerReleased += OnPointerReleased;
            _element.PointerCanceled += OnPointerCanceled;
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // Set the pointer capture to the element being interacted with so that only it
            // will fire pointer-related events
            _element.CapturePointer(e.Pointer);

            // Feed the current point into the gesture recognizer as a down event
            _recognizer.ProcessDownEvent(e.GetCurrentPoint(_reference));
        }

        private void OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            // Feed the set of points into the gesture recognizer as a move event
            _recognizer.ProcessMoveEvents(e.GetIntermediatePoints(_reference));
        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            // Feed the current point into the gesture recognizer as an up event
            _recognizer.ProcessUpEvent(e.GetCurrentPoint(_reference));

            // Release the pointer
            _element.ReleasePointerCapture(e.Pointer);
        }

        private void OnPointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            _recognizer.CompleteGesture();
            _element.ReleasePointerCapture(e.Pointer);
        }

        public void Reset()
        {
            _recognizer.CompleteGesture();
        }
    }
}
