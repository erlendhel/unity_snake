using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointTorque : MonoBehaviour
{
    private Rigidbody rb;
    public double i;
    private double time;

    private double sine;

    private JointParent jointParent;

    void Start()
    {
        jointParent = FindObjectOfType<JointParent>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        time = time + Time.deltaTime;
        sine = (jointParent.A * Math.Sin((2.0f * Math.PI / jointParent.T) * time + i * jointParent.phaseDifference) * 57.556f);

        Vector3 pos = new Vector3(0.0f, (float)sine, 0.0f);
        Quaternion p = Quaternion.Euler(pos);
        
        rb.MoveRotation(p);
    }
}