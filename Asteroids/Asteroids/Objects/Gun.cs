using EntityEngine.Engine;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Objects
{
    public class Gun : Weapon
    {
        public Texture2D BulletTexture { get; protected set; }

        public Gun(Entity e, Texture2D bullettexture)
            : base(e)
        {
            BulletTexture = bullettexture;
        }

        public override void Fire()
        {
            var b = new Bullet(BulletTexture, Entity.Body.Position, Entity.StateRef)
            {
                Body = { Position = Entity.Body.Position, Angle = Entity.Body.Angle },
            };

            foreach (var partner in Entity.Collision.Partners)
            {
                b.Collision.AddPartner(partner);
            }

            b.Physics.Thrust(-5);
            b.Physics.Drag = 1;
            Entity.AddEntity(b);
        }
    }
}