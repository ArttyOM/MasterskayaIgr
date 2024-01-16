using UnityEngine;
using System.Collections;

public class HT_DestroyOnContact : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
    }
}