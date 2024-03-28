using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Manager
{
    public GameObject Levers;
    [Header("Rotation Angle")]
    [Space(5, order = 0)]
    [Tooltip("Default X ratation value of your lever")]
    [Range(-90,90)] public float DefaultX;

    [Tooltip("Default Y ratation value of your lever")]
    [Range(-90,90)] public float DefaultY;

    [Tooltip("Default Z ratation value of your lever")]
    [Range(-90,90)] public float DefaultZ;

    [Tooltip("Minimum angle your leaver should rotate ")]
    [Range(-90,90)] public float MinValue;

    [Tooltip("Maximum angle your leaver should rotate ")]
    [Range(-90, 90)] public float MaxValue;

    [HideInInspector]
    public float total;
    [HideInInspector]
    public float time;
    [HideInInspector]
    public bool rotbck = false;
    [HideInInspector]
    public  bool PressedE = false;
    [HideInInspector]
    public bool MousePressed = false;


    //  public Quaternion Dest;
    public enum HitType
    {
        Lever,
        Scroll
    }
    public enum Axis
    {
        x,
        y, 
        z
    }
    [Space(10, order = 0)]
    [Tooltip("Lever if its a lever ,Scroll if its a Gear ")]   
    public HitType interaction;

    [Tooltip("Axis on which to rotate")]
    public Axis direction;

}

