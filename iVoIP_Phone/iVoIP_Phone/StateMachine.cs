using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iVoIP_Phone
{
    public enum State
    {
        OnCall,
        ACW,
        Idle
    }

    public enum BigState
    {
        Ready,
        NotReady
    }

    class StateMachine
    {
        public delegate void StateChangeEvent(State newState);

        public event StateChangeEvent StateChanged;

        public State currentState { get; set; }
        public State previousState { get; set; }

        public StateMachine()
        {
            this.currentState = State.Idle;
        }

        public void ChangeState(State state, State previousState)
        {
            this.currentState = state;
            this.previousState = previousState;
            if (StateChanged != null) StateChanged(state);
        }
    }
}
