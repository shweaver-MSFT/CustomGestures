# CustomGestures

Enable custom gestures in UWP XAML apps!

This sample demonstrates a Custom Gesture system that enables the creation of custom gestures on UWP. It was designed with a comparable consumption signature as the existing Windows GestureRecognizer for easy integration.

The sample comes with an example MultiTap event to enable support for n-finger tap events on XAML elements.

## Running the Sample

1. Open the solution in Visual Studio 2017
2. Set the SampleApp build architecture (not Any CPU)
3. F5 to deploy

Once the sample is running, use your fingers to tap the circle. Successful MultiTaps will change the button color and show the number of fingers used.

> Note: A multitouch enabled device is required to interact with the sample. This solution was tested against a Surface Book 1 touch display.

## Extend with your own Gestures

To add your own gestures, simply follow these steps:

1. Add a new gesture class to the Gestures namespace that extends the `CustomGestures.Core.Gesture` base class.
2. Add a new *EventArgs class to the Gestures namespace that extends the `CustomGestures.Core.CustomGestureEventArgs` base class.
3. Add the gesture name to the `CustomGestureSettings` enum, so it can be flagged on/off.
4. Integrate into the `CustomGestureRecognizer`. This is much easier than it sounds. Add the gesture to the two switch/case blocks, and add a corresponding event to fire.

## Contributing

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/)
or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.