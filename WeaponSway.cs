using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{

    public float swayIntensity;
    public float swaySmoothing;

    private Quaternion originRotation;

    public bool swayQueen = true;

    //public float maxXRotation;
    //public float maxYRotation;
    private levelManager theLevelManager;

    // Start is called before the first frame update
    void Start()
    {
        originRotation = transform.rotation;
        theLevelManager = FindObjectOfType<levelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!theLevelManager.isPaused)
        {
            if (swayQueen)
            {
                UpdateSway();
            }
        }
    }

    private void UpdateSway()
    {
        //float mouseX = Input.GetAxis("Mouse X"); //* Time.deltaTime?
        //float mouseY = Input.GetAxis("Mouse Y");
        float mouseX = theLevelManager.lookInput.x * 1/10; //* looksensX ?
        float mouseY = theLevelManager.lookInput.y * 1/10;

        //calc target rotation
        Quaternion adjustmentRotationX = Quaternion.AngleAxis(-swayIntensity * mouseX, Vector3.up);
        Quaternion adjustmentRotationY = Quaternion.AngleAxis(swayIntensity * mouseY, Vector3.right);

        //cap the target rotations
        /*
        if(adjustmentRotationX.eulerAngles.z > maxXRotation)
        {
            adjustmentRotationX = Quaternion.Euler(adjustmentRotationX.eulerAngles.x, adjustmentRotationX.eulerAngles.z, maxXRotation); 
        }
        if (adjustmentRotationY.eulerAngles.x > maxYRotation)
        {
            adjustmentRotationY = Quaternion.Euler(maxYRotation, adjustmentRotationX.eulerAngles.y, adjustmentRotationX.eulerAngles.z);
        }*/

        Quaternion targetRotation = originRotation * adjustmentRotationX * adjustmentRotationY;

        //lerp to it
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.fixedDeltaTime * swaySmoothing);
    }
}
