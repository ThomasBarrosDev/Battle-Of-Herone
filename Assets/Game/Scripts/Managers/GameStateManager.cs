using BatteOfHerone.Enuns;
using BatteOfHerone.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace BatteOfHerone.Managers
{
    public class GameStateManager : SingletonBehaviour<GameStateManager>
    {
        [SerializeField] private TurnState currentTurnState = TurnState.Start;
        [SerializeField] private GameTurnState currentTurnState2 = GameTurnState.Waiting;
        [SerializeField] private PlayerState currentPlayerTurn = PlayerState.PlayerOne;

        public TurnAction StartTurnAction { get; set; }
        public TurnAction EndTurnAction { get; set; }

        public TurnState CurrentTurnState => currentTurnState;


        void Update()
        {
            if (currentTurnState == TurnState.Turn)
            {
                // Execute lógica durante o turno
            }
            else
            {
                // Pause ou outras ações fora do turno
            }
        }
        public void ChangePlayerTurn()
        {
            currentPlayerTurn = (currentPlayerTurn == PlayerState.PlayerOne) ? PlayerState.PlayerTwo : PlayerState.PlayerOne;
        }

        public void NextTurnState()
        {
            switch (currentTurnState)
            {
                case TurnState.Start:
                    StartTurn();
                    currentTurnState = TurnState.Turn;
                    break;
                case TurnState.Turn:
                    currentTurnState = TurnState.End;
                    break;
                case TurnState.End:
                    EndTurn();
                    ChangePlayerTurn();
                    currentTurnState = TurnState.Start;
                    break;
                default:
                    break;
            }
        }

        private void StartTurn()
        {
            PlayerManager playerManager = (currentPlayerTurn == PlayerState.PlayerOne) ? GameManager.Instance.PlayerOne : GameManager.Instance.PlayerTwo;
            // Execute ações gerais do início do turno
            StartTurnAction?.Invoke();
            playerManager.OnTurnStart();

            // Execute as ações das unidades no início do turno
            foreach (var unit in playerManager.Units)
            {
                unit.OnTurnStart?.Invoke();
            }
        }

        private void EndTurn()
        {
            PlayerManager playerManager = (currentPlayerTurn == PlayerState.PlayerOne) ? GameManager.Instance.PlayerOne : GameManager.Instance.PlayerTwo;

            // Execute ações gerais do fim do turno
            EndTurnAction?.Invoke();
            playerManager.OnTurnEnd();

            // Execute as ações das unidades no fim do turno
            foreach (var unit in playerManager.Units)
            {
                unit.OnTurnEnd?.Invoke();
            }
        }
    }
}
