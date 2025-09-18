using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RouletteSpinManager : MonoBehaviour
{
    [SerializeField] private Transform slotsParent;
    [SerializeField] private float baseSpinSpeed = 0.1f;
    [SerializeField] private float minimumSpins = 20;
    [SerializeField] private float maxExtraSpins = 10;
    [SerializeField] private float winSeconds = 0.5f;

    private RouletteSlot[] allSlots;
    private bool isSpinning = false;
    private int currentHighlightedIndex = -1;

    public bool isSpinningPublic => isSpinning;
    [SerializeField] private WalletManager walletManager;
    [SerializeField] private ItemAnimation burstAnimator;
    [SerializeField] private SpinButton spinButton;

    void Start()
    {
        allSlots = slotsParent.GetComponentsInChildren<RouletteSlot>();
        System.Array.Sort(allSlots, (a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));
        Debug.Log($"Found {allSlots.Length} slots.");
    }


    IEnumerator SpinSequence()
    {
        spinButton.HideButton();

        if (currentHighlightedIndex >= 0)
            allSlots[currentHighlightedIndex].SetHighlighted(false);
        currentHighlightedIndex = -1;
        int totalSpins = (int)(minimumSpins + Random.Range(0, maxExtraSpins));
        yield return StartCoroutine(AnimateSpin(totalSpins));
        yield return StartCoroutine(FlashLandedSlot());
        walletManager.AddFood(allSlots[currentHighlightedIndex].AssignedFood, allSlots[currentHighlightedIndex].RewardAmount);
        burstAnimator.PlayBurstAnimation(allSlots[currentHighlightedIndex].AssignedFood, allSlots[currentHighlightedIndex].transform);
        yield return new WaitForSeconds(winSeconds);
        allSlots[currentHighlightedIndex].SetDisabled(true);
        isSpinning = false;
        yield return new WaitForSeconds(0.1f);
        spinButton.ShowButton();
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

        while (allSlots[currentHighlightedIndex].IsDisabled)
        {
            if (currentHighlightedIndex >= 0)
                allSlots[currentHighlightedIndex].SetHighlighted(false);

            currentHighlightedIndex = (currentHighlightedIndex + 1) % allSlots.Length;
            allSlots[currentHighlightedIndex].SetHighlighted(true);

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

        landedSlot.SetWin();
        landedSlot.AnimateTickIcon();
    }

    public void StartSpin()
    {
        if (!isSpinning)
        {
            isSpinning = true;
            StartCoroutine(SpinSequence());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var collectedFoods = walletManager.GetAllFoods();
            Debug.Log($"Wallet contains {collectedFoods.Count} items:");
            foreach (var food in collectedFoods)
            {
                Debug.Log($"- {food.Key}: {food.Value}");
            }
        }
    }
}
