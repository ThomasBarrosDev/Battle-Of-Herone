using BatteOfHerone.Entities;
using BatteOfHerone.Enuns;
using System.Collections.Generic;
using UnityEngine;

namespace BatteOfHerone.Scriptables
{

    [CreateAssetMenu(fileName = "NewRace", menuName = "Custom/Race")]
    public class RaceScriptable : ScriptableObject
    {
        public RaceType Race;
        public string Description;
        public List<Structure> structures = new List<Structure>();
    }
}