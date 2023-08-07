using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BatteOfHerone.Enuns;
using BatteOfHerone.Managers;
using BatteOfHerone.Block;

namespace BatteOfHerone.Character
{
    public class CharacterScript : MonoBehaviour
    {
        private const float SPEED = 1.5f;

        [SerializeField] private PlayerEnum playerEnum;
        [SerializeField] private int movements;
        [SerializeField] private BlockScript m_positionBlock;
        [SerializeField] Animator m_animator;

        private bool m_isWalk;
        private bool m_isChangePosition;

        public bool isWalk { get => m_isWalk; set => m_isWalk = value; }
        public PlayerEnum PlayerEnum { get => playerEnum; set => playerEnum = value; }
        public bool IsChangePosition { get => m_isChangePosition; set => m_isChangePosition = value; }
        public BlockScript PositionBlock { get => m_positionBlock; set => m_positionBlock = value; }

        private void Start()
        {
            m_animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            m_animator.SetBool("walk", m_isWalk);

        }
        public void Moviment(BlockScript destiny)
        {
            StartCoroutine(MoveSequence(destiny));
        }

        //mover para uma classe state
        private IEnumerator MoveSequence(BlockScript destiny)
        {
            List<BlockScript> path = SearchPath(m_positionBlock, destiny);

            yield return StartCoroutine(Move(path));

            m_isWalk = false;

            yield return null;

            float t = 0;

            while (true)
            {
                yield return null;

                t += Time.deltaTime;

                if (t > 1)
                    break;

                if (playerEnum == PlayerEnum.PlayerOne)
                    LookForward();
                else
                    LookBackwards();

            }
        }

        public void SetPossibilities()
        {
            PlatformManager.Instance.ClearSearch();
            PlatformManager.Instance.SearchBlocksMovement(PositionBlock, movements);
        }


        public List<BlockScript> SearchPath(BlockScript pos, BlockScript destiny)
        {
            List<BlockScript> path = new();

            BlockScript b = destiny;

            while (b != pos)
            {
                path.Add(b);
                b = b.Prev;
            }

            path.Reverse();

            return path;
        }

        public IEnumerator Move(List<BlockScript> path)
        {
            for (int i = 0; i < path.Count; i++)
            {
                BlockScript b = path[i];
                yield return Walk(b);
            }
        }

        private IEnumerator Walk(BlockScript blockTarget)
        {
            m_isWalk = true;

            PositionBlock.IsFree = true;
            Vector3 ltarget = blockTarget.Movepos.position;

            while (true)
            {
                if (transform.position == ltarget)
                {
                    PositionBlock = blockTarget;
                    break;
                }

                Look(blockTarget.Position);

                transform.position = Vector3.MoveTowards(transform.position, ltarget, SPEED * Time.deltaTime);

                yield return null;
            }

            PositionBlock.SetInBlock(this);
        }

        private void Look(Vector2Int destiny)
        {
            Vector2Int temp = destiny - PositionBlock.Position;

            if (temp == new Vector2Int(0, 1))
            {
                LookUp();
                return;
            }
            if (temp == new Vector2Int(0, -1))
            {
                LookDown();
                return;
            }
            if (temp == new Vector2Int(1, 0))
            {
                LookForward();
                return;
            }
            if (temp == new Vector2Int(-1, 0))
            {
                LookBackwards();
                return;
            }
        }

        private void LookUp()
        {
            transform.GetChild(0).rotation = Quaternion.Slerp(transform.GetChild(0).rotation, new Quaternion(0, 0, 0, 3), 3f * Time.deltaTime);
        }

        private void LookDown()
        {
            transform.GetChild(0).rotation = Quaternion.Slerp(transform.GetChild(0).rotation, new Quaternion(0, 1, 0, 0), 3f * Time.deltaTime);
        }

        private void LookForward()
        {
            transform.GetChild(0).rotation = Quaternion.Slerp(transform.GetChild(0).rotation, new Quaternion(0, 1, 0, 1), 3f * Time.deltaTime);
        }

        private void LookBackwards()
        {
            transform.GetChild(0).rotation = Quaternion.Slerp(transform.GetChild(0).rotation, new Quaternion(0, -1, 0, 1), 3f * Time.deltaTime);
        }

    }
}