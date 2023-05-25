using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        Invoke(nameof(LateDestroy), 0.5f);
    }

    void LateDestroy()
    {
        Destroy(gameObject);
    }
}
