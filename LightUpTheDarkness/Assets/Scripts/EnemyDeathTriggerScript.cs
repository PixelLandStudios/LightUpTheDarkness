using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class EnemyDeathTriggerScript : MonoBehaviour
{
    [SerializeField]
    EnemyAI enemyAI;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chandelier"))
        {
            GameObject.Find("ChandelierLever").GetComponent<ChandelierScript>().enabled = false;
            GameObject.Find("ChandelierLever").GetComponent<XRKnob>().enabled = false;

            enemyAI.KillEnemy();
        }
    }
}