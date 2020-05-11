using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCutscene : MonoBehaviour
{
    [SerializeField] private List<GameObject> toActivate;
    [SerializeField] private GameObject donutShop;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        transform.position = new Vector3(ScreenUtility.getXRightOnscreen(renderer, camera), transform.position.y);

        // Set static background donut shop's position to the same as animation's
        donutShop.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called by Animation Event attached to IntroCutScene Animation
    public void onFinishTransition()
    {
        // Activate all the deactivated objects
        foreach (GameObject toActivateObject in toActivate)
        {
            toActivateObject.SetActive(true);
        }

        // Activate player movement
        playerMovement.enabled = true;

        // Destroy this object
        Destroy(gameObject);
    }
}
