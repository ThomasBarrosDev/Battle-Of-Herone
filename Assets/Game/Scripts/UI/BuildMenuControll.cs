using BatteOfHerone.Entities;
using BatteOfHerone.Scriptables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BatteOfHerone.UI
{
    public class BuildMenuControll : MonoBehaviour
    {
        [SerializeField] private CustomButton _isActual;

        public GameObject Panel { get; set; }


        public void SetActiveButton(CustomButton button)
        {
            if (_isActual != null)
                _isActual.SetActive(false);

            _isActual = button;

            _isActual.SetActive(true);
        }


    }
}