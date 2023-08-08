using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BatteOfHerone.Entities
{
    [CreateAssetMenu(fileName = "NewUnit", menuName = "UI/Unit")]
    public class UnitButton : BasedCard
    {
        public GameObject Hero;
    }
}
