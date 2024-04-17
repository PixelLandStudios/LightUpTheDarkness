using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHoleScript : MonoBehaviour
{
    [SerializeField]
    Door door;

    [SerializeField]
    GameObject Message;

    [SerializeField]
    GameObject KeyHoleUnlocked;

    public bool OpenTheDoor = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (OpenTheDoor)
        {
            OpenTheDoor = false;
            door.OpenDoor();

            this.GetComponent<AudioSource>().Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            GameObject.Destroy(other.gameObject);

            Message.SetActive(false);

            //open door
            door.OpenDoor();

            this.GetComponent<AudioSource>().Play();
        }
    }

    public void OnSelect()
    {
        this.GetComponent<AudioSource>().Play();

        Message.SetActive(true);

        KeyHoleUnlocked.SetActive(true);

        GameObject.Destroy(this);
    }
}