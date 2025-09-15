using UnityEngine;
public class SlotTester : MonoBehaviour
{
    [SerializeField] private RouletteSlot[] allSlots;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Test highlighting random slot
            foreach (var slot in allSlots) slot.SetHighlighted(false);
            allSlots[Random.Range(0, allSlots.Length)].SetHighlighted(true);
        }
    }
}