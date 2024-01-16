using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HT_Restart : MonoBehaviour
{
    public void OnMouseDown()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}