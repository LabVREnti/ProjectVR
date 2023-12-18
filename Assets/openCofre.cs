using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class openCofre : MonoBehaviour
{
    [SerializeField] GameObject arch;

    private bool open;
    public void AbrirCofre()
    {
        open = !open;
        if (open)
        {
             arch.transform.RotateAround(arch.transform.position, Vector3.back, 90);
        }
        else
        {
            arch.transform.RotateAround(arch.transform.position, Vector3.back, -90);
        }
    }
}
