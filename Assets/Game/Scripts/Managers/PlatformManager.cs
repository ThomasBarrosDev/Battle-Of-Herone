using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using BatteOfHerone.Blocks;
using BatteOfHerone.Enuns;
using System;
using BatteOfHerone.Utils;

namespace BatteOfHerone.Managers
{
    public class PlatformManager : SingletonBehaviour<PlatformManager>
    {
        [Serializable]
        public class LineGrid
        {
            public BlockScript[] blocks;
            public LineGrid()
            {
                blocks = new BlockScript[5];
            }
        }
        [Header("Grid")]
        [SerializeField] private BlockScript[] m_gridSet;
        [SerializeField] private AssetReference[] m_blocksReferences;

        private LineGrid[] m_lines = new LineGrid[10];

        public LineGrid[] Grid { get => m_lines; set => m_lines = value; }
      
        public List<BlockScript> BlocksList { get; set; } = new List<BlockScript>();


        private Vector2Int[] m_directions = new Vector2Int[4]
        {
           Vector2Int.up,
           Vector2Int.down,
           Vector2Int.right,
           Vector2Int.left,

        };

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
                Grid[i] = new LineGrid();
                for (int j = 0; j < 5; j++)
                {
                    Grid[i].blocks[j] = m_gridSet[n];
                    Grid[i].blocks[j].Position = new Vector2Int(i, j);
                    n++;
                }
            }
        }
        

        public void ClearSearch()
        {
            foreach (BlockScript b in BlocksList)
            {
                b.Prev = null;
                b.Distance = float.MaxValue;
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

            start.Distance = 0;
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

                        if (next.Distance <= b.Distance + 1 || b.Distance + 1 > qntMovement || next == null)
                            continue;


                        next.Distance = b.Distance + 1;
                        next.Prev = b;
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
    }
}