using System.Collections.Generic;
using UnityEngine;
using BatteOfHerone.Enuns;
using BatteOfHerone.Character;
using BatteOfHerone.Block;

namespace BatteOfHerone.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        public PlayerEnum playerEnum;

        public CharacterScript player;

        public BlockScript block;

        private bool IsActiveEffect;
        public LayerMask GroundLayerMask;
        public float selectionAreaSize = 1f;
        private List<BlockScript> blocks = new List<BlockScript>();
        private Vector3 selectionAreaCenter;

        public GameObject effect;
        public GameObject effectinstance;
        public bool effectInstantiated;
        private void Start()
        {
            
        }
        private void FixedUpdate()
        {
            if (effectInstantiated && player == null)
            {
                effectInstantiated = false;
                IsActiveEffect = false;

                effectinstance.transform.position = new Vector3(10000, effect.transform.position.y, 10000);
                Destroy(effectinstance);
                effectinstance = null;
            }

            if (IsActiveEffect)
            {
                if (!effectInstantiated && player != null)
                {
                    effectinstance = Instantiate(effect);
                    effectInstantiated = true;
                }
                
                // Lança um raio do mouse na direção da câmera
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // Faz o raycast e verifica se acertou alguma plataforma
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, GroundLayerMask))
                {
                    if (hit.transform.CompareTag("Block") )
                    {
                        BlockScript lBlock = hit.transform.GetComponent<BlockScript>();

                        effectinstance.transform.position = new Vector3(lBlock.Position.x, effect.transform.position.y, lBlock.Position.y);

                        if (lBlock.IsSelection)
                        {
                            PlatformManager.Instance.EnableEffect = true;
                        }
                        else
                        {
                            PlatformManager.Instance.EnableEffect = false;
                        }

                       
                    }
                    else if(hit.transform.CompareTag("Ground"))
                    {
                        effectinstance.transform.position = new Vector3(10000, effect.transform.position.y, 1000);
                    }

                }
                else
                    DeSelect();
            }
            else
                DeSelect();
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                IsActiveEffect = !IsActiveEffect;
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                Vector3 angle = effectinstance.transform.localEulerAngles;
                angle.y += 90;
                effectinstance.transform.localEulerAngles = angle;
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

                            player.Moviment(block);
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

                List<BlockScript> blocks = PlatformManager.Instance.SearchBlocksEffects();
            }

        }
        private void OnDrawGizmos()
        {
            // Desenha um wireframe do cubo representando a área de seleção enquanto estiver ativada
            if (IsActiveEffect)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(selectionAreaCenter, new Vector3(selectionAreaSize, selectionAreaSize, selectionAreaSize));
            }
        }

        private void DeSelect()
        {
           /* if (blocks.Count > 0)
            {
                blocks.ForEach(block => block.DeselectMat());
                blocks.Clear();
            }*/
        }
    }
}

/*using BatteOfHerone.Blocks;
using System.Collections.Generic;
using UnityEngine;

namespace BatteOfHerone.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        public LayerMask platformLayerMask;
        public float selectionAreaSize = 1f; // Tamanho da área de seleção (lado do cubo)
        private Vector3 selectionAreaCenter; // Centro da área de seleção
        private List<BlockScript> blocks = new List<BlockScript>();
        public bool IsActiveEffect;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) // Botão direito do mouse para ativar a área de seleção
            {
                IsActiveEffect = !IsActiveEffect;
            }

            if (IsActiveEffect)
            {
                // Atualiza o centro da área de seleção de acordo com a posição do mouse
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, platformLayerMask))
                {
                    selectionAreaCenter = hit.point;

                    DeSelect();

                    Collider[] colliders = Physics.OverlapBox(selectionAreaCenter, new Vector3(selectionAreaSize /, selectionAreaSize, selectionAreaSize / 2), Quaternion.identity, platformLayerMask);
                    Collider[] colliders2 = Physics.OverlapBox(selectionAreaCenter, new Vector3(selectionAreaSize, selectionAreaSize, selectionAreaSize), Quaternion.identity, platformLayerMask);

                    foreach (var collider in colliders)
                    {
                        BlockScript lBlock = collider.GetComponent<BlockScript>();
                        blocks.Add(lBlock);
                        lBlock.SelectMat();
                    }
                }
                else
                    DeSelect();
            }
            else
                DeSelect();



        }

        private void OnDrawGizmos()
        {
            // Desenha um wireframe do cubo representando a área de seleção enquanto estiver ativada
            if (IsActiveEffect)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(selectionAreaCenter, selectionAreaSize);
            }
        }

        private void DeSelect()
        {
            if (blocks.Count > 0)
            {
                blocks.ForEach(block => block.DeselectMat());
                blocks.Clear();
            }
        }

    }
}
*/