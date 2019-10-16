using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ObjectInLevel : MonoBehaviour {

    public void Collect()
    {
        Deactivate();
        transform.DOScale(0, 0.5F);
        GetComponent<SpriteRenderer>().DOFade(0, 0.5F);
        CreateFloatingText();
        DestroyObject(2);
    }

    public int BonusAmount()
    {
        return (int)(transform.localScale.magnitude * 25);
    }

    void CreateFloatingText()
    {
        var go = Instantiate(
            Storage.instance.floatingTextPrefab, 
            transform.position, 
            Quaternion.identity, 
            null);

        var txt = go.GetComponentInChildren<Text>();
        txt.text = "+" + BonusAmount().ToString();
        txt.transform.DOScale(transform.localScale.x * 1.5F * Vector3.one, 1);
        txt.DOFade(0, 1);

        Destroy(go, 2);
    }

    public void CollideAsEnemy()
    {
        Deactivate();
        transform.DOShakePosition(1, 0.5F, 5, 40);
        GetComponent<SpriteRenderer>().DOFade(0, 1);
        DestroyObject(2);
    }

    private void Deactivate()
    {
        GetComponent<Collider2D>().enabled = false;

        foreach (var loop in GetComponents<Loop>())
        {
            loop.StopLooping();
        }
    }

	private void DestroyObject(float delay)
    {
        Destroy(gameObject, delay);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
