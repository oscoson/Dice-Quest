using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustEffectDestroy : MonoBehaviour
{
    [SerializeField] Animator dust;
    // Start is called before the first frame update
    void Start()
    {
        dust.Play("Player Step", 1);
        StartCoroutine(DestroyIn(0.4f));
    }

    private IEnumerator DestroyIn(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        Destroy(gameObject);
    }

}
