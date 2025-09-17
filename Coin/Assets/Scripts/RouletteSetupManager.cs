using Unity.VisualScripting;
using UnityEngine;

public class RouletteSetupManager : MonoBehaviour
{
    [SerializeField] private FoodItem[] availableFoodItems;
    [SerializeField] private Transform slotsParent;
    private RouletteSlot[] allSlots;

    void Start()
    {
        SetupRoulette();
    }

    void SetupRoulette()
    {
        allSlots = slotsParent.GetComponentsInChildren<RouletteSlot>();

        FoodItem[] shuffledFoods = new FoodItem[availableFoodItems.Length];
        System.Array.Copy(availableFoodItems, shuffledFoods, availableFoodItems.Length);

        for (int i = 0; i < shuffledFoods.Length; i++)
        {
            int randomIndex = Random.Range(i, shuffledFoods.Length);
            FoodItem temp = shuffledFoods[i];
            shuffledFoods[i] = shuffledFoods[randomIndex];
            shuffledFoods[randomIndex] = temp;
        }

        for (int i = 0; i < allSlots.Length && i < shuffledFoods.Length; i++)
        {
            allSlots[i].AssignFood(shuffledFoods[i]);
        }
    }
}