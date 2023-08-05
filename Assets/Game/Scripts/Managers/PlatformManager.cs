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

            public LineGrid(int x)
            {
                blocks = new BlockScript[x];
            }
        }
        public int Platform_X;
        public int Platform_Y;
        public BlockScript block;

        [Header("Grid")]
        [SerializeField] private BlockScript[] m_gridSet;
        [SerializeField] private AssetReference[] m_blocksReferences;
        [SerializeField] private List<BlockScript> m_blockSelecitonEffectList = new();

        [SerializeField] private LineGrid[] m_lines;

        public LineGrid[] Grid { get => m_lines; set => m_lines = value; }

        public bool EnableEffect { get; set; }

        public List<BlockScript> BlocksList { get; set; } = new();
        public List<BlockScript> BlockSelecitonEffectList { get => m_blockSelecitonEffectList; set => m_blockSelecitonEffectList = value; }

        private Vector2Int[] m_directions = new Vector2Int[4]
        {
           Vector2Int.up,
           Vector2Int.down,
           Vector2Int.right,
           Vector2Int.left,

        };

        private IEnumerator Start()
        {
            Grid = new LineGrid[Platform_X];
            InstancePlatform();
            yield return new WaitForEndOfFrame();

            GameManager.Instance.InstantiateMonster(GameManager.Instance.m_monstersPrefabs[1], Grid[0].blocks[2], PlayerEnum.PlayerOne);
            GameManager.Instance.InstantiateMonster(GameManager.Instance.m_monstersPrefabs[1], Grid[2].blocks[2], PlayerEnum.PlayerOne);
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

        public void SearchBlocks(BlockScript myPosition, int qntMovement)
        {
            BlocksList = SearchList(myPosition, qntMovement);
            foreach (var item in BlocksList) { item.SelectBlock(); }
        }

        public void SearchBlocksMovement(BlockScript myPosition, int qntMovement)
        {
            BlocksList = SearchList(myPosition, qntMovement);
            foreach (var item in BlocksList) { item.SelectBlockMovement(); }
        }

        public List<BlockScript> SearchBlocksEffects()
        {
            List<BlockScript> blocksEffects = new();

            for (int i = 0; i < Grid.Length; i++)
            {
                for (int j = 0; j < Grid[i].blocks.Length; j++)
                {
                    if (!Grid[i].blocks[j].IsSelectionEffect)
                        continue;

                    blocksEffects.Add(Grid[i].blocks[j]);
                }
            }
            return blocksEffects;
        }

        private List<BlockScript> SearchList(BlockScript start, int qntMovement)
        {
            List<BlockScript> blocksSearch = new List<BlockScript>
            {
                start
            };

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
                        //Debug.Log(temp + " : não tem essa posição");
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

        private void InstancePlatform()
        {
            GameObject lPlatform = new GameObject("Platform");
            int n = 0;
            for (int i = 0; i < Platform_X; i++)
            {
                Grid[i] = new LineGrid(Platform_Y);
                for (int j = 0; j < Platform_Y; j++)
                {
                    Grid[i].blocks[j] = Instantiate(block, lPlatform.transform);
                    Vector2Int XZ = new Vector2Int(i, j);
                    Grid[i].blocks[j].transform.localPosition = new Vector3(XZ.x, 0, XZ.y);
                    Grid[i].blocks[j].Position = XZ;
                    n++;
                }
            }
        }
    }
}