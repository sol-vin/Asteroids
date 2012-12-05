using EntityEngine.Engine;

namespace Asteroids.Objects
{
    public class AsteroidEntity : Entity
    {
        public AsteroidEntity(EntityState es)
            : base(es)
        {
        }

        public override void Update()
        {
            base.Update();
            const int buffer = 20;
            if (Body.Position.X < -buffer)
                Body.Position.X = StateRef.GameRef.Viewport.Right + buffer;
            else if (Body.Position.X > StateRef.GameRef.Viewport.Right + buffer)
                Body.Position.X = -buffer;

            if (Body.Position.Y < -buffer)
                Body.Position.Y = StateRef.GameRef.Viewport.Bottom + buffer;
            else if (Body.Position.Y > StateRef.GameRef.Viewport.Bottom + buffer)
                Body.Position.Y = -buffer;
        }
    }
}