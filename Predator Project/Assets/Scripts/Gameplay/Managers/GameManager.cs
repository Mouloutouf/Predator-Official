using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public class GameManager : MonoBehaviour
    {
        public InputManager inputManager;

        private bool iat;
        public bool isActiveTurn
        {
            get => iat;

            private set {
                iat = value;
                if (value == true)
                {
                    inputManager.inputActive = true;
                    aIManager.aIIsActive = false;

                    playerManager.SetPlayerTurn();

                    playerInterface.gameObject.SetActive(true);
                    aIInterface.gameObject.SetActive(false);
                }
                else
                {
                    inputManager.inputActive = false;
                    aIManager.aIIsActive = true;

                    aIManager.SetAITurn();

                    playerInterface.gameObject.SetActive(false);
                    aIInterface.gameObject.SetActive(true);
                }
                inputManager.ResetAction();
            }
        }

        public PlayerManager playerManager;
        public AIManager aIManager;

        public Transform playerInterface { get => playerManager.playerInterface; }
        public Transform aIInterface { get => aIManager.aIInterface; }

        void Start()
        {
            isActiveTurn = true;
        }

        public void ChangeTurn()
        {
            isActiveTurn = !isActiveTurn;
        }
    } 
}
