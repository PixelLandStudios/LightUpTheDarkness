using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenLeverScript : MonoBehaviour
{
    [SerializeField]
    GameObject UpstairsLever;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LeverHandle"))
        {
            UpstairsLever.SetActive(true);

            GameObject.Destroy(this);

            GameObject.Destroy(other.gameObject);
        }
    }
}