using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;

    [SerializeField]
    private Sprite[] livesSprites;
    [SerializeField]
    private Image liveImage;

    [SerializeField]
    private GameObject gameOverText;
    [SerializeField]
    private GameObject restartText;

    private Player player; 
    // Start is called before the first frame update
    void Start()
    {
        liveImage.material.color = Color.white;
        scoreText.text = "Score: " + 0;
        gameOverText.SetActive(false);
        restartText.SetActive(false);
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    public void UpdateScore(int playerScore)
    {
        scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        liveImage.sprite = livesSprites[currentLives];
        if(currentLives == 0)
        {
            liveImage.material.color = Color.red;
            StartCoroutine(TextFlicker());
            restartText.SetActive(true);
        }
    }

    private IEnumerator TextFlicker()
    {
        while(player._lives == 0)
        {
            gameOverText.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            gameOverText.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }

    }
}
