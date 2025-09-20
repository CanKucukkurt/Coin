using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using NUnit.Framework;

public class WalletUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button walletBagButton;
    [SerializeField] private GameObject walletPanel;
    [SerializeField] private Transform foodListContainer;
    [SerializeField] private GameObject foodItemDisplayPrefab;

    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private Vector2 panelOffset = new Vector2(0, -100f);
    [SerializeField] private Ease openAnimationEase = Ease.OutQuad;
    [SerializeField] private Ease closeAnimationEase = Ease.InQuad;

    private bool isAnimating = false;

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

    public void ToggleWallet()
    {
        if (isAnimating) return;
        if (isPanelOpen)
        {
            CloseWallet();
        }
        else
        {
            OpenWallet();
        }
    }

    public void OpenWallet()
    {
        if (isPanelOpen || isAnimating) return;

        isAnimating = true;
        isPanelOpen = true;

        RectTransform panelRect = walletPanel.GetComponent<RectTransform>();
        RectTransform buttonRect = walletBagButton.GetComponent<RectTransform>();

        walletPanel.SetActive(true);
        panelRect.anchoredPosition = buttonRect.anchoredPosition;
        panelRect.localScale = Vector3.zero;

        Sequence openSequence = DOTween.Sequence();
        openSequence.Append(panelRect.DOAnchorPos(panelOffset, animationDuration).SetEase(openAnimationEase));
        openSequence.Join(panelRect.DOScale(new Vector3(7, 10, 1), animationDuration).SetEase(openAnimationEase));
        openSequence.OnComplete(() =>
        {
            isAnimating = false;
        });
    }

    public void CloseWallet()
    {
        if (!isPanelOpen || isAnimating) return;

        isAnimating = true;
        isPanelOpen = false;

        // Animate back to wallet button position and scale down
        RectTransform panelRect = walletPanel.GetComponent<RectTransform>();
        Vector3 buttonPosition = walletBagButton.GetComponent<RectTransform>().anchoredPosition;

        Sequence closeSequence = DOTween.Sequence();
        closeSequence.Append(panelRect.DOAnchorPos(buttonPosition, animationDuration).SetEase(closeAnimationEase));
        closeSequence.Join(panelRect.DOScale(Vector3.zero, animationDuration).SetEase(closeAnimationEase));
        closeSequence.OnComplete(() =>
        {
            walletPanel.SetActive(false);
            isAnimating = false;
        });
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