using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearType2 : MonoBehaviour
{
    public Manager manager;
  //  public GameObject Lever;
  //  public GameObject Gear;
    public GameObject Comb;
    RaycastHit hit;
    Ray ray;
    [Tooltip("Minimun distance between the camera and the object to able to intract")]
    public int Interaction_Range;
    int layerMask = 1 << 8;

    public float Speed=0;

    public GameObject Driver_gear;
    [Tooltip("Number of TEETHS in Driver Gear")]
    public int Driver_gear_teeth;


    public GameObject Driven_gear;
    [Tooltip("Number of TEETHS in Driven Gear")]
    public int Driven_gear_teeth;
    // Start is called before the first frame update
    [HideInInspector]
     public float Ratio;
    [HideInInspector]
    public GameObject current;

    float Drvr_teeth, Drvn_teeth;
    Camera MainCamera;

    void Start()
    {
        MainCamera = Camera.main;
        Drvn_teeth = Driven_gear_teeth;
        Drvr_teeth = Driver_gear_teeth;
        
       
        Ratio = Drvr_teeth/Drvn_teeth;
        float DefaultX = manager.DefaultX;
        float DefaultY = manager.DefaultY;
        float DefaultZ = manager.DefaultZ;
        //x axis
        if (manager.direction == Manager.Axis.x)
        {
           
            if (DefaultX >= manager.MinValue && DefaultX <= manager.MaxValue)
            {
                Driver_gear.transform.localRotation = Quaternion.Euler(DefaultX, DefaultY, DefaultZ);
                Driven_gear.transform.localRotation = Quaternion.Euler(-Ratio*DefaultX, DefaultY, DefaultZ);
                manager.total = manager.MaxValue - DefaultX;
                if (DefaultX == manager.MaxValue)
                {
                    manager.rotbck = true;
                    manager.total = manager.MaxValue - manager.MinValue;
                }
            }

            else
            {
                Driver_gear.transform.localRotation = Quaternion.Euler(manager.MinValue, DefaultY, DefaultZ);
                Driven_gear.transform.localRotation = Quaternion.Euler(manager.MinValue, DefaultY, DefaultZ);
                manager.total = manager.MaxValue - manager.MinValue;
            }

        }

        //y axis
        if (manager.direction == Manager.Axis.y)
        {
            if (DefaultY >= manager.MinValue && DefaultY <= manager.MaxValue)
            {

                Driver_gear.transform.localRotation = Quaternion.Euler(DefaultX, DefaultY, DefaultZ);
                Driven_gear.transform.localRotation = Quaternion.Euler(DefaultX,-Ratio* DefaultY, DefaultZ);
                manager.total = manager.MaxValue - DefaultY;
                if (DefaultY == manager.MaxValue)
                {
                    manager.rotbck = true;
                    manager.total = manager.MaxValue - manager.MinValue;

                }

            }
            else
            {

                Driver_gear.transform.localRotation = Quaternion.Euler(DefaultX, manager.MinValue, DefaultZ);
                Driven_gear.transform.localRotation = Quaternion.Euler(DefaultX, -manager.MinValue, DefaultZ);

                manager.total = manager.MaxValue - manager.MinValue;
            }
        }
        float abc;
        //z axis
        if (manager.direction == Manager.Axis.z)
        {
            if (DefaultZ >= manager.MinValue && DefaultZ <= manager.MaxValue)
            {
                Driver_gear.transform.localRotation = Quaternion.Euler(DefaultX, DefaultY, DefaultZ);
                Driven_gear.transform.localRotation = Quaternion.Euler(DefaultX, DefaultY, (Ratio*(90-DefaultZ))+90);
                manager.total = manager.MaxValue - DefaultZ;
                if (DefaultZ == manager.MaxValue)
                {
                    manager.rotbck = true;
                    manager.total = manager.MaxValue - manager.MinValue;

                }
            }
            else
            {
                Driver_gear.transform.localRotation = Quaternion.Euler(DefaultX, DefaultY, manager.MinValue);
                Driven_gear.transform.localRotation = Quaternion.Euler(DefaultX, DefaultY, (Ratio * (90 - manager.MinValue)) + 90);

                manager.total = manager.MaxValue - manager.MinValue;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
        ray = new Ray(MainCamera.transform.position, MainCamera.transform.forward);

        if (Physics.Raycast(ray, out hit, Interaction_Range, layerMask))
        {
            current = hit.collider.gameObject;
            if (Input.GetKeyDown(KeyCode.E) && current== Driver_gear )
            {

                print(current);
               
                manager.PressedE = !manager.PressedE;
                StartCoroutine(Rotate_function(current,Driven_gear));

            }
        }

    }

    IEnumerator Rotate_function(GameObject MainGear,GameObject SecondaryGear)
    {
        if (manager.interaction == Manager.HitType.Lever && manager.direction == Manager.Axis.x)
        {
            if (Input.GetKeyDown(KeyCode.E) && manager.rotbck == false || manager.PressedE == true && manager.rotbck == false)
            {
                while (!manager.rotbck)
                {

                    manager.time += 20f * Time.deltaTime;
                    if (manager.time > manager.total)
                    {
                        manager.total = manager.MaxValue - manager.MinValue;
                        manager.rotbck = true;
                        manager.time = 0f;
                        manager.PressedE = false;

                    }

                    manager.Levers.transform.Rotate(20f * Time.deltaTime, 0f, 0f, Space.Self);
                    yield return null;
                }

                manager.Levers.transform.localRotation = Quaternion.Euler(manager.MaxValue, 0f, 0f);

            }
            else if (Input.GetKeyDown(KeyCode.E) && manager.rotbck == true || manager.PressedE == true && manager.rotbck == true)
            {
                while (manager.rotbck)
                {

                    manager.time += 20f * Time.deltaTime;
                    if (manager.time > manager.total)
                    {
                        manager.total = manager.MaxValue - manager.MinValue;
                        manager.rotbck = false;
                        manager.time = 0f;
                        manager.PressedE = false;
                    }

                    yield return null;
                    manager.Levers.transform.Rotate(-20f * Time.deltaTime, 0f, 0f, Space.Self);
                }
                manager.Levers.transform.localRotation = Quaternion.Euler(manager.MinValue, 0f, 0f);

            }




        }
 
        if ( manager.interaction == Manager.HitType.Lever && manager.direction == Manager.Axis.z)
        {
           
            if (Input.GetKeyDown(KeyCode.E) && manager.rotbck == false || manager.PressedE == true && manager.rotbck == false)
            {
             
                while (!manager.rotbck)
                {
                    

                    manager.time += 20f * Time.deltaTime*Speed;
                    if (manager.time >= manager.total)
                    {
                     
                        manager.total = manager.MaxValue - manager.MinValue;
                        manager.rotbck = true;
                        manager.time = 0f;
                        manager.PressedE = false;

                    }

                    manager.Levers.transform.Rotate(0f, 0f, 20f * Time.deltaTime*Speed, Space.Self);
                    SecondaryGear.transform.Rotate(0f, 0f, -20f * Time.deltaTime * Ratio*Speed, Space.Self);
                    yield return null;
                }
                manager.Levers.transform.localRotation = Quaternion.Euler(0f, 0f, manager.MaxValue);
                SecondaryGear.transform.localRotation = Quaternion.Euler(0f, 0f,((Ratio* (90-manager.MaxValue))+90));



            }
            else if (Input.GetKeyDown(KeyCode.E) && manager.rotbck == true || manager.PressedE == true && manager.rotbck == true)
            {
                while (manager.rotbck)
                {


                    manager.time += 20f * Time.deltaTime*Speed;
                    if (manager.time > manager.total)
                    {
                        manager.rotbck = false;
                        manager.time = 0f;
                        manager.PressedE = false;
                    }

                    manager.Levers.transform.Rotate(0f, 0f, -20f * Time.deltaTime*Speed, Space.Self);
                    SecondaryGear.transform.Rotate(0f, 0f, 20f * Time.deltaTime*Ratio*Speed, Space.Self);

                    yield return null;
                }
                manager.Levers.transform.localRotation = Quaternion.Euler(0f, 0f, manager.MinValue);
                SecondaryGear.transform.localRotation = Quaternion.Euler(0f, 0f, ((Ratio*(90-manager.MinValue))+90));

            }
        }

    }



}





