using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Experiment
{
    class FindAnswer
    {
       public static List<string> FindAnswer1(String h)
        {
            List<string> answer = new List<string>();
            String p = "https://www.zhihu.com/question/([^\"]*)answer";
            MatchCollection matches = new Regex(p).Matches(h);
            foreach (Match match in matches)
            {
                answer.Add(match.ToString().Replace("/answer", "").Trim());
                Console.WriteLine(match.ToString().Replace("/answer", "").Trim());
            }
            return answer;
        }
    }
}
