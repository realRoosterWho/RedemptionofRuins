using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(QuitGame);
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}