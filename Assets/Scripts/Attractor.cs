using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    static public Vector3 POS = Vector3.zero;

    [Header("Set in Inspector")]
    public float radius = 10f;
    public float xPhase = 0.5f;
    public float yPhase = 0.4f;
    public float zPhase = 0.1f;

    void FixedUpdate()
    {
        Vector3 tPos = Vector3.zero;
        Vector3 scale = transform.localScale;
        tPos.x = Mathf.Sin(xPhase * Time.time) * scale.x * radius;
        tPos.y = Mathf.Sin(yPhase * Time.time) * scale.y * radius;
        tPos.z = Mathf.Sin(zPhase * Time.time) * scale.z * radius;
        transform.position = tPos;
        POS = tPos;
    }
}
