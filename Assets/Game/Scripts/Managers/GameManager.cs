using BatteOfHerone.Utils;
using BatteOfHerone.Enuns;
using BatteOfHerone.Blocks;
using BatteOfHerone.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BatteOfHerone.Managers
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        [SerializeField] private GameObject[] m_cams;
        [SerializeField] public GameObject[] m_monstersPrefabs;
        private int m_idCams = 0;
        protected override void Awake()
        {            
            base.Awake();

            m_cams[m_idCams].SetActive(true);
        }
        private void SetCams()
        {
            int idTemp = m_idCams;
            m_idCams++;
            m_cams[idTemp].SetActive(false);
            m_cams[m_idCams].SetActive(true);
        }

        public void InstantiateMonster(GameObject monsterPrefab, BlockScript blockScript, PlayerEnum playerEnum)
        {
            GameObject go = Instantiate(monsterPrefab);
            
            go.transform.position = blockScript.Movepos.position;
            
            go.GetComponent<CharacterScript>().PlayerEnum = playerEnum;
            
            go.GetComponent<CharacterScript>().PositionBlock = blockScript;

            go.GetComponent<CharacterScript>().PositionBlock.IsFree = false;

            if (playerEnum == PlayerEnum.PlayerOne)
                go.transform.Rotate(0, 90, 0);
            else
                go.transform.Rotate(0, -90, 0);

        }

    }
}
