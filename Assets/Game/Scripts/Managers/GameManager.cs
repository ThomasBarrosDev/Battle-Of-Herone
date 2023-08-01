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
        [SerializeField] public GameObject[] m_monstersPrefabs;
        private int m_idCams = 0;

        protected override void Awake()
        {            
            base.Awake();
        }

        public void InstantiateMonster(GameObject monsterPrefab, BlockScript blockScript, PlayerEnum playerEnum)
        {
            GameObject go = Instantiate(monsterPrefab, blockScript.transform);
            
            go.transform.position = blockScript.Movepos.position;
            
            go.GetComponent<CharacterScript>().PlayerEnum = playerEnum;
            
            go.GetComponent<CharacterScript>().PositionBlock = blockScript;

            go.GetComponent<CharacterScript>().PositionBlock.SetInBlock(go.GetComponent<CharacterScript>());

            if (playerEnum == PlayerEnum.PlayerOne)
                go.transform.GetChild(0).Rotate(0, 90, 0);
            else
                go.transform.GetChild(0).Rotate(0, -90, 0);

        }

    }
}
