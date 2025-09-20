using UnityEngine;

[CreateAssetMenu(fileName = "New Slot Configuration", menuName = "Slot Configuration")]
public class SlotConfiguration : ScriptableObject
{
    [Header("Visual Sprites")]
    public Sprite normalSlotSprite;
    public Sprite highlightedSlotSprite;
    public Sprite disabledSlotSprite;
    public Sprite winSlotSprite;

    [Header("Animation Settings")]
    public float tickSeconds = 0.8f;
}