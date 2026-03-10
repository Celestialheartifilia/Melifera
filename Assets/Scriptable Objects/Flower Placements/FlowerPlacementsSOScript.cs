using UnityEngine;

[CreateAssetMenu(fileName = "FlowerPlacementsSOScript", menuName = "Scriptable Objects/FlowerPlacementsSOScript")]
public class FlowerPlacementsSOScript : ScriptableObject
{
    public ItemsSOScript flowerItem;

    [Header("First Flower In Bouquet")]
    public Vector3 firstPosition;
    public Vector3 firstRotation;

    [Header("Second Flower In Bouquet")]
    public Vector3 secondPosition;
    public Vector3 secondRotation;
}
