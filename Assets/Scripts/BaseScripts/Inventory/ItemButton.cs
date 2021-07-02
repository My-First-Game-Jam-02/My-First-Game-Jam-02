using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    private Button itemButton;
    private InventoryManager inventoryManager;

    public int buttonIndex;
    public Image itemImage;
    public string itemButtonType;
    public Item item;
    // Start is called before the first frame update
    void Awake()
    {
        itemButton = GetComponent<Button>();
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeactivateButton()
    {
        itemButton.interactable = false;
        itemButtonType = "nothing";
        itemImage.sprite = null;
    }

    public void SetUpButton(Item item)
    {
        itemButton.interactable = true;
        itemButtonType = item.itemName;
        itemImage.sprite = item.icon;
        this.item = item;
    }

    public void SetItemAsCurrentlySelected()
    {
        inventoryManager.SetItemAsCurrentlySelected(buttonIndex, item);
    }
}
