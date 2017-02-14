using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    public static GameManager instance;

    private ParticleSystem ps;
    private Salt salt;
    public SteakController[] steaks;

    public float timeBetween = 6f;
    private int numberOfFails; 
    private int score = 0; 
    private int highScore = 0; 

    public int NumberOfFails{ get {return numberOfFails;} set { numberOfFails = value;}}
    public int Score {get {return score;} set { score = value;} }

    [HeaderAttribute ("UI")]
    public Text scoreText;
    public Text highScoreText;
    public GameObject deathScreen;
    public GameObject mainMenu;
    public GameObject HUD;
    public GameObject[] failUI;

    public int numberOfGames;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        salt = GameObject.Find("Salt").GetComponent<Salt>();
        ps = GameObject.Find("ParticleSystem").GetComponent<ParticleSystem>();

        ps.Play();
        deathScreen.SetActive(false);

        if (PlayerPrefs.HasKey("BestScore"))
        {
            highScore = PlayerPrefs.GetInt("BestScore");
        }
    }

    public void Begin()
    {   
        

        numberOfFails = 0;
        score = 0;
        ps.Stop();

        steaks[3].gameObject.SetActive(true);

        foreach (GameObject image in failUI)
            image.SetActive(false);
        foreach (SteakController steak in steaks)
            steak.speed = 1.5f;

        StartCoroutine("TreadmillStart");
    }

    void Update()
    {
        scoreText.text = score.ToString();
        highScoreText.text = highScore.ToString();

        //Ads


        if (score > highScore)
        {
            highScore = score;
            highScoreText.text = highScore.ToString();
            PlayerPrefs.SetInt("BestScore", highScore);
        }

        switch (numberOfFails)
        {
            case 1:
                failUI[0].SetActive(true);
                break;
            case 2:
                failUI[1].SetActive(true);
                break;
            case 3:
                failUI[2].SetActive(true);
                break;
        }

        if (numberOfFails == 3)
        {
            foreach (SteakController steak in steaks)
            {
                steak.ResetStats();
                steak.speedUp = false;
                steak.gameObject.SetActive(false);
            }
            numberOfFails = 0;

            //Ads
            /*numberOfGames++;

            if(numberOfGames == 3)
            {
                numberOfGames = 0;
                StartCoroutine("GameOverAd");
            }*/

            deathScreen.SetActive(true);
            ps.Play();
            Invoke("BackToMainMenu", 3f);
        }
    }

    /*IEnumerator GameOverAd()
    {
        deathScreen.SetActive(true);
        ps.Play();
        yield return new WaitForSeconds(3f);
        //PlayAd.instance.ShowAd();
        Invoke("BackToMainMenu", 1);
    }*/

    void BackToMainMenu()
    {
        ps.Play();
        deathScreen.SetActive(false);
        HUD.SetActive(false);
        mainMenu.SetActive(true);
    }

    IEnumerator TreadmillStart()
    {
        yield return new WaitForSeconds(1.5f);
        steaks[0].gameObject.SetActive(true);
        yield return new WaitForSeconds(timeBetween);
        steaks[1].gameObject.SetActive(true);
        yield return new WaitForSeconds(timeBetween);
        steaks[2].gameObject.SetActive(true);
        yield return new WaitForSeconds(timeBetween);

        foreach (SteakController steak in steaks)
            steak.speedUp = true;
    }

    public void Salt()
    {
        ps.Play();
        salt.RealeaseSalt();
        Invoke("StopSalt", 1.5f);
    }

    public void StopSalt()
    {
        ps.Stop();
    }
}
