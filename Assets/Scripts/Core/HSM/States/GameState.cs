using Features;
using Tools;

namespace Core.HSM.States
{
    public class GameState : BaseState
    {
        protected override void OnActivate()
        {
            DotsExtensions.SwitchSystem<GameplaySystemGroup>(true);                 
        }

        protected override void OnDeactivate()
        {
            DotsExtensions.SwitchSystem<GameplaySystemGroup>(false);
        }
    }
}