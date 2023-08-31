using BatteOfHerone.Character;
using BatteOfHerone.Entities;
using BatteOfHerone.Enuns;
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
            StartCoroutine(InitIalized(Race));
        }

        void Update()
        {

        }

        public IEnumerator InitIalized(RaceScriptable race)
        {
            yield return new WaitForSeconds(2);

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
                        CustomButton lButton = Instantiate(_button, lBuildPanel.transform);
                        lButton.UpdateIcon(x.Icon);
                        lButton.gameObject.name = x.Name;

                        switch (x.Type)
                        {
                            case Enuns.ButtonType.Unit:

                                lButton.ListeringAction(() =>
                                {
                                    UnitScriptable unit = (UnitScriptable)x;

                                    PlatformManager.Instance.Core.SetAdjacents();

                                    PlatformManager.Instance.unitselect = unit.Hero;

                                    PlayerManager.BlockClick = (x) => GameManager.Instance.InstantiateMonster(PlatformManager.Instance.unitselect, x, PlayerState.PlayerOne);
                                    
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