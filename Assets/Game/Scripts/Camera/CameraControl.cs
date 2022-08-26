using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BatteOfHerone.CameraManager
{
    public class CameraControl : MonoBehaviour
    {
        public Transform[] positions;


        private void ChangePosition(Transform pos)
        {
            transform.position = pos.position;
            transform.rotation = pos.rotation;
        }

        public void BottunChangePosition(int i)
        {
            ChangePosition(positions[i]);
        }

    }
}