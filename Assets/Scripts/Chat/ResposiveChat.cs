using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResposiveChat : MonoBehaviour
{
    RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(Screen.width / 4, rt.sizeDelta.y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rt.sizeDelta = new Vector2(Screen.width / 4, rt.sizeDelta.y);
    }
}
