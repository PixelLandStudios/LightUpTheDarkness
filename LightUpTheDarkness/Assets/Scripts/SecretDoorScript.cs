using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretDoorScript : MonoBehaviour
{
    [SerializeField]
    Animation SecretDoorAnimation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnLeverOpen()
    {
        SecretDoorAnimation.Play();

        this.GetComponent<AudioSource>().Play();
    }
}