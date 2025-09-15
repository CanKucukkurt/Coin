using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class RouletteSlot : MonoBehaviour
{
    [SerializeField] private Sprite normalSlotSprite;
    [SerializeField] private Sprite highlightedSlotSprite;
    [SerializeField] private Sprite disabledSlotSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int slotIndex;

    public bool isHighlighted = false;
    public bool isDisabled = false;

    public int SlotIndex => slotIndex;
    public bool IsHighlighted => isHighlighted;
    public bool IsDisabled => isDisabled;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalSlotSprite;
    }

    public void SetSlotIndex(int index)
    {
        slotIndex = index;
    }

    public void SetHighlighted(bool highlighted)
    {
        if (isDisabled) return;
        isHighlighted = highlighted;
        spriteRenderer.sprite = highlighted ? highlightedSlotSprite : normalSlotSprite;
    }

    public void SetDisabled(bool disabled)
    {
        isDisabled = disabled;
        if (isDisabled)
        {
            spriteRenderer.sprite = disabledSlotSprite;
        }
        else
        {
            spriteRenderer.sprite = isHighlighted ? highlightedSlotSprite : normalSlotSprite;
        }
    }
}
