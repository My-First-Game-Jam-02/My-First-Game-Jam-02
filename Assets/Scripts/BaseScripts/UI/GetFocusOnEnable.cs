using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetFocusOnEnable : MonoBehaviour
{
    public Button myButton;

    void OnEnable()
    {
        FocusOnButton();
    }

    public void FocusOnButton()
    {
        myButton.Select();
        myButton.OnSelect(null);
    }
}
