using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour {

    public ParticleSystem destroyPS;
    public ParticleSystem burstPS;

    [HideInInspector]
    public ColorScript colorScript;
    ForceController forceController;

    public static Action OnLeavingCameraArea;
    public static Action<GameObject, Collision2D> OnCollision;
    public static Action<GameObject, Collider2D> OnTrigger;

    public static Transform _transform;
    float maxX;

    bool isMortal = true;
    public bool IsMortal
    {
        get
        {
            return isMortal;
        }
        set
        {
            isMortal = value;
            if (!value)
            {
                //GetComponent<SpriteRenderer>().DOFade
                StartCoroutine(BecomingMortal(0.5F));
            }
        }
    }

    IEnumerator BecomingMortal(float delay)
    {
        yield return new WaitForSeconds(delay);
        isMortal = true;
    }

    private void Start()
    {
        maxX = Storage.instance.maxX;
        IsMortal = false;
    }

    private void OnEnable()
    {
        InputManager.OnPressed += forceController.StartChangingDirection;
        InputManager.OnHoldingKey += forceController.SetIncreasingBonusCoeff;
        InputManager.OnPressed += PlayTurnSound;
        InputManager.OnHoldingKey += ToggleEmitting;

        colorScript.OnColorChanged += ChangeColor;
    }

    private void OnDisable()
    {
        InputManager.OnPressed -= forceController.StartChangingDirection;
        InputManager.OnHoldingKey -= forceController.SetIncreasingBonusCoeff;
        InputManager.OnPressed -= PlayTurnSound;
        InputManager.OnHoldingKey -= ToggleEmitting;

        colorScript.OnColorChanged -= ChangeColor;
    }

    private void Awake()
    {
        _transform = transform;
        forceController = GetComponent<ForceController>();
        colorScript = GetComponent<ColorScript>();
    }

    private void OnDestroy()
    {
        _transform = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollision(gameObject, collision);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnTrigger(gameObject, other);
    }

    private void Update()
    {
        var x = transform.position.x;
        if (Mathf.Abs(x) > maxX && forceController.isStarted)
            OnLeavingCameraArea?.Invoke();
    }

    private void OnBecameInvisible()
    {
        //OnLeavingCameraArea?.Invoke();
    }

    void ChangeColor(Color color)
    {
        var trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.startColor = color;
        trailRenderer.endColor = color;

        burstPS.startColor = color;
    }

    public void DestroyPlayer()
    {
        destroyPS.transform.SetParent(null);
        destroyPS.startColor = colorScript.color;
        destroyPS.Play();
        Destroy(gameObject, 0.01F);
    }

    public void PlayTurnSound()
    {
        SoundManager.instance.PlaySound(Storage.instance.turnSound);
    }

    public void ToggleEmitting(bool state)
    {
        burstPS.enableEmission = state;
        return;

        if (state)
        {
            if (!burstPS.isPlaying)
                //burstPS.gameObject.SetActive(true);
                burstPS.enableEmission = true;
        }
        else
        {
            if (burstPS.isPlaying)
                //burstPS.gameObject.SetActive(false);
                burstPS.enableEmission = false;
        }

    }

}
