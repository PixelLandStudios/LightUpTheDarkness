using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class ChandelierScript : MonoBehaviour
{
    [SerializeField]
    Transform Chandelier;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnValueChange(float x)
    {
        float AbsX = Mathf.Abs(x);
        Chandelier.localPosition = new Vector3(Chandelier.localPosition.x, Chandelier.localPosition.y - (AbsX / 1000), Chandelier.localPosition.z);
    }
}