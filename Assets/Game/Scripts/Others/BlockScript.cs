using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BatteOfHerone.Others
{
    class BlockScript : MonoBehaviour
    {
        [SerializeField] private bool m_isMoved;
        [SerializeField] private Vector2Int myPositionInGrid;
        [SerializeField] private bool m_notIsFree;
        public Vector2Int MyPositionInGrid { get => myPositionInGrid; set => myPositionInGrid = value; }
        public bool IsMoved { get => m_isMoved; set => m_isMoved = value; }
        public bool NotIsFree { get => m_notIsFree; set => m_notIsFree = value; }



        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Character"))
            {

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Character"))
            {

            }
        }
    }
}