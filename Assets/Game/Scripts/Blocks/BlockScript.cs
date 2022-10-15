using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BatteOfHerone.Managers;

namespace BatteOfHerone.Blocks
{
    public class BlockScript : MonoBehaviour
    {

        public BlockScript prev;
        public float distance = float.MaxValue;

        public bool m_isSelection;
        [SerializeField] private Transform m_movePos;
        [SerializeField] private Vector2Int position;
        [SerializeField] private bool m_IsFree;

        [SerializeField] private Material m_originalMat;
        [SerializeField] private Material m_selectMat;
        [SerializeField] private Material m_blankMat;

        public Vector2Int Position { get => position; set => position = value; }
        public bool IsSelection { get => m_isSelection; set => m_isSelection = value; }
        public bool IsFree { get => m_IsFree; set => m_IsFree = value; }
        public Transform Movepos { get => m_movePos; set => m_movePos = value; }

        private void Start()
        {
            IsFree = true;
            m_isSelection = false;
        }
        public void SelectBlock()
        {
            if (IsFree)
            {
                GetComponentInChildren<MeshRenderer>().material = m_selectMat;
                IsSelection = true;
            }
            else
            {
                Debug.Log("este bloco não está livre");
            }
        }
        public void DeSelectBlock()
        {
            GetComponentInChildren<MeshRenderer>().material = m_originalMat;
            IsSelection = false;
        }
        public void DestinationBlock()
        {
            GetComponentInChildren<MeshRenderer>().material = m_blankMat;
        }


        private void OnTriggerEnter(Collider other)
        {

        }
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Character"))
            {
                m_IsFree = false;
                //Debug.Log(myPositionInGrid.x + "," + myPositionInGrid.y + " Tem gente");

            }
        }

        private void OnCollisionStay(Collision collision)
        {

        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Character"))
            {
                m_IsFree = true;

            }
        }
    }
}