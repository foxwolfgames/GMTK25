using FWGameLib.Common.EventSystem;

namespace Chronomance.UI
{
    public class UIButtonPressEvent : FWEvent<UIButtonPressEvent>
    {
        public readonly UIButton.UIButtonAction Action;

        public UIButtonPressEvent(UIButton.UIButtonAction action)
        {
            Action = action;
        }
    }
}