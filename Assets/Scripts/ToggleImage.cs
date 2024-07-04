using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ToggleImage : MonoBehaviour
{
    public Image imageToToggle;

    // Start is called before the first frame update
    void Start()
    {
        if (imageToToggle != null)
        {
            imageToToggle.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check if the touch began
            if (touch.phase == TouchPhase.Began)
            {
                // Touch started, hide the image
                if (imageToToggle != null)
                {
                    imageToToggle.gameObject.SetActive(false);
                }
            }
            // Check if the touch ended
            else if (touch.phase == TouchPhase.Ended)
            {
                // Touch ended, show the image
                if (imageToToggle != null)
                {
                    imageToToggle.gameObject.SetActive(true);
                }
            }
        }
    }
}
