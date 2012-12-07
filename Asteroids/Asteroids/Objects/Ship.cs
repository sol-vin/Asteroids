using System;
using EntityEngine.Components;
using EntityEngine.Engine;
using EntityEngine.Input;
using EntityEngine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids.Objects
{
    public class Ship : AsteroidEntity
    {
        private readonly KeyboardInput _attackkey;
        private readonly KeyboardInput _upkey;
        private readonly KeyboardInput _leftkey;
        private readonly KeyboardInput _rightkey;
        private readonly KeyboardInput _downkey;
        private readonly KeyboardInput _debugkey;

        public Weapon Weapon { get; protected set; }

        public Emitter ThrustEmitter, DeathEmitter;

        public Ship(Texture2D shiptexture, Texture2D bullettexture, EntityState es)
            : base(es)
        {
            Body = new Body(this, new Vector2(es.GameRef.Viewport.Width / 2.0f, es.GameRef.Viewport.Height / 2.0f),
                            new Vector2(shiptexture.Width, shiptexture.Height));
            Components.Add(Body);

            Physics = new Physics(this) { Drag = 0.9f };
            Components.Add(Physics);

            Render = new Render(this, shiptexture) { Scale = .5f };
            Components.Add(Render);

            Weapon = new Gun(this, bullettexture);
            Components.Add(Weapon);

            Health = new Health(this, 5);
            Health.DiedEvent += Destroy;
            Health.DiedEvent += EmitEventHandler;
            Components.Add(Health);

            Collision = new Collision(this);
            Components.Add(Collision);

            ThrustEmitter = new ThrustEmitter(this, StateRef.GameRef.Game.Content.Load<Texture2D>(@"particles/thrustparticle"));
            Components.Add(ThrustEmitter);

            DeathEmitter = new DeathEmitter(this, StateRef.GameRef.Game.Content.Load<Texture2D>(@"particles/shipdeathparticle123"));
            Components.Add(DeathEmitter);

            

            _attackkey = new KeyboardInput(Keys.Enter);
            _upkey = new KeyboardInput(Keys.W);
            _downkey = new KeyboardInput(Keys.S);
            _leftkey = new KeyboardInput(Keys.A);
            _rightkey = new KeyboardInput(Keys.D);
            _debugkey = new KeyboardInput(Keys.L);
        }

        public override void Update()
        {
            base.Update();

            ControlShip();
        }

        public void EmitEventHandler(Entity e = null)
        {
            DeathEmitter.Emit(100);
        }

        virtual protected void ControlShip()
        {
            if (_upkey.Down())
            {
                Physics.Thrust(-.25f);
                ThrustEmitter.Emit(1);
            }
            else if (_downkey.Down())
            {
                Physics.Thrust(.13f);
            }

            if (_leftkey.Down())
                Body.Angle += -0.05f;
            if (_rightkey.Down())
                Body.Angle += 0.05f;

            if (_attackkey.RapidFire(150))
                Weapon.Fire();

            //TODO: Remove debug code!!!!
            if (_debugkey.Pressed())
            {
                EmitEventHandler();
            }
        }
    }

    internal class ThrustEmitter : Emitter
    {
        private readonly Random _rand = new Random(DateTime.Now.Millisecond);

        public ThrustEmitter(Entity e, Texture2D particletexture)
            : base(e, particletexture, Vector2.One * 5)
        {
        }

        protected override Particle GenerateNewParticle()
        {
            int index = _rand.Next(0, 3);
            int ttl = _rand.Next(10, 16);

            //Rotate the point based on the center of the sprite
            // p = unrotated point, o = rotation origin
            //p'x = cos(theta) * (px-ox) - sin(theta) * (py-oy) + ox
            //p'y = sin(theta) * (px-ox) + cos(theta) * (py-oy) + oy

            var origin = Entity.Body.Position;

            var unrotatedposition = new Vector2(
                Entity.Body.BoundingBox.X,
                Entity.Body.BoundingBox.Bottom - 10);
            var angle = Entity.Body.Angle;
            var position = new Vector2(
                (float)(Math.Cos(angle) * (unrotatedposition.X - origin.X) - Math.Sin(angle) * (unrotatedposition.Y - origin.Y) + origin.X),
                (float)(Math.Sin(angle) * (unrotatedposition.X - origin.X) + Math.Cos(angle) * (unrotatedposition.Y - origin.Y) + origin.Y)
            );

            var p = new Particle(index, position, ttl, this);

            var anglev = (float)_rand.NextDouble() - .5f;
            p.Physics.Thrust((float)_rand.NextDouble(), angle - anglev);
            return p;
        }
    }

    internal class DeathEmitter : Emitter
    {
        public DeathEmitter(Entity e, Texture2D texture)
            : base(e, texture, Vector2.One * 5)
        {
        }

        public override void Emit(int amount)
        {
            var random = new Random(DateTime.Now.Millisecond);
            var pislice = MathHelper.TwoPi / amount;
            for (var i = 0; i < amount; i++)
            {
                var index = random.Next(0, 3);

                var angle = pislice * i;
                var position = Entity.Body.Position;

                var p = new FadeParticle(index, position, 40, this)
                    {
                        Body = {Angle = angle - (float) random.NextDouble()},
                        Physics = {AngularVelocity = (float) random.NextDouble()*.5f - .1f},
                        TimeToLive = 200
                    };
                p.Physics.Drag = 0.99f;
                p.Physics.Thrust((float)random.NextDouble() * 2.0f + 1f);
                p.Render.Scale = (float)random.NextDouble() + .75f;
                Entity.AddEntity(p);
            }
        }
    }
}