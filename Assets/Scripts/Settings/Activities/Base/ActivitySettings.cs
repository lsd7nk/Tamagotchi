using System.Collections.Generic;
using Modules.Localization;
using Modules.Navigation;
using UnityEngine;
using System;
using Core;

namespace Settings.Activity
{
    public abstract class ActivitySettings : ScriptableObject
    {
        [field: SerializeField] public ActivityLocalization Localization { get; private set; }
        [field: SerializeField] public List<ParameterRange> ParametersChanges { get; private set; }

        public abstract NavigationElementType Type { get; }
    }


    [Serializable]
    public sealed class ActivityLocalization
    {
        public string Title => LocalizationProvider.GetText(_asset, "title");

        public string MainContent => LocalizationProvider.GetText(_asset, "main_popup/content");
        public string ResultContent => LocalizationProvider.GetText(_asset, "result_popup/content");

        public string LeftButtonContent => LocalizationProvider.GetText(_asset, "button_content/left");
        public string RightButtonContent => LocalizationProvider.GetText(_asset, "button_content/right");

        public string ResultButton => LocalizationProvider.GetText("ok/button");

        public string TypeName => LocalizationProvider.GetText(_asset, "name/type");


        [SerializeField] private LocalizedText _asset;

        public string GetValueTypeContent<T>(T type) where T : Enum
        {
            return LocalizationProvider.GetText(_asset, $"type/{type}");
        }
    }
}