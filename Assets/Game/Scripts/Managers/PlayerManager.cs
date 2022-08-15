using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject player;
    public bool walk;
    public float speed;
    public Vector2Int target;
    private void Start()
    {

    }
    void Update()
    {

        if (walk)
        {
            Move(target);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Block"))
                {
                    if (hit.transform.GetComponent<BlockManager>().isMoved)
                    {
                        target = hit.transform.GetComponent<BlockManager>().MyPositionInGrid;
                        PlatformManager.Instance.DisableGridSelect();
                        walk = true;
                    }

                }
            }
        }
    }

    private void Move(Vector2Int target)
    {
        Vector3 position = PlatformManager.Instance.Grid[target.x, target.y].transform.position;
        position.y = player.transform.position.y;
        player.transform.position = Vector3.MoveTowards(player.transform.position, position, speed * Time.deltaTime);
        PlatformManager.Instance.m_isSelect = true;
        if (player.transform.position == position)
        {
            PlatformManager.Instance.m_myPos = target;
            PlatformManager.Instance.m_isSelect = false;
            walk = false;
        }
    }
}
