using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearType1 : MonoBehaviour
{
    public Manager[] managers;
    RaycastHit hit;
    Ray ray;

    [Space(10, order = 0)]
    [Tooltip("Minimun distance between the camera and the object to able to intract")]
    public int Interaction_Range;
    GameObject Current_Ray_Hitting_Object;
    float OriginalPos;

    Quaternion Pos;
    float speed = 1f;
    
    public GameObject stopper;
    GameObject Current_selected_object;

    public GameObject Player;

    float timer;

    int layerMask = 1 << 8;


    Vector3 playerPos;
    // Start is called before the first frame update

    Camera MainCamera;
    void Start()
    {
        MainCamera= Camera.main;

        foreach (Manager lev in managers)
        {

            float Orirotx = lev.DefaultX;
            float Oriroty = lev.DefaultY;
            float Orirotz = lev.DefaultZ;

            //x axis
            if (lev.direction == Manager.Axis.x)
            {

                if (Orirotx > lev.MinValue && Orirotx < lev.MaxValue)
                {
                    lev.Levers.transform.localRotation = Quaternion.Euler(Orirotx,Oriroty, Orirotz);
                    lev.total = lev.MaxValue - Orirotx;
                }

                else
                {
                    lev.Levers.transform.localRotation = Quaternion.Euler(lev.MinValue, Oriroty, Orirotz);
                    lev.total = lev.MaxValue - lev.MinValue;
                }

            }

            //y axis
            if (lev.direction == Manager.Axis.y)
            {
                if (Oriroty > lev.MinValue && Oriroty < lev.MaxValue)
                {

                    lev.Levers.transform.localRotation = Quaternion.Euler(Orirotx, Oriroty, Orirotz);
                    lev.total = lev.MaxValue - Oriroty;

                }
                else
                {

                    lev.Levers.transform.localRotation = Quaternion.Euler(Orirotx, lev.MinValue, Oriroty);
                    lev.total = lev.MaxValue - lev.MinValue;
                }
            }

            //z axis
            if (lev.direction == Manager.Axis.z)
            {
                if (Orirotz > lev.MinValue && Orirotz < lev.MaxValue)
                {
                    lev.Levers.transform.localRotation = Quaternion.Euler(Orirotx, Oriroty, Orirotz);
                    lev.total = lev.MaxValue - Orirotz;
                }
                else
                {
                    lev.Levers.transform.localRotation = Quaternion.Euler(Orirotx, Oriroty, lev.MinValue);
                    lev.total = lev.MaxValue - lev.MinValue;
                }
            }
        }


    }

    // Update is called once per frame
    void Update()
    {

        ray = new Ray(MainCamera.transform.position, MainCamera.transform.forward);
        if (Physics.Raycast(ray, out hit, Interaction_Range, layerMask))
        {
            Current_Ray_Hitting_Object = hit.collider.gameObject;

            for (int i = 0; i < managers.Length; ++i)
            {
                var currentManager = managers[i];

                if (Current_selected_object != null && Current_Ray_Hitting_Object != Current_selected_object && currentManager.interaction == Manager.HitType.Scroll)
                {
                    currentManager.PressedE = false;
                }

                   
                // For Button Press
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Current_selected_object = hit.collider.gameObject;
                   // DisplayPanel();

                    if (Current_Ray_Hitting_Object == currentManager.Levers)
                    {
                        currentManager.PressedE = !currentManager.PressedE;
                        currentManager.MousePressed = !currentManager.MousePressed;
                        StartCoroutine(RotateFun(Current_Ray_Hitting_Object));
                    }
                }
            }
        }
    }


    IEnumerator RotateFun(GameObject obj)
    {
       
        foreach (Manager var in managers)
        {
            // Button pressed and game objects rotates in X  Axis
            if (obj == var.Levers && var.interaction == Manager.HitType.Lever && var.direction == Manager.Axis.x)
            {

                if (Input.GetKeyDown(KeyCode.E) && var.rotbck == false || var.PressedE == true && var.rotbck == false)
                {
                    while (!var.rotbck)
                    {

                        var.time += 20f * Time.deltaTime;
                        if (var.time > var.total)
                        {
                            var.total = var.MaxValue - var.MinValue;
                            var.rotbck = true;
                            var.time = 0f;
                            var.PressedE = false;

                        }

                        var.Levers.transform.Rotate(20f * Time.deltaTime, 0f, 0f, Space.Self);
                        yield return null;
                    }

                    var.Levers.transform.localRotation = Quaternion.Euler(var.MaxValue, 0f, 0f);

                }
                else if (Input.GetKeyDown(KeyCode.E) && var.rotbck == true || var.PressedE == true && var.rotbck == true)
                {
                    while (var.rotbck)
                    {

                        var.time += 20f * Time.deltaTime;
                        if (var.time > var.total)
                        {
                            var.total = var.MaxValue - var.MinValue;
                            var.rotbck = false;
                            var.time = 0f;
                            var.PressedE = false;
                        }

                        yield return null;
                        var.Levers.transform.Rotate(-20f * Time.deltaTime, 0f, 0f, Space.Self);
                    }
                    var.Levers.transform.localRotation = Quaternion.Euler(var.MinValue, 0f, 0f);

                }
            }
            // Button pressed and game objects rotates in Y  Axis
            if (obj == var.Levers && var.interaction == Manager.HitType.Lever && var.direction == Manager.Axis.y)
            {

                if (Input.GetKeyDown(KeyCode.E) && var.rotbck == false || var.PressedE == true && var.rotbck == false)
                {
                    while (!var.rotbck)
                    {

                        var.time += 20f * Time.deltaTime;
                        if (var.time > var.total)
                        {
                            var.total = var.MaxValue - var.MinValue;
                            var.rotbck = true;
                            var.time = 0f;
                            var.PressedE = false;

                        }

                        var.Levers.transform.Rotate(0f, 20f * Time.deltaTime, 0f, Space.Self);
                        yield return null;
                    }
                    var.Levers.transform.localRotation = Quaternion.Euler(0f, var.MaxValue, 0f);

                }
                else if (Input.GetKeyDown(KeyCode.E) && var.rotbck == true || var.PressedE == true && var.rotbck == true)
                {
                    while (var.rotbck)
                    {


                        var.time += 20f * Time.deltaTime;
                        if (var.time > var.total)
                        {
                            var.total = var.MaxValue - var.MinValue;
                            var.rotbck = false;
                            var.time = 0f;
                            var.PressedE = false;
                        }

                        var.Levers.transform.Rotate(0f, -20f * Time.deltaTime, 0f, Space.Self);
                        yield return null;
                    }
                    var.Levers.transform.localRotation = Quaternion.Euler(0f, var.MinValue, 0f);

                }
            }
            // Button pressed and game objects rotates in Z  Axis
            if (obj == var.Levers && var.interaction == Manager.HitType.Lever && var.direction == Manager.Axis.z)
            {
                if (Input.GetKeyDown(KeyCode.E) && var.rotbck == false || var.PressedE == true && var.rotbck == false)
                {
                    while (!var.rotbck)
                    {

                        var.time += 20f * Time.deltaTime;
                        if (var.time > var.total)
                        {
                            var.total = var.MaxValue - var.MinValue;
                            var.rotbck = true;
                            var.time = 0f;
                            var.PressedE = false;

                        }

                        var.Levers.transform.Rotate(0f, 0f, 20f * Time.deltaTime, Space.Self);
                        yield return null;
                    }
                    var.Levers.transform.localRotation = Quaternion.Euler(0f, 0f, var.MaxValue);

                }
                else if (Input.GetKeyDown(KeyCode.E) && var.rotbck == true || var.PressedE == true && var.rotbck == true)
                {
                    while (var.rotbck)
                    {


                        var.time += 20f * Time.deltaTime;
                        if (var.time > var.total)
                        {
                            var.rotbck = false;
                            var.time = 0f;
                            var.PressedE = false;
                        }

                        var.Levers.transform.Rotate(0f, 0f, -20f * Time.deltaTime, Space.Self);
                        yield return null;
                    }
                    var.Levers.transform.localRotation = Quaternion.Euler(0f, 0f, var.MinValue);
                }
            }

            // Scroll pressed and game objects rotates in X  Axis
            if (obj == var.Levers && var.interaction == Manager.HitType.Scroll && var.direction == Manager.Axis.x)
            {
                if (var.MousePressed == true)
                {
                    var.MousePressed = false;
                    while (true && var.PressedE)
                    {
                        if (Current_Ray_Hitting_Object == var.Levers)
                            var.Levers.transform.Rotate(60f * Input.GetAxis("Mouse ScrollWheel"), 0f, 0f, Space.Self);


                        yield return null;
                    }
                }

            }
            // Scroll pressed and game objects rotates in Y  Axis

            if (obj == var.Levers && var.interaction == Manager.HitType.Scroll && var.direction == Manager.Axis.y)
            {
                if (var.MousePressed == true)
                {
                    var.MousePressed = false;
                    while (true && var.PressedE)
                    {
                        if (Current_Ray_Hitting_Object == var.Levers)

                            var.Levers.transform.Rotate(0f, 60f * Input.GetAxis("Mouse ScrollWheel"), 0f, Space.Self);

                        yield return null;
                    }
                }

            }
            // Scroll pressed and game objects rotates in Z  Axis

            if (obj == var.Levers && var.interaction == Manager.HitType.Scroll && var.direction == Manager.Axis.z)
            {
                if (var.MousePressed == true)
                {
                    while (true && var.PressedE)
                    {

                        if (Input.GetAxis("Mouse ScrollWheel") > 0)
                        {
                            if (Current_Ray_Hitting_Object == var.Levers)

                                var.Levers.transform.Rotate(0f, 0f, 10f * Input.GetAxis("Mouse ScrollWheel"), Space.Self);
                        }

                        else if ((Input.GetAxis("Mouse ScrollWheel") <= 0) && stopper.transform.localRotation.z >= 0.05f && var.Levers.gameObject.tag == "Player")
                        {

                            ;
                        }
                        else
                        {
                            if (Current_Ray_Hitting_Object == var.Levers)

                                var.Levers.transform.Rotate(0f, 0f, 10f * Input.GetAxis("Mouse ScrollWheel"), Space.Self);

                        }

                        yield return null;
                        var.MousePressed = false;
                    }
                }

            }

        }
    }




}





