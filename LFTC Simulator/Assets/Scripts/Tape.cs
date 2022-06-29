using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tape : MonoBehaviour
{
    public string content = "test";

    public GameObject tape, symbol;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CreateTape(string input)
    {
        int tapeSize = input.Length;

        var arr = input.ToCharArray();
        foreach (char c in arr)
        {
            // Instantiate()
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
