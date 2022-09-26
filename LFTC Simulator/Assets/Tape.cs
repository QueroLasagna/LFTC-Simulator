using Assembly_CSharp;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

public partial class Tape : MonoBehaviour
{
    // Unity properties
    public string Content = "x";
    public Vector3 OffSet = new Vector3();
    public Symbol Symbol = null;
    private List<Symbol> Symbols = new List<Symbol>();
    public TMP_Text UIRules;

    // MT properties
    public bool IsActive { get; set; } = true;
    public string InicialState { get; set; }
    public string CurrentState { get; set; }
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

        UIRules.SetText(BuildUiRulesText());
    }

    private string BuildUiRulesText()
    {
        var result = new StringBuilder();

        foreach (var state in this.States)
        {
            var stateName = state.Name;
            foreach (var rule in state.Rules)
            {
                var line = $"{stateName} {rule.RuleToString()}";
                result.AppendLine(line);
            }
        }
        return result.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        bool next = Input.GetKey(KeyCode.Space);
        if (next)
        {
            next = false;
            NextMove();
        }
    }

    void NextMove()
    {

    }

    void Move(bool right)
    {
        Vector3 direction = new Vector3(100, 0, 0);
        this.transform.Translate(right ? direction : -direction);
    }
}
