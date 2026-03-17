using UnityEngine;

[CreateAssetMenu(fileName = "ItemsSOScript", menuName = "Scriptable Objects/ItemsSOScript")]
public class ItemsSOScript : ScriptableObject
{
    public string itemID;
    public string itemName;
    public Sprite itemSprite;

    public ItemType itemType;
}
public enum ItemType
{
    Flower,
    Wrap,
    Accessory
}
