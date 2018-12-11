// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Windows.UI.Input;

namespace CustomGestures.Core
{
    /// <summary>
    /// A class to maintain the event lifecycle and progress of a single pointer during custom gesture input.
    /// </summary>
    public class PointerInput
    {
        public PointerEvent Initial { get; private set; }
        public PointerEvent Current { get; private set; }
        public PointerEvent Previous { get; private set; }

        public uint Identifier { get; }

        private Dictionary<CustomGestureSettings, GestureProgress> _progress;

        public PointerInput(PointerPoint pointerPoint, PointerEventType eventType)
        {
            _progress = new Dictionary<CustomGestureSettings, GestureProgress>();

            var currentEvent = new PointerEvent(pointerPoint, eventType);
            Initial = Current = Previous = currentEvent;
            Identifier = currentEvent.Pointer.PointerId;
        }

        public void Update(PointerPoint pointerPoint, PointerEventType eventType)
        {
            Previous = Current;
            Current = new PointerEvent(pointerPoint, eventType);
        }

        public GestureProgress GetGestureProgress(CustomGestureSettings gestureId)
        {
            if (!_progress.ContainsKey(gestureId))
            {
                _progress.Add(gestureId, new GestureProgress());
            }

            return _progress[gestureId];
        }

        public void ResetProgress(CustomGestureSettings gestureId)
        {
            _progress[gestureId] = new GestureProgress();
        }

        public void ResetAllProgress()
        {
            _progress.Clear();
        }
    }
}
