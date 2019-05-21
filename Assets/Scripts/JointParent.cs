using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JointParent : MonoBehaviour
{
    public double A;
    public double T;
    public double phaseDifference;

    public GameObject[] links;
    private List<Rigidbody> rigidBodies = new List<Rigidbody>();

    
    // Start is called before the first frame update
    void Start()
    {
        A = Math.PI / 8.0f;
        T = 2.25f;
        phaseDifference = 2.0f * Math.PI / 12.0f;
        // InitializeJoints();

        for (int i = 0; i <= 100; ++i)
        {
            CreateLink(i);
        }

        links = GetLinks();
        CreateJoints();
        InitializeJoints();
    }

    void InitializeJoints()
    {
        for (int i = 0; i <= links.Length - 1; ++i)
        {
            links[i].AddComponent<JointTorque>();
            links[i].GetComponent<JointTorque>().i = i;
        }
    }
    
    private GameObject[] GetLinks()
    {
        GameObject[] found = GameObject.FindGameObjectsWithTag("link");
        Array.Sort(found, CompareObjectNames);
        return found;
    }

    int CompareObjectNames(GameObject x, GameObject y)
    {
        // Sort with integers instead of names because 10 is smaller than 1 in string comparison
        return Int32.Parse(x.name.Substring(5)).CompareTo(Int32.Parse(y.name.Substring(5)));
    }

    public void CreateLink(int index)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.gameObject.tag = "link";
        cube.name = "link_" + index.ToString();
        cube.transform.localScale = new Vector3(0.01f, 0.05f, 0.05f);
        cube.transform.position = new Vector3(0.0f + (index * 0.03f), 0.1f, 0.1f);
        cube.AddComponent<Rigidbody>();
        
        Vector3 pos = new Vector3(0.0f, 0.0f, 0.0f);
        Quaternion p = Quaternion.Euler(pos);
        
        cube.transform.rotation = p;
    }

    void CreateJoints()
    {
        Debug.Log(links);

        for (int i = 0; i <= links.Length - 1; i++)
        {
            if (i != 0)
            {
                links[i].AddComponent(typeof(HingeJoint));
                links[i].GetComponent<HingeJoint>().connectedBody =
                    GameObject.Find("link_" + (i - 1).ToString()).GetComponent<Rigidbody>();
                links[i].GetComponent<HingeJoint>().axis = new Vector3(0.0f, 1.0f, 0.0f);
                links[i].GetComponent<HingeJoint>().anchor = new Vector3(-1.5f, 0.0f, 0.0f);
                links[i].GetComponent<HingeJoint>().autoConfigureConnectedAnchor = false;
                links[i].GetComponent<HingeJoint>().connectedAnchor = new Vector3(1.5f, 0.0f, 0.0f);
                links[i].GetComponent<HingeJoint>().enableCollision = true;
            }
        }
    }
}
