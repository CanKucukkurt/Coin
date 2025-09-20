using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    [SerializeField] private Button startButton;

    void Start()
    {
        if (startButton == null)
            startButton = GetComponent<Button>();

        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        Debug.Log("Start button clicked - Loading roulette scene");
        GameSceneManager.Instance?.LoadRouletteGame();
    }

    void OnDestroy()
    {
        if (startButton != null)
            startButton.onClick.RemoveListener(OnStartButtonClicked);
    }
}