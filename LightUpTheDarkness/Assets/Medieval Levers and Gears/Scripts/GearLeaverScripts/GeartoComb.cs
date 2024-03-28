using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeartoComb : MonoBehaviour
{
    public GearType2 parentGear;
    public GameObject Comb;
    [HideInInspector]
    public bool Comb_Move_up=true;
    [HideInInspector]
    public float Max_Height;
    [HideInInspector]
    public float Comb_Total_Movement;
    [HideInInspector]
    public float time = 0f;
    RaycastHit hit;
    Ray ray;
    [HideInInspector]
    public GameObject current;
    int layerMask = 1 << 8;
    public float Radius_of_Gear;
    float Speed;
    // Start is called before the first frame update
    float Default_Y_Location;
    float Ratio;
    public enum Axis_of_translation
    {
        x, y, z
    }
    [HideInInspector]
    public Axis_of_translation Comb_Axis_translation;
    [HideInInspector]
    float Drvr_teeth, Drvn_teeth;
    [HideInInspector]
    public float distance;
    [HideInInspector]
    public float manager_total_time;
    Camera MainCamera;

    void Start()
    {
        MainCamera = Camera.main;
        Drvr_teeth = parentGear.Driver_gear_teeth;
        Drvn_teeth = parentGear.Driven_gear_teeth;
        Ratio = Drvr_teeth / Drvn_teeth;
        print(distance);
        
        if (Comb_Axis_translation == Axis_of_translation.y)
        {
            Max_Height = Comb.transform.localPosition.y;

            if (parentGear.manager.DefaultZ >= parentGear.manager.MinValue &&
                parentGear.manager.DefaultZ <= parentGear.manager.MaxValue &&
                parentGear.manager.direction == Manager.Axis.z)
            {
                Default_Y_Location = 2f * Mathf.PI * Radius_of_Gear * ((90 - parentGear.manager.DefaultZ) * Ratio) / 360;
                Comb.transform.localPosition = new Vector3(Comb.transform.localPosition.x, Comb.transform.localPosition.y - Default_Y_Location, Comb.transform.localPosition.z);

                Comb_Total_Movement = (Max_Height - Comb.transform.localPosition.y );
                if (parentGear.manager.DefaultZ == parentGear.manager.MaxValue)
                {
                    Comb_Move_up = false;
                    Comb_Total_Movement = 2f * Mathf.PI * Radius_of_Gear * (parentGear.manager.MaxValue - parentGear.manager.MinValue) * Ratio / 360;

                }


            }

            else
            {
                Default_Y_Location = 2f * Mathf.PI * Radius_of_Gear * ((Ratio*(90-parentGear.manager.MinValue)) / 360);
                Comb.transform.localPosition = new Vector3(Comb.transform.localPosition.x, Comb.transform.localPosition.y - Default_Y_Location, Comb.transform.localPosition.z);
                Comb_Total_Movement = 2f * Mathf.PI * Radius_of_Gear * (parentGear.manager.MaxValue - parentGear.manager.MinValue) * Ratio / 360;
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        manager_total_time = parentGear.manager.total;

        ray = new Ray(MainCamera.transform.position, MainCamera.transform.forward);
        if (Physics.Raycast(ray, out hit, parentGear.Interaction_Range, layerMask))
        {

            current = hit.collider.gameObject;
            if (Input.GetKeyDown(KeyCode.E) && current == parentGear.Driver_gear)
            {
                StartCoroutine(LeverMove());

            }

        }

    }


    IEnumerator LeverMove()
    {
        if (Comb_Axis_translation == Axis_of_translation.y)
        {
            if ( Comb_Move_up == true)
            {
                while (Comb_Move_up)
                {
                     
                    time += 20f * Time.deltaTime * parentGear.Speed;
                    if (time >= manager_total_time)
                    {
                        manager_total_time = parentGear.manager.MaxValue - parentGear.manager.MinValue;
                        Comb_Total_Movement = 2f * Mathf.PI * Radius_of_Gear * (parentGear.manager.MaxValue - parentGear.manager.MinValue) * Ratio / 360;
                        time = 0f;
                        Comb_Move_up = false;
                    }
                    distance = 2 * Mathf.PI * Radius_of_Gear * (20f * Time.deltaTime * parentGear.Speed * Ratio) / 360;
                    Comb.transform.Translate(0f,distance,0f);
                    
                    yield return null;
                }
                Comb.transform.localPosition = new Vector3(Comb.transform.localPosition.x, Max_Height-(2*Mathf.PI*Radius_of_Gear*(90-parentGear.manager.MaxValue)*Ratio/360), Comb.transform.localPosition.z);
            }
            else if( Comb_Move_up == false)
            {
                while(!Comb_Move_up)
                {

                    time += 20f * Time.deltaTime * parentGear.Speed;
                    if (time >= manager_total_time)
                    {
                  
                        time = 0f;
                        Comb_Move_up = true;
                    }
                    distance = 2 * Mathf.PI * Radius_of_Gear * (20f * Time.deltaTime * parentGear.Speed * Ratio) / 360;
                    Comb.transform.Translate(0f, -distance, 0f);
                    yield return null;

                }
                Comb.transform.localPosition = new Vector3(Comb.transform.localPosition.x, Max_Height - (2 * Mathf.PI * Radius_of_Gear * (90 - parentGear.manager.MinValue) * Ratio / 360), Comb.transform.localPosition.z);

            }

        }
    }







}
