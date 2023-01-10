using UnityEngine;
using UnityEngine.UI;

public class BlinkController : MonoBehaviour
{
    //public BlinkImage blinkImage;
    public BlinkSprite blinkSprite;

    public void StartBlinking()
    {
        //blinkImage.StartBlinking();
        blinkSprite.StartBlinkingSprite();
    }

    public void StopBlinking()
    {
        //blinkImage.StopAllCoroutines();
        //blinkImage.image.enabled = true;
        blinkSprite.StopAllCoroutines();
        blinkSprite.sprite.GetComponent<SpriteRenderer>().enabled = true;
    }
}
