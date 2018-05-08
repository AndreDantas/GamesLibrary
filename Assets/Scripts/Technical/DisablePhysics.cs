using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class DisablePhysics : MonoBehaviour
{

    private void Awake()
    {
        Physics.autoSimulation = false;
        Physics2D.autoSimulation = false;
    }
}
