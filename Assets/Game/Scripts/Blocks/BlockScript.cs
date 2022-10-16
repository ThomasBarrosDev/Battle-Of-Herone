using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BatteOfHerone.Managers;

namespace BatteOfHerone.Blocks
{
    public class BlockScript : MonoBehaviour
    {
        //setar pos que o player vai andar
        [SerializeField] private Transform m_movePos;

        [SerializeField] private Material m_originalMat;
        [SerializeField] private Material m_selectMat;
        [SerializeField] private Material m_originMat;


        private bool m_IsFree;
        private Vector2Int position;
        private bool m_isSelection;
        private BlockScript m_prev;
        private float m_distance = float.MaxValue;


        public Vector2Int Position { get => position; set => position = value; }
        public bool IsSelection { get => IsSelection1; set => IsSelection1 = value; }
        public bool IsFree { get => m_IsFree; set => m_IsFree = value; }
        public Transform Movepos { get => m_movePos; set => m_movePos = value; }
        public BlockScript Prev { get => m_prev; set => m_prev = value; }
        public float Distance { get => m_distance; set => m_distance = value; }
        public bool IsSelection1 { get => m_isSelection; set => m_isSelection = value; }

        private void Start()
        {
            IsFree = true;
            IsSelection1 = false;
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
            GetComponentInChildren<MeshRenderer>().material = m_originMat;
        }
    }
}