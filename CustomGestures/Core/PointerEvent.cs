// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Windows.UI.Input;

namespace CustomGestures.Core
{
    /// <summary>
    /// A wrapper for pointer events being passed in during custom gesture recognition.
    /// </summary>
    public class PointerEvent
    {
        public PointerEventType EventType { get; }
        public PointerPoint Pointer { get; }

        public PointerEvent(PointerPoint pointer, PointerEventType eventType)
        {
            Pointer = pointer;
            EventType = eventType;
        }
    }
}
