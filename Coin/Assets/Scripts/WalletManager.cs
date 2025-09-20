using System.Collections.Generic;
using UnityEngine;

public class WalletManager : MonoBehaviour
{
    private Dictionary<FoodItem, int> collectedFoods = new Dictionary<FoodItem, int>();
    [SerializeField] private WalletUI walletUI;


    private void Start()
    {
        LoadWalletFromPlayerPrefs();
    }
    public void AddFood(FoodItem food, int amount)
    {
        if (collectedFoods.ContainsKey(food))
        {
            collectedFoods[food] += amount;
        }
        else
        {
            collectedFoods[food] = amount;
        }
        Debug.Log($"Added {amount} {food.itemName} to wallet. Total: {collectedFoods[food]}");
        SaveWalletToPlayerPrefs();
        walletUI.RefreshWalletDisplay();

    }

    public bool HasFood(FoodItem food)
    {
        return collectedFoods.ContainsKey(food);
    }

    public int GetFoodCount(FoodItem food)
    {
        return collectedFoods.ContainsKey(food) ? collectedFoods[food] : 0;
    }

    public int GetTotalItems()
    {
        int total = 0;
        foreach (var amount in collectedFoods.Values)
        {
            total += amount;
        }
        return total;
    }

    public Dictionary<FoodItem, int> GetAllFoods()
    {
        return new Dictionary<FoodItem, int>(collectedFoods);
    }

    public void SaveWalletToPlayerPrefs()
    {
        foreach (var entry in collectedFoods)
        {
            PlayerPrefs.SetInt(entry.Key.itemName, entry.Value);
        }
        PlayerPrefs.Save();
        Debug.Log("Wallet saved to PlayerPrefs.");
    }

    public void LoadWalletFromPlayerPrefs()
    {
        collectedFoods.Clear();
        // Assuming you have a predefined list of all possible FoodItems in the game
        FoodItem[] allFoodItems = Resources.LoadAll<FoodItem>("FoodItems");
        foreach (var food in allFoodItems)
        {
            int amount = PlayerPrefs.GetInt(food.itemName, 0);
            if (amount > 0)
            {
                collectedFoods[food] = amount;
            }
        }
        Debug.Log("Wallet loaded from PlayerPrefs.");
        walletUI.RefreshWalletDisplay();
    }

    public void ClearWallet()
    {
        collectedFoods.Clear();

        // Clear from PlayerPrefs too
        FoodItem[] allFoodItems = Resources.LoadAll<FoodItem>("FoodItems");
        foreach (var food in allFoodItems)
        {
            PlayerPrefs.DeleteKey(food.itemName);
        }
        PlayerPrefs.Save();

        walletUI.RefreshWalletDisplay();
        Debug.Log("Wallet cleared completely.");
    }
}