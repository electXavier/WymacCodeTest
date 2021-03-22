using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * This class is an utility class to extract commands from given text file
 */
public static class CommandExtractor
{
    //list to add commands
    private static List<string> commands;
    //variables to ignore certain lines
    private static string comment = "//";
    private static string newline = "\r";

    //function to load command from text asset and return list of string
    public static List<string> LoadCommand(TextAsset textAsset)
    {
        commands = new List<string>();
        //divide text with new line
        string[] list = textAsset.text.Split('\n');

        //From the list ignore certain strings and add the rest
        foreach (string line in list)
        {

            if (!line.Contains(comment))
            {
                if (!line.Equals(newline) && line != null)
                {
                    commands.Add(line);
                }
            }
        }

        return commands;
    }
}


