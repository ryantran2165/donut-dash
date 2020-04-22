using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCutscene : MonoBehaviour
{
    [SerializeField] private List<GameObject> toActivate;
    [SerializeField] private GameObject donutShop;
    [SerializeField] private PlayerMovement playerMovement;

    private const float Y_POS = 6.7f;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        float x = ScreenUtility.getRightEdge() - renderer.bounds.size.x / 2f;
        transform.position = new Vector3(x, Y_POS);

        // Set static background donut shop's position to animation's
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
