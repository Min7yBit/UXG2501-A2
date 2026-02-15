using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonSFX : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(PlayClickSFX);
    }

    private void PlayClickSFX()
    {
        if (GlobalSFX.Instance != null)
        {
            GlobalSFX.Instance.PlayUIClick();
        }
        else
        {
            Debug.LogWarning("[UIButtonSFX] GlobalSFX.Instance is null – no click sound will play.");
        }
    }
}
