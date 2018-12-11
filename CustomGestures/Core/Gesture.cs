// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Windows.UI.Xaml;

namespace CustomGestures.Core
{
    /// <summary>
    /// The base class for custom gestures.
    /// </summary>
    public abstract class Gesture : IGesture
    {
        public abstract CustomGestureSettings GestureType { get; }
        public GestureProperties Properties { get; }

        public Gesture(GestureProperties properties)
        {
            Properties = properties;
        }
        
        public virtual void Start(List<PointerInput> inputs)
        {
            
        }

        public virtual void Move(List<PointerInput> inputs)
        {
            
        }

        public virtual ICustomGestureEventArgs End(List<PointerInput> inputs)
        {
            return null;
        }

        protected void ResetAllProgress(List<PointerInput> inputs)
        {
            foreach(var input in inputs)
            {
                input.ResetProgress(GestureType);
            }
        }
    }
}
