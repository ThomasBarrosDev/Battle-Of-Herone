using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BatteOfHerone.Entities
{
    [System.Serializable]
    public class Structure
    {
        public string Name;
        public string Description;
        public Sprite Icon;
        public List<Army> Armies = new List<Army>();
        public List<UIButtonData> Upgradings = new List<UIButtonData>();
        public List<Structure> RequiredStructures = new List<Structure>();
        private bool IsActive;
    }
}