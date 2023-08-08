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
        public List<BasedCard> Childs = new List<BasedCard>();
        public List<string> RequiredStructures = new List<string>();
        private bool IsActive;
    }
}