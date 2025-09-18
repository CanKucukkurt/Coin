using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using DG.Tweening;

public class RouletteSlot : MonoBehaviour
{
    [SerializeField] private Sprite normalSlotSprite;
    [SerializeField] private Sprite highlightedSlotSprite;
    [SerializeField] private Sprite disabledSlotSprite;
    [SerializeField] private Sprite winSlotSprite;
    private UnityEngine.UI.Image slotImage;
    [SerializeField] private UnityEngine.UI.Image foodIcon;
    [SerializeField] private UnityEngine.UI.Image tickIcon;

    [SerializeField] private int slotIndex;
    [SerializeField] private FoodItem assignedFood;
    [SerializeField] private int rewardAmount = 1;
    [SerializeField] private int visualAmount = 4;
    [SerializeField] private float tickSeconds = 0.5f;




    public bool isHighlighted = false;
    public bool isDisabled = false;

    public int SlotIndex => slotIndex;
    public bool IsHighlighted => isHighlighted;
    public bool IsDisabled => isDisabled;

    public FoodItem AssignedFood => assignedFood;

    public int RewardAmount => rewardAmount;
    public int VisualAmount => visualAmount;

    void Start()
    {
        tickIcon.gameObject.SetActive(false);
        tickIcon.fillAmount = 0f;
        slotImage = GetComponent<UnityEngine.UI.Image>();
        slotImage.sprite = normalSlotSprite;
        AssignFood(assignedFood);
    }

    public void SetSlotIndex(int index)
    {
        slotIndex = index;
    }

    public void SetHighlighted(bool highlighted)
    {
        isHighlighted = highlighted;
        if (isDisabled) slotImage.sprite = highlighted ? highlightedSlotSprite : disabledSlotSprite;
        else slotImage.sprite = highlighted ? highlightedSlotSprite : normalSlotSprite;
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

    public void SetWin()
    {
        slotImage.sprite = winSlotSprite;
    }


    public void AnimateTickIcon()
    {
        if (tickIcon != null)
        {
            tickIcon.gameObject.SetActive(true);
            tickIcon.fillAmount = 0f;
            tickIcon.DOFillAmount(1f, 0.3f).SetEase(Ease.OutQuad);
        }
    }

    public void HideTickIcon()
    {
        if (tickIcon != null)
        {
            tickIcon.gameObject.SetActive(false);
            tickIcon.fillAmount = 0f;
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
