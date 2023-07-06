using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinFlip : MonoBehaviour
{
    public AnimationCurve flipCurve;
    private float flipTime = 3f; // in seconds
    private float flipTarget = 360;//target flip angle, 0 or 360 degrees is Player 1 and 180 degree is Player 2
    private bool flip = false;

    void Awake()
    {
        FlipCoin(2);
    }

    private void Update()
    {
        if (flip)
        {
            transform.rotation = Quaternion.Euler(flipTarget * flipCurve.Evaluate(Time.time / flipTime), 0f, 0f);

            if (transform.rotation.x == flipTarget)
                flip = false;
        }
    }

    public void FlipCoin(int playerIDWin, float flipTime = 3f)
    {
        this.flipTime = flipTime;
        flipTarget = 720;
        if(playerIDWin == 2)
        {
            flipTarget += 180;
        }
        flip = true;
    }
}
