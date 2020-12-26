using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public class GameManager : MonoBehaviour
    {
        private bool _playerTurn; public bool _PlayerTurn
        {
            get => _playerTurn;
            private set {
                _playerTurn = value;
                // Player Turn
                if (value == true) {
                    SwitchTurn(true);
                    playerManager.SetPlayerTurn();
                }
                // AI Turn
                else {
                    SwitchTurn(false);
                    aIManager.SetAITurn();
                }
                inputManager.ResetAction();
            }
        }
        private void SwitchTurn(bool playerTurn)
        {
            inputManager.inputActive = playerTurn;
            aIManager.aIIsActive = !playerTurn;

            _playerInterface.gameObject.SetActive(playerTurn);
            _aIInterface.gameObject.SetActive(!playerTurn);
        }

        public InputManager inputManager;
        public PlayerManager playerManager; public Transform _playerInterface { get => playerManager.playerInterface; }
        public AIManager aIManager; public Transform _aIInterface { get => aIManager.aIInterface; }

        void Start()
        {
            _PlayerTurn = true;
        }

        public void ChangeTurn() => _PlayerTurn = !_PlayerTurn;
    } 
}
