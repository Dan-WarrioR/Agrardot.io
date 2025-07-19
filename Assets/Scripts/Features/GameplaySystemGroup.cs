using Unity.Entities;

namespace Features
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class GameplaySystemGroup : ComponentSystemGroup 
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            Enabled = false;
        }
    }
}