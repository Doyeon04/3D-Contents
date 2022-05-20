using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshDestroyTimer : MonoBehaviour
{
    public float fDestroyTime;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, fDestroyTime); // 지정한 시간이 지나면 gameObject destroy
    }

    // Update is called once per frame
    void Update()
    {

    }

}
