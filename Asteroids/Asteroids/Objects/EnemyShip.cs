using System;
using EntityEngine.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Objects
{
    public class EnemyShip : Ship
    {
        private float _turndirection = 1.0f;
        private readonly Random _rand;

        public EnemyShip(Texture2D shiptexture, Texture2D bullettexture, EntityState es)
            : base(shiptexture, bullettexture, es)
        {
            _rand = new Random(DateTime.Now.Millisecond);
            Body.Position = new Vector2(_rand.Next(40, es.GameRef.Viewport.Right - 40), _rand.Next(40, es.GameRef.Viewport.Bottom - 40));
        }

        protected override void ControlShip()
        {
            if (_rand.NextDouble() < 0.1f) _turndirection = -_turndirection;
            Body.Angle += _turndirection * 0.05f;
            Physics.Thrust(-(float)_rand.NextDouble() * .25f);
            ThrustEmitter.Emit(1);
            if (_rand.NextDouble() < 0.05) Weapon.Fire();
        }
    }
}