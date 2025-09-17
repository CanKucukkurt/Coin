using System.Collections.Generic;
using UnityEngine;

public class WalletManager : MonoBehaviour
{
    private List<FoodItem> collectedFoods = new List<FoodItem>();

    public void AddFood(FoodItem food)
    {
        if (food != null)
        {
            collectedFoods.Add(food);
            Debug.Log($"Added {food.itemName} to wallet.");
        }
    }

    public bool HasFood(FoodItem food)
    {
        return collectedFoods.Contains(food);
    }

    public int GetFoodCount()
    {
        return collectedFoods.Count;
    }

    public List<FoodItem> GetAllFoods()
    {
        return new List<FoodItem>(collectedFoods);
    }

}