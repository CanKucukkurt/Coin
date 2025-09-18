using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WalletUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button walletBagButton;
    [SerializeField] private GameObject walletPanel;
    [SerializeField] private Transform foodListContainer;
    [SerializeField] private GameObject foodItemDisplayPrefab;

    [Header("Dependencies")]
    [SerializeField] private WalletManager walletManager;

    // Track created UI elements to avoid duplicates
    private Dictionary<FoodItem, GameObject> createdDisplays = new Dictionary<FoodItem, GameObject>();

    private bool isPanelOpen = false;

    void Start()
    {
        // Setup button click listener
        if (walletBagButton != null)
        {
            walletBagButton.onClick.AddListener(ToggleWallet);
        }

        // Start with panel closed
        walletPanel?.SetActive(false);
        isPanelOpen = false;
    }

    void Update()
    {
        // Refresh UI when panel is open (to catch new items from roulette spins)
        if (isPanelOpen)
        {
            RefreshWalletDisplay();
        }
    }

    public void ToggleWallet()
    {
        isPanelOpen = !isPanelOpen;
        walletPanel?.SetActive(isPanelOpen);

        if (isPanelOpen)
        {
            RefreshWalletDisplay();
        }
    }

    public void RefreshWalletDisplay()
    {
        if (walletManager == null || foodListContainer == null) return;

        // Get current foods from wallet manager
        Dictionary<FoodItem, int> allFoods = walletManager.GetAllFoods();

        // Create or update displays for each food
        foreach (var foodEntry in allFoods)
        {
            FoodItem food = foodEntry.Key;
            int count = foodEntry.Value;

            // Skip if count is 0 (shouldn't happen but just in case)
            if (count <= 0) continue;

            // Create new display if this food doesn't have one yet
            if (!createdDisplays.ContainsKey(food))
            {
                CreateFoodDisplay(food);
            }

            // Update the count display
            UpdateFoodDisplay(food, count);
        }

        // Remove displays for foods that no longer exist (if count drops to 0)
        List<FoodItem> toRemove = new List<FoodItem>();
        foreach (var display in createdDisplays)
        {
            if (!allFoods.ContainsKey(display.Key) || allFoods[display.Key] <= 0)
            {
                toRemove.Add(display.Key);
            }
        }

        foreach (var food in toRemove)
        {
            RemoveFoodDisplay(food);
        }
    }

    private void CreateFoodDisplay(FoodItem food)
    {
        if (foodItemDisplayPrefab == null || food == null) return;

        // Instantiate the prefab
        GameObject newDisplay = Instantiate(foodItemDisplayPrefab, foodListContainer);

        // Get the image component for the food icon
        Image foodIcon = newDisplay.GetComponentInChildren<Image>();
        if (foodIcon != null && food.itemSprite != null)
        {
            foodIcon.sprite = food.itemSprite;
        }

        // Store reference to this display
        createdDisplays[food] = newDisplay;

        Debug.Log($"Created UI display for {food.itemName}");
    }

    private void UpdateFoodDisplay(FoodItem food, int count)
    {
        if (!createdDisplays.ContainsKey(food)) return;

        GameObject display = createdDisplays[food];
        if (display == null) return;

        // Find and update the count text
        TextMeshProUGUI countText = display.GetComponentInChildren<TextMeshProUGUI>();
        if (countText != null)
        {
            countText.text = count.ToString();
        }
    }

    private void RemoveFoodDisplay(FoodItem food)
    {
        if (!createdDisplays.ContainsKey(food)) return;

        GameObject display = createdDisplays[food];
        if (display != null)
        {
            Destroy(display);
        }

        createdDisplays.Remove(food);
        Debug.Log($"Removed UI display for {food.itemName}");
    }
}