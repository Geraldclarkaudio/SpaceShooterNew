using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    private SpawnManager _spawnManager;
    private UIManager _uiManager; 

    public GameInput _input;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _speedMultiplier;
  

    //Laser Stuff
    [Header("Laser Stuff")]
    [SerializeField]
    private GameObject laserPrefab;
    [SerializeField]
    private GameObject tripleShotPrefab;
    private float _canFire = -1f;
    [SerializeField]
    private float fireRate = 0.1f;
    private bool fired;

    [Header("Lives")]
    [SerializeField]
    public int _lives;
    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    private GameObject[] engines;

    [Header("PowerUps")]
    [SerializeField]
    private GameObject shields;
    public bool isTripleShotEnabled = false;
    public bool isShieldActive = false;
    public bool isSpeedACtive = false;
    public GameObject Thruster;

    [SerializeField]
    private int score;

    void Start()
    {
        _lives = 3; 
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _input = new GameInput();
        _input.PlayerControls.Enable();
        _input.PlayerControls.Fire.performed += Fire_performed;
        _input.PlayerControls.Restart.performed += Restart_performed;
        _input.PlayerControls.Thruster.performed += Thruster_performed;
        _input.PlayerControls.Thruster.canceled += Thruster_canceled;

        if (_uiManager == null)
        {
            Debug.LogError("UI MAnager is null");
        }

        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }    
    }

    private void Thruster_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Not Thrusting anymore");
        Thruster.SetActive(false);
    }

    private void Thruster_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Thrusting");
        Thruster.SetActive(true);
    }

    private void Restart_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if(GameManager.Instance.isGameOver == true)
        {
            RestartGame();
        }
        else
        {
            return;
        }
        
    }

    private void Fire_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            fired = true;

            if(fired ==true && Time.time > _canFire)
            {
                Fire();
            }
        }
    }

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        //poll or check input readings 
        var move = _input.PlayerControls.Move.ReadValue<Vector2>();

        transform.Translate(move * _speed * Time.deltaTime);

        //set bounds: X Axis
        if(transform.position.x <= -16f)
        {
            transform.position = new Vector3(15.9f, transform.position.y, transform.position.z);
        }

        else if(transform.position.x >= 16f)
        {
            transform.position = new Vector3(-15.9f, transform.position.y, transform.position.z);
        }

        //Set Bounds: Y Axis
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -6.5f, 0));
    }

    private void Fire()
    {
        _canFire = Time.time + fireRate;

        if (isTripleShotEnabled == true)
        {
            fired = false;
            Instantiate(tripleShotPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        }
        else
        {
            fired = false;
            Instantiate(laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        }
    }

    public void Damage()
    {
        if(isShieldActive == true)
        {
            //deactivateshields
            isShieldActive = false;
            shields.SetActive(false);
            return;
        }

        _lives--;

        if (_lives == 2)
        {
            engines[0].SetActive(true);
        }

        if(_lives == 1)
        {
            engines[1].SetActive(true);
        }

        _uiManager.UpdateLives(_lives);


        //You dead boi
        if(_lives <= 0)
        {
            _spawnManager.PlayerDied();
            GameManager.Instance.isGameOver = true;
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public void ScoreKeeper(int points)
    {
        score += points;
        _uiManager.UpdateScore(score);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SpaceShooter");
    }


    #region POWERUPS
    public void TripleShotActive()
    {
        isTripleShotEnabled = true;
        StartCoroutine(TripleShotCoolDown());
    }

    private IEnumerator TripleShotCoolDown()
    {
        yield return new WaitForSeconds(5.0f);
        isTripleShotEnabled = false;
    }

    public void SpeedActive()
    {
        _speed *= _speedMultiplier;
        isSpeedACtive = true;
        StartCoroutine(SpeedCoolDown());
    }

    private IEnumerator SpeedCoolDown()
    {
        yield return new WaitForSeconds(5f);
        isSpeedACtive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        isShieldActive = true;
        shields.SetActive(true);
    }

    public void AddHealth()
    {

        if (_lives == 3)
        {
            return;
        }
        if(_lives == 2)
        {
            _lives++;
            _uiManager.UpdateLives(_lives);
            engines[0].SetActive(false);
        }

        if(_lives == 1)
        {
            _lives++;
            _uiManager.UpdateLives(_lives);
            engines[1].SetActive(false);
        }

    }

    #endregion


}
