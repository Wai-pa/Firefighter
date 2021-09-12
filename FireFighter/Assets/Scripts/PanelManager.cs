using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private GameObject introPanel;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject endPanel;

    private GameManager manager;

    void Start()
    {
        manager = GameManager.instance;

        introPanel.SetActive(true);
        infoPanel.SetActive(false);
        endPanel.SetActive(false);
    }

    void Update()
    {
        if (manager.GameIsOver())
        {
            introPanel.SetActive(false);
            infoPanel.SetActive(false);
            endPanel.SetActive(true);
            StartCoroutine(ExitCoroutine());
        }
    }

    IEnumerator ExitCoroutine()
    {
        yield return new WaitForSecondsRealtime(4);
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void OnPlay()
    {
        SceneManager.LoadScene("FireFighter");
    }

    public void OnInfo()
    {
        infoPanel.SetActive(true);
        introPanel.SetActive(false);
    }

    public void OnBack()
    {
        introPanel.SetActive(true);
        infoPanel.SetActive(false);
    }
}
