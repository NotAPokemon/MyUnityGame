using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnityEngine;
public class MagicTokenizer
{
    public List<BaseCommand> commands = new List<BaseCommand>();
    public BaseCommand structuredCommands;

    public class Line
    {
        private string line;
        private int index;

        public Line(string str)
        {
            line = str;
            index = 0;
        }

        public char next()
        {
            if (index >= line.Length) throw new InvalidOperationException("No more characters.");
            return line[index++];
        }

        public int len() => line.Length - index;

        public override string ToString()
        {
            return line;
        }
    }

    public class CommandFactory
    {
        Line str;
        public CommandFactory(string str) 
        {
            this.str = new Line(str);
        }

        public BaseCommand toCommand()
        {
            string name = "";
            char t = str.next();
            while (t != '{')
            {
                name += t;
                t = str.next();
            }
            string args = "";
            t = str.next();
            while (t != '}')
            {
                args += t;
                try
                {
                    t = str.next();
                } catch
                {
                    break;
                }
            }

            Type type = getClass(name);
            if (type != null)
            {
                return (BaseCommand)Activator.CreateInstance(type, new object[] { args.Split(",") });
            }

            return new BaseCommand(args.Split(","));
        }

        Type getClass(string name) 
        {
            var commandType = Assembly.GetExecutingAssembly().GetTypes()
            .FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                                 typeof(BaseCommand).IsAssignableFrom(t));

            return commandType;
        }

    }

    public MagicTokenizer(string path) 
    {
        string content = File.ReadAllText(path);
        string[] split = content.Split(";");
        List<Line> lines = new List<Line>();
        for (int i = 0; i < split.Length; i++)
        {
            lines.Add(new Line(split[i].Trim()));
        }

        lines[lines.Count - 1] = new Line("EOF");

        List<string> tokens = tokenize(lines);
        
        foreach (string token in tokens)
        {
            commands.Add(new CommandFactory(token).toCommand());
        }
    }

    public void structure()
    {
        structuredCommands = commands[0];
        for (int i = 1; i < commands.Count; i++)
        {
            commands[i].parent = commands[i - 1];
        }
        for (int i = 0;i < commands.Count - 1;i++)
        {
            structuredCommands.getChild(i).setChild(commands[i + 1]);
        }
        
    }


    public void runAll()
    {
        float totalCost = 0;
        for (int i = 0; i < commands.Count; i++)
        {
            if (Player.player.mana >= commands[i].manaCost)
            {
                commands[i].run();
                Player.player.mana -= commands[i].manaCost;
                totalCost += commands[i].manaCost;
            }
        }
        commands[0].self.GetComponent<DestroyOnDone>().sustain(totalCost / 10f);
    }

    public List<string> tokenize(List<Line> list)
    {
        List<string> result = new List<string>();
        for (int i = 0;i < list.Count;i++)
        {
            char nextItem = list[i].next();
            while (nextItem == '[')
            {
                char t = list[i].next();
                result.Add("");
                while (t != ']')
                {
                    result[result.Count - 1] += t;
                    try
                    {
                        t = list[i].next();
                    } catch { }
                    
                    if (t == ']')
                    {
                        try
                        {
                            nextItem = list[i].next();
                        } catch
                        {
                            nextItem = ' ';
                        }
                    }
                }
            }
        }
        return result;
    }

    

}
