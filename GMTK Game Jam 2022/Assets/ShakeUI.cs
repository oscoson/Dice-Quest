using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeUI : MonoBehaviour
{
    [SerializeField] private Transform canvasPos;


    public IEnumerator Shake(float duration, float magnitude)
    {
        Debug.Log("Begin Shake");
        Vector3 originalPos = canvasPos.localPosition;
        
        float elapsed = 0.0f;

        while(elapsed < duration)
        {
            Debug.Log("Shaking");
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }

}
