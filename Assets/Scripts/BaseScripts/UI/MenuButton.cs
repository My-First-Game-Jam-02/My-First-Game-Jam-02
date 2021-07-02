using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{

    protected Button menuButton;
    public bool isStartingButton;
    public Sprite normalButton;
    public Sprite selectedButton;

    private void OnEnable()
    {
        menuButton = GetComponent<Button>();
        if (isStartingButton)
        {
            menuButton.image.sprite = selectedButton;
        } else
        {
            menuButton.image.sprite = normalButton;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (eventData.selectedObject == this.gameObject)
        {
            menuButton.image.sprite = selectedButton;
        }
    }

    public void OnDeselect(BaseEventData data)
    {
        menuButton.image.sprite = normalButton;
    }


}
