using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class TextParser : MonoBehaviour {

    static Regex markup_sec = new Regex(@"<Wait Seconds: *(\d+)>");

    public class ParsedChunk
    {
        public float delay;
        public string text;

        public override string ToString()
        {
            return "T: [[" + text + "]]; D: " + delay;
        }
    }

    public static List<ParsedChunk> parse(string intext)
    {
        List<ParsedChunk> resl = new List<ParsedChunk>();

        string[] parts = intext.Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach(string a in parts)
        {
            Debug.Log(a);
        }

        for(int i = 0; i < parts.Length; )
        {
            if(markup_sec.IsMatch(parts[i]))
            {
                float waitSec = float.Parse(markup_sec.Match(parts[i]).Groups[1].Value);
                ParsedChunk ch = new ParsedChunk();
                if (i < parts.Length - 1)
                {
                    ch.delay = waitSec;
                    ch.text = parts[i + 1];
                    Debug.Log(i + ". adding to resl: [[" + ch.text + "]] after " + ch.delay + " seconds.");
                    resl.Add(ch);
                }
                i = i + 2;
            } else {
                ParsedChunk ch = new ParsedChunk();
                ch.delay = 0f;
                ch.text = parts[i];
                Debug.Log(i + ". adding to resl: [[" + ch.text + "]] after " + ch.delay + " seconds.");
                resl.Add(ch);
                i++;
            }
        }

        foreach(ParsedChunk a in resl)
        {
            Debug.Log(a.ToString());
        }

        return resl;
    }
}
