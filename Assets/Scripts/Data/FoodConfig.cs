using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Configs/Food Config")]
    public class FoodConfig : ScriptableObject
    {
        public float initialRadius = 1f;
        public float playerRadius = 2f;
    }
}