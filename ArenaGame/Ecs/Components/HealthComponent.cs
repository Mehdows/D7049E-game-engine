using Microsoft.Xna.Framework;

namespace ArenaGame.Ecs.Components
{
    internal class HealthComponent : IComponent
    {
        public float Health { get; set; }
        public float CurrentHealth { get; set; }
        public float Regeneration { get; set; }
        
        public HealthComponent(float health, float currentHealth, float regeneration)
        {
            Health = health;
            CurrentHealth = currentHealth;
            Regeneration = regeneration;
        }
        
        public HealthComponent() 
        {
            Health = 100;
            CurrentHealth = 100;
            Regeneration = 0;
        }
    }
}
