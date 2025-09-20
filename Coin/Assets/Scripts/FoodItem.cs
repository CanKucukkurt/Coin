using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "New Food Item", menuName = "Food Item")]
public class FoodItem : ScriptableObject
{
    public string itemName = "Food Item";
    public string itemID = "food_item";

    [SerializeField] private AssetReferenceSprite itemSpriteReference;

    // Cache the loaded sprite
    private Sprite cachedSprite;

    public AssetReferenceSprite ItemSpriteReference => itemSpriteReference;

    // Property that returns cached sprite or null if not loaded yet
    public Sprite itemSprite => cachedSprite;

    // Load and cache the sprite (call this once when needed)
    public async System.Threading.Tasks.Task LoadSpriteAsync()
    {
        if (cachedSprite == null && itemSpriteReference != null)
        {
            var handle = itemSpriteReference.LoadAssetAsync<Sprite>();
            cachedSprite = await handle.Task;
        }
    }

    // Check if sprite is loaded
    public bool IsSpriteLoaded => cachedSprite != null;
}