using System.Collections.Generic;
using _Game.Code.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Game.Code.Infrastructure.LifetimeScopes
{
    public class MenuLifetimeScope : LifetimeScope
    {
        [SerializeField] private List<LevelButton> _levelButtons;
        
        protected override void Configure(IContainerBuilder builder)
        {
            foreach (LevelButton levelButton in _levelButtons)
                builder.RegisterComponent(levelButton);
        }
    }
}