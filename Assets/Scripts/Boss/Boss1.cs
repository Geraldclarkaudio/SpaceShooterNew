using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class Boss1 : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private Transform[] waypoints;

    [SerializeField]
    private Transform target;

    private BossDialog bossDialog;

    public int i;

    public GameObject laserPrefab;
    public GameObject beamPrefab;

    private Player player;

    private Vector3 leftFire;
    private Vector3 rightFire;


    // Start is called before the first frame update
    void Start()

    {
        player = GameObject.Find("Player").GetComponent<Player>();
        bossDialog = GetComponent<BossDialog>();
        transform.position = new Vector3(0, 16, 0); //Spawn above screen bounds.
        leftFire = new Vector3(-4, -4, 0);
        rightFire = new Vector3(4, -4, 0);

        Fire();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        target = waypoints[i];
        float distance = Vector3.Distance(transform.position, target.position);
        target.position = waypoints[i].position;
        transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);

        if (distance <= 0.5f)
        {
            if (target.position == waypoints[0].position)
            {
                i++;
            }

            if (target.position == waypoints[2].position)
            {
                i--;
            }

            if (target.position == waypoints[1].position)
            {
                i++;
            }
        }
    }

    public void Fire()
    {
        StartCoroutine(LeftFireRoutine());
        StartCoroutine(RightFireRoutine());
        StartCoroutine(FireBeamRoutine());

    }

    IEnumerator LeftFireRoutine()
    {
        while(GameManager.Instance.isGameOver == false)
        {
            yield return new WaitForSeconds(Random.Range(2.5f, 5));
            Instantiate(laserPrefab, transform.position + leftFire, Quaternion.identity);
        }
    }

    IEnumerator RightFireRoutine()
    {
        while (GameManager.Instance.isGameOver == false)
        {
            yield return new WaitForSeconds(Random.Range(3, 5));
            Instantiate(laserPrefab, transform.position + rightFire, Quaternion.identity);
        }
    }
    IEnumerator FireBeamRoutine()
    {
        while (GameManager.Instance.isGameOver == false)
        {
            yield return new WaitForSeconds(Random.Range(10,15));
            beamPrefab.SetActive(true);
            yield return new WaitForSeconds(5f);
            beamPrefab.SetActive(false);
            
        }
    }
}
