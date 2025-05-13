using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Object.DontDestroyOnLoad example.
//
// This script example manages the playing audio. The GameObject with the
// "music" tag is the BackgroundMusic GameObject. The AudioSource has the
// audio attached to the AudioClip.

public class DataCarry : MonoBehaviour
{
    void Awake()
    {
        GameObject statManager = GameObject.FindGameObjectWithTag("StatManager");

        if (statManager != null)
        {
            Debug.Log("Stat manager found.");
            DontDestroyOnLoad(statManager);
        }
        
        //Destroy(this.gameObject);
    }
}