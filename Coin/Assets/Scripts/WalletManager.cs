using System.Collections.Generic;
using UnityEngine;

public class WalletManager : MonoBehaviour
{
    private Dictionary<FoodItem, int> collectedFoods = new Dictionary<FoodItem, int>();

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
}