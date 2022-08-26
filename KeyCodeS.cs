using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KeyCodeS : MonoBehaviour
{
    public KeyCode Forward = KeyCode.W;
    public KeyCode Left = KeyCode.A;
    public KeyCode Back = KeyCode.S;
    public KeyCode Right = KeyCode.D;
    public KeyCode Run = KeyCode.LeftShift;
    public KeyCode Kick = KeyCode.V;
    public KeyCode Jump = KeyCode.Space;

    public KeyCode InteractionObject = KeyCode.E;
    public KeyCode InspectionObject = KeyCode.F;
    public KeyCode ExitInspection = KeyCode.T;
    public KeyCode QuickMenu = KeyCode.R;
    public KeyCode Treatment = KeyCode.H;
    public KeyCode DiscardObject = KeyCode.Q;
    public KeyCode VisualBackPack = KeyCode.Tab;

    private void Awake()
    {
        Forward = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("WalkForward"));
        Left = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("WalkLeft"));
        Back = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("WalkBack"));
        Right = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("WalkRight"));
        Run = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Run"));
        Kick = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Kick"));
        Jump = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump"));
        InteractionObject = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interaction"));
        InspectionObject = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Inspection"));
        ExitInspection = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ExitInspection"));
        QuickMenu = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("QuickMenu"));
        Treatment = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("MedicalKitUse"));
        DiscardObject = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("DiscardObject"));
        VisualBackPack = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("VisualBackpack"));
    }
}
