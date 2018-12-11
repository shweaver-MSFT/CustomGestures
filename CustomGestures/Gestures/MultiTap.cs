// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using CustomGestures.Core;
using System;
using System.Collections.Generic;

namespace CustomGestures.Gestures
{
    /// <summary>
    /// A custom MultiTap gesture. Fires when the user touches multiple fingers down and then up, quickly and without moving them.
    /// </summary>
    public class MultiTap : Gesture, IGesture
    {
        // Defaults
        public const int DEFAULT_MIN_DELAY_MS = 0;
        public const int DEFAULT_MAX_DELAY_MS = 300;
        public const int DEFAULT_MIN_INPUTS = 2;
        public const int DEFAULT_MAX_INPUTS = 5;
        public const int DEFAULT_PIXEL_MOVE_TOLERANCE = 10;

        // Properties
        public int MinDelay { get; }
        public int MaxDelay { get; }
        public int MinInputs { get; }
        public int MaxInputs { get; }
        public int Tolerance { get; }

        public override CustomGestureSettings GestureType => CustomGestureSettings.MultiTap;

        private uint _touchCount;

        public MultiTap(GestureProperties gestureProperties = null) : base(gestureProperties)
        {
            _touchCount = 0;

            // Update property values with the passed in GestureProperties or defaults
            MinDelay = gestureProperties?.MinDelay?? DEFAULT_MIN_DELAY_MS;
            MaxDelay = gestureProperties?.MaxDelay ?? DEFAULT_MAX_DELAY_MS;
            MinInputs = gestureProperties?.MinInputs ?? DEFAULT_MIN_INPUTS;
            MaxInputs = gestureProperties?.MaxInputs ?? DEFAULT_MAX_INPUTS;
            Tolerance = gestureProperties?.Tolerance ?? DEFAULT_PIXEL_MOVE_TOLERANCE;
        }

        public override void Start(List<PointerInput> inputs)
        {
            _touchCount = (uint)inputs.Count;

            foreach (var input in inputs)
            {
                var progress = input.GetGestureProgress(GestureType);
                progress.Start();
            }
        }

        public override void Move(List<PointerInput> inputs)
        {
            foreach(var input in inputs)
            {
                if (input.Current.EventType != PointerEventType.Move)
                {
                    continue;
                }

                var current = input.Current;
                var previous = input.Previous;
                if (!CustomGestureHelpers.IsNearby(current.Pointer.Position, previous.Pointer.Position, Tolerance))
                {
                    ResetAllProgress(inputs);
                }
            }
        }

        public override ICustomGestureEventArgs End(List<PointerInput> inputs)
        {
            // Make sure we have the correct inputs
            if (inputs.Count < MinInputs || inputs.Count > MaxInputs || _touchCount != inputs.Count)
            {
                return null;
            }

            var startTime = DateTime.MaxValue;
            foreach(var input in inputs)
            {
                // Check that all inputs have ended
                if (input.Current.EventType != PointerEventType.Up)
                {
                    return null;
                }

                // Check that all inputs have progress
                var progress = input.GetGestureProgress(GestureType);
                if (progress.StartTime == null)
                {
                    return null;
                }

                // Save off the oldest start time
                if (progress.StartTime < startTime)
                {
                    startTime = progress.StartTime;
                }
            }

            var interval = DateTime.Now.Subtract(startTime);
            if (MinDelay > interval.TotalMilliseconds || MaxDelay < interval.TotalMilliseconds)
            {
                ResetAllProgress(inputs);
                return null;
            }

            return new MultiTappedEventArgs()
            {
                Interval = interval,
                TouchCount = _touchCount,
                TapCount = 1
            };
        }
    }
}
