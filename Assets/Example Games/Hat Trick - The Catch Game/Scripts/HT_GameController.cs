using UnityEngine;
using System.Collections;

public class HT_GameController : MonoBehaviour
{
    public Camera cam;
    public GameObject[] balls;

    public float timeLeft;

    //public GUIText timerText;
    public GameObject gameOverText;
    public GameObject restartButton;
    public GameObject splashScreen;
    public GameObject startButton;
    public HT_HatController hatController;

    private float maxWidth;
    private bool counting;

    // Use this for initialization
    private void Start()
    {
        if (cam == null) cam = Camera.main;
        var upperCorner = new Vector3(Screen.width, Screen.height, 0.0f);
        var targetWidth = cam.ScreenToWorldPoint(upperCorner);
        var ballWidth = balls[0].GetComponent<Renderer>().bounds.extents.x;
        maxWidth = targetWidth.x - ballWidth;
        //timerText.text = "TIME LEFT:\n" + Mathf.RoundToInt (timeLeft);
    }

    private void FixedUpdate()
    {
        if (counting)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0) timeLeft = 0;
            //timerText.text = "TIME LEFT:\n" + Mathf.RoundToInt (timeLeft);
        }
    }

    public void StartGame()
    {
        splashScreen.SetActive(false);
        startButton.SetActive(false);
        hatController.ToggleControl(true);
        StartCoroutine(Spawn());
    }

    public IEnumerator Spawn()
    {
        yield return new WaitForSeconds(2.0f);
        counting = true;
        while (timeLeft > 0)
        {
            var ball = balls[Random.Range(0, balls.Length)];
            var spawnPosition = new Vector3(
                transform.position.x + Random.Range(-maxWidth, maxWidth),
                transform.position.y,
                0.0f
            );
            var spawnRotation = Quaternion.identity;
            Instantiate(ball, spawnPosition, spawnRotation);
            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
        }

        yield return new WaitForSeconds(2.0f);
        gameOverText.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        restartButton.SetActive(true);
    }
}