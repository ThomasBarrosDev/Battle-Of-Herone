using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using BatteOfHerone.Blocks;
using BatteOfHerone.Enuns;
using System;
using UnityEditor;
using BatteOfHerone.Utils;

namespace BatteOfHerone.Managers
{
    public class PlatformManager : SingletonBehaviour<PlatformManager>
    {
        [Serializable]
        public class Line
        {
            public BlockScript[] blocks;
            public Line()
            {
                blocks = new BlockScript[5];
            }
        }
        [Header("Grid")]
        [SerializeField] private BlockScript[] m_gridSet;

        private Line[] m_lines = new Line[10];

        [SerializeField] private int m_width;
        [SerializeField] private int m_height;
        [SerializeField] private AssetReference[] m_blocksReferences;

        private List<GameObject> childs = new();

        public int movements;
        [SerializeField] private List<Vector2Int> adjacentpositions = new();
        private bool m_isSelect;

        public int Height { get => m_height; set => m_height = value; }
        public int Width { get => m_width; set => m_width = value; }
        public Line[] Grid { get => m_lines; set => m_lines = value; }
        public List<Vector2Int> Adjacentpositions { get => adjacentpositions; set => adjacentpositions = value; }
        public bool IsSelect { get => m_isSelect; set => m_isSelect = value; }

        public List<BlockScript> BlocksList { get; set; } = new List<BlockScript>();

        public BlockScript DestinoFinal;

        private Vector2Int[] m_directions = new Vector2Int[4]
        {
           Vector2Int.up,
           Vector2Int.down,
           Vector2Int.right,
           Vector2Int.left,

        };

        //private readonly Dictionary<AssetReference, AsyncOperationHandle<GameObject>> loadOperations = new();
        //private readonly Dictionary<AssetReference, int> retainedSpawnQuantity = new();



        // Start is called before the first frame update
        private IEnumerator Start()
        {
            SetPlatform();
            yield return new WaitForEndOfFrame();

            GameManager.Instance.InstantiateMonster(GameManager.Instance.m_monstersPrefabs[1], Grid[0].blocks[2], PlayerEnum.PlayerOne);
            GameManager.Instance.InstantiateMonster(GameManager.Instance.m_monstersPrefabs[1], Grid[2].blocks[2], PlayerEnum.PlayerOne);
        }

        private void SetPlatform()
        {
            int n = 0;
            for (int i = 0; i < 10; i++)
            {
                Grid[i] = new Line();
                for (int j = 0; j < 5; j++)
                {
                    Grid[i].blocks[j] = m_gridSet[n];
                    Grid[i].blocks[j].Position = new Vector2Int(i, j);
                    n++;
                }
            }
        }

        // Update is called once per frame
        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(ChangePlatform(m_blocksReferences[1]));
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                StartCoroutine(ChangePlatform(m_blocksReferences[2]));
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                StartCoroutine(ChangePlatform(m_blocksReferences[3]));
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                StartCoroutine(ChangePlatform(m_blocksReferences[4]));
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                StartCoroutine(ChangePlatform(m_blocksReferences[0]));
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
                Grid[newPos.x].blocks[newPos.y].DestinationBlock();
                Grid[oldPos.x].blocks[oldPos.y].DeSelectBlock();
                return true;
            }
            catch (System.Exception)
            {
                Debug.Log("nao exite essa pos: " + newPos.x + ", " + newPos.y);
                return false;
            }

        }
        public void ChangeMaterialOfBlock(Vector2Int pos, Material mat)
        {
            Grid[pos.x].blocks[pos.y].SelectBlock();
            Grid[pos.x].blocks[pos.y].DeSelectBlock();
            Grid[pos.x].blocks[pos.y].DestinationBlock();
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
                Grid[pos.x].blocks[pos.y] = instancia.GetComponent<BlockScript>();
                instancia.GetComponent<BlockScript>().Position = pos;
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

        public void ClearSearch()
        {
            foreach (BlockScript b in BlocksList)
            {
                b.prev = null;
                b.distance = float.MaxValue;
                b.DeSelectBlock();
            }
            BlocksList = new();
        }
        public void Search(BlockScript myPosition, int qntMovement)
        {
            BlocksList = SearchList(myPosition, qntMovement);
            foreach (var item in BlocksList)
            {
                if (item.IsFree)
                {
                    item.SelectBlock();
                }
            }
        }

        private List<BlockScript> SearchList(BlockScript start, int qntMovement)
        {
            List<BlockScript> blocksSearch = new List<BlockScript>();

            blocksSearch.Add(start);
            ClearSearch();

            Queue<BlockScript> checkNext = new Queue<BlockScript>();
            Queue<BlockScript> checkNow = new Queue<BlockScript>();

            start.distance = 0;
            checkNow.Enqueue(start);

            while (checkNow.Count > 0)
            {
                BlockScript b = checkNow.Dequeue();
                for (int i = 0; i < 4; i++)
                {
                    Vector2Int temp = b.Position + m_directions[i];
                    try
                    {

                        BlockScript next = Grid[temp.x].blocks[temp.y];

                        if (next.distance <= b.distance + 1 || b.distance + 1 > qntMovement || next == null)
                            continue;


                        next.distance = b.distance + 1;
                        next.prev = b;
                        checkNext.Enqueue(next);
                        blocksSearch.Add(next);
                    }
                    catch (Exception)
                    {
                        Debug.Log(temp + " : não tem essa posição");
                        continue;
                    }

                }

                if (checkNow.Count == 0)
                    SwapReference(ref checkNow, ref checkNext);
            }
            return blocksSearch;
        }

        private void SwapReference(ref Queue<BlockScript> now, ref Queue<BlockScript> next)
        {
            Queue<BlockScript> temp = now;
            now = next;
            next = temp;
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

                Grid[item.x].blocks[item.y].DeSelectBlock();

                Grid[x].blocks[y].GetComponent<BlockScript>().IsSelection = false;

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
        private void SelectPossibilities(List<BlockScript> casas)
        {
            foreach (var item in casas)
            {
                if (item != null)
                {
                    if (item.IsFree)
                    {
                        Grid[item.Position.x].blocks[item.Position.y].DeSelectBlock();
                        item.IsSelection = true;
                    }
                }
            }
        }
        private List<BlockScript> GameObjectInGrid(Vector2Int[] pos)
        {
            List<BlockScript> gridPossibility = new();
            int i = 0;

            foreach (var item in pos)
            {
                int x = item.x;
                int y = item.y;

                try
                {
                    gridPossibility.Add(Grid[x].blocks[y]);
                    AddInListAdjacent(item);
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