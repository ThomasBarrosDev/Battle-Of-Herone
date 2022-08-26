using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using BatteOfHerone.Others;
using BatteOfHerone.Enuns;


namespace BatteOfHerone.Managers
{
    public class PlatformManager : SingletonBehaviour<PlatformManager>
    {
        [SerializeField] private int m_width;
        [SerializeField] private int m_height;
        [SerializeField] private AssetReference[] Blocks;
        [SerializeField] private Material originalMat;
        [SerializeField] private Material blankMat;

        private List<GameObject> childs = new();
        private GameObject[,] m_grid;

        public int movements;
        [SerializeField] private List<Vector2Int> adjacentpositions = new();
        private bool m_isSelect;

        public int Height { get => m_height; set => m_height = value; }
        public int Width { get => m_width; set => m_width = value; }
        public GameObject[,] Grid { get => m_grid; set => m_grid = value; }
        public Material BlankMat { get => blankMat; private set => blankMat = value; }
        public List<Vector2Int> Adjacentpositions { get => adjacentpositions; set => adjacentpositions = value; }
        public bool IsSelect { get => m_isSelect; set => m_isSelect = value; }

        //private readonly Dictionary<AssetReference, AsyncOperationHandle<GameObject>> loadOperations = new();
        //private readonly Dictionary<AssetReference, int> retainedSpawnQuantity = new();



        // Start is called before the first frame update
        private IEnumerator Start()
        {
            Grid = new GameObject[Height, Width];

            yield return new WaitUntil(() => CreatePlatform(Blocks[0]));
        }


        // Update is called once per frame
        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(ChangePlatform(Blocks[1]));
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                StartCoroutine(ChangePlatform(Blocks[2]));
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                StartCoroutine(ChangePlatform(Blocks[3]));
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                StartCoroutine(ChangePlatform(Blocks[4]));
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                StartCoroutine(ChangePlatform(Blocks[0]));
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                DisableGridSelect();
            }


        }
        public bool ChangePosition(Vector2Int oldPos, Vector2Int newPos)
        {
            try
            {
                ChangeMaterialOfBlock(newPos, BlankMat);
                OccupyingBlock(newPos.x, newPos.y);
                ChangeMaterialOfBlock(oldPos, originalMat);
                VacatingBlock(oldPos.x, oldPos.y);
                return true;
            }
            catch (System.Exception)
            {
                Debug.Log("nao exite essa pos: " + newPos.x + ", " + newPos.y);
                return false;
            }

        }
        private void OccupyingBlock(int x, int y)
        {
            Grid[x, y].GetComponent<BlockScript>().NotIsFree = true;
        }
        private void VacatingBlock(int x, int y)
        {
            Grid[x, y].GetComponent<BlockScript>().NotIsFree = false;
        }
        public void ChangeMaterialOfBlock(Vector2Int pos, Material mat)
        {
            Grid[pos.x, pos.y].GetComponentInChildren<MeshRenderer>().material = mat;
        }

        public bool CreatePlatform(AssetReference block)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    CreateBlock(block, new Vector2Int(i, j));
                }
            }
            return true;

        }
        public void CreateBlock(AssetReference block, Vector2Int pos)
        {
            AsyncOperationHandle<GameObject> operation = block.InstantiateAsync();
            GameObject instancia;
            operation.Completed += operation =>
            {

                float size = operation.Result.transform.localScale.x;
                instancia = operation.Result;
                instancia.transform.parent = transform;
                instancia.transform.localPosition = new Vector3(pos.x * size, 0, pos.y * size);
                instancia.SetActive(true);
                childs.Add(instancia);
                Grid[pos.x, pos.y] = instancia;
                instancia.GetComponent<BlockScript>().MyPositionInGrid = pos;
            };

        }
        public IEnumerator ChangePlatform(AssetReference newBlock)
        {
            foreach (var item in childs)
            {
                Destroy(item.gameObject);
            }
            childs.Clear();
            CreatePlatform(newBlock);
            yield return new WaitForEndOfFrame();
        }

        public void SetThePossibilitiesForMove(int movements, Vector2Int pos)
        {
            if (IsSelect)
                return;


            IsSelect = true;
            SelectPossibilities(GameObjectInGrid(PositionInVectorForMove(pos)));
            for (int i = 1; i < movements; i++)
            {
                List<Vector2Int> newList = new List<Vector2Int>(Adjacentpositions);

                foreach (var item in newList)
                {
                    SelectPossibilities(GameObjectInGrid(PositionInVectorForMove(item)));
                }
            }

        }
        public void SetThePossibilitiesForThrowCard(PlayerEnum player)
        {
            SelectPossibilities(GameObjectInGrid(PositionInVectorForThrowCard(player)));
        }
        public void DisableGridSelect()
        {
            foreach (var item in Adjacentpositions)
            {
                int x = item.x;
                int y = item.y;

                ChangeMaterialOfBlock(item, originalMat);
                Grid[x, y].GetComponent<BlockScript>().IsMoved = false;

            }
            Adjacentpositions.Clear();
            IsSelect = false;

        }
        private Vector2Int[] PositionInVectorForMove(Vector2Int PosInGrid)
        {
            Vector2Int[] pos = {
            PosInGrid + Vector2Int.up ,
            PosInGrid + Vector2Int.down,
            PosInGrid + Vector2Int.right,
            PosInGrid + Vector2Int.left,
            PosInGrid + new Vector2Int(1,1),
            PosInGrid + new Vector2Int(-1,-1),
            PosInGrid + new Vector2Int(1,-1),
            PosInGrid + new Vector2Int(-1,1),
            };


            return pos;
        }
        private Vector2Int[] PositionInVectorForThrowCard(PlayerEnum player)
        {
            Vector2Int[] pos = new Vector2Int[12];
            int k = 0;
            if (player == PlayerEnum.PlayerOne)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        pos[k] = new Vector2Int(i, j);
                        k++;
                    }
                }
            }
            if (player == PlayerEnum.PlayerTwo)
            {
                for (int i = 9; i > 6; i--)
                {
                    for (int j = 3; j > -1; j--)
                    {
                        pos[k] = new Vector2Int(i, j);
                        k++;
                    }
                }
            }

            return pos;
        }
        private void SelectPossibilities(List<GameObject> casas)
        {
            foreach (var item in casas)
            {
                if (item != null)
                {
                    ChangeMaterialOfBlock(item.GetComponent<BlockScript>().MyPositionInGrid, BlankMat);
                    item.GetComponent<BlockScript>().IsMoved = true;
                }
            }
        }
        private List<GameObject> GameObjectInGrid(Vector2Int[] pos)
        {
            List<GameObject> gridPossibility = new();
            int i = 0;
            
            foreach (var item in pos)
            {
                int x = item.x;
                int y = item.y;

                try
                {
                    gridPossibility.Add(Grid[x, y]);
                    AddInListAdjacent(item);

                    if (gridPossibility[i].GetComponent<BlockScript>().NotIsFree)
                    {
                        Adjacentpositions.Remove(item);
                        gridPossibility[i] = null;
                    }
                }
                catch (System.Exception)
                {
                    //Debug.Log("nao exite essa pos: " + x + ", " + y);
                }
                i++;
            }
            return gridPossibility;
        }
        public void AddInListAdjacent(Vector2Int item)
        {
            if (!Adjacentpositions.Contains(item))
            {
                Adjacentpositions.Add(item);
            }
        }

    }
}