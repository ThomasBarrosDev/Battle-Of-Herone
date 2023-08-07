using BatteOfHerone.Scriptables;
using BatteOfHerone.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BatteOfHerone.Controllers
{
    public class UIController : MonoBehaviour
    {
        [Header("Teste")]
        public RaceScriptable Race;
        [Header("Components")]
        [SerializeField] private GameObject _structureMenuContent;
        [SerializeField] private GameObject _armyAndUpgradeViewPort;

        [Header("Prefabs")]
        [SerializeField] private GameObject _armyContent;
        [SerializeField] private GameObject _upgradingContent;
        [SerializeField] private PanelButton _button;


        void Start()
        {
            InitIalized(Race);
        }

        void Update()
        {

        }

        public void InitIalized(RaceScriptable race)
        {
            race.structures.ForEach((x) =>
            {
                PanelButton lButton= Instantiate(_button, _structureMenuContent.transform);
                lButton.UpdateIcon(x.Icon);
                lButton.gameObject.name = x.Name;
                StructureMenu structure = lButton.gameObject.AddComponent<StructureMenu>();
                structure.Initialized(_armyContent, _upgradingContent, _armyAndUpgradeViewPort, _button, x);
            });
        }
    }
}