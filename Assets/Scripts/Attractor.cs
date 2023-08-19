using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    public static Vector3 POS = Vector3.zero;

    [Header("Set in Inspector")]
    public float radius = 10;
    public float xPhase = 0.5f;
    public float yPhase = 0.4f;
    public float zPhase = 0.1f;

    private void FixedUpdate()
    {
        Vector3 tempPos = Vector3.zero;
        Vector3 scale = transform.localScale;
        tempPos.x = Mathf.Sin(xPhase * Time.time) * radius * scale.x;
        tempPos.y = Mathf.Sin(yPhase * Time.time) * radius * scale.y;
        tempPos.z = Mathf.Sin(zPhase * Time.time) * radius * scale.z;
        transform.position = tempPos;
        POS = tempPos;
    }
}
