// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using CustomGestures.Core;
using System;

namespace CustomGestures.Gestures
{
    /// <summary>
    /// The event args for a custom MultiTap gesture event. 
    /// </summary>
    public class MultiTappedEventArgs : CustomGestureEventArgs, ICustomGestureEventArgs
    {
        public TimeSpan Interval { get; set; }
        public uint TouchCount { get; set; }
        public uint TapCount { get; set; }
    }
}
