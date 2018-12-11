// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace SampleApp
{
    /// <summary>
    /// Code adapted from BasicInput sample, Scenario 5 - GestureRecognizer
    /// https://github.com/Microsoft/Windows-universal-samples/blob/master/Samples/BasicInput/cs/5-GestureRecognizer.xaml.cs
    /// </summary>
    public class ManipulationInputProcessor
    {
        private GestureRecognizer _recognizer;
        private UIElement _element;
        private readonly UIElement _reference;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gestureRecognizer"></param>
        /// <param name="target"></param>
        /// <param name="referenceFrame"></param>
        public ManipulationInputProcessor(GestureRecognizer gestureRecognizer, UIElement target, UIElement referenceFrame)
        {
            _recognizer = gestureRecognizer;
            _element = target;
            _reference = referenceFrame;

            // Set up pointer event handlers. These receive input events that are used by the gesture recognizer.
            _element.PointerPressed += OnPointerPressed;
            _element.PointerMoved += OnPointerMoved;
            _element.PointerReleased += OnPointerReleased;
            _element.PointerCanceled += OnPointerCanceled;
        }

        // Route the pointer pressed event to the gesture recognizer.
        // The points are in the reference frame of the canvas that contains the rectangle _element.
        private void OnPointerPressed(object sender, PointerRoutedEventArgs args)
        {
            // Set the pointer capture to the element being interacted with so that only it
            // will fire pointer-related events
            _element.CapturePointer(args.Pointer);

            // Feed the current point into the gesture recognizer as a down event
            _recognizer.ProcessDownEvent(args.GetCurrentPoint(_reference));
        }

        /// <summary>
        /// Route the pointer moved event to the gesture recognizer.
        /// The points are in the reference frame of the canvas that contains the rectangle _element.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnPointerMoved(object sender, PointerRoutedEventArgs args)
        {
            // Feed the set of points into the gesture recognizer as a move event
            _recognizer.ProcessMoveEvents(args.GetIntermediatePoints(_reference));
        }

        /// <summary>
        /// Route the pointer released event to the gesture recognizer.
        /// The points are in the reference frame of the canvas that contains the rectangle _element.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnPointerReleased(object sender, PointerRoutedEventArgs args)
        {
            // Feed the current point into the gesture recognizer as an up event
            _recognizer.ProcessUpEvent(args.GetCurrentPoint(_reference));

            // Release the pointer
            _element.ReleasePointerCapture(args.Pointer);
        }

        /// <summary>
        /// Route the pointer canceled event to the gesture recognizer.
        /// The points are in the reference frame of the canvas that contains the rectangle _element.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnPointerCanceled(object sender, PointerRoutedEventArgs args)
        {
            _recognizer.CompleteGesture();
            _element.ReleasePointerCapture(args.Pointer);
        }

        public void Reset()
        {
            _recognizer.CompleteGesture();
        }
    }
}