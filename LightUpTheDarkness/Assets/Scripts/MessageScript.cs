using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageScript : MonoBehaviour
{
    [SerializeField]
    GameObject Message;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnSelect()
    {
        Message.SetActive(true);
    }
}