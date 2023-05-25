using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchStickController : MonoBehaviour
{
    public GameObject fire;
    public bool isFireActive = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActiveFire()
    {
        if (fire != null)
        {
            isFireActive = true;
            Invoke(nameof(FireActive), 0.4f);
        }
    }

    public void SelfDestroy()
    {
        Invoke(nameof(LateDestroy), 0.5f);
    }

    public void LateDestroy()
    {
        Destroy(gameObject);
    }

    private void FireActive()
    {
        fire.SetActive(true);
        Invoke(nameof(FireDisable), 180f);
    }

    private void FireDisable()
    {
        fire.SetActive(false);
        isFireActive = false;
    }
}
