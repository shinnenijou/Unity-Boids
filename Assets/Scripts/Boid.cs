using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [Header("Set Dynamically")]
    public Rigidbody rigid;
    private Neighborhood neighborhood;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        neighborhood = GetComponent<Neighborhood>();

        pos = Random.insideUnitSphere * Spawner.S.spawnRadius;

        Vector3 v = Random.onUnitSphere * Spawner.S.velocity;

        rigid.velocity = v;

        LookAhead();

        Color randColor = Color.black;
        while (randColor.r + randColor.g + randColor.b < 1.0f)
        {
            randColor = new Color(Random.value, Random.value, Random.value);
        }

        Renderer[] rends = GetComponentsInChildren<Renderer>();

        foreach(Renderer r in rends)
        {
            r.material.SetColor("_TintColor", randColor);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 vel = rigid.velocity;

        Vector3 velAvoid = Vector3.zero;
        Vector3 tooClosePos = neighborhood.avgClosePos;

        if (tooClosePos != Vector3.zero)
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

        if(velCenter != Vector3.zero)
        {
            velCenter -= transform.position;
            velCenter.Normalize();
            velCenter *= Spawner.S.velocity;
        }

        Vector3 delta = Attractor.POS - pos;

        bool attracted = delta.magnitude > Spawner.S.attractPushDist;
        Vector3 velAttract = delta.normalized * Spawner.S.velocity;

        // Apply all velocities
        float fdt = Time.fixedDeltaTime;

        if (attracted)
        {
            vel = Vector3.Lerp(vel, velAttract, fdt * Spawner.S.attractPull);
        }
        else
        {
            vel = Vector3.Lerp(vel, -velAttract, fdt * Spawner.S.attractPull);
        }

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
            else
            {
                if (velCenter != Vector3.zero)
                {
                    vel = Vector3.Lerp(vel, velCenter, Spawner.S.flockCentering * fdt);
                }
                else
                {
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
            }
        }

        vel = vel.normalized * Spawner.S.velocity;
        rigid.velocity = vel;
        LookAhead();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LookAhead()
    {
        transform.LookAt(pos + rigid.velocity);
    }

    public Vector3 pos { 
        get { return transform.position; }
        set { transform.position = value; }
    }
}
