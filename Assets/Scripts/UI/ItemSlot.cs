using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
	public ItemData Item;

	public Button button;
	public Image icon;
	public TextMeshProUGUI quantityText;
	private Outline outline;

	public UIInventory Inventory;

	public int index;
	public int quantity;
	public bool equipped;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public void Set()
	{

	}

	public void Clear()
	{

	}
}