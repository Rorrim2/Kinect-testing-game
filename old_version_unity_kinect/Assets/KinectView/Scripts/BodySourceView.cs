using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

using Kinect = Windows.Kinect;
using Joint = Windows.Kinect.Joint;

public class BodySourceView : MonoBehaviour 
{
    public BodySourceManager mBodySourceManager = null;
    public GameObject mJointObject;


    private Dictionary<ulong, GameObject> mBodies = new Dictionary<ulong, GameObject>();
    private List<Kinect.JointType> _joints = new List<Kinect.JointType>
    {
        Kinect.JointType.HandLeft,
        Kinect.JointType.HandRight,
    };

    
    void Update() 
    {

        #region Get Kinect data
        //if (BodySourceManager == null)
        //{
        //    return;
        //}

        //mBodySourceManager = BodySourceManager.GetComponent<BodySourceManager>(); // co tu się?
        //if (mBodySourceManager == null)
        //{
        //    return;
        //}
        if (mBodySourceManager == null)
            return;
        Kinect.Body[] data = mBodySourceManager.GetData();
        if (data == null)
            return;

        List<ulong> trackedIDs = new List<ulong>();
        foreach(var body in data)
        {
            if (body == null)
               continue;
            if (body.IsTracked)
                trackedIDs.Add(body.TrackingId);
        }
        #endregion
        #region Delete Kinect bodies
        List<ulong> knownIds = new List<ulong>(mBodies.Keys);
        foreach(ulong trackingId in knownIds)
        {
            if(!trackedIDs.Contains(trackingId))
            {
                //Destroy body object
                Destroy(mBodies[trackingId]);

                //Remove from list
                mBodies.Remove(trackingId);
            }
        }
        #endregion
        #region Create Kinect Bodies
        foreach(var body in data)
        {
            //if no body, skip
            if (body == null)
                continue;
            if(body.IsTracked)
            {
                //if body is not tracked, create body
                if (!mBodies.ContainsKey(body.TrackingId))
                    mBodies[body.TrackingId] = CreateBodyObject(body.TrackingId);

                //Update position
                UpdateBodyObject(body, mBodies[body.TrackingId]);
            }
        }
        #endregion
    }

    public GameObject CreateBodyObject(ulong id)
    {
        // Create body parent
        GameObject body = new GameObject("Body:" + id);
        
        //Create joints
        foreach(Kinect.JointType joint in _joints)
        {
            //create object
            GameObject newJoint = Instantiate(mJointObject);
            newJoint.name = joint.ToString();

            //parent to body
            newJoint.transform.parent = body.transform;
        }
        
        return body;
    }
    
    public void UpdateBodyObject(Kinect.Body body, GameObject bodyObject)
    {
        //update joints
        foreach(Kinect.JointType _joint in _joints)
        {
            
            //get new target position
            Joint sourceJoint = body.Joints[_joint];
            Vector3 targetPosition = GetVector3FromJoint(sourceJoint);
            targetPosition.z = 0;

            //get joint, set new position
            Transform jointObject = bodyObject.transform.Find(_joint.ToString());
            jointObject.position = targetPosition;

        }
    }
    
    public static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }
}
