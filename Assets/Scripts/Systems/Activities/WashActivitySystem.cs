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
    public sealed class WashActivitySystem : ActivitySystem<FreeActivitySettings>
    {
        protected override NavigationElementType Type => NavigationElementType.WashActivity;

        private WashType _selectedWashType;

        protected override void StartActivity(bool isEnable)
        {
            World.NewEntity().Replace(new ShowPopupComponent
            {
                Settings = new PopupToShow<DefaultPopup>(new DefaultPopup
                {
                    Title = Settings.Localization.Title,
                    Icon = Icon,
                    DropdownSettings = GetDropdownSettings<WashType>(),
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
                                _selectedWashType = defaultPopup.Dropdowns[0].GetCurrentValue<WashType>();

                                World.NewEntity().Replace(new ChangePetAnimationEvent(AnimationType.Swim));
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
            var washType = Settings.Localization.GetValueTypeContent(_selectedWashType);

            World.NewEntity().Replace(new ShowPopupComponent
            {
                Settings = new PopupToShow<ResultPopup>(new ResultPopup()
                {
                    Title = Settings.Localization.Title,
                    Content = string.Format(Settings.Localization.ResultContent, washType.ToLower()),
                    InfoParameterSettings = GetInfoParameterSettings(),
                    ButtonSettings = new List<TextButtonSettings>
                    {
                        new TextButtonSettings
                        {
                            Title = Settings.Localization.ResultButton,
                            Action = () =>
                            {
                                World.NewEntity().Replace(new HidePopupComponent());
                                World.NewEntity().Replace(new ChangePetAnimationEvent(default));
                            }
                        }
                    },
                    UseIcon = useIcon,
                    UsePetIcon = usePetIcon
                })
            });
        }


        private enum WashType : sbyte
        {
            Muzzle = 0,
            Body = 1,
            Paws = 2,
        }
    }
}