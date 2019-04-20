using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public Sprite mBubbleSprite;
    public Sprite mPopSprite;

    [HideInInspector]
    public BubbleManager mBubbleManager = null;

    private Vector3 mMovementDirection = Vector3.zero;
    private SpriteRenderer mSpriteRenderer = null;
    private Coroutine mCurrentChanger = null;

    private void OnDestroy()
    {
        StopCoroutine(Pop());
    }

    public void Awake()
    {
        mSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Start()
    {
        mSpriteRenderer.sprite = mPopSprite;
        mCurrentChanger = StartCoroutine(DirectionChanger());
    }

    public void OnBecameInvisible()
    {
        transform.position = mBubbleManager.GetPlanePosition();
    }

    public void Update()
    {
        //movement
        transform.position += mMovementDirection * Time.deltaTime * 0.35f;
        //rotation
        transform.Rotate(Vector3.forward * Time.deltaTime * mMovementDirection.x * 20, Space.Self);
    }

    public IEnumerator Pop()
    {
        if (this.mSpriteRenderer != null && this.mSpriteRenderer.sprite != null)
        {
            mSpriteRenderer.sprite = mPopSprite;

            StopCoroutine(mCurrentChanger);
            mMovementDirection = Vector3.zero;

            // yield return new WaitForSeconds(0.5f);

            //transform.position = mBubbleManager.GetPlanePosition();

            mSpriteRenderer.sprite = mBubbleSprite;
            yield return new WaitForSeconds(0.5f);
            try
            {
                mSpriteRenderer.sprite = null;
                mCurrentChanger = StartCoroutine(DirectionChanger());
            }
            catch (MissingReferenceException) { }
        }   

    }

    public IEnumerator DirectionChanger()
    {
        while(gameObject.activeSelf)
        {
            mMovementDirection = new Vector2(Random.Range(-100, 100) * 0.01f,
                Random.Range(0, 100) * 0.01f);

            yield return new WaitForSeconds(1.0f);
        }
    }


}
