using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Effects;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    //UI Manager
    public Text flamesTxt;
    public Text hoseTxt;
    public Text damageTxt;
    public Text timeSecondsTxt;
    public Text timeMinutesTxt;
    public Text scoreTxt;
    public string percentage = " %";

    //Level Manager
    public float timerSeconds = 0.0f;
    public int timerMinutes = 0;
    public float timerSecondsSupport = 0.0f;
    [SerializeField] private int numberOfFlames = 0;
    [SerializeField] private float timeToInstantiateFlame = 20.0f;
    public List<GameObject> inactiveFlames = new List<GameObject>();
    public List<GameObject> activeFlames = new List<GameObject>();
    public List<GameObject> extinguishedFlames = new List<GameObject>();

    public List<Vector3> flamesPosition = new List<Vector3>();

    //Sandbox
    [SerializeField] private int level = 1;
    [SerializeField] private int score = 0;
    public double levelMultiplier;
    public string preTextScore = "Score: ";
    const int points = 150;
    [SerializeField] private double waterLevel;
    [SerializeField] private bool levelIsOver = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy (gameObject);
        }
    }

    void Start()
    {
        UpdateScoreUI();
        StandardValues();
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

        if (!levelIsOver)
        {
            FlamesOverTimeInstantiation(level);
        }
    }

    void StandardValues()
    {
        levelMultiplier = 0.9f;
    }

    void LevelManagement()
    {

    }

    void FlamesFirstInstantiation(int level)
    {
        for(int i = 0; i <= level + 1; i++)
        {
            int rndm = Random.Range(0, inactiveFlames.Count-1);
            inactiveFlames[rndm].SetActive(true);
            activeFlames.Add(inactiveFlames[rndm]);
            inactiveFlames.Remove(inactiveFlames[rndm]);

            /*
            if(level > 1)
            {
                Debug.Log("flames first instantiation level 2");
                activeFlames[rndm].GetComponent<ExtinguishableParticleSystem>().RenableFlame();
            }
            */
        }
    }

    void FlamesOverTimeInstantiation(int level)
    {
        if (timerSecondsSupport >= timeToInstantiateFlame)
        {
            int rndm = Random.Range(0, inactiveFlames.Count - 1);
            inactiveFlames[rndm].SetActive(true);
            activeFlames.Add(inactiveFlames[rndm]);
            inactiveFlames.Remove(inactiveFlames[rndm]);
            timerSecondsSupport = 0;

            if (level > 1)
            {
                activeFlames[rndm].GetComponent<ExtinguishableParticleSystem>().RenableFlame();
            }
        }
    }

    public void AddFlames()
    {
        numberOfFlames++;
    }

    public void AddScore()
    {
        numberOfFlames--;
        score += points;
        UpdateScoreUI();

        if (numberOfFlames <= 0)
        {
            LevelIsOver();
        }
    }

    public void LevelIsOver()
    {
        levelIsOver = true;
        timerSecondsSupport = 0.0f;
        timerSeconds = 0.0f;
        timerMinutes = 0;

        level++;
        levelMultiplier = (1 - (0.1 * level));
        timeToInstantiateFlame -= 3.5f;

        score = 0;
        waterLevel = 0;

        ClearFlamesLists();

        /*
        foreach (GameObject g in inactiveFlames)
        {
            g.GetComponent<ExtinguishableParticleSystem>().RenableFlame();
        }
        */

        FlamesFirstInstantiation(level);
    }

    public void UpdateFlamesUI()
    {
        flamesTxt.text = (numberOfFlames * 100 / 14).ToString() + percentage;
    }

    public void UpdateHoseUI()
    {
        hoseTxt.text = (waterLevel / 100).ToString("f2") + percentage;
    }

    public void UpdateDamageUI()
    {

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

    public void ClearFlamesLists()
    {
        foreach (GameObject g in extinguishedFlames)
        {
            inactiveFlames.Add(g);
        }

        //just in case
        foreach (GameObject g in activeFlames)
        {
            inactiveFlames.Add(g);
        }

        foreach(GameObject g in inactiveFlames)
        {
            g.SetActive(false);
        }

        activeFlames.Clear();
        extinguishedFlames.Clear();
    }

    public void SetFlamesPosition()
    {
        flamesPosition.Add(new Vector3(-2.92f, 0.97f, -5.135f));
        flamesPosition.Add(new Vector3(-4.81f, 0.97f, -5.135f));
        flamesPosition.Add(new Vector3(2.872f, 0.97f, -5.135f));
        flamesPosition.Add(new Vector3(4.704f, 0.97f, -5.135f));
        flamesPosition.Add(new Vector3(4.704f, 3.97f, -5.135f));
        flamesPosition.Add(new Vector3(-2.92f, 3.97f, -5.135f));
        flamesPosition.Add(new Vector3(-4.81f, 3.97f, -5.135f));
        flamesPosition.Add(new Vector3(2.872f, 3.97f, -5.135f));
        flamesPosition.Add(new Vector3(4.704f, 6.65f, -5.14f));
        flamesPosition.Add(new Vector3(-2.92f, 6.65f, -5.14f));
        flamesPosition.Add(new Vector3(-4.81f, 6.65f, -5.14f));
        flamesPosition.Add(new Vector3(2.872f, 6.65f, -5.14f));
        flamesPosition.Add(new Vector3(0.87f, 6.65f, -5.14f));
        flamesPosition.Add(new Vector3(-0.96f, 6.65f, -5.14f));
    }
}
