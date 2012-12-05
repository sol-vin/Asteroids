using EntityEngine.Engine;
using EntityEngine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    public class AsteroidGame : EntityGame
    {
        private readonly GameState _gs;
        private readonly PauseState _pausestate;
        private readonly KeyboardInput _switchkey = new KeyboardInput(Keys.Space);

        public AsteroidGame(Game game, GraphicsDeviceManager g, SpriteBatch sb)
            : base(game, g, new Rectangle(0, 0, 600, 600), sb)
        {
            _gs = new GameState(this);
            CurrentState = _gs;

            _pausestate = new PauseState(this);
        }

        public override void Update()
        {
            base.Update();

            if (CurrentState == _pausestate && _switchkey.Pressed())
            {
                _gs.Show();
            }
            else if (CurrentState == _gs && _switchkey.Pressed())
            {
                _pausestate.Show();
            }
        }

        public override void Draw()
        {
            base.Draw();
            if (CurrentState == _pausestate)
                _gs.Draw(SpriteBatch);
        }
    }
}