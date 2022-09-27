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
    public string TapeString = "";
    public Vector3 OffSet = new Vector3();
    public Symbol Symbol = null;
    private List<Symbol> Symbols = new List<Symbol>();
    public TMP_Text UIRules;
    public TMP_Text UICurrentState;


    // MT properties
    private bool m_IsActive { get; set; } = true;
    private string m_InicialState { get; set; }
    private string m_CurrentState { get; set; }
    private List<State> m_States { get; set; } = new List<State>();
    private string m_HeadBuffer { get; set; }
    private int m_StringIndex { get; set; }
    private string m_FinalState { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        Vector3 position = transform.position;
        m_StringIndex = 0;
        m_HeadBuffer = TapeString[m_StringIndex].ToString();

        // Instanciate tape Symbols 
        foreach (char c in TapeString)
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

        m_InicialState = m_CurrentState = lines.ElementAt(1).ToString();
        UICurrentState.SetText(m_InicialState);

        var allStates = lines.ElementAt(0).Split(' ');
        m_FinalState = lines.ElementAt(2).ToString();
        var allRules = lines.Skip(3).ToList();

        foreach (string state in allStates)
        {
            bool isFinal = state == m_FinalState ? true : false;

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
            m_States.Add(new State(state, isFinal, stateRules));
        }

        Debug.Log("MT program Loaded");

        UIRules.SetText(BuildUiRulesText());
    }

    private string BuildUiRulesText()
    {
        var result = new StringBuilder();

        foreach (var state in this.m_States)
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
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("User input detected");
            NextMove();
        }
    }

    void NextMove()
    {
        if (m_CurrentState == m_FinalState)
        {
            m_IsActive = false;
            return;
        }

        var currentState = m_States.Find(state => state.Name == m_CurrentState);
        if (currentState == null)
        {
            m_IsActive = false;
            return;
        }

        var currentRule = currentState.Rules.Find(rule => rule.ReadChar == m_HeadBuffer);
        if (currentRule == null)
        {
            m_IsActive = false;
            return;
        }

        var stringArr = TapeString.ToCharArray();
        var writeChar = currentRule.WriteChar.ToCharArray()[0];

        stringArr[m_StringIndex] = writeChar;

        if (m_StringIndex >= TapeString.Length)
        {
            var newSymbol = Instantiate<Symbol>(Symbol);
            newSymbol.SetChar(writeChar);
            newSymbol.transform.Translate(new Vector3(2, 0, 0));
            newSymbol.transform.parent = this.transform;
            Symbols.Add(newSymbol);
        }
        else
        {
            Symbols[m_StringIndex].SetChar(writeChar);
        }

        TapeString = stringArr.ArrayToString();

        m_CurrentState = currentRule.TargetState;
        UICurrentState.SetText(m_CurrentState);

        Move(currentRule.MoveRight);

        if (m_StringIndex >= TapeString.Length)
        {
            m_HeadBuffer = "!";
            TapeString = TapeString + "!";
        }
        else
        {
            m_HeadBuffer = TapeString[m_StringIndex].ToString();
        }
    }

    void Move(bool right)
    {
        Vector3 direction = new Vector3(2, 0, 0);
        transform.Translate(right ? -direction : direction);
        if (right) m_StringIndex++; 
        else m_StringIndex--;
    }
}
