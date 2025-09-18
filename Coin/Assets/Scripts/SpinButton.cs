using UnityEngine;
using UnityEngine.UI;

public class SpinButton : MonoBehaviour
{
    [SerializeField] private Button spinButton;
    [SerializeField] private RouletteSpinManager spinManager;

    void Start()
    {
        spinButton = GetComponent<Button>();
        spinButton.onClick.AddListener(OnSpinButtonClicked);
    }

    private void OnSpinButtonClicked()
    {
        Debug.Log("Spin button clicked.");
        spinManager.StartSpin();
    }

    public void HideButton()
    {
        spinButton.gameObject.SetActive(false);
    }

    public void ShowButton()
    {
        spinButton.gameObject.SetActive(true);
    }

    void OnDestroy()
    {
        spinButton.onClick.RemoveListener(OnSpinButtonClicked);
    }
}