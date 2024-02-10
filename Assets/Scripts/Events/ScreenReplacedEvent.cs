using UI.Controller.Screen;

namespace Events
{
    public sealed class ScreenReplacedEvent
    {
        public ScreenController CurrentScreen;
        public bool FadeOffRequired;
    }
}