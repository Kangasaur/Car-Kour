using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpsideDownTrigger : MonoBehaviour
{
    public GameObject canvas;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform")) canvas.SendMessage("DoDeath");
    }
}
