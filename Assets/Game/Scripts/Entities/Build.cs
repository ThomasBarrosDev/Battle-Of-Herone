using System.Collections.Generic;
using UnityEngine;

namespace BatteOfHerone.Entities
{
    [System.Serializable]
    public class Build
    {
        public string Name;
        public string Description;
        public Sprite Icon;
        public List<BasedCardScriptable> Childs = new List<BasedCardScriptable>();
        public List<string> RequiredStructures = new List<string>();
        private bool IsActive;
    }
}