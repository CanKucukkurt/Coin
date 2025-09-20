using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using DG.Tweening;

public class RouletteSlot : MonoBehaviour
{
    private UnityEngine.UI.Image slotImage;
    [SerializeField] private UnityEngine.UI.Image foodIcon;
    [SerializeField] private UnityEngine.UI.Image tickIcon;

    [SerializeField] private int slotIndex;
    [SerializeField] private FoodItem assignedFood;

    [SerializeField] private SlotConfiguration slotConfig;
    [SerializeField] private int rewardAmount = 1;
    [SerializeField] private int visualAmount = 4;
    [SerializeField] private TMPro.TextMeshProUGUI rewardAmountText;




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
        slotImage.sprite = slotConfig.normalSlotSprite;
        AssignFood(assignedFood);
        UpdateRewardAmountDisplay();
    }

    public void SetSlotIndex(int index)
    {
        slotIndex = index;
    }

    public void SetHighlighted(bool highlighted)
    {
        isHighlighted = highlighted;
        if (isDisabled) slotImage.sprite = highlighted ? slotConfig.highlightedSlotSprite : slotConfig.disabledSlotSprite;
        else slotImage.sprite = highlighted ? slotConfig.highlightedSlotSprite : slotConfig.normalSlotSprite;
    }

    public void SetDisabled(bool disabled)
    {
        isDisabled = disabled;
        if (isDisabled)
        {
            slotImage.sprite = slotConfig.disabledSlotSprite;
        }
        else
        {
            slotImage.sprite = isHighlighted ? slotConfig.highlightedSlotSprite : slotConfig.normalSlotSprite;
        }
    }

    public void SetWin()
    {
        slotImage.sprite = slotConfig.winSlotSprite;
    }


    public void AnimateTickIcon()
    {
        if (tickIcon != null)
        {
            tickIcon.gameObject.SetActive(true);
            tickIcon.fillAmount = 0f;
            tickIcon.DOFillAmount(1f, slotConfig.tickSeconds).SetEase(Ease.OutQuad);
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

    public async void AssignFood(FoodItem food)
    {
        assignedFood = food;

        if (foodIcon != null && food != null)
        {
            // Wait for sprite to be loaded if it's not ready yet
            if (!food.IsSpriteLoaded)
            {
                await food.LoadSpriteAsync();
            }

            foodIcon.sprite = food.itemSprite;
            foodIcon.gameObject.SetActive(true);
        }
    }

    private void UpdateRewardAmountDisplay()
    {
        if (rewardAmountText != null && rewardAmount > 1)
        {
            rewardAmountText.text = rewardAmount.ToString();
        }
    }
}
