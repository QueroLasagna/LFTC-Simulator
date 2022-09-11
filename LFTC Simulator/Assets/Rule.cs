using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembly_CSharp
{
    public class Rule
    {
        public string ReadChar { get; set; }
        public string TargetState { get; set; }
        public string WriteChar { get; set; }
        public bool MoveRight { get; set; }

        public Rule(string line)
        {
            var s = line.Split(' ').Skip(1);
            this.ReadChar = s.ElementAt(0);
            this.TargetState = s.ElementAt(1);
            this.WriteChar = s.ElementAt(2);
            this.MoveRight = String.Equals(s.ElementAt(3), "R") ? true : false;
        }

        public string RuleToString()
        {
            List<string> s = new List<string>();
            s.Add(this.ReadChar);
            s.Add(this.TargetState);
            s.Add(this.WriteChar);
            s.Add(this.MoveRight ? "R" : "L");

            return String.Join(' ', s);
        }
    }
}
