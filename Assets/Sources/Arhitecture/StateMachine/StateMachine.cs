using System;

namespace Sources.StateMachines
{
    public interface IReadonlyStateMachine<TState> where TState : Enum
    {
        public Action<TState, TState> StateChanged { get; set; }
        public TState CurrentState { get; }
    }

    public class StateMachine<TState> : IReadonlyStateMachine<TState> where TState : Enum
    {
        public Action<TState, TState> StateChanged;

        private TState _currentState;

        public StateMachine(TState state)
        {
            _currentState = state;

            StateChanged?.Invoke(state, state);
        }

        Action<TState, TState> IReadonlyStateMachine<TState>.StateChanged { get => StateChanged; set => StateChanged = value; }
        public TState CurrentState => _currentState;

        /// <summary>
        /// Set state and invoke events.
        /// </summary>
        /// <param name="state"></param>
        public void SetState(TState state)
        {
            StateChanged?.Invoke(state, _currentState);

            _currentState = state;
        }
    }
}
