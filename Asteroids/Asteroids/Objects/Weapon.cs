using EntityEngine.Components;
using EntityEngine.Engine;

namespace Asteroids.Objects
{
    public class Weapon : Component
    {
        public Weapon(Entity e)
            : base(e)
        { }

        public virtual void Fire()
        {
        }
    }
}