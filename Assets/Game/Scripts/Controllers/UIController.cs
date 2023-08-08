using BatteOfHerone.Character;
using BatteOfHerone.Entities;
using BatteOfHerone.Managers;
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
        [SerializeField] private GameObject _menuViewPort;

        [Header("Prefabs")]
        [SerializeField] private GameObject _MenuPanel;
        [SerializeField] private CustomButton _button;
        [SerializeField] private CustomButton _backButton;


        void Start()
        {
            InitIalized(Race);
        }

        void Update()
        {

        }

        public void InitIalized(RaceScriptable race)
        {
            GameObject lHomePanel = Instantiate(_MenuPanel, _menuViewPort.transform);

            race.Buildings.ForEach((x) =>
            {
                CustomButton lButton = Instantiate(_button, lHomePanel.transform);
                lButton.UpdateIcon(x.Icon);
                lButton.gameObject.name = x.Name;

                GameObject lBuildPanel = Instantiate(_MenuPanel, _menuViewPort.transform);
                lBuildPanel.SetActive(false);

                BuildMenuControll lBuildControll = lBuildPanel.gameObject.AddComponent<BuildMenuControll>();
                lBuildControll.Panel = lBuildPanel;

                lButton.ListeringAction(()=> lBuildControll.Panel.SetActive(true));
                lButton.ListeringAction(()=> lHomePanel.SetActive(false));

                lButton.InitButton();

                _backButton.ListeringAction(() => lBuildControll.Panel.SetActive(false));
                if (x.Childs.Count > 0)
                {
                    x.Childs.ForEach((x) =>
                    {
                        PlatformManager.Instance.Core.GetComponent<CharacterScript>().SetPossibilities();

                        CustomButton lButton = Instantiate(_button, lBuildPanel.transform);
                        lButton.UpdateIcon(x.Icon);
                        lButton.gameObject.name = x.Name;

                        switch (x.Type)
                        {
                            case Enuns.ButtonType.Unit:

                                lButton.ListeringAction(() =>
                                {
                                    UnitButton unit = (UnitButton)x;
                                    Instantiate(unit.Hero);
                                });

                                break;
                            case Enuns.ButtonType.Upgrade:
                                break;
                            default:
                                break;
                        }

                        lButton.InitButton();
                    });

                }

            });

            _backButton.ListeringAction(() => lHomePanel.SetActive(true));
            _backButton.InitButton();


           
        }
    }
}