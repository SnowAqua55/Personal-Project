using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
	public ItemSlot[] slots;

	public GameObject inventoryWindow;
	public Transform slotPanel;
	public Transform dropPosition;

	[Header("Select Item")]
	public TextMeshProUGUI selectedItemName;
	public TextMeshProUGUI selectedItemDescription;
	public TextMeshProUGUI selectedStatName;
	public TextMeshProUGUI selectedStatValue;
	public GameObject useButton;
	public GameObject equipButton;
	public GameObject unequipButton;
	public GameObject dropButton;

	private PlayerController controller;
	private PlayerCondition condition;

	void Start()
    {
		controller = CharacterManager.Instance.Player.controller;
		condition = CharacterManager.Instance.Player.condition;
		dropPosition = CharacterManager.Instance.Player.dropPosition;

		controller.inventory += Toggle;
		CharacterManager.Instance.Player.addItem += AddItem;

		inventoryWindow.SetActive(false);
		slots = new ItemSlot[slotPanel.childCount];

		for(int i = 0; i < slots.Length; i++)
		{
			slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
			slots[i].index = i;
			slots[i].Inventory = this;
		}
		ClearSelectedItemWindow();
    }

	void ClearSelectedItemWindow()
	{
		selectedItemName.text = string.Empty;
		selectedItemDescription.text = string.Empty;
		selectedStatName.text = string.Empty;
		selectedStatValue.text = string.Empty;

		useButton.SetActive(false);
		equipButton.SetActive(false);
		unequipButton.SetActive(false);
		dropButton.SetActive(false);
	}

	public void Toggle()
	{
		if (IsOpen())
		{
			inventoryWindow.SetActive(false);
		}
		else
		{
			inventoryWindow.SetActive(true);
		}
	}

	public bool IsOpen()
	{
		return inventoryWindow.activeInHierarchy;
	}

	void AddItem()
	{
		ItemData data = CharacterManager.Instance.Player.itemData;

		if (data.canStack)
		{
			ItemSlot slot = GetItemStack(data);
			if(slot != null)
			{
				slot.quantity++;
				UpdateUI();
				CharacterManager.Instance.Player.itemData = null;
				return;
			}
		}

		ItemSlot emptySlot = GetEmptySlot();
        if (emptySlot != null)
        {
			emptySlot.Item = data;
			emptySlot.quantity = 1;
			UpdateUI();
			CharacterManager.Instance.Player.itemData = null;
			return;
        }

		ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
	}

	void UpdateUI()
	{
		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i] != null )
			{
				slots[i].Set();
			}
			else
			{
				slots[i].Clear();
			}
		}
	}

	ItemSlot GetItemStack(ItemData data)
	{
		for(int i = 0; i < slots.Length; i++)
		{
			if (slots[i].Item == data && slots[i].quantity < data.maxStackAmount)
			{
				return slots[i];
			} 
		}
		return null;
	}

	ItemSlot GetEmptySlot()
	{
		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i].Item == null)
				return slots[i];
		}
		return null;
	}

	void ThrowItem(ItemData data)
	{
		Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
	}
}