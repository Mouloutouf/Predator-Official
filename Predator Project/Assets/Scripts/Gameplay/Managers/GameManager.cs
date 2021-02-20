using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Predator
{
    public class GameManager : MonoBehaviour
    {
        public InputManager inputManager;
        public PlayerManager playerManager; public Transform _playerInterface { get => playerManager.playerInterface; }
        public AIManager aIManager; public Transform _aIInterface { get => aIManager.aIInterface; }

        public Transform gameOverInterface;

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

        public void ChangeTurn() => _PlayerTurn = !_PlayerTurn;

        void Start()
        {
            _PlayerTurn = true;
        }

        public void GameOver()
        {
            gameOverInterface.gameObject.SetActive(true);

            inputManager.inputActive = false;
            aIManager.aIIsActive = false;
        }

        public void StartGame()
        {
            SceneManager.LoadScene(0);
        }

        public void CheckAtEachAction()
        {
            foreach (EnemyManager enemy in AIManager.enemies)
            {
                if (enemy.detectionBehavior != null) enemy.detectionBehavior.DetectionCheck();
            }
        }
    } 
}
