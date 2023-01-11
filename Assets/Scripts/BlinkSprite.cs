using System.Collections;
using UnityEngine;

public class BlinkSprite : MonoBehaviour
{
    public GameObject sprite;
    public float blinkIntervalSprite = 0.5f;



    public void StartBlinkingSprite()
    {
        StopAllCoroutines();
        StartCoroutine(DoBlinkSprite());
    }

    private IEnumerator DoBlinkSprite()
    {
        while (true)
        {
            sprite.GetComponent<SpriteRenderer>().enabled = !sprite.GetComponent<SpriteRenderer>().enabled;
            yield return new WaitForSeconds(blinkIntervalSprite);
        }
    }
}