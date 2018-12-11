// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using CustomGestures.Core;
using CustomGestures.Gestures;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Input;

namespace CustomGestures
{
    /// <summary>
    /// Recognizes custom built gestures input on XAML elements.
    /// </summary>
    public class CustomGestureRecognizer
    {
        // Gesture events
        public event TypedEventHandler<CustomGestureRecognizer, MultiTappedEventArgs> MultiTapped;

        public CustomGestureSettings GestureSettings { get; set; }

        private GestureProperties _gestureProperties;
        private Dictionary<CustomGestureSettings, IGesture> _gestureProgress;
        private Dictionary<uint, PointerInput> _inputs;

        public CustomGestureRecognizer(GestureProperties gestureProperties = null)
        {
            _gestureProperties = gestureProperties;
            _gestureProgress = new Dictionary<CustomGestureSettings, IGesture>();
            _inputs = new Dictionary<uint, PointerInput>();
        }

        public void ProcessDownEvent(PointerPoint pointerPoint)
        {
            UpdatePointerEvent(pointerPoint, PointerEventType.Down);
        }

        public void ProcessMoveEvents(IList<PointerPoint> pointerPoints)
        {
            foreach (var pointerPoint in pointerPoints)
            {
                UpdatePointerEvent(pointerPoint, PointerEventType.Move);
            }
        }

        public void ProcessUpEvent(PointerPoint pointerPoint)
        {
            UpdatePointerEvent(pointerPoint, PointerEventType.Up);

            // Check if all touches are up. If so, clear the inputs.
            bool allUp = !_inputs.Values.Any(input => input.Current.EventType != PointerEventType.Up);
            if (allUp)
            {
                _inputs.Clear();
            }
        }

        /// <summary>
        /// End all of the gestures and fire any valid gesture events
        /// </summary>
        public void CompleteGesture()
        {
            var gestureProgress = _gestureProgress;
            var inputs = _inputs.Values.ToList();

            var customGestureTypes = Enum.GetValues(typeof(CustomGestureSettings));
            foreach (CustomGestureSettings gestureType in customGestureTypes)
            {
                // Get the cooresponding gesture
                if (!gestureProgress.TryGetValue(gestureType, out IGesture gesture)) continue;

                // End the gesture and get the args
                var args = gesture.End(inputs);
                CompleteGesture(gestureType, args);
            }

            _inputs.Clear();
        }

        /// <summary>
        /// End a specific gestures and fire any valid gesture events
        /// </summary>
        private void CompleteGesture(CustomGestureSettings gestureType, ICustomGestureEventArgs args)
        {
            // Check the args
            if (args == null) return;

            // Skip gestures that are not flagged in the GestureSettings
            if (!GestureSettings.HasFlag(gestureType)) return;

            // Fire the cooresponding gesture event
            switch (gestureType)
            {
                case CustomGestureSettings.MultiTap:
                    OnMultiTapped(args as MultiTappedEventArgs);
                    break;
                case CustomGestureSettings.None:
                default:
                    // This gesture type is not supported.
                    return;
            }
        }

        /// <summary>
        /// Update the various gesture interpreters with the new pointer event data
        /// </summary>
        /// <param name="pointerPoint"></param>
        /// <param name="pointerEventType"></param>
        private void UpdatePointerEvent(PointerPoint pointerPoint, PointerEventType pointerEventType)
        {
            var customGestureTypes = Enum.GetValues(typeof(CustomGestureSettings));
            foreach (CustomGestureSettings gestureType in customGestureTypes)
            {
                // Skip gestures that are not flagged in the GestureSettings
                if (!GestureSettings.HasFlag(gestureType)) continue;

                // Check for an existing started gesture to update.
                // If absent, create one.
                if (!_gestureProgress.TryGetValue(gestureType, out IGesture gesture))
                {
                    // Build the appropriate Gesture object based on the configured CustomGestureSettings
                    switch (gestureType)
                    {
                        case CustomGestureSettings.MultiTap:
                            gesture = new MultiTap(_gestureProperties);
                            break;
                        case CustomGestureSettings.None:
                        default:
                            // This gesture type is not supported.
                            continue;
                    }

                    // Add the gesture to the dictionary by type.
                    // There will only ever be one Gesture per type being analysed by this recognizer instance.
                    _gestureProgress.Add(gestureType, gesture);
                }

                // Track all inputs. If new, add it to the dictionary for tracking.
                // If not, update the existing item.
                if (!_inputs.TryGetValue(pointerPoint.PointerId, out PointerInput pointerInput))
                {
                    pointerInput = new PointerInput(pointerPoint, pointerEventType);
                    _inputs.Add(pointerPoint.PointerId, pointerInput);
                }
                else
                {
                    pointerInput.Update(pointerPoint, pointerEventType);
                    _inputs[pointerPoint.PointerId] = pointerInput;
                }

                // Pipe the input data to the gesture
                var inputs = _inputs.Values.ToList();
                switch (pointerEventType)
                {
                    case PointerEventType.Down:
                        gesture.Start(inputs);
                        break;

                    case PointerEventType.Move:
                        gesture.Move(inputs);
                        break;

                    case PointerEventType.Up:
                        var args = gesture.End(inputs);
                        if (args != null)
                        {
                            CompleteGesture(gestureType, args);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Fires a MultiTapped event with the provided args
        /// </summary>
        /// <param name="args"></param>
        private void OnMultiTapped(MultiTappedEventArgs args)
        {
            MultiTapped?.Invoke(this, args);
        }
    }
}
