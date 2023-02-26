using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionSettings : MonoBehaviour
{
    public static bool inTransition = false;
    public void InTransition(int number)
    {
        if (number == 1)
            inTransition = true;
        else if (number == 0)
            inTransition = false;
    }
}
