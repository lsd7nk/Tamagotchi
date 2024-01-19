using Application = Tamagotchi.Application;
using System.Collections.Generic;
using UnityEngine.UI;
using UI.Controller;
using UnityEngine;
using DG.Tweening;
using UI.Settings;
using Settings;

namespace UI.Popups
{
    public class PopupView<T> : PopupViewBase where T : Popup
    {
        [Header("Base")]
        [SerializeField] private RectTransform _rootRect;
        [SerializeField] private Button _overlayButton;

        [Header("Blocks")]
        [SerializeField] private RectTransform _topParent;
        [SerializeField] private RectTransform _middleParent;
        [SerializeField] private RectTransform _bottomParent;

        [Header("Button parents")]
        [SerializeField] private RectTransform _someButtonParent;
        [SerializeField] private RectTransform _oneButtonParent;

        private readonly float _durationTween = 0.3f;
        private bool _ignoreOverlayButtonAction;

        public virtual void Setup(T settings)
        {
            _ignoreOverlayButtonAction = settings.IgnoreOverlayButtonAction;
            InitializeButtons(settings.ButtonSettings);
        }

        public override void Show()
        {
            base.Show();

            AddListeners();
            DoShow();
        }

        public override void Hide()
        {
            base.Hide();

            RemoveListeners();
            DoHide();
        }

        private void InitializeButtons<B>(List<B> buttonSettings) where B : ButtonSettings
        {
            if (buttonSettings == null || buttonSettings.Count == 0)
                return;

            var prefabsSet = SettingsProvider.Get<PrefabsSet>();

            _oneButtonParent.gameObject.SetActive(buttonSettings.Count == 1);
            _someButtonParent.gameObject.SetActive(buttonSettings.Count != 1);

            if (buttonSettings.Count == 1)
            {
                var settings = buttonSettings[0];

                //if (settings is TextButtonSettings textButtonSettings)
                //{
                //    var prefab = prefabsSet.Buttons.First(x => x.GetComponent<TextButtonController>() != null)
                //        .GetComponent<TextButtonController>();

                //    Instantiate(prefab, _oneButtonParent, false)
                //        .Setup(textButtonSettings);
                //}
            }
            else
            {
                //foreach (var setting in buttonSettings)
                //{
                //    if (setting is TextButtonSettings textButtonSettings)
                //    {
                //        var prefab = prefabsSet.Buttons.First(x => x.GetComponent<TextButtonController>() != null)
                //            .GetComponent<TextButtonController>();

                //        Instantiate(prefab, _someButtonParent, false)
                //            .Setup(textButtonSettings);
                //    }
                //}
            }
        }

        private void DoShow()
        {
            var startOffset = Vector3.down.normalized * Application.MainCanvas.sizeDelta.y;
            var targetPosition = _rootRect.localPosition;

            if (Mathf.Abs(startOffset.sqrMagnitude) - Mathf.Abs(Vector2.zero.sqrMagnitude) <= Mathf.Epsilon)
                return;

            _rootRect.localPosition += startOffset;
            _rootRect.DOAnchorPos(targetPosition, _durationTween)
                .SetEase(Ease.InOutCubic)
                .OnComplete(() => _overlayButton.gameObject.SetActive(true));
        }

        private void DoHide()
        {
            var targetPosition = Vector3.down.normalized * Application.MainCanvas.sizeDelta.y;

            if (Mathf.Abs(targetPosition.sqrMagnitude) - Mathf.Abs(Vector2.zero.sqrMagnitude) <= Mathf.Epsilon)
                return;

            _overlayButton.gameObject.SetActive(false);
            _rootRect.DOAnchorPos(targetPosition, _durationTween)
                .SetEase(Ease.InOutCubic)
                .OnComplete(() => Destroy(gameObject));
        }

        private void AddListeners()
        {
            if (_ignoreOverlayButtonAction)
                return;

            _overlayButton?.onClick.AddListener(Hide);
        }

        private void RemoveListeners()
        {
            _overlayButton?.onClick?.RemoveListener(Hide);
        }
    }
}
