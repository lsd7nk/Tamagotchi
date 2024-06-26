using System.Collections.Generic;
using UnityEngine.UI;
using UI.Controller;
using UI.Settings;
using UnityEngine;
using System.Linq;
using Settings;
using System;
using TMPro;

namespace UI.Popups
{
    public sealed class DefaultPopupView : PopupView<DefaultPopup>
    {
        public List<DropdownController> Dropdowns { get; private set; }

        [Header("Labels")]
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _content;

        [Header("Icons")]
        [SerializeField] private Image _icon;
        [SerializeField] private RawImage _petIcon;

        [Header("Other")]
        [SerializeField] private RectTransform _infoParent;
        [SerializeField] private RectTransform _iconParent;

        public override void Setup(DefaultPopup settings)
        {
            base.Setup(settings);

            var prefabSet = SettingsProvider.Get<PrefabsSet>();

            SetIcons(settings);
            SetTitle(settings.Title);
            SetContent(settings.Content);
            SetDropdowns(settings.DropdownSettings, prefabSet.Dropdown);
            SetInfoFields(settings.InfoFieldSettings, prefabSet.InfoField);
        }

        public override void Show()
        {
            base.Show();
            DoShow();
        }

        public override void Hide(Action onHideCallback = null)
        {
            base.Hide(onHideCallback);
            DoHide(onHideCallback);
        }

        private void SetTitle(string text)
        {
            _title.text = text;
        }

        private void SetContent(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            _content.gameObject.SetActive(true);
            _content.text = text;
        }

        private void SetDropdowns(List<DropdownSettings> dropdownsSettings, DropdownController prefab)
        {
            if (dropdownsSettings == null || !dropdownsSettings.Any())
                return;

            Dropdowns = new List<DropdownController>();

            _infoParent.gameObject.SetActive(true);

            dropdownsSettings.ForEach(settings =>
            {
                var dropdown = Instantiate(prefab, _infoParent);

                dropdown.Setup(settings);
                Dropdowns.Add(dropdown);
            });
        }

        private void SetInfoFields(List<InfoFieldSettings> infoFieldSettings, InfoFieldController prefab)
        {
            if (infoFieldSettings == null || !infoFieldSettings.Any())
                return;

            _infoParent.gameObject.SetActive(true);

            infoFieldSettings.ForEach(settings =>
            {
                Instantiate(prefab, _infoParent)
                    .Setup(settings);
            });
        }

        private void SetIcons(DefaultPopup settings)
        {
            _iconParent.gameObject.SetActive(settings.UseIcon || settings.UseIcon);

            if (settings.UseIcon)
                _icon.sprite = settings.Icon;

            _petIcon.gameObject.SetActive(settings.UsePetIcon);
            _icon.gameObject.SetActive(settings.UseIcon);
        }
    }
}