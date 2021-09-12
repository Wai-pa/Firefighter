using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Effects;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [Header("Level Settings")]
    [SerializeField] private GameObject flamePrefab;
    [SerializeField] private int level = 1;
    [SerializeField] private int score = 0;
    [SerializeField] private double waterLevel;
    [SerializeField] private double buildingDamage;
    [SerializeField] private int points = 150;
    [SerializeField] private double levelMultiplier;
    [SerializeField] private bool levelIsOver = false;

    [Header("Level Miscelaneous")]
    [SerializeField] private int inactiveFlames = 14;
    [SerializeField] private int activeFlames = 0;
    public int extinguishedFlames = 0;
    [SerializeField] private List<GameObject> instantiatedFlames = new List<GameObject>();
    [SerializeField] private List<Vector3> inactiveFlamesPosition = new List<Vector3>();
    [SerializeField] private List<Vector3> activeFlamesPosition = new List<Vector3>();
    private Vector3 flamesRotation = new Vector3(-90, 0, 0);

    [Header("Time Tracker")]
    [SerializeField] private float timerSeconds = 0.0f;
    [SerializeField] private int timerMinutes = 0;
    [SerializeField] private float timerSecondsSupport = 0.0f;
    [SerializeField] private float timeToInstantiateFlame = 20.0f;

    [Header("Panels")]
    [SerializeField] private GameObject levelIsOverPanel;
    [SerializeField] private GameObject levelFailedPanel;
    [SerializeField] private Text levelTxt;
    [SerializeField] private Text waterLevelTxt;
    [SerializeField] private Text buildingDamageTxt;
    [SerializeField] private Text totalTimeTxt;
    [SerializeField] private Text levelScoreTxt;

    [Header("UI Settings")]
    [SerializeField] private Text flamesTxt;
    [SerializeField] private Text hoseTxt;
    [SerializeField] private Text damageTxt;
    [SerializeField] private Text timeSecondsTxt;
    [SerializeField] private Text timeMinutesTxt;
    [SerializeField] private Text scoreTxt;
    private string percentageTxt = " %";
    private string preTextScore = "Score: ";

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else if(instance != this)
        {
            Destroy (gameObject);
        }
    }

    void Start()
    {
        StandardValues();
        SetFlamesPosition();
        FlamesFirstInstantiation(1);
    }

    void Update()
    {
        timerSeconds += Time.deltaTime;
        timerSecondsSupport += Time.deltaTime;

        if(timerSeconds >= 60)
        {
            timerMinutes++;
            timerSeconds = 0.0f;
        }

        if (Input.GetMouseButton(0))
        {
            waterLevel++;
            waterLevel -= levelMultiplier;
        }

        UpdateTimeUI();
        UpdateFlamesUI();
        UpdateHoseUI();
        UpdateDamageUI();

        if (!levelIsOver)
        {
            FlamesOverTimeInstantiation();
        }
        
        if(activeFlames >= 14 || buildingDamage >= 10000 || waterLevel >= 10000)
        {
            GameOver();
        }
    }

    void StandardValues()
    {
        levelMultiplier = 0.9f;
        points = 150;
    }

    public void AddScore()
    {
        activeFlames--;
        score += points;
        UpdateScoreUI();

        if (activeFlames <= 0)
        {
            LevelIsOver();
        }
    }

    public void LevelIsOver()
    {
        level++;
        levelIsOver = true;
        StartCoroutine(LevelOverPanelCoroutine());
    }

    public void UpdateFlamesUI()
    {
        flamesTxt.text = (activeFlames * 100 / 14).ToString() + percentageTxt;
    }

    public void UpdateHoseUI()
    {
        hoseTxt.text = (waterLevel * 1.4f / 100).ToString("f2") + percentageTxt;
    }

    public void UpdateDamageUI()
    {
        damageTxt.text = (CalculateDamage() / 100).ToString("f2") + percentageTxt;
    }

    public void UpdateTimeUI()
    {
        timeMinutesTxt.text = timerMinutes.ToString();
        timeSecondsTxt.text = ":" + timerSeconds.ToString("f2");
    }

    public void UpdateScoreUI()
    {
        scoreTxt.text = preTextScore + score.ToString("D8");
    }

    public void ClearFlames()
    {
        foreach(Vector3 v in activeFlamesPosition)
        {
            inactiveFlamesPosition.Add(v);
        }

        activeFlamesPosition.Clear();

        for (int i = 0; i < instantiatedFlames.Count; i++)
        {
            instantiatedFlames[i].GetComponent<ExtinguishableParticleSystem>().DestroyIteself();
        }

        instantiatedFlames.Clear();

        inactiveFlames = 14;
        activeFlames = 0;
        extinguishedFlames = -1;
    }

    public void SetFlamesPosition()
    {
        inactiveFlamesPosition.Add(new Vector3(-2.92f, 0.97f, -5.135f));
        inactiveFlamesPosition.Add(new Vector3(-4.81f, 0.97f, -5.135f));
        inactiveFlamesPosition.Add(new Vector3(2.872f, 0.97f, -5.135f));
        inactiveFlamesPosition.Add(new Vector3(4.704f, 0.97f, -5.135f));
        inactiveFlamesPosition.Add(new Vector3(4.704f, 3.97f, -5.135f));
        inactiveFlamesPosition.Add(new Vector3(-2.92f, 3.97f, -5.135f));
        inactiveFlamesPosition.Add(new Vector3(-4.81f, 3.97f, -5.135f));
        inactiveFlamesPosition.Add(new Vector3(2.872f, 3.97f, -5.135f));
        inactiveFlamesPosition.Add(new Vector3(4.704f, 6.65f, -5.14f));
        inactiveFlamesPosition.Add(new Vector3(-2.92f, 6.65f, -5.14f));
        inactiveFlamesPosition.Add(new Vector3(-4.81f, 6.65f, -5.14f));
        inactiveFlamesPosition.Add(new Vector3(2.872f, 6.65f, -5.14f));
        inactiveFlamesPosition.Add(new Vector3(0.87f, 6.65f, -5.14f));
        inactiveFlamesPosition.Add(new Vector3(-0.96f, 6.65f, -5.14f));
    }

    void FlamesFirstInstantiation(int level)
    {
        levelIsOver = false;

        for (int i = 0; i <= level + 1; i++)
        {
            int rndm = Random.Range(0, inactiveFlames - 1);
            var cloneFlame = Instantiate(flamePrefab, inactiveFlamesPosition[rndm], Quaternion.Euler(flamesRotation));
            instantiatedFlames.Add(cloneFlame);
            activeFlames++;
            inactiveFlames--;
            activeFlamesPosition.Add(inactiveFlamesPosition[rndm]);
            inactiveFlamesPosition.Remove(inactiveFlamesPosition[rndm]);
            Debug.Log(level);
        }
    }

    void FlamesOverTimeInstantiation()
    {
        if (timerSecondsSupport >= timeToInstantiateFlame)
        {
            int rndm = Random.Range(0, inactiveFlames - 1);
            var cloneFlame = Instantiate(flamePrefab, inactiveFlamesPosition[rndm], Quaternion.Euler(flamesRotation));
            instantiatedFlames.Add(cloneFlame);
            activeFlames++;
            inactiveFlames--;
            activeFlamesPosition.Add(inactiveFlamesPosition[rndm]);
            inactiveFlamesPosition.Remove(inactiveFlamesPosition[rndm]);
            timerSecondsSupport = 0;
        }
    }

    double CalculateDamage()
    {
        buildingDamage = timerSeconds * activeFlames * 10 * activeFlames;
        buildingDamage /= levelMultiplier;

        return buildingDamage;
    }

    public void ShowPlayerPerformance()
    {
        levelTxt.text = "Level: " + level.ToString();
        levelScoreTxt.text = "Score: " + score.ToString();
        waterLevelTxt.text = "Water Level: " + (waterLevel / 100).ToString("f2") + percentageTxt;
        buildingDamageTxt.text = "Building Damage: " + ((14 - extinguishedFlames) / 100 + CalculateDamage() / 100).ToString("f2") + percentageTxt;
        totalTimeTxt.text = "Time: " + timerMinutes.ToString() + ":" + timerSeconds.ToString("f2");
    }

    IEnumerator LevelOverPanelCoroutine()
    {
        levelIsOverPanel.SetActive(true);
        ShowPlayerPerformance();
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(4);
        levelIsOverPanel.SetActive(false);
        Time.timeScale = 1;

        if (level > 4)
        {
            SceneManager.LoadScene("SampleScene");
        }

        else
        {
            timerSecondsSupport = 0.0f;
            timerSeconds = 0.0f;
            timerMinutes = 0;
            score = 0;
            waterLevel = 0;

            levelMultiplier = (1 - (0.1 * level));
            timeToInstantiateFlame -= 3.5f;
            points *= level;

            ClearFlames();
            FlamesFirstInstantiation(level);
            UpdateScoreUI();
        }
    }

    public void GameOver()
    {
        StartCoroutine(GameOverPanelCoroutine());
    }

    IEnumerator GameOverPanelCoroutine()
    {
        levelFailedPanel.SetActive(true);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(4);
        levelFailedPanel.SetActive(false);
        Time.timeScale = 1;
        level = 10;
        SceneManager.LoadScene("SampleScene");
    }

    public bool GameIsOver()
    {
        return (level > 4 ? true : false);
    }
}
