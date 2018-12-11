// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace CustomGestures
{
    /// <summary>
    /// An enum of supported gesture types.
    /// </summary>
    [FlagsAttribute]
    public enum CustomGestureSettings : uint
    {
        //
        // Summary:
        //     Disable support for gestures and manipulations.
        None = 0,
        //
        // Summary:
        //     Enable support for the tap gesture with multiple contacts.
        MultiTap = 1,
    }
}
