using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable,
    Resources
}

public enum ConsumableType
{
    Health,
    Hunger
}

[SerializeField]
public class ItemDataConsumable
{
    public ConsumableType Type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]

public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;
    
    [Header("Consumable")]
    public ItemDataConsumable[] consumables;
}