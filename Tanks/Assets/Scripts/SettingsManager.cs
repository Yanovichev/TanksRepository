using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Sprite joystickImage, buttonImage;
    [SerializeField] private GameObject buttonControl;

    private void Awake()
    {
        ShowControl();
    }
    private void ShowControl()
    {
        if (Init.Instance.playerData.control == Control.Joystick)
        {
            Init.Instance.playerData.control = Control.Joystick;
            buttonControl.GetComponent<Image>().sprite = joystickImage;
        }
    }
    public void SwithControl()
    {
        if (Init.Instance.playerData.control == Control.Joystick)
        {
            Init.Instance.playerData.control = Control.Buttons;
            buttonControl.GetComponent<Image>().sprite = buttonImage;
        }
        else
        {
            Init.Instance.playerData.control = Control.Joystick;
            buttonControl.GetComponent<Image>().sprite = joystickImage;
        }
    }
}
