using Assembly_CSharp;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public partial class Tape : MonoBehaviour
{
    // Unity properties
    public string Content = "x";
    public Vector3 OffSet = new Vector3();
    public Symbol Symbol = null;
    private List<Symbol> Symbols = new List<Symbol>();

    // MT properties
    public bool IsActive { get; set; } = true;
    public string InicialState { get; set; }
    public List<State> States { get; set; } = new List<State>();

    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        Vector3 position = this.transform.position;

        // Instanciate tape Symbols 
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

        // Generate Turing Machine program
        var lines = File.ReadLines(@"A:\Repos\Playground\LFTC-Simulator\LFTC Simulator\Program.txt");

        this.InicialState = lines.ElementAt(1).ToString();

        var allStates = lines.ElementAt(0).Split(' ');
        var finalState = lines.ElementAt(2).ToString();
        var allRules = lines.Skip(3).ToList();

        foreach (string state in allStates)
        {
            bool isFinal = state == finalState ? true : false;

            List<string> stateRules = new List<string>();

            if (!isFinal)
            {

                foreach (string rule in allRules)
                {
                    if (rule.StartsWith(state))
                    {
                        stateRules.Add(rule);
                    }
                }
            }
            this.States.Add(new State(state, isFinal, stateRules));
        }

        Debug.Log("MT program Loaded");
    }

    // Update is called once per frame
    void Update()
    {
        bool next = Input.GetKey(KeyCode.Space);
        if (next)
        {

        }
    }

    void NextMove()
    {

    }
}
