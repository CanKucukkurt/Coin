using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class RouletteSlot : MonoBehaviour
{
    [SerializeField] private Sprite normalSlotSprite;
    [SerializeField] private Sprite highlightedSlotSprite;
    [SerializeField] private Sprite disabledSlotSprite;
    private UnityEngine.UI.Image slotImage;
    [SerializeField] private UnityEngine.UI.Image foodIcon;
    [SerializeField] private int slotIndex;
    private FoodItem assignedFood;


    public bool isHighlighted = false;
    public bool isDisabled = false;

    public int SlotIndex => slotIndex;
    public bool IsHighlighted => isHighlighted;
    public bool IsDisabled => isDisabled;

    public FoodItem AssignedFood => assignedFood;


    void Start()
    {
        slotImage = GetComponent<UnityEngine.UI.Image>();
        slotImage.sprite = normalSlotSprite;
    }

    public void SetSlotIndex(int index)
    {
        slotIndex = index;
    }

    public void SetHighlighted(bool highlighted)
    {
        if (isDisabled) return;
        isHighlighted = highlighted;
        slotImage.sprite = highlighted ? highlightedSlotSprite : normalSlotSprite;
    }

    public void SetDisabled(bool disabled)
    {
        isDisabled = disabled;
        if (isDisabled)
        {
            slotImage.sprite = disabledSlotSprite;
        }
        else
        {
            slotImage.sprite = isHighlighted ? highlightedSlotSprite : normalSlotSprite;
        }
    }


    public void AssignFood(FoodItem food)
    {
        assignedFood = food;

        if (foodIcon != null && food != null)
        {
            foodIcon.sprite = food.itemSprite;
            foodIcon.gameObject.SetActive(true);
        }
    }
}
