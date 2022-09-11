using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembly_CSharp
{
    public class State
    {
        public string Name { get; set; }
        public bool IsFinal { get; set; }
        public List<Rule> Rules { get; set; } = new List<Rule>();

        public State(
            string name,
            bool isFinal,
            List<string> rules)
        {
            this.Name = name;
            this.IsFinal = isFinal;
            if (rules.Count() > 0)
            {
                foreach (string rule in rules)
                {
                    this.Rules.Add(new Rule(rule));
                }
            }
        }

        public void PrintRules()
        {
            foreach (Rule rule in this.Rules)
            {
                Console.WriteLine(this.Name + ' ' + rule.RuleToString());
            }
        }
    }
}
