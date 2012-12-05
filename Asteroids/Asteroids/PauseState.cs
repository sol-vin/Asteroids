using EntityEngine.Engine;

namespace Asteroids
{
    public class PauseState : EntityState
    {
        public PauseState(EntityGame eg)
            : base(eg)
        {
        }

        public override void Show()
        {
            base.Show();
            Reset();
        }
    }
}