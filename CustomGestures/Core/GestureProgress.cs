// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace CustomGestures.Core
{
    /// <summary>
    /// A class to maintain the progress data for a gesture. 
    /// Add additional properties and methods as needed to support new gesture types.
    /// </summary>
    public class GestureProgress
    {
        public DateTime StartTime { get; private set; }

        public GestureProgress()
        {

        }

        public void Start()
        {
            StartTime = DateTime.Now;
        }
    }
}
