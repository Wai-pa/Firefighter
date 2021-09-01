using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Effects;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private int score = 0;
    public string preTextScore = "Score: ";
    const int points = 150;
    public Text txtScore;

    //Level Manager
    private int flames = 0;
    [SerializeField] private GameObject[] levels;
    private int currentLevel = 0;
    private GameObject currentBoard;

    //private ExtinguishableParticleSystem exting;

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

    // Start is called before the first frame update
    void Start()
    {
        //exting = GameObject.FindGameObjectWithTag("Flame").GetComponent<ExtinguishableParticleSystem>();

        txtScore.text = preTextScore + score.ToString("D8");
    }

    public void AddFlames()
    {
        flames++;
    }

    public void AddScore()
    {
        flames--;
        score += points;
        txtScore.text = preTextScore + score.ToString("D8");

        if(flames <= 0)
        {
            Debug.Log("deu certo");
        }

    }

    // Update is called once per frame
    void Update()
    {
        //this is a test
    }
}
