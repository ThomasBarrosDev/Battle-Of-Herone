using BatteOfHerone.Utils;
using BatteOfHerone.Enuns;
using BatteOfHerone.Character;
using UnityEngine;
using BatteOfHerone.Block;
using System.Collections.Generic;

namespace BatteOfHerone.Managers
{
    public delegate void TurnAction();

    public class GameManager : SingletonBehaviour<GameManager>
    {
        [SerializeField] public GameObject[] m_monstersPrefabs;

        [SerializeField] private PlayerManager playerOne;
        [SerializeField] private PlayerManager playerTwo;

        public List<Unit> UnitsPlayerTwo { get; set; } = new List<Unit>();
        public PlayerManager PlayerOne { get => playerOne; set => playerOne = value; }
        public PlayerManager PlayerTwo { get => playerTwo; set => playerTwo = value; }

        private int m_idCams = 0;

        protected override void Awake()
        {            
            base.Awake();
        }

        public Unit InstantiateMonster(GameObject monsterPrefab, BlockScript blockScript, PlayerState playerEnum)
        {
            Debug.Log("entoru" + monsterPrefab);
            GameObject go = Instantiate(monsterPrefab, blockScript.transform);
            
            go.transform.position = blockScript.Movepos.position;
            
            go.GetComponent<Unit>().PlayerState = playerEnum;
            
            go.GetComponent<Unit>().PositionBlock = blockScript;

            go.GetComponent<Unit>().PositionBlock.SetInBlock(go.GetComponent<Unit>());

            if (playerEnum == PlayerState.PlayerOne)
                go.transform.GetChild(0).Rotate(0, 90, 0);
            else
                go.transform.GetChild(0).Rotate(0, -90, 0);

            return go.GetComponent<Unit>();

        }

    }
}
