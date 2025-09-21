using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "New Food Item", menuName = "Food Item")]
public class FoodItem : ScriptableObject
{
    public string itemName = "Food Item";
    public string itemID = "food_item";

    [SerializeField] private AssetReferenceSprite itemSpriteReference;

    private Sprite cachedSprite;

    public AssetReferenceSprite ItemSpriteReference => itemSpriteReference;

    public Sprite itemSprite => cachedSprite;

    public async System.Threading.Tasks.Task LoadSpriteAsync()
    {
        if (cachedSprite == null && itemSpriteReference != null)
        {
            var handle = itemSpriteReference.LoadAssetAsync<Sprite>();
            cachedSprite = await handle.Task;
        }
    }

    public bool IsSpriteLoaded => cachedSprite != null;
}