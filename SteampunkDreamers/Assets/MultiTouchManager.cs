using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MultiTouchManager : MonoBehaviour
{
    public bool IsTouching { get; private set; }

    public float minZoomInch = 0.2f;
    public float maxZoomInch = 0.5f;
    private float minZoomPixel;
    private float maxZoomPixel;
    public float ZoomInch {  get; private set; } // -1~1 사이값
    private List<int> fingerIdList = new List<int>();

    private void Awake()
    {
        //Debug.Log(Screen.width);
        //Debug.Log(Screen.height);
        //Debug.Log(Screen.dpi); // 실제 디스플레이 비율로 터치 먹일 수 있음
    }

    public void UpdatePinch()
    {


        if (fingerIdList.Count >= 2)
        {
            // [0] 1st Touch / [1] 2nd Touch
            Vector2[] prevTouchPos = new Vector2[2];
            Vector2[] currentTouchPos = new Vector2[2];
            // PrevFrame Distance
            for (int i = 0; i < 2; ++i)
            {
                var touch = Array.Find(Input.touches, x => x.fingerId ==  fingerIdList[i]);
                currentTouchPos[i] = Input.touches[i].position;
                prevTouchPos[i] = touch.position - touch.deltaPosition;
            }

            // PreveFrame Distance
            var prevFrameDist = Vector2.Distance(prevTouchPos[0], prevTouchPos[1]);
            // CurrFrame Distance
            var currFrameDist = Vector2.Distance(currentTouchPos[0], currentTouchPos[1]);

            //Debug.Log(currFrameDist - prevFrameDist);

            var distancePixel = prevFrameDist - currFrameDist;
            //var distanceInch = distancePixel / Screen.dpi;
            //Debug.Log(distanceInch);
            ZoomInch = distancePixel / Screen.dpi;
        }
    }

    public void Update()
    {
        foreach (var touch in Input.touches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    fingerIdList.Add(touch.fingerId);
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    fingerIdList.Remove(touch.fingerId);
                    break;
            }
        }
#if UNITY_EDITOR || UNITY_STANDALONE
// 휠을 이용한 줌 인/아웃 코드
#elif UNITY_ANDROID || UNITY_IOS
UpdatePinch();
#endif 
    }
}
