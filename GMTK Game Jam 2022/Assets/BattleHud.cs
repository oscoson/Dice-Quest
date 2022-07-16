using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHud : MonoBehaviour
{
    public static BattleHud Instance;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

}
