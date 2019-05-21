using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    private JointParent jp;
    private GameObject parent;

    public Slider amplitudeSlider;
    public Slider tSlider;
    public Slider phaseDifferenceSlider;

    public Slider linkGeneratorSlider;

    public Text amplitudeText;
    public Text tText;
    public Text phaseDifferenceText;

    // Start is called before the first frame update
    void Start()
    {
        parent = GameObject.FindGameObjectWithTag("parent");

        jp = parent.GetComponent<JointParent>();

        amplitudeSlider = GameObject.Find("AmplitudeSlider").GetComponent<Slider>();
        amplitudeSlider.value = (float) parent.GetComponent<JointParent>().A;
        tSlider = GameObject.Find("TSlider").GetComponent<Slider>();
        tSlider.value = (float) parent.GetComponent<JointParent>().T;
        phaseDifferenceSlider = GameObject.Find("PhaseDifferenceSlider").GetComponent<Slider>();
        phaseDifferenceSlider.value = (float) parent.GetComponent<JointParent>().phaseDifference;
        linkGeneratorSlider = GameObject.Find("LinkGenerator").GetComponent<Slider>();
        linkGeneratorSlider.value = parent.GetComponent<JointParent>().links.Length;

        GameObject.Find("AmplitudeText").GetComponent<Text>().text = amplitudeSlider.value.ToString();
        GameObject.Find("TText").GetComponent<Text>().text = tSlider.value.ToString();
        GameObject.Find("PhaseDifferenceText").GetComponent<Text>().text = phaseDifferenceSlider.value.ToString();

        amplitudeSlider.onValueChanged.AddListener(delegate { AmplitudeValueChange(); });
        tSlider.onValueChanged.AddListener(delegate { TValueChange(); });
        phaseDifferenceSlider.onValueChanged.AddListener(delegate { PhaseDiffValueChange(); });
        linkGeneratorSlider.onValueChanged.AddListener(delegate { LinkCountChange(); });
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AmplitudeValueChange()
    {
        parent.GetComponent<JointParent>().A = amplitudeSlider.value;
        GameObject.Find("AmplitudeText").GetComponent<Text>().text = amplitudeSlider.value.ToString();
    }

    public void TValueChange()
    {
        parent.GetComponent<JointParent>().T = tSlider.value;
        GameObject.Find("TText").GetComponent<Text>().text = tSlider.value.ToString();
    }

    public void PhaseDiffValueChange()
    {
        parent.GetComponent<JointParent>().phaseDifference = phaseDifferenceSlider.value;
        GameObject.Find("PhaseDifferenceText").GetComponent<Text>().text = phaseDifferenceSlider.value.ToString();
    }

    public void LinkCountChange()
    {
        if (linkGeneratorSlider.value < GameObject.FindGameObjectsWithTag("link").Length)
        {
            for (int i = GameObject.FindGameObjectsWithTag("link").Length; i >= linkGeneratorSlider.value; --i)
            {
                Destroy(GameObject.Find("link_" + i.ToString()));
            }

            // Debug.Log(GameObject.FindGameObjectsWithTag("link").Length - linkGeneratorSlider.value);
        }
        else if (linkGeneratorSlider.value > GameObject.FindGameObjectsWithTag("link").Length)
        {
            for (int i = GameObject.FindGameObjectsWithTag("link").Length; i <= linkGeneratorSlider.value; ++i)
            {
                jp.CreateLink(i);
                GameObject.Find("link_" + i.ToString()).AddComponent(typeof(HingeJoint));
                GameObject.Find("link_" + i.ToString()).GetComponent<HingeJoint>().connectedBody =
                    GameObject.Find("link_" + (i - 1).ToString()).GetComponent<Rigidbody>();
                GameObject.Find("link_" + i.ToString()).GetComponent<HingeJoint>().axis = new Vector3(0.0f, 1.0f, 0.0f);
                GameObject.Find("link_" + i.ToString()).GetComponent<HingeJoint>().anchor =
                    new Vector3(-1.5f, 0.0f, 0.0f);
                GameObject.Find("link_" + i.ToString()).GetComponent<HingeJoint>().autoConfigureConnectedAnchor = false;
                GameObject.Find("link_" + i.ToString()).GetComponent<HingeJoint>().connectedAnchor =
                    new Vector3(1.5f, 0.0f, 0.0f);
                GameObject.Find("link_" + i.ToString()).GetComponent<HingeJoint>().enableCollision = true;
                GameObject.Find("link_" + i.ToString()).AddComponent<JointTorque>();
                GameObject.Find("link_" + i.ToString()).GetComponent<JointTorque>().i = i;
            }
        }
    }
}