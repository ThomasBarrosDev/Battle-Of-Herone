using BatteOfHerone.Block;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static BatteOfHerone.Managers.PlatformManager;

public static class PlatformActions
{
    public static List<BlockScript> SearchAdjacentes(BlockScript start, LineGrid[] Grid)
    {
        Vector2Int[] m_directions = new Vector2Int[8]
        {
           Vector2Int.up,
           Vector2Int.down,
           Vector2Int.right,
           Vector2Int.left,
           new Vector2Int(1, 1),
           new Vector2Int(-1, 1),
           new Vector2Int(-1, -1),
           new Vector2Int(1, -1)
         };

        return Search(start, 1, Grid, m_directions, true);
    }

    public static List<BlockScript> SearchListMoviment(BlockScript start, int qntMovement, LineGrid[] Grid)
    {
        Vector2Int[] m_directions = new Vector2Int[4]
      {
           Vector2Int.up,
           Vector2Int.down,
           Vector2Int.right,
           Vector2Int.left
       };

        return Search(start, qntMovement, Grid, m_directions, true);
    }
    private static void SwapReference(ref Queue<BlockScript> now, ref Queue<BlockScript> next)
    {
        Queue<BlockScript> temp = now;
        now = next;
        next = temp;
    }

    private static List<BlockScript> Search(BlockScript start, int qntMovement, LineGrid[] Grid, Vector2Int[] m_directions, bool Swap = false)
    {
        List<BlockScript> blocksSearch = new List<BlockScript>
            {
                start
            };

        Queue<BlockScript> checkNext = new Queue<BlockScript>();
        Queue<BlockScript> checkNow = new Queue<BlockScript>();

        start.Distance = 0;
        checkNow.Enqueue(start);

        while (checkNow.Count > 0)
        {
            BlockScript b = checkNow.Dequeue();
            for (int i = 0; i < m_directions.Length; i++)
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
            if (Swap)
            {
                if (checkNow.Count == 0)
                    SwapReference(ref checkNow, ref checkNext);
            }
        }
        return blocksSearch;
    }
}
