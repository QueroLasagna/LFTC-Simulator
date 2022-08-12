using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Tape : MonoBehaviour
{
    public string Content = "aabb";
    public Vector3 OffSet = new Vector3();
    public Symbol Symbol = null;
    private List<Symbol> Symbols = new List<Symbol>();

    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        Vector3 position = this.transform.position;

        foreach (char c in Content)
        {
            var newSymbol = Instantiate<Symbol>(Symbol);
            newSymbol.SetChar(c);
            newSymbol.transform.Translate(position);
            newSymbol.transform.parent = this.transform;
            Symbols.Add(newSymbol);
            position += OffSet;
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
