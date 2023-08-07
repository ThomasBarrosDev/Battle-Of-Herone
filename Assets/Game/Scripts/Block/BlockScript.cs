using UnityEngine;
using BatteOfHerone.Managers;
using BatteOfHerone.Character;

namespace BatteOfHerone.Block
{
    public class BlockScript : MonoBehaviour
    {
        //setar pos que o player vai andar
        [SerializeField] private Transform m_movePos;
        [SerializeField] private Material m_originalMat;
        [SerializeField] private Material m_selectMat;
        [SerializeField] private Material m_enableMat;
        [SerializeField] private Material m_disableMat;
        [SerializeField] private CharacterScript m_characterScript;
        [SerializeField] private bool m_IsFree;
        [SerializeField] private Vector2Int position;
        [SerializeField] private bool m_isSelection;
        [SerializeField] private bool m_isSelectionEffect;
        [SerializeField] private BlockScript m_prev;
        [SerializeField] private float m_distance = float.MaxValue;


        public Vector2Int Position { get => position; set => position = value; }
        public bool IsSelection { get => m_isSelection; set => m_isSelection = value; }
        public bool IsFree { get => m_IsFree; set => m_IsFree = value; }
        public Transform Movepos { get => m_movePos; set => m_movePos = value; }
        public BlockScript Prev { get => m_prev; set => m_prev = value; }
        public float Distance { get => m_distance; set => m_distance = value; }
        public CharacterScript CharacterScript { get => m_characterScript; set => m_characterScript = value; }
        public bool IsSelectionEffect { get => m_isSelectionEffect; set => m_isSelectionEffect = value; }

        private void Start()
        {
            IsFree = true;
            IsSelection = false;
        }

        public void SetInBlock(CharacterScript characterScript)
        {
            IsFree = false;
            m_characterScript = characterScript;
            m_characterScript.transform.SetParent(this.transform);
        }

        private void ChangeMatBlock(Material mat)
        {
            GetComponentInChildren<MeshRenderer>().material = mat;
        }

        public void SelectMat()
        {
            ChangeMatBlock(m_selectMat);
        }

        public void DeselectMat()
        {
            ChangeMatBlock(m_originalMat);
        }

        public void EffectArea()
        {
            if (PlatformManager.Instance.EnableEffect)
                ChangeMatBlock(m_enableMat);
            else
            ChangeMatBlock(m_disableMat);

        }

        public void SelectBlockMovement()
        {
            if (IsFree)
            {
                SelectMat();
                IsSelection = true;
            }
            else
            {
                //Debug.Log("este bloco não está livre");
            }
        }

        public void SelectBlock()
        {
            SelectMat();
            IsSelection = true;
        }


        public void DeSelectBlock()
        {
            DeselectMat();
            IsSelection = false;
        }

        public void DestinationBlock()
        {
            DeselectMat();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Effect"))
            {
                IsSelectionEffect = true;

                EffectArea();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Effect"))
            {
                IsSelectionEffect = false;

                if (IsSelection)
                {
                    SelectMat();
                    return;
                }
                DeselectMat();
            }
        }
    }
}