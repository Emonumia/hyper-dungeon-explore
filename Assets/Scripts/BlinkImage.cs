using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlinkImage : MonoBehaviour
{
    public Image image;
    public float blinkInterval = 0.5f;
    public bool blinkOnStart = true;

    private void Start()
    {
        if (blinkOnStart)
        {
            StartBlinking();
        }
    }

    public void StartBlinking()
    {
        StopAllCoroutines();
        StartCoroutine(DoBlink());
    }

    private IEnumerator DoBlink()
    {
        while (true)
        {
            image.enabled = !image.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
