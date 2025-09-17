using UnityEngine;
[CreateAssetMenu(fileName = "New Food Item", menuName = "Food Item")]
public class FoodItem : ScriptableObject
{
    public string itemName = "Food Item";
    public string itemID = "food_item";
    public Sprite itemSprite;
}
