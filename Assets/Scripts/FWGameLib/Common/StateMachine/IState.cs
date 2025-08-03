namespace FWGameLib.Common.StateMachine
{
    public interface IState
    {
        void Tick(float deltaTime) { }
        void FixedTick() { }
        void OnEnter() { }
        void OnExit() { }
    }
}