using Assets.Game.Scripts.UI;
using BatteOfHerone.Enuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BatteOfHerone.Entities
{
    [CreateAssetMenu(fileName = "NewUpgrade", menuName = "UI/Upgrade")]
    public class BasedCardScriptable : ScriptableObject
    {
        public string Name;
        public string Description;
        public ButtonType Type;
        public Sprite Icon;
    }
}