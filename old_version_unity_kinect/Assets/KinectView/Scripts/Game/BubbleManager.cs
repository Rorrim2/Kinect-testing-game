﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour {

    public GameObject mBubblePrefab;

    private List<Bubble> mAllBubbles = new List<Bubble>();
    private Vector2 mBottomLeft = Vector2.zero;
    private Vector2 mTopRight = Vector2.zero;

    public void Awake()
    {
        //Bounding values
        mBottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.farClipPlane));
        mTopRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight / 2, Camera.main.farClipPlane));
    }

    public void Start()
    {
        StartCoroutine(CreateBubbles());
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.farClipPlane)), 0.5f);
        Gizmos.DrawSphere(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight / 2, Camera.main.farClipPlane)), 0.5f);
    }

    public Vector3 GetPlanePosiotion()
    {
        float targetX = Random.Range(mBottomLeft.x, mTopRight.x);
        float targetY = Random.Range(mBottomLeft.y, mTopRight.y);

        return new Vector3(targetX, targetY, 0); 
    }

    public IEnumerator CreateBubbles()
    {
        while(mAllBubbles.Count < 20)
        {
            //create and add
            GameObject newBubbleObject = Instantiate(mBubblePrefab, GetPlanePosiotion(), Quaternion.identity, transform);
            Bubble newBubble = newBubbleObject.GetComponent<Bubble>();

            //Setup bubble
            newBubble.mBubbleManager = this;
            mAllBubbles.Add(newBubble);

            yield return new WaitForSeconds(0.5f);
        }
    }
}
