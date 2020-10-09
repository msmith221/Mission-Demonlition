using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set in Inspector")]
    public int numClouds = 40;
    public GameObject cloudPrefab;
    public Vector3 cloudPositionMin = new Vector3(-50, -5, 10);
    public Vector3 cloudPositionMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1;
    public float cloudScaleMax = 3;
    public float cloudSpeedMult = 0.5f;

    private GameObject[] cloudInstances;

    void Awake()
    {
        //make an array large enough to hold all the cloud instances
        cloudInstances = new GameObject[numClouds];
        //find the anchor
        GameObject anchor = GameObject.Find("CloudAnchor");
        //iterate through make clouds
        GameObject cloud;
        for (int i = 0; i < numClouds; i++)
        {
            cloud = Instantiate<GameObject>(cloudPrefab);

            //position cloud
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPositionMin.x, cloudPositionMax.x);
            cPos.y = Random.Range(cloudPositionMin.y, cloudPositionMax.y);

            //scale cloud
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudPositionMin.y, cPos.y, scaleU);

            //smaller clouds (with smaller scaleU) should be nearer to the ground
            cPos.y = Mathf.Lerp(cloudPositionMin.y, cPos.y, scaleU);
            //smaller clouds should be farther away
            cPos.z = 100 - 90 * scaleU;
            //apply transforms to cloud
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleU;
            //make child of the parent anchor
            cloud.transform.SetParent(anchor.transform);
            cloudInstances[i] = cloud;


        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject cloud in cloudInstances)
        {
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            //move large clouds faster
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            //if a cloud has moved too far to the left...
            if (cPos.x <= cloudPositionMin.x)
            {
                //move it to the far right
                cPos.x = cloudPositionMax.x;
            }
            cloud.transform.position = cPos;
        }
    }
}
