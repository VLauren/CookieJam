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
    }

    void OnAnyKey(InputValue value)
    {
        if (!storyShown)
        {
            Debug.Log("Showing the story");
            ShowPanel(storyGroup);
            storyShown = true;
            return;
        }
        if (!controlsShown)
        {
            Debug.Log("Showing the controls");
            ShowPanel(controlsInfoGroup);
            controlsShown = true;
            return;
        }
        Debug.Log("Loading first level");
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
