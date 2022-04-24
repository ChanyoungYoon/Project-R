using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom_Area : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Explosion explosion = this.GetComponentInParent<Explosion>();
        explosion.checkPlayer = true;
    }
}
