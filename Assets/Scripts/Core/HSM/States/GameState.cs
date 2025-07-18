using Features;
using Unity.Entities;

namespace Core.HSM.States
{
    public class GameState : BaseState
    {
        protected override void OnActivate()
        {
            SwitchSystem<GameplaySystemGroup>(true);
        }

        protected override void OnDeactivate()
        {
            SwitchSystem<GameplaySystemGroup>(false);
        }
        
        private void SwitchSystem<T>(bool isEnabled) where T : ComponentSystemBase
        {
            var world = World.DefaultGameObjectInjectionWorld;

            if (world == null)
            {
                return;
            }
            
            world.GetExistingSystemManaged<T>().Enabled = isEnabled;
        }
    }
}