// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace CustomGestures.Core
{
    /// <summary>
    /// A base class for custom gesture event arguments.
    /// </summary>
    public abstract class CustomGestureEventArgs : EventArgs, ICustomGestureEventArgs
    {
        public CustomGestureEventArgs()
        {
            
        }
    }
}
