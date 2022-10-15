using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BatteOfHerone.Blocks;
using BatteOfHerone.Enuns;
using BatteOfHerone.Utils;
using BatteOfHerone.Character;

namespace BatteOfHerone.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        public PlayerEnum playerEnum;
        
        public CharacterScript player;
        
        public BlockScript block;
        
        private void teste()
        {
            Debug.Log("Event Iniciado");
        }

        void Update()
        {

            if (Input.GetKeyDown(KeyCode.A))
            {
                EventManager.StartListening(EventName.InitialTurn, teste, true);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                EventManager.TriggerEvent(EventName.InitialTurn);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                EventManager.StopListening(EventName.InitialTurn, teste);
            }



            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.CompareTag("Block"))
                    {
                        block = hit.transform.GetComponent<BlockScript>();

                        if (block.IsSelection)
                        {
                            if (!player)
                            {
                                PlatformManager.Instance.ClearSearch();
                                return;
                            }

                            player.Mover(block);                              
                        }
                    }

                    player = null;

                    PlatformManager.Instance.ClearSearch(); 

                    if (hit.transform.CompareTag("Character"))
                    {
                        player = hit.transform.gameObject.GetComponent<CharacterScript>();

                        if (playerEnum == player.PlayerEnum)
                        {
                            if (!player.IsChangePosition && !player.isWalk)
                                player.SetPossibilities();
                        }
                        else
                        {
                            Debug.Log("personagem do outro player");
                        }
                    }

                }
                else
                {
                    PlatformManager.Instance.DisableGridSelect();
                }
            }



            if (Input.GetKeyDown(KeyCode.A))
            {
                PlatformManager.Instance.SetThePossibilitiesForThrowCard(playerEnum);
            }
        }

        /* private void Move(Vector2Int target)
         {
             Vector3 position = PlatformManager.Instance.Grid[target.x, target.y].transform.position;
             position.y = player.transform.position.y;
             player.transform.position = Vector3.MoveTowards(player.transform.position, position, speed * Time.deltaTime);
             PlatformManager.Instance.m_isSelect = true;
             if (player.transform.position == position)
             {
                 PlatformManager.Instance.m_myPos = target;
                 PlatformManager.Instance.m_isSelect = false;
                 player.GetComponent<CharacterScript>().walk = false;
                 walk = false;
             }
         }*/
    }
}