using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshDestroyTimer : MonoBehaviour
{
    public float fDestroyTime;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, fDestroyTime); // ������ �ð��� ������ gameObject destroy
    }

    // Update is called once per frame
    void Update()
    {

    }

}
