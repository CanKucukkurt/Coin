using System.Collections;
using UnityEngine;

public class RouletteSpinManager : MonoBehaviour
{
    [SerializeField] private Transform slotsParent;
    [SerializeField] private float regularSpinSpeed = 0.1f;
    [SerializeField] private float slowSpinSpeed = 0.3f;
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
        Debug.Log($"Landed on slot: {allSlots[currentHighlightedIndex].name}  Item: {allSlots[currentHighlightedIndex].AssignedFood.itemName}");

        if (AreAllSlotsDisabled())
        {
            Debug.Log("All slots are disabled. Resetting slots.");
            yield return new WaitForSeconds(1f);
            GameSceneManager.Instance.LoadMainMenu();
        }
        else
        {
            spinButton.ShowButton();
        }
    }

    IEnumerator AnimateSpin(int totalSpins)
    {
        for (int i = 0; i < totalSpins; i++)
        {
            if (currentHighlightedIndex >= 0)
                allSlots[currentHighlightedIndex].SetHighlighted(false);

            currentHighlightedIndex = (currentHighlightedIndex + 1) % allSlots.Length;
            allSlots[currentHighlightedIndex].SetHighlighted(true);

            float spinSpeed = (i >= totalSpins - 3) ? slowSpinSpeed : regularSpinSpeed;

            yield return new WaitForSeconds(spinSpeed);
        }

        while (allSlots[currentHighlightedIndex].IsDisabled)
        {
            if (currentHighlightedIndex >= 0)
                allSlots[currentHighlightedIndex].SetHighlighted(false);

            currentHighlightedIndex = (currentHighlightedIndex + 1) % allSlots.Length;
            allSlots[currentHighlightedIndex].SetHighlighted(true);

            yield return new WaitForSeconds(slowSpinSpeed);
        }
    }

    IEnumerator FlashLandedSlot()
    {
        RouletteSlot landedSlot = allSlots[currentHighlightedIndex];
        for (int i = 0; i < 6; i++)
        {
            landedSlot.SetHighlighted(false);
            yield return new WaitForSeconds(0.08f);
            landedSlot.SetHighlighted(true);
            yield return new WaitForSeconds(0.08f);
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

    public bool AreAllSlotsDisabled()
    {
        foreach (var slot in allSlots)
        {
            if (!slot.IsDisabled)
            {
                return false;
            }
        }
        return true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            walletManager.ClearWallet();
        }
    }
}