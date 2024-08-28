using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

public class RegexTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Regex r = new Regex(@"^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}");
        //class Regex Repesents an immutable regular expression.  

        string[] str = { "+91-9678967101", "9678967101", "+91-9678-967101", "+91-96789-67101", "+919678967101", "000" };
        //Input strings for Match valid mobile number.  
        foreach (string s in str)
        {
            Debug.Log(System.String.Format("{0} {1} a valid mobile number.", s, r.IsMatch(s) ? "is" : "is not"));
            //The IsMatch method is used to validate a string or  
            //to ensure that a string conforms to a particular pattern.  
        }

        Regex pattern = new Regex(@"(\s?(\S+)){2}$");

        string text = "Hello my name is Josh.\n<nobr>How do you do?</nobr>\nWhat's up homies?\n";

        //string formatted = pattern.Replace(text, "<nobr>$1</nobr>");

        string formatted = Regex.Replace(text, @"(\s?(\S+)){2}$", "<nobr>$0</nobr>", RegexOptions.Multiline, TimeSpan.FromSeconds(0.5));
        formatted = Regex.Replace(formatted, "<nobr>", "", RegexOptions.Multiline, TimeSpan.FromSeconds(0.5));
        formatted = Regex.Replace(formatted, "</nobr>", "", RegexOptions.Multiline, TimeSpan.FromSeconds(0.5));
        formatted = Regex.Replace(formatted, @"(\s?(\S+)){2}$", "<nobr>$0</nobr>", RegexOptions.Multiline, TimeSpan.FromSeconds(0.5));
        //string formatted = Regex.Replace(text, @"\s", "-");

        Debug.Log(formatted);
    }

}
