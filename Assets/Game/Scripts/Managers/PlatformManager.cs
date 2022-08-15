using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PlatformManager : SingletonBehaviour<PlatformManager>
{
    [SerializeField] private int m_width;
    [SerializeField] private int m_height;
    [SerializeField] private AssetReference[] Blocks;
    [SerializeField] private Material originalMat;
    [SerializeField] private Material blankMat;


    //teste apenas
    public Vector2Int m_myPos;
    public Vector2Int m_oldPos;
    public GameObject play;
    public Transform plat;
    public bool walk;
    public bool m_isSelect;

    private List<GameObject> childs = new();
    private GameObject[,] m_grid;

    public int movements;
    [SerializeField] private List<Vector2Int> adjacentpositions = new();

    public int Height { get => m_height; set => m_height = value; }
    public int Width { get => m_width; set => m_width = value; }
    public GameObject[,] Grid { get => m_grid; set => m_grid = value; }

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

        if (Input.GetKeyDown(KeyCode.G))
        {
            ChangePosition(m_oldPos, m_myPos);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            m_myPos += Vector2Int.right;
            if (!ChangePosition(m_oldPos, m_myPos))
            {
                m_myPos -= Vector2Int.right;
                ChangePosition(m_oldPos, m_myPos);
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            m_myPos += Vector2Int.left;
            if (!ChangePosition(m_oldPos, m_myPos))
            {
                m_myPos -= Vector2Int.left;
                ChangePosition(m_oldPos, m_myPos);
            }

        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            m_myPos += Vector2Int.up;
            if (!ChangePosition(m_oldPos, m_myPos))
            {
                m_myPos -= Vector2Int.up;
                ChangePosition(m_oldPos, m_myPos);
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {

            m_myPos += Vector2Int.down;
            if (!ChangePosition(m_oldPos, m_myPos))
            {
                m_myPos -= Vector2Int.down;
                ChangePosition(m_oldPos, m_myPos);
            }
        }

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

        if (walk)
            play.transform.position =
                Vector3.MoveTowards(play.transform.position, new Vector3(Grid[m_myPos.y, m_myPos.x].transform.position.x,
                play.transform.position.y, Grid[m_myPos.y, m_myPos.x].transform.position.z),
                3 * Time.deltaTime);
        m_oldPos = m_myPos;

        if (Input.GetKeyDown(KeyCode.J))
        {
            SetThePossibilities(movements, m_myPos);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            DisableGridSelect();
        }


    }
    private bool ChangePosition(Vector2Int oldPos, Vector2Int newPos)
    {
        try
        {
            Grid[oldPos.x, oldPos.y].GetComponentInChildren<MeshRenderer>().material = originalMat;
            Grid[newPos.x, newPos.y].GetComponentInChildren<MeshRenderer>().material = blankMat;
            return true;
        }
        catch (System.Exception)
        {
            Debug.Log("nao exite essa pos: " + newPos.x + ", " + newPos.y);
            return false;
        }

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
            instancia.GetComponent<BlockManager>().MyPositionInGrid = pos;
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

    public void SetThePossibilities(int movements, Vector2Int pos)
    {
        if (m_isSelect)
            return;


        m_isSelect = true;
        AddInListAdjacent(pos);
        SelectPossibilities(GameObjectInGrid(PositionInVector(pos)));
        for (int i = 1; i < movements; i++)
        {
            List<Vector2Int> newList = new List<Vector2Int>(adjacentpositions);

            foreach (var item in newList)
            {
                SelectPossibilities(GameObjectInGrid(PositionInVector(item)));
            }
        }

    }
    public void DisableGridSelect()
    {
        foreach (var item in adjacentpositions)
        {
            int x = item.x;
            int y = item.y;

            Grid[x, y].GetComponentInChildren<MeshRenderer>().material = originalMat;
            Grid[x, y].GetComponent<BlockManager>().isMoved = false;

        }
        adjacentpositions.Clear();
        m_isSelect = false;

    }
    public Vector2Int[] PositionInVector(Vector2Int PosInGrid)
    {
        Vector2Int[] pos = {
            PosInGrid + Vector2Int.up ,
            PosInGrid + Vector2Int.down,
            PosInGrid + Vector2Int.right,
            PosInGrid + Vector2Int.left
        };


        return pos;
    }
    public void SelectPossibilities(GameObject[] casas)
    {
        foreach (var item in casas)
        {
            if (item != null)
            {
                item.GetComponentInChildren<MeshRenderer>().material = blankMat;
                item.GetComponent<BlockManager>().isMoved = true;
            }
        }
    }
    public GameObject[] GameObjectInGrid(Vector2Int[] pos)
    {
        GameObject[] gridPossibility = new GameObject[4];
        int i = 0;
        foreach (var item in pos)
        {
            int x = item.x;
            int y = item.y;

            try
            {
                gridPossibility[i] = Grid[x, y];
                AddInListAdjacent(item);
            }
            catch (System.Exception)
            {
                Debug.Log("nao exite essa pos: " + x + ", " + y);
            }
            i++;
        }
        return gridPossibility;
    }
    public void AddInListAdjacent(Vector2Int item)
    {
        if (!adjacentpositions.Contains(item))
        {
            adjacentpositions.Add(item);
        }
    }

}
