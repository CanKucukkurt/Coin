using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ItemAnimation : MonoBehaviour
{
    [SerializeField] private Transform backpackTarget;
    [SerializeField] private Canvas uiCanvas;
    [SerializeField] private int burstCount = 4;
    [SerializeField] private float burstRadius = 150f;
    [SerializeField] private float burstDuration = 0.5f;
    [SerializeField] private float flyDuration = 1f;

    public void PlayBurstAnimation(FoodItem item, Transform slotTransform)
    {
        Vector2 startPosition = slotTransform.position;

        for (int i = 0; i < burstCount; i++)
        {
            float delay = i * 0.1f;
            CreateAndAnimateItem(item, startPosition, slotTransform.GetComponent<RectTransform>(), delay);
        }
    }

    private void CreateAndAnimateItem(FoodItem item, Vector2 startPosition, RectTransform originalSlot, float flightDelay)
    {
        GameObject itemObj = new GameObject("BurstItem");
        itemObj.transform.SetParent(uiCanvas.transform, false);

        Image itemImage = itemObj.AddComponent<Image>();
        itemImage.sprite = item.itemSprite;

        RectTransform rectTransform = itemObj.GetComponent<RectTransform>();

        Image foodIcon = originalSlot.GetComponentInChildren<Image>();
        if (foodIcon != null && foodIcon.sprite == item.itemSprite)
        {
            rectTransform.sizeDelta = foodIcon.rectTransform.sizeDelta;
        }
        else
        {
            rectTransform.sizeDelta = new Vector2(50, 50);
        }

        rectTransform.position = startPosition;

        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector2 burstTarget = startPosition + randomDirection * burstRadius;
        Vector3 backpackPosition = backpackTarget.position;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(rectTransform.DOMove(burstTarget, burstDuration).SetEase(Ease.OutQuad));
        sequence.Append(rectTransform.DOMove(backpackPosition, flyDuration).SetEase(Ease.InQuad).SetDelay(flightDelay));
        sequence.OnComplete(() =>
        {
            backpackTarget.DOKill();
            backpackTarget.localScale = new Vector3(1.4f, 1.2f, 1.2f);
            backpackTarget.DOPunchScale(Vector3.one * 0.1f, 0.3f, 2, 0.5f);
            Destroy(itemObj);
        });
    }
}