using Components.Modules.Navigation;
using System.Collections.Generic;
using Modules.Navigation;
using Leopotam.Ecs;
using System.Linq;
using Utils;

namespace Systems.Navigation
{
    public sealed class PetActionsScreenNavigationElements : IEcsInitSystem, INavigationElement
    {
        public HashSet<NavigationElementType> Types => new HashSet<NavigationElementType>
        {
            NavigationElementType.PetActionsScreen,
        };

        private EcsFilter<BlockComponent> _blockFilter;
        
        public void Init()
        {
            _blockFilter.RegisterElement(NavigationBlockType.Main, this);
        }

        public bool CanDisplay(NavigationElementType elementType)
        {
            return true;
        }

        public bool IsEnable(NavigationElementType elementType)
        {
            return true;
        }

        public bool NotificationIsEnable(NavigationElementType elementType)
        {
            var childPoints = _blockFilter.GetChildPointsOfType(NavigationBlockType.Main, elementType);
            var result = childPoints.Any(point => point.Element.NotificationIsEnable(point.Type));

            return result;
        }

        public bool OnClick(NavigationElementType elementType)
        {
            return true;
        }

        public NavigationButtonData GetButtonData(NavigationElementType elementType)
        {
            return _blockFilter.GetNavigationButtonData(NavigationBlockType.Main, elementType, this);
        }

        public NavigationScreenData GetScreenData(NavigationElementType elementType)
        {
            return _blockFilter.GetNavigationScreenData(NavigationBlockType.Main, elementType);
        }
    }
}