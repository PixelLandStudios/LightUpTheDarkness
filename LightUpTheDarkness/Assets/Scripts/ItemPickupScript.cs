using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupScript : MonoBehaviour
{
    [SerializeField]
    GameObject Item;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnItemPickup()
    {
        Item.SetActive(true);

        this.GetComponent<AudioSource>().Play();

        GameObject.Destroy(this);
    }
}
