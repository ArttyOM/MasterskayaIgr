using UnityEngine;
using System.Collections;

public class HT_StartGame : MonoBehaviour
{
    public HT_GameController gameController;

    private void OnMouseDown()
    {
        gameController.StartGame();
    }
}