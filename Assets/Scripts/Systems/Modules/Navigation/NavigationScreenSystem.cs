using Components.Modules.Navigation;
using UI.Controller.Screen;
using Modules.Navigation;
using Leopotam.Ecs;
using UnityEngine;
using Modules;
using System;

namespace Systems.Modules.Navigation
{
    public sealed class NavigationScreenSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter<NavigationPointChangedEvent> _pointChangedFilter;

        private ScreenManager _screenManager;

        public void Init()
        {
            _world.NewEntity().Replace(new NavigationActivateBlockEvent
            {
                BlockType = NavigationBlockType.Main
            });
        }

        public void Run()
        {
            foreach (var i in _pointChangedFilter)
            {
                var comp = _pointChangedFilter.Get1(i);

                if (comp.CurrentPoint == null || comp.CurrentPoint.Element == null)
                    continue;

                var screenController = comp.TransitionType switch
                {
                    TransitionType.In => GetInTransitionScreen(comp.CurrentPoint.Type),
                    TransitionType.Out => GetOutTransitionScreen(comp.CurrentPoint.Type),
                    _ => null
                };

                if (screenController == null)
                    continue;

                _screenManager.ReplacePreviousScreen
                (
                    screenController,
                    GetShowDirection(screenController),
                    GetHideDirection(screenController)
                );
            }
        }

        private Type GetInTransitionScreen(NavigationElementType type)
        {
            return type switch
            {
                NavigationElementType.MainScreen => typeof(MainScreenController),
                NavigationElementType.MenuScreen => typeof(MenuScreenController),
                NavigationElementType.ActivitiesScreen => typeof(ActionsScreenController),
                NavigationElementType.JobScreen => typeof(JobScreenController),
                NavigationElementType.HappinessActivities => typeof(ActionsScreenController),
                NavigationElementType.SatietyActivities => typeof(ActionsScreenController),
                NavigationElementType.HygieneActivities => typeof(ActionsScreenController),
                NavigationElementType.HealthActivities => typeof(ActionsScreenController),
                NavigationElementType.TrainingActivities => typeof(ActionsScreenController),
                _ => null
            };
        }

        private Type GetOutTransitionScreen(NavigationElementType type)
        {
            return type switch
            {
                NavigationElementType.MainScreen => typeof(MainScreenController),
                NavigationElementType.MenuScreen => typeof(MenuScreenController),
                NavigationElementType.ActivitiesScreen => typeof(ActionsScreenController),
                NavigationElementType.JobScreen => typeof(JobScreenController),
                NavigationElementType.HappinessActivities => typeof(ActionsScreenController),
                NavigationElementType.SatietyActivities => typeof(ActionsScreenController),
                NavigationElementType.HygieneActivities => typeof(ActionsScreenController),
                NavigationElementType.HealthActivities => typeof(ActionsScreenController),
                NavigationElementType.TrainingActivities => typeof(ActionsScreenController),
                _ => null
            };
        }

        private Vector2 GetShowDirection(Type screenControllerType)
        {
            if (screenControllerType == typeof(JobScreenController))
                return Vector2.left;

            if (screenControllerType == typeof(MainScreenController))
            {
                if (_screenManager.CurrentScreen?.GetType() == typeof(ActionsScreenController))
                    return Vector2.left;
            }

            return Vector2.right;
        }

        private Vector2 GetHideDirection(Type screenControllerType)
        {
            if (screenControllerType == typeof(JobScreenController))
                return Vector2.right;

            return Vector2.left;
        }
    }
}