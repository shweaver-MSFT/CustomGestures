// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace CustomGestures.Core
{
    /// <summary>
    /// The base interface for a custom gesture
    /// </summary>
    internal interface IGesture
    {
        CustomGestureSettings GestureType { get; }

        ICustomGestureEventArgs End(List<PointerInput> inputs);
        void Move(List<PointerInput> inputs);
        void Start(List<PointerInput> inputs);
    }
}