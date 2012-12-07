using System.Collections.Generic;
using EntityEngine.Engine;
using EntityEngine.Input;
using Asteroids.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    public sealed class GameState : EntityState
    {
        private Ship _ship;
        private EnemyShip _enemyship;
        private readonly KeyboardInput _resetkey = new KeyboardInput(Keys.F11);
        private SpriteFont _font;
        private bool _gameover;
        private string _gameovertext;

        public GameState(EntityGame eg)
            : base(eg)
        {
            Start();
        }

        public new void Start()
        {
            _ship = new Ship(GameRef.Game.Content.Load<Texture2D>("ship"), GameRef.Game.Content.Load<Texture2D>("bullet"), this);
            AddEntity(_ship);

            _enemyship = new EnemyShip(GameRef.Game.Content.Load<Texture2D>("enemyship"), GameRef.Game.Content.Load<Texture2D>("bullet"), this);
            AddEntity(_enemyship);

            _ship.Collision.AddPartner(_enemyship);
            _enemyship.Collision.AddPartner(_ship);

            for (var i = 0; i < 10; i++)
            {
                var a = new Asteroid(GameRef.Game.Content.Load<Texture2D>("asteroid"), this);

                while (a.Collision.TestCollision(_ship))
                {
                    a = new Asteroid(GameRef.Game.Content.Load<Texture2D>("asteroid"), this);
                }
                _ship.Collision.AddPartner(a);
                a.Collision.AddPartner(_ship);

                AddEntity(a);
            }

            _font = GameRef.Game.Content.Load<SpriteFont>("font");
        }

        public override void Reset()
        {
            base.Reset();
            _gameover = false;
            Start();
        }

        public override void Update()
        {
            base.Update();

            if (_resetkey.Pressed())
            {
                Reset();
            }

            if (_ship.Collision.Partners.Count == 0)
                GameOver(true);
            if (!_ship.Health.Alive)
                GameOver(false);
        }

        public void GameOver(bool won)
        {
            _gameover = true;
            _gameovertext = won ? "You Won! \nPress F11 to Restart" : "Game Over!\nPress F11 to Restart";
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            if (_gameover)
            {
                sb.DrawString(_font, _gameovertext, Vector2.One * 10, Color.White);
            }
        }
    }
}