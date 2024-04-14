using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchScript : MonoBehaviour
{
    [SerializeField]
    List<Light> Lights;

    [SerializeField]
    GameObject TextMessage1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TurnLightOn()
    {
        foreach (var item in Lights)
        {
            item.enabled = true;
        }

        //monster screech

        //remove TextMessage1
        GameObject.Destroy(TextMessage1);
    }

    public void TurnLightOff()
    {
        foreach (var item in Lights)
        {
            item.enabled = false;
        }
    }
}
