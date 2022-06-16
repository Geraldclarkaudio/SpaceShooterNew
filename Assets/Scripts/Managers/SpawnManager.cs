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
            int randomPowerUp = Random.Range(0, 4);
            GameObject newPowerUp = Instantiate(powerups[randomPowerUp], posToSPawn, Quaternion.identity);
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
}
