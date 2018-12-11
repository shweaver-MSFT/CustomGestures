// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace CustomGestures.Core
{
    /// <summary>
    /// A class to configure and store the properties for custom gesture recognition.
    /// General properties can be added for all gestures, or gesture specific functionality can be built in.
    /// </summary>
    public class GestureProperties
    {
        public int? MinDelay { get; set; }
        public int? MaxDelay { get; set; }
        public int? MinInputs { get; set; }
        public int? MaxInputs { get; set; }
        public int? Tolerance { get; set; }

        public GestureProperties()
        {

        }
    }
}
