using BatteOfHerone.Entities;
using BatteOfHerone.Scriptables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BatteOfHerone.UI
{
    public class StructureMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _armyAndUpgradeViewPort;
        [SerializeField] private GameObject _armyContentMenu;
        [SerializeField] private GameObject _upgradingContent;

        private Structure _structure;

        public void Initialized(GameObject armyContentMenu, GameObject upgradeContentMenu, GameObject armyAndUpgradeViewPort, PanelButton button, Structure structure)
        {
            _armyContentMenu = Instantiate(armyContentMenu, armyAndUpgradeViewPort.transform);
            _upgradingContent = Instantiate(upgradeContentMenu, armyAndUpgradeViewPort.transform);
            _structure = structure;

            _structure.Armies.ForEach(a =>
            {
                PanelButton lButton = Instantiate(button, armyContentMenu.transform);
                lButton.UpdateIcon(a.Icon);
                lButton.gameObject.name = a.Name;
                ArmyButton structure = lButton.gameObject.AddComponent<ArmyButton>();
                structure.Initialized(a);
            });


        }

        public void SetHeros()
        {

        }



    }
}