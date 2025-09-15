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

    void Start()
    {
        allSlots = slotsParent.GetComponentsInChildren<RouletteSlot>();
        System.Array.Sort(allSlots, (a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));
        Debug.Log($"Found {allSlots.Length} slots.");
    }


    IEnumerator SpinSequence()
    {
        int targetSlotIndex = Random.Range(0, allSlots.Length);
        int totalSpins = (int)(minimumSpins + Random.Range(0, maxExtraSpins));
        int slotsToAdd = (targetSlotIndex - (totalSpins % allSlots.Length) + allSlots.Length) % allSlots.Length;
        totalSpins += slotsToAdd;

        yield return StartCoroutine(AnimateSpin(totalSpins, targetSlotIndex));
        isSpinning = false;
        Debug.Log($"Landed on slot: {allSlots[targetSlotIndex].name}");
    }

    IEnumerator AnimateSpin(int totalSpins, int targetSlotIndex)
    {
        float spinSpeed = baseSpinSpeed;
        for (int i = 0; i < totalSpins; i++)
        {
            if (currentHighlightedIndex >= 0)
                allSlots[currentHighlightedIndex].SetHighlighted(false);

            currentHighlightedIndex = (currentHighlightedIndex + 1) % allSlots.Length;
            allSlots[currentHighlightedIndex].SetHighlighted(true);

            // Gradually slow down
            if (i > totalSpins * 0.7f)
                spinSpeed += baseSpinSpeed * 0.1f;

            yield return new WaitForSeconds(spinSpeed);
        }

        allSlots[currentHighlightedIndex].SetHighlighted(true);
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
    }
}
