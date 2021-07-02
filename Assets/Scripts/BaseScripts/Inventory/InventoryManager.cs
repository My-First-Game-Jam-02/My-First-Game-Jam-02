using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance;

    private PlayerBehavior playerBehavior;
    private CameraController cameraController;

    public GameObject inventoryUI;
    public Button useButton;
    public GameObject healSFX;
    public GameObject barrierSFX;

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public Image itemImage;

    [Header("Base Data")]
    public List<Item> itemList;
    public GameObject itemButtonGrid;
    public Button[] itemButtons;
    public Item currentlySelectedItem;
    public int currentButtonIndex;

    [Header("Saved Data")]
    public string nameString;
    public int saveIndex;
    public List<string> inventoryItemsList = new List<string>(10);
    public List<Item> inventoryItems = new List<Item>(10);
    //NOT USED RIGHT NOW
    public int savedObjectAmount;
    public string[] savedObjects;
    public int savedObjectiveQuantity;
    public string[] savedObjectives;

    [Header("Inventory-Related Values")]
    public int selectedMain;
    public int selectedSlot;
    public bool discarding;

    [Header("MISC")]
    public bool loaded;
    public bool freshSave;
    public bool sceneToScene;
    public int dummyInt;

    private void Awake()
    {
        cameraController = FindObjectOfType<CameraController>();
        playerBehavior = FindObjectOfType<PlayerBehavior>();
        itemButtons = itemButtonGrid.GetComponentsInChildren<Button>();
    }

    void Start()
    {

        Item[] allItems = Resources.LoadAll<Item>("ItemList/");
        for(int i = 0; i < allItems.Length; i++)
        {
            itemList.Add(allItems[i]);
        }
        CloseInventory();
    }

    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            if (!GameManager.Instance.inventoryActive && !GameManager.Instance.dialogueActive && !GameManager.Instance.pauseMenuActive)
            {
                OpenInventory();

            }
            else if (GameManager.Instance.inventoryActive)
            {
                CloseInventory();
            }
        }
    }
    public void PickUp(string itemName)
    {

        //Checks to see if player can pickup items or if inventory is full.
        bool canPickUpItems = false;
        for (int i = 0; i < inventoryItemsList.Count; i++)
        {
            if (string.IsNullOrEmpty(inventoryItemsList[i]))
            {
                canPickUpItems = true;
            }
        }

        if (!canPickUpItems)
        {
            print("playing inventory full");
            return;
        }

        //Adds the item to the player's item list.
        for (int t = 0; t < inventoryItemsList.Count; t++)
        {
            if(string.IsNullOrEmpty(inventoryItemsList[t]))
            {
                inventoryItemsList[t] = itemName;
                return;
            }
        }
    }

    void OpenInventory()
    {
        playerBehavior.interactEnable = false;
        GameManager.Instance.inventoryActive = true;
        GameManager.Instance.PauseGame();
        inventoryUI.SetActive(true);
        RefreshUI();
    }

    public void CloseInventory()
    {
        playerBehavior.interactEnable = true;
        GameManager.Instance.inventoryActive = false;
        GameManager.Instance.UnPauseGame();
        currentlySelectedItem = null;
        currentButtonIndex = -1;
        ClearItemDescription();
        inventoryUI.SetActive(false);
    }

    //This code will be used by the Use Button to use an item.
    public void Use()
    {

        if(currentButtonIndex == -1)
        {
            return;
        }

        string itemType = currentlySelectedItem.itemName;

        switch (itemType)
        {
            case "match":
                break;
            default:
                break;
        }

        RemoveItemFromSlot(currentButtonIndex);
        ClearItemDescription();
        RefreshUI();
        CloseInventory();
    }

    //Turns the inventory strings in the inventoryItemsList to actual items in the inventoryItems.
    public void InventoryStringsToItems()
    {
        for (int i = 0; i < inventoryItemsList.Count; i++)
        {

            if (!string.IsNullOrEmpty(inventoryItemsList[i]))
            {
                for (int t = 0; t < itemList.Count; t++)
                {
                    if (inventoryItemsList[i] == itemList[t].name)
                    {
                        inventoryItems[i] = itemList[t];
                    }
                }
            }
        }
    }

    public void AssignInventoryToButtons()
    {
        for (int i = 0; i < itemButtons.Length; i++)
        {
            ItemButton itemButton = itemButtons[i].GetComponent<ItemButton>();
            if(inventoryItems[i] != null)
            {
                itemButton.SetUpButton(inventoryItems[i]);
            }
            else
            {
                itemButton.DeactivateButton();
            }
        }
    }

    //This activates and deactivates the use button depending on whether there are useable items.
    public void CheckUseButtonShouldBeActive()
    {
        if(currentlySelectedItem.canUse == false)
        {
            useButton.interactable = false;
        } else
        {
            useButton.interactable = true;
        }
    }

    public void SetItemAsCurrentlySelected(int buttonIndex, Item item)
    {
        itemDescription.text = item.description;
        itemImage.sprite = item.icon;
        itemName.text = item.itemName;
        currentlySelectedItem = item;
        currentButtonIndex = buttonIndex;
        CheckUseButtonShouldBeActive();
    }

    public void RemoveItemFromSlot(int slotIndex)
    {
        inventoryItemsList.RemoveAt(slotIndex);
        inventoryItemsList.Add("");
        inventoryItems.RemoveAt(slotIndex);
        inventoryItems.Add(null);
    }

    public void ClearItemDescription()
    {
        itemDescription.text = "Item Description";
        itemImage.sprite = null;
        itemName.text = "Selected Item";
    }

    public void RefreshUI()
    {
        currentlySelectedItem = null;
        currentButtonIndex = -1;
        InventoryStringsToItems();
        AssignInventoryToButtons();
    }

    public bool CheckInventoryFull()
    {
        bool isFull = true;
        for (int i = 0; i < inventoryItemsList.Count; i++)
        {
            if (string.IsNullOrEmpty(inventoryItemsList[i]))
            {
                isFull = false;
            }   
        }

        return isFull;
    }

    public void Drop(int slotIndex)
    {
        RefreshUI();
    }

    public void ClearSlot(int slotIndex)
    {
        RefreshUI();
    }

    public void PlayHealSFX()
    {
        Instantiate(healSFX, transform.position, transform.rotation);
    }

    public void PlayBarrierSFX()
    {
        Instantiate(barrierSFX, transform.position, transform.rotation);
    }
}
