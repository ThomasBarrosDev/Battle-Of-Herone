using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BatteOfHerone.Enuns;
using BatteOfHerone.Managers;

namespace BatteOfHerone.Others
{
    public class CharacterScript : MonoBehaviour
    {
        private const float SPEED = 1.5f;

        [SerializeField]private PlayerEnum playerEnum;
        [SerializeField] private int movements;
        [SerializeField] private Vector2Int m_myPositionBlock;
        [SerializeField] private Material blank;
        private bool m_walk;
        private bool m_isChangePosition;
        private Vector2Int target;

        public Vector2Int Target { get => target; set => target = value; }
        public bool Walk { get => m_walk; set => m_walk = value; }
        public PlayerEnum PlayerEnum { get => playerEnum; set => playerEnum = value; }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Walk)
            {
                Move(Target);
            }
        }
        public void isChangePosition()
        {
            m_isChangePosition = true;
            PlatformManager.Instance.ChangePosition(m_myPositionBlock, Target);
        }

        public void Move(Vector2Int target)
        {
            if (!m_isChangePosition)
            {
                isChangePosition();
            }
            Vector3 position = PlatformManager.Instance.Grid[target.x, target.y].transform.position;
            position.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, position, SPEED * Time.deltaTime);
            PlatformManager.Instance.IsSelect = true;
            m_myPositionBlock = target;



            if (transform.position == position)
            {
                PlatformManager.Instance.IsSelect = false;
                Walk = false;
                m_isChangePosition = false;
            }
        }
        public void SetPossibilities()
        {
            PlatformManager.Instance.SetThePossibilitiesForMove(movements, m_myPositionBlock);
        }
    }
}