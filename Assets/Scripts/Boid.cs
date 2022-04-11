using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [Header("Set Dynamically")]
    public Rigidbody rigid;

    private Neighborhood neighborhood;

    void Awake()
    {
        neighborhood = GetComponent<Neighborhood>();
        rigid = GetComponent<Rigidbody>();
        pos = Random.insideUnitSphere * Spawner.S.spawnRadius;

        Vector3 vel = Random.onUnitSphere * Spawner.S.velocity;
        rigid.velocity = vel;

        LookAhead();

        Color randColor = Color.black;

        while (randColor.r + randColor.g + randColor.b < 1.0f)
        {
            randColor = new Color(Random.value, Random.value, Random.value);
        }

        Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
        {
            r.material.color = randColor;
        }

        TrailRenderer tRend = GetComponent<TrailRenderer>();
        tRend.material.SetColor("_TintColor", randColor);
    }

    void FixedUpdate()
    {
        Vector3 vel = rigid.velocity;
        Vector3 delta = Attractor.POS - pos;

        Vector3 velAvoid = Vector3.zero;
        Vector3 tooClosePos = neighborhood.avgClosePos;
        if(tooClosePos != Vector3.zero)
        {
            velAvoid = pos - tooClosePos; 
            velAvoid.Normalize(); 
            velAvoid *= Spawner.S.velocity;
        }

        Vector3 velAlign = neighborhood.avgVel;
        if (velAlign != Vector3.zero) 
        {
            velAlign.Normalize();
            velAlign *= Spawner.S.velocity;
        }
        Vector3 velCenter = neighborhood.avgPos;
        if (velCenter != Vector3.zero) 
        {
            velCenter -= transform.position; 
            velCenter.Normalize(); 
            velCenter *= Spawner.S.velocity;
        }

        bool attracted = (delta.magnitude > Spawner.S.attractPushDist);
        Vector3 velAttract = delta.normalized * Spawner.S.velocity;

        float fdt = Time.fixedDeltaTime;
        if (velAvoid != Vector3.zero)
        {
            vel = Vector3.Lerp(vel, velAvoid, Spawner.S.collAvoid * fdt);
        }
        else
        {
            if (velAlign != Vector3.zero)
            {
                vel = Vector3.Lerp(vel, velAlign, Spawner.S.velMatching * fdt);
            }
            if (velCenter != Vector3.zero)
            {
                vel = Vector3.Lerp(vel, velAlign, Spawner.S.flockCentering * fdt);
            }
            if (velAttract != Vector3.zero)
            {
                if (attracted)
                {
                    vel = Vector3.Lerp(vel, velAttract, Spawner.S.attractPull * fdt);
                }
                else
                {
                    vel = Vector3.Lerp(vel, -velAttract, Spawner.S.attractPull * fdt);
                }
            }
        }

        vel = vel.normalized * Spawner.S.velocity;
        rigid.velocity = vel;

        LookAhead();
    }

    void LookAhead()
    {
        transform.LookAt(pos + rigid.velocity);
    }

    public Vector3 pos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }
}
