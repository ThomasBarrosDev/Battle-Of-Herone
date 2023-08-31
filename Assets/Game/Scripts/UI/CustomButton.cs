using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour
{
    [SerializeField] private Image _imageBg;
    [SerializeField] private Image _imageIcon;
    [SerializeField] private Sprite _spriteBgOn;
    [SerializeField] private Sprite _spriteBgOff;
    [SerializeField] private UnityAction _actionButton;
    [SerializeField] private Button _button;
    public UnityAction ActionButton { get => _actionButton; private set => _actionButton = value; }

    public void UpdateIcon(Sprite icon)
    {
        if (_imageIcon == null)
            _imageIcon = transform.GetChild(1).GetComponent<Image>();

        _imageIcon.sprite = icon;
    }

    public void SetActive(bool State)
    {
        if (State)
        {
            _imageBg.sprite = _spriteBgOn;
        }
        else
        {
            _imageBg.sprite = _spriteBgOff;

        }
    }

    public void ListeringAction (UnityAction action)
    {
        ActionButton += action;
    }

    public void InitButton()
    {
        if (_button == null)
            _button = GetComponent<Button>();
        
        _button.onClick.AddListener(ActionButton);
    }


}
