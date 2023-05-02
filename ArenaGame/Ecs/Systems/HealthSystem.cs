using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;


namespace ArenaGame.Ecs.Systems
{
    public class HealthSystem: ISystem
    {
        private readonly Entity HealthComponent;
        private float damageTaken = 0f;

        public HealthSystem(Entity healthComponenet) 
        { 
            HealthComponent = healthComponenet;
        }
        public void Update(GameTime gameTime)
        {
            float elapsedTime = gameTime.ElapsedGameTime.Seconds;

            float health = ((HealthComponent)HealthComponent.GetComponent<HealthComponent>()).Health;
            float currentHealth = ((HealthComponent)HealthComponent.GetComponent<HealthComponent>()).CurrentHealth;
            float regeneration = ((HealthComponent)HealthComponent.GetComponent<HealthComponent>()).Regeneration * elapsedTime;
            if (health < currentHealth+regeneration) { 
                currentHealth = health;
            }
            else
            {
                currentHealth += regeneration;
            }

            if (currentHealth > damageTaken)
            {
                currentHealth -= damageTaken;
                
            }
            else
            {
                currentHealth = 0;
            }

            damageTaken = 0;

            ((HealthComponent)HealthComponent.GetComponent<HealthComponent>()).CurrentHealth = currentHealth;
        }

        public void DealDamage(float  damage) 
        {
            damageTaken += damage;
        }
    }
}
