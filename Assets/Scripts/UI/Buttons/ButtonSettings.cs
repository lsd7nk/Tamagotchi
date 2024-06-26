using UnityEngine;
using UI.Popups;
using System;

namespace UI.Settings
{
    public class ButtonSettings
    {
        public Action Action;
        public Action<PopupViewBase> ActionWithInstance;

        public PopupViewBase PopupInstance;

        public bool IsOneClickButton;
        public bool MoneySignState;
        public bool AdsSignState;

        public int MoneyValue;
    }

    public sealed class TextButtonSettings : ButtonSettings
    {
        public string Title;
    }

    public sealed class ImageButtonSettings : ButtonSettings
    {
        public Sprite Icon;
    }
}