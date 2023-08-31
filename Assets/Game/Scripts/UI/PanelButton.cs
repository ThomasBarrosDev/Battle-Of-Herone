using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelButton : MonoBehaviour
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private Sprite _bg;

    public void UpdateIcon(Sprite icon)
    {
        _icon = icon;
        transform.GetChild(0).GetComponent<Image>().sprite = _icon;
    }

    public void UpdateBg(Sprite icon)
    {
        _icon = icon;
        GetComponent<Image>().sprite = _icon;
    }
}
