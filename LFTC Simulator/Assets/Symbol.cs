using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Symbol : MonoBehaviour
{
    public TMP_Text Content;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetChar(char c)
    {
        Content.SetText(c.ToString());
    } 

    // Update is called once per frame
    void Update()
    {
        
    }
}
