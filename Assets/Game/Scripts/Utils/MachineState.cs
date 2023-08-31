using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BatteOfHerone.Enuns;

namespace BatteOfHerone.Utils
{
    public static class MachineState
    {
        public static State CurrentState { get; private set; }
        public static PlayerState CurrentPlayer { get; private set; }

        public static void ChangeTurn()
        {
            if (CurrentPlayer == PlayerState.PlayerOne)
            {
                CurrentPlayer = PlayerState.PlayerTwo;
            }
            else
            {
                CurrentPlayer = PlayerState.PlayerOne;
            }
        }

        public static void EndTurn(Action<object> ActionEvent)
        {
            EventManager.StartListening(EventName.EndTurn, ActionEvent);
        }

        public static void InitialTurn(Action<object> ActionEvent)
        {
            EventManager.StartListening(EventName.InitialTurn, ActionEvent);
        }

    }
}
