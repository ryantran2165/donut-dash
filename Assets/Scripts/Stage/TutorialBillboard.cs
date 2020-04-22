using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBillboard : MonoBehaviour
{
    private const float Y_POS = 5f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(ScreenUtility.getXRightOffscreen(GetComponent<SpriteRenderer>()), Y_POS);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
