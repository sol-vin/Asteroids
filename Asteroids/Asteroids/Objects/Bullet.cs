using System;
using EntityEngine.Components;
using EntityEngine.Engine;
using EntityEngine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Objects
{
    public class Bullet : AsteroidEntity
    {
        public int Age { get; private set; }

        public Emitter Emitter;

        public Bullet(Texture2D bullettexture, Vector2 position, EntityState es)
            : base(es)
        {
            Body = new Body(this, position, new Vector2(bullettexture.Width, bullettexture.Height));
            Components.Add(Body);

            Physics = new Physics(this);
            Components.Add(Physics);

            Render = new Render(this, bullettexture);
            Components.Add(Render);

            Emitter = new HitEmitter(this, StateRef.GameRef.Game.Content.Load<Texture2D>(@"particles/hitparticle"));
            Components.Add(Emitter);
        }

        public override void Update()
        {
            base.Update();

            foreach (var entity in Targets)
            {
                if (!Body.TestCollision(entity)) continue;
                entity.Health.Hurt(1);
                Emitter.Emit(10);
                Destroy();
                return;
            }

            Age++;
            if (Age > 30) Render.Alpha -= .1f;
            if (Age > 45) Destroy();
        }
    }

    internal class HitEmitter : Emitter
    {
        private readonly Random _rand = new Random(DateTime.Now.Millisecond);

        public HitEmitter(Entity e, Texture2D particletexture)
            : base(e, particletexture, Vector2.One * 3)
        {
        }

        protected override Particle GenerateNewParticle()
        {
            int index = _rand.Next(0, 2);

            Particle p = new FadeParticle(index, Entity.Body.Position, 20, this);
            p.TimeToLive = 40;
            p.Body.Angle = Entity.Body.Angle + (float)_rand.NextDouble() * _rand.Next(-1, 2);
            p.Physics.Thrust((float)_rand.NextDouble() * 2);
            p.Render.Scale = (float)_rand.NextDouble() + 1f;
            return p;
        }
    }
}