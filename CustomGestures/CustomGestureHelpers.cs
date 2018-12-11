// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
using Windows.Foundation;

namespace CustomGestures
{
    /// <summary>
    /// Helper functions for calculations in custom gestures.
    /// </summary>
    public static class CustomGestureHelpers
    {
        /// <summary>
        /// Check the distance between two points is less than or equal to a provided tolerance.
        /// </summary>
        /// <param name="pointA"></param>
        /// <param name="pointB"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool IsNearby(Point pointA, Point pointB, int tolerance)
        {
            double x1 = pointA.X,
                   y1 = pointA.Y,
                   x2 = pointB.X,
                   y2 = pointB.Y;

            return ((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)) <= tolerance * tolerance;
        }
    }
}
