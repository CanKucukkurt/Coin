using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class FoodItemManager : MonoBehaviour
{
    public static FoodItemManager Instance { get; private set; }

    private Dictionary<string, FoodItem> loadedFoodItems = new Dictionary<string, FoodItem>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async Task<List<FoodItem>> LoadAllFoodItemsAsync()
    {
        try
        {
            // Load all assets with the "FoodItems" label
            var handle = Addressables.LoadAssetsAsync<FoodItem>("FoodItems", null);
            var foodItems = await handle.Task;

            // Cache the loaded items and their sprites
            foreach (var item in foodItems)
            {
                if (!loadedFoodItems.ContainsKey(item.itemID))
                {
                    loadedFoodItems[item.itemID] = item;
                    await item.LoadSpriteAsync();
                }
            }

            Debug.Log($"Loaded {foodItems.Count} food items successfully");
            return new List<FoodItem>(foodItems);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load FoodItems: {e.Message}");
            return new List<FoodItem>();
        }
    }
}