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

    private Dictionary<FoodItem, GameObject> createdDisplays = new Dictionary<FoodItem, GameObject>();

    private bool isPanelOpen = false;

    void Start()
    {
        if (walletBagButton != null)
        {
            walletBagButton.onClick.AddListener(ToggleWallet);
        }

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

        Dictionary<FoodItem, int> allFoods = walletManager.GetAllFoods();

        foreach (var foodEntry in allFoods)
        {
            FoodItem food = foodEntry.Key;
            int count = foodEntry.Value;

            if (count <= 0) continue;

            if (!createdDisplays.ContainsKey(food))
            {
                CreateFoodDisplay(food);
            }

            UpdateFoodDisplay(food, count);
        }

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

        GameObject newDisplay = Instantiate(foodItemDisplayPrefab, foodListContainer);

        Image foodIcon = newDisplay.GetComponentInChildren<Image>();
        if (foodIcon != null && food.itemSprite != null)
        {
            foodIcon.sprite = food.itemSprite;
        }

        createdDisplays[food] = newDisplay;

        Debug.Log($"Created UI display for {food.itemName}");
    }

    private void UpdateFoodDisplay(FoodItem food, int count)
    {
        if (!createdDisplays.ContainsKey(food)) return;

        GameObject display = createdDisplays[food];
        if (display == null) return;

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