using Modules.Navigation;
using UI.Controller;
using UnityEngine;
using UI.Settings;
using Modules;
using Events;
using TMPro;

namespace UI
{
    public sealed class NavigationPanel : MonoBehaviour, IUpdatable<UpdateCurrentScreen>
    {
        [Header("Buttons")]
        [SerializeField] private ImageButtonController _backButton;
        [SerializeField] private ImageButtonController _homeButton;

        [Header("Labels")]
        [SerializeField] private TMP_Text _label;

        private NavigationElementType _type;

        public void Setup()
        {
            _backButton?.Setup(new ImageButtonSettings
            {
                Action = () =>
                {
                    EventSystem.Send(new NavigationPointBackEvent());
                }
            });
            _homeButton?.Setup(new ImageButtonSettings
            {
                Action = () =>
                {
                    EventSystem.Send(new NavigationPointHomeEvent());
                }
            });

            UpdateState();
        }

        public void UpdateState(UpdateCurrentScreen data = null)
        {
            if (_type == default)
                return;

            // to do: set text
        }

        private void Start()
        {
            EventSystem.Subscribe<UpdateCurrentScreen>(UpdateState);
        }

        private void OnDestroy()
        {
            EventSystem.Unsubscribe<UpdateCurrentScreen>(UpdateState);
        }
    }
}