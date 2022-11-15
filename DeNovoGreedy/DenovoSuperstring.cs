using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeNovoGreedy
{
    internal class DenovoSuperstring
    {
        string[] _kMers;
        int _k;
        int _l;
        List<Edge> _edges = new();
        public DenovoSuperstring(string[] kMers, int l)
        {
            _kMers = kMers;
            _l = l;
            _k = kMers[0].Length;

            BuildGraphEdges(_kMers);
        }
        public DenovoSuperstring(string s, int k, int l)
        {
            if (k > s.Length) throw new Exception("K > s.length");
            if (l > k) throw new Exception("K > l");
            _k = k;
            _l = l;

            _kMers = SplitToKmers(s, k);

            BuildGraphEdges(_kMers);
        }

        private void BuildGraphEdges(string[] kMers)
        {
            for (int i = 0; i < kMers.Length; i++)
            {
                string currentSuff = kMers[i];
                for (int j = 0; j < kMers.Length; j++)
                {
                    if (i == j) continue;
                    string toComparePref = kMers[j];

                    //int cov = kMers.Length + i - j;
                    int cov = 0;
                    for (int it = 0; it < _k; it++)
                    {
                        if (IsMatch(currentSuff, it, toComparePref))
                        {
                            cov = _k - it;
                            break;
                        }
                    }
                    if (cov >= _l)
                    {
                        var edge = new Edge
                        {
                            fromIndex = i,
                            fromS = currentSuff,
                            toIndex = j,
                            toS = toComparePref,
                            coverage = cov
                        };
                        _edges.Add(edge);
                    }
                }
            }
        }

        private string[] SplitToKmers(string s, int k)
        {
            //string[] kMers = new string[s.Length - k + 1];
            HashSet<string> kMers = new();
            for (int i = 0; i < s.Length - k + 1; i++)
            {
                //kMers[i] = s.Substring(i, k);
                kMers.Add(s.Substring(i, k));
            }
            return kMers.ToArray();
        }

        internal string FindSuperstring(bool print)
        {
            List<string> verticies = _kMers.ToList();
            List<Edge> edges = _edges.ToList();

            while(edges.Count > 0)
            {
                var puvodni = edges.ToList();
                int maxCov = edges.Max(e => e.coverage);
                int c = edges.Where(e => e.coverage == maxCov).Count();
                var highestEdge = edges.Where(e => e.coverage == maxCov).ToArray()[new Random().Next(c)];

                string newStr = verticies[highestEdge.fromIndex] + verticies[highestEdge.toIndex].Substring(highestEdge.coverage);
                if (print)
                {
                    bool a = false, b = false;
                    for (int i = 0; i < verticies.Count; i++)
                    {
                        if (i == highestEdge.fromIndex)
                        {
                            a = true;
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.Write($"F<{verticies[i]}> ");
                            Console.BackgroundColor = ConsoleColor.Black;
                        }
                        else if (i == highestEdge.toIndex)
                        {
                            b = true;
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.Write($"T<{verticies[i]}> ");
                            Console.BackgroundColor = ConsoleColor.Black;
                        }
                        else
                            Console.Write($"{verticies[i]} ");
                    }
                    Console.Write($"    {highestEdge.coverage} \n");
                    if (!a || !b)
                    {

                    }
                }

                verticies[verticies.IndexOf(highestEdge.fromS)] = newStr;
                verticies.RemoveAt(verticies.IndexOf(highestEdge.toS));
                edges.Remove(highestEdge);
                //removed.Add(verticies[highestEdge.toIndex]);

                for (int i = edges.Count - 1; i >= 0; i--)
                {
                    Edge edge = edges[i];

                    if (edge.toS == highestEdge.toS) edges.Remove(edge);

                    if (edge.fromS == highestEdge.fromS) edges.Remove(edge);

                    if (edge.fromS == highestEdge.toS && edge.toS == highestEdge.fromS) edges.Remove(edge);
                }
                for (int i = 0; i < edges.Count; i++)
                {
                    Edge edge = edges[i];

                    if (edge.fromS == highestEdge.toS)
                    {
                        edge.fromS = newStr;
                    }
                    if (edge.toS == highestEdge.fromS)
                    {
                        edge.toS = newStr;
                    }
                }
                //fix indexes
                foreach (Edge edge in edges)
                {
                    edge.fromIndex = verticies.IndexOf(edge.fromS);
                    edge.toIndex = verticies.IndexOf(edge.toS);
                }
                /*
                verticies[highestEdge.fromIndex] = newStr;
                removed.Add(verticies[highestEdge.toIndex]);
                verticies.RemoveAt(highestEdge.toIndex);
                vertCount--;
                for (int i = 0; i < edges.Count; i++)
                {
                    Edge edge = edges[i];

                    // smazani vsech edge ze smazaneho
                    if (highestEdge.toIndex == edge.toIndex)
                    {
                        edges.RemoveAt(i);
                        continue;
                    }
                    if (highestEdge.fromIndex == edge.fromIndex)
                    {
                        edges.RemoveAt(i);
                        continue;
                    }
                }
                for (int i = edges.Count - 1; i >= 0; i--)
                {
                    Edge edge = edges[i];
                    
                    // prekotveni ze smazaneho
                    if (highestEdge.toIndex == edge.fromIndex)
                    {
                        //edge.fromIndex = highestEdge.fromIndex
                        if (edge.toS == newStr)
                        {

                        }
                        edge.fromS = newStr;
                    }
                    // prejmenovani na zmeneny retezec
                    if (highestEdge.fromIndex == edge.toIndex)
                    {
                        //edge.toIndex = highestEdge.toIndex;
                        if (edge.fromS == newStr)
                        {

                        }
                        edge.toS = newStr;
                    }
                    
                }
                foreach (Edge edge in edges)
                {
                    edge.fromIndex = verticies.IndexOf(edge.fromS);
                    edge.toIndex = verticies.IndexOf(edge.toS);
                }
                */
                
            }

            string superstring = "";

            for (int i = 0; i < verticies.Count; i++)
            {
                superstring += verticies[i];
            }

            return superstring;
        }
        private bool IsMatch(string suffS, int s1Start, string prefS)
        {
            for (int i = s1Start, j = 0; i < suffS.Length; i++, j++)
            {
                if (suffS[i] != prefS[j]) return false;
            }
            return true;
        }
        private class Edge
        {
            public int fromIndex;
            public string fromS;
            public string toS;
            public int toIndex;
            public int coverage;
        }
    }
}
