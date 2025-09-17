using System.Collections;
using UnityEngine;

public class RouletteSpinManager : MonoBehaviour
{
    [SerializeField] private Transform slotsParent;
    [SerializeField] private float baseSpinSpeed = 0.1f;
    [SerializeField] private float minimumSpins = 20;
    [SerializeField] private float maxExtraSpins = 10;

    private RouletteSlot[] allSlots;
    private bool isSpinning = false;
    private int currentHighlightedIndex = -1;

    public bool isSpinningPublic => isSpinning;
    [SerializeField] private WalletManager walletManager;
    [SerializeField] private ItemAnimation burstAnimator;

    void Start()
    {
        allSlots = slotsParent.GetComponentsInChildren<RouletteSlot>();
        System.Array.Sort(allSlots, (a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));
        Debug.Log($"Found {allSlots.Length} slots.");
    }


    IEnumerator SpinSequence()
    {
        int totalSpins = (int)(minimumSpins + Random.Range(0, maxExtraSpins));
        yield return StartCoroutine(AnimateSpin(totalSpins));
        yield return StartCoroutine(FlashLandedSlot());
        walletManager.AddFood(allSlots[currentHighlightedIndex].AssignedFood);
        walletManager.AddFood(allSlots[currentHighlightedIndex].AssignedFood);
        burstAnimator.PlayBurstAnimation(allSlots[currentHighlightedIndex].AssignedFood, allSlots[currentHighlightedIndex].transform);
        isSpinning = false;
        Debug.Log($"Landed on slot: {allSlots[currentHighlightedIndex].name}  Item: {allSlots[currentHighlightedIndex].AssignedFood.itemName}");
    }

    IEnumerator AnimateSpin(int totalSpins)
    {
        float spinSpeed = baseSpinSpeed;
        for (int i = 0; i < totalSpins; i++)
        {
            if (currentHighlightedIndex >= 0)
                allSlots[currentHighlightedIndex].SetHighlighted(false);

            currentHighlightedIndex = (currentHighlightedIndex + 1) % allSlots.Length;
            allSlots[currentHighlightedIndex].SetHighlighted(true);

            if (i > totalSpins * 0.7f)
                spinSpeed += baseSpinSpeed * 0.1f;

            yield return new WaitForSeconds(spinSpeed);
        }
    }

    IEnumerator FlashLandedSlot()
    {
        RouletteSlot landedSlot = allSlots[currentHighlightedIndex];
        for (int i = 0; i < 6; i++)
        {
            landedSlot.SetHighlighted(false);
            yield return new WaitForSeconds(0.05f);
            landedSlot.SetHighlighted(true);
            yield return new WaitForSeconds(0.05f);
        }
    }

    void TestSpin()
    {
        if (!isSpinning && Input.GetKeyDown(KeyCode.Space))
        {
            isSpinning = true;
            StartCoroutine(SpinSequence());
        }
    }

    void Update()
    {
        TestSpin();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var collectedFoods = walletManager.GetAllFoods();
            Debug.Log($"Wallet contains {collectedFoods.Count} items:");
            foreach (var food in collectedFoods)
            {
                Debug.Log($"- {food.itemName}");
            }
        }
    }
}
