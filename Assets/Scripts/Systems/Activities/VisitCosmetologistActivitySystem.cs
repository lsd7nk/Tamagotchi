using System.Collections.Generic;
using Modules.Navigation;
using Settings.Activity;
using Leopotam.Ecs;
using UI.Settings;
using Components;
using UI.Popups;
using Utils;

namespace Systems.Activities
{
    public sealed class VisitCosmetologistActivitySystem : ActivitySystem<PaidActivitySettings>
    {
        protected override NavigationElementType Type => NavigationElementType.VisitCosmetologistActivity;

        private EcsFilter<BankAccountComponent> _bankAccountFilter;

        protected override void StartActivity(bool isEnable)
        {
            World.NewEntity().Replace(new ShowPopupComponent
            {
                Settings = new PopupToShow<DefaultPopup>(new DefaultPopup
                {
                    Title = Settings.Localization.Title,
                    Icon = Icon,
                    Content = Settings.Localization.MainContent,
                    InfoFieldSettings = new List<InfoFieldSettings>
                    {
                        new InfoFieldSettings
                        {
                            Title = Settings.Price.ToMoneyString(),
                            IconState = true
                        }
                    },
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
                                if (!_bankAccountFilter.TrySpendMoney(Settings.Price))
                                    return;

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