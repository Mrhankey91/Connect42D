using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private ParticleSystem particle;
    private ParticleSystem particleWin;
    private ParticleSystem.MainModule particleMain;
    private ParticleSystem.MainModule particleMainWin;
    private AudioSource audioSource;

    private Vector3 targetPosition;
    private bool move = false;
    private float moveSpeed = 10f;

    private int playerID = 0;
    public int PlayerID
    {
        get { return playerID; }
        set { playerID = value; }
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        particle = transform.Find("Particle").GetComponent<ParticleSystem>();
        particleWin = transform.Find("WinParticle").GetComponent<ParticleSystem>();
        particleMain = particle.main;
        particleMainWin = particleWin.main;
    }

    private void Update()
    {
        if(move)
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
    }

    public void Reset()
    {
        move = false;
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
        particleMain.startColor = color; 
        particleMainWin.startColor = color;
    }

    public void Place(Vector3 position)
    {
        SetTargetPosition(position);
        move = true;
        audioSource.Play();
    }

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    public void Win()
    {
        particleWin.Play();
    }
}
