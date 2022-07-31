using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panel Groups")]
    [SerializeField] GameObject mainMenuGroup;
    [SerializeField] GameObject storyGroup;
    [SerializeField] GameObject controlsInfoGroup;

    bool storyShown;
    bool controlsShown;

    private void Start()
    {
        storyShown = false;
        controlsShown = false;

        AudioManager.Stop("galleta_music_test2");
        AudioManager.Stop("galleta_music_test2_capa_extra");
    }

    void OnAnyKey(InputValue value)
    {
        if (!storyShown)
        {
            AudioManager.Play("checkpoint", false, 1);
            ShowPanel(storyGroup);
            storyShown = true;
            return;
        }
        if (!controlsShown)
        {
            // AudioManager.Play("checkpoint", false, 1);
            // ShowPanel(controlsInfoGroup);
            // controlsShown = true;

            // HACK mientras no tengamos img de ctrles
            AudioManager.Play("music_ganar", false, 0.75f);
            CJGame.LoadFirstLevel();

            return;
        }
        AudioManager.Play("music_ganar", false, 0.75f);
        CJGame.LoadFirstLevel();
    }

    private void HideAllPanels()
    {
        mainMenuGroup.SetActive(false);
        storyGroup.SetActive(false);
        controlsInfoGroup.SetActive(false);
    }

    private void ShowPanel(GameObject panel)
    {
        HideAllPanels();
        panel.SetActive(true);
    }
}
