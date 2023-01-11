using System.Collections;
using UnityEngine;
using static GameplayHelper;

public class BlinkSprite : MonoBehaviour
{
    public GameObject sprite;
    //public SpriteRenderer spriteRenderer;
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
            //spriteRenderer.enabled = !spriteRenderer.enabled;
            
            sprite.GetComponent<SpriteRenderer>().enabled = !sprite.GetComponent<SpriteRenderer>().enabled;
            yield return new WaitForSeconds(blinkIntervalSprite);
        }
    }


}