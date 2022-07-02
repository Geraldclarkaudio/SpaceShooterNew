using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject plainEnemy;
    [SerializeField]
    private GameObject enemyContainer;

    [SerializeField]
    private GameObject powerUpContainer;
    [SerializeField]
    private GameObject[] powerups;
    [SerializeField]
    private int[] puWeights;

    public bool playerDied = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnPlainEnemy());
        StartCoroutine(PowerUpSpawner());
    }
    public void PlayerDied()
    {
        playerDied = true;
        Destroy(enemyContainer);
    }

    IEnumerator PowerUpSpawner()
    {
        yield return new WaitForSeconds(2.0f); 
        while (playerDied == false)
        {
            Vector3 posToSPawn = new Vector3(Random.Range(-15f, 15f), 9, 0);
            int randomPowerUp = Random.Range(0, 5);
            // GameObject newPowerUp = Instantiate(powerups[randomPowerUp], posToSPawn, Quaternion.identity);
            GameObject newPowerUp = Instantiate(powerups[GetRandomPowerUp(puWeights)], posToSPawn, Quaternion.identity);
            newPowerUp.transform.parent = powerUpContainer.transform;
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }

    IEnumerator SpawnPlainEnemy()
    {
        yield return new WaitForSeconds(2.0f);

        while (playerDied == false)
        {
            Vector3 posToSPawn = new Vector3(Random.Range(-15f, 15f), 9, 0);
            GameObject newEnemy = Instantiate(plainEnemy, posToSPawn, Quaternion.identity);
            newEnemy.transform.parent = enemyContainer.transform;
            yield return new WaitForSeconds(2.5f);
        }
    }

    public int GetRandomPowerUp(int[] Weights)
    {
        int sumOfWeights = 0;
        int randNum;

        for (int i = 0; i < powerups.Length; i++)
        {
            sumOfWeights += Weights[i];
        }

        randNum = Random.Range(0, sumOfWeights);

        for (int i = 0; i < puWeights.Length; i++)
        {
            if (randNum < puWeights[i])
            {
                return i;
            }

            randNum -= puWeights[i];
        }

        return -1;
    }




}
