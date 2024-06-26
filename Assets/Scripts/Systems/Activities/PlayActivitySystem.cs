using System.Collections.Generic;
using Modules.Navigation;
using Settings.Activity;
using Core.Animation;
using Leopotam.Ecs;
using UI.Settings;
using Components;
using UI.Popups;

namespace Systems.Activities
{
    public sealed class PlayActivitySystem : ActivitySystem<FreeActivitySettings>
    {
        protected override NavigationElementType Type => NavigationElementType.PlayActivity;

        private PlayType _selectedPlayType;

        protected override void StartActivity(bool isEnable)
        {
            World.NewEntity().Replace(new ShowPopupComponent
            {
                Settings = new PopupToShow<DefaultPopup>(new DefaultPopup
                {
                    Title = Settings.Localization.Title,
                    Icon = Icon,
                    Content = Settings.Localization.MainContent,
                    DropdownSettings = GetDropdownSettings<PlayType>(),
                    ButtonSettings = new List<TextButtonSettings>
                    {
                        new TextButtonSettings
                        {
                            Title = Settings.Localization.LeftButtonContent,
                            Action = () =>
                            {
                                World.NewEntity().Replace(new HidePopupComponent());
                            }
                        },
                        new TextButtonSettings
                        {
                            Title = Settings.Localization.RightButtonContent,
                            ActionWithInstance = (popup) =>
                            {
                                var defaultPopup = (DefaultPopupView)popup;
                                _selectedPlayType = defaultPopup.Dropdowns[0].GetCurrentValue<PlayType>();

                                World.NewEntity().Replace(new ChangePetEyesAnimationEvent(EyesAnimationType.Happy));
                                EndActivity(false, true);
                            }
                        }
                    },
                    UseIcon = true
                })
            });
        }

        protected override void EndActivity(bool useIcon, bool usePetIcon)
        {
            var playType = Settings.Localization.GetValueTypeContent(_selectedPlayType);

            World.NewEntity().Replace(new ShowPopupComponent
            {
                Settings = new PopupToShow<ResultPopup>(new ResultPopup()
                {
                    Title = Settings.Localization.Title,
                    Content = string.Format(Settings.Localization.ResultContent, playType.ToLower()),
                    InfoParameterSettings = GetInfoParameterSettings(),
                    ButtonSettings = new List<TextButtonSettings>
                    {
                        new TextButtonSettings
                        {
                            Title = Settings.Localization.ResultButton,
                            Action = () =>
                            {
                                World.NewEntity().Replace(new HidePopupComponent());
                                World.NewEntity().Replace(new ChangePetEyesAnimationEvent(default));
                            }
                        }
                    },
                    UseIcon = useIcon,
                    UsePetIcon = usePetIcon
                })
            });
        }


        private enum PlayType : sbyte
        {
            WithABall = 0,
            WithAToy = 1,
        }
    }
}