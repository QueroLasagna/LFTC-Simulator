using Assembly_CSharp;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

public partial class Tape : MonoBehaviour
{
    // Unity properties
    public string u_TapeString = "test";
    public Vector3 u_SymbolOffSet = new Vector3(2, 0, 0);
    public Symbol Symbol = null;
    private List<Symbol> u_SymbolsList = new List<Symbol>();
    public TMP_Text UIRules;
    public TMP_Text UINextRule;
    public TMP_Text UICurrentState;


    // MT properties
    private bool m_IsActive { get; set; } = true;
    private string m_InicialState { get; set; }
    private string m_CurrentState { get; set; }
    private Rule m_CurrentRule { get; set; }
    private List<State> m_States { get; set; } = new List<State>();
    private string m_HeadBuffer { get; set; }
    private int m_HeadIndex { get; set; }
    private string m_FinalState { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("TM program loading initialized...");

        Vector3 position = transform.position;
        foreach (char c in u_TapeString)
        {
            InstanciateNewSymbol(c, position);
            position += u_SymbolOffSet;
        }

        var lines = File.ReadLines(@"A:\Repos\Playground\LFTC-Simulator\LFTC Simulator\Program.txt");
        var initialState = lines.ElementAt(1).ToString();
        var finalState = lines.ElementAt(2).ToString();
        var allStates = lines.ElementAt(0).Split(' ');
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
            m_States.Add(new State(state, isFinal, stateRules));
        }

        Debug.Log("TM program Loaded, Initialazing...");

        m_InicialState = m_CurrentState = initialState;
        m_FinalState = finalState;
        m_HeadIndex = 0;

        Debug.Log("TM Initialized, reading first symbol...");

        ReadSymbol();
        LoadNextRule();
        UIRules.SetText(BuildUiRulesText());
        LoadUI();

        Debug.Log("TM ready. Waiting for player...");

        if (!m_IsActive)
        {
            StopTM();
        }
    }

    private void StopTM()
    {
        Debug.Log("The Turing Machine has come to a stop");
        m_IsActive = false;
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
        if (!m_IsActive)
        {
            StopTM();
            return;
        }
        MoveHead();
        ReadSymbol();
        LoadNextRule();
        LoadUI();
        // Wait for player next move
    }

    private void LoadUI()
    {
        UICurrentState.SetText(m_CurrentState);
        if (m_IsActive)
        {
            UINextRule.SetText($"{m_CurrentState} {m_CurrentRule.RuleToString()}");
        }
        else
        {
            UINextRule.SetText("Final state");
        }
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

    private void LoadNextRule()
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

        m_CurrentRule = currentState.Rules.Find(rule => rule.ReadChar == m_HeadBuffer);
        if (m_CurrentRule == null)
        {
            m_IsActive = false;
            return;
        }
    }

    private void ReadSymbol()
    {
        if (m_HeadIndex >= u_SymbolsList.Count)
        {
            InstanciateNewSymbol('!', Vector3.zero);
        }
        if (m_HeadIndex < 0)
        {
            StopTM();
        }
        m_HeadBuffer = u_SymbolsList[m_HeadIndex].Content.text;
    }

    void MoveHead()
    {
        m_CurrentState = m_CurrentRule.TargetState;
        u_SymbolsList[m_HeadIndex].SetChar(m_CurrentRule.WriteChar.ToCharArray()[0]);
        transform.Translate(m_CurrentRule.MoveRight ? -u_SymbolOffSet : u_SymbolOffSet);
        if (m_CurrentRule.MoveRight) m_HeadIndex++; 
        else m_HeadIndex--;
    }

    private void InstanciateNewSymbol(char c, Vector3 position)
    {
        var newSymbol = Instantiate<Symbol>(Symbol);
        newSymbol.SetChar(c);
        newSymbol.transform.Translate(position);
        newSymbol.transform.parent = this.transform;
        u_SymbolsList.Add(newSymbol);
    }
}
