using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour
{
    public AudioSource tapAudio;
    public AudioSource dieAudio;
    public AudioSource scoreAudio;
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;
    public float tapForce = 10;
    public float tiltSmooth = 5;
    public Vector3 startPos;
    Rigidbody2D rb;
    Quaternion downRotation;
    Quaternion forwardRotation;

    GameManager gameManager;

    private void OnEnable() {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    private void OnDisable() {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;

    }

    void OnGameStarted(){
        rb.velocity = Vector2.zero;
        rb.simulated = true;
    }

    void OnGameOverConfirmed(){
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0,0,-90);
        forwardRotation = Quaternion.Euler(0,0,35);
        gameManager = GameManager.Instance;
        rb.simulated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GameOver){
            return;
        }

        if (Input.GetMouseButtonDown(0)){
            tapAudio.Play();
            transform.rotation = forwardRotation;
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
        }   

        transform.rotation = Quaternion.Lerp(transform.rotation,downRotation,tiltSmooth * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "ScoreZone"){
            OnPlayerScored();
            scoreAudio.Play();
        }

        if (other.gameObject.tag == "DeadZone"){
            rb.simulated = false;
            OnPlayerDied();
            dieAudio.Play();
        }
    }
}
