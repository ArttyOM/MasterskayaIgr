using UnityEngine;
using System.Collections;

public class HT_HatController : MonoBehaviour
{
    public Camera cam;

    private float maxWidth;
    private bool canControl;

    // Use this for initialization
    private void Start()
    {
        if (cam == null) cam = Camera.main;
        var upperCorner = new Vector3(Screen.width, Screen.height, 0.0f);
        var targetWidth = cam.ScreenToWorldPoint(upperCorner);
        var hatWidth = GetComponent<Renderer>().bounds.extents.x;
        maxWidth = targetWidth.x - hatWidth;
        canControl = false;
    }

    // Update is called once per physics timestep
    private void FixedUpdate()
    {
        if (canControl)
        {
            var rawPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            var targetPosition = new Vector3(rawPosition.x, 0.0f, 0.0f);
            var targetWidth = Mathf.Clamp(targetPosition.x, -maxWidth, maxWidth);
            targetPosition = new Vector3(targetWidth, targetPosition.y, targetPosition.z);
            GetComponent<Rigidbody2D>().MovePosition(targetPosition);
        }
    }

    public void ToggleControl(bool toggle)
    {
        canControl = toggle;
    }
}