using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BatteOfHerone.Others;
using BatteOfHerone.Enuns;

namespace BatteOfHerone.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        public PlayerEnum playerEnum;
        public GameObject player;
        public bool walk;
        public float speed;
        public CharacterScript charar;
        private void Start()
        {

        }
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.CompareTag("Block"))
                    {
                        if (hit.transform.GetComponent<BlockScript>().IsMoved)
                        {
                            if (!player)
                                return;

                            player.GetComponent<CharacterScript>().Target = hit.transform.GetComponent<BlockScript>().MyPositionInGrid;

                            player.GetComponent<CharacterScript>().Walk = true;


                            PlatformManager.Instance.Adjacentpositions.Remove(hit.transform.GetComponent<BlockScript>().MyPositionInGrid);

                            PlatformManager.Instance.DisableGridSelect();
                        }
                    }
                    player = null;
                    PlatformManager.Instance.DisableGridSelect();
                    if (hit.transform.CompareTag("Character"))
                    {
                        if (playerEnum == hit.transform.gameObject.GetComponent<CharacterScript>().PlayerEnum)
                        {
                            player = hit.transform.gameObject;
                            if (!player.GetComponent<CharacterScript>().Walk)
                                player.GetComponent<CharacterScript>().SetPossibilities();
                        }
                        else
                        {
                            Debug.Log("personagem do player 2");
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