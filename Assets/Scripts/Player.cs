﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int thiccLevel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addThicc(int thicc)
    {
        thiccLevel += thicc;
        Debug.Log("Thicc: " + thiccLevel);
    }

}
