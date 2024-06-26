using System.Collections.Generic;
using Modules.Navigation;
using Settings.Activity;
using Leopotam.Ecs;
using UI.Settings;
using Components;
using UI.Popups;

namespace Systems.Activities
{
    public sealed class YogaActivitySystem : ActivitySystem<FreeActivitySettings>
    {
        protected override NavigationElementType Type => NavigationElementType.YogaActivity;

        protected override void StartActivity(bool isEnable)
        {
            World.NewEntity().Replace(new ShowPopupComponent
            {
                Settings = new PopupToShow<DefaultPopup>(new DefaultPopup
                {
                    Title = Settings.Localization.Title,
                    Icon = Icon,
                    Content = Settings.Localization.MainContent,
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
                            Action = () =>
                            {
                                EndActivity(true, false);
                            }
                        }
                    },
                    UseIcon = true
                })
            });
        }
    }
}