using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;


namespace My_Console_Text
{
    class Program
    {
        static void Main(string[] args)
        {
            
            MyConsole console = new MyConsole();
            console.Run();
        }
    }
    public class MyConsole
    {
        string root; 
        string BackSlash = "\\"; 
        string CommandCd = "cd";
        string CommandDir = "dir";
        string[] commandSplit;
        List<string> history = new List<string>();
        private String _currentDirectory = Directory.GetCurrentDirectory();
        
        public void Run()
        {
            root = Directory.GetDirectoryRoot(_currentDirectory);
            while (true)
            {
                String command = Prompt();
                commandSplit = command.Split();
                history.Add(command);
                root = Directory.GetDirectoryRoot(_currentDirectory);

                if (command == CommandCd + " ..")
                {
                    ChangeDirectory(_currentDirectory); // Revien au repertoire parents
                }
                else if (command == CommandCd + " /" || command == CommandCd +" "+BackSlash)
                {
                    _currentDirectory = root;
                }
                else if (commandSplit[0] == CommandCd)
                {
                    ChangeDirectoryCd(_currentDirectory, command); // Ouvre un dossier ou affiche le chemin actuel
                }

                else if (commandSplit[0] == "color")
                {
                    ListeColor();
                }                
                else if (commandSplit[0] == "fgcolor")
                {
                    ChangeFgColor(commandSplit[1]);
                }
                else if (commandSplit[0] == "bgcolor")
                {
                    ChangeBgColor(commandSplit[1]);
                }
                else if (commandSplit[0] == CommandDir)
                {
                    if (command == CommandDir)
                    {
                        ListDirectory(_currentDirectory); //Affiche uniquement les dossiers
                    }
                    else if (commandSplit[1] == "/l")
                    {
                        ListFiles(_currentDirectory); // Affiche uniquement les fichiers
                    }
                    else if (commandSplit[1] == "/t" && commandSplit.Length == 3)
                    {
                        Alphabetique(_currentDirectory, commandSplit[2]);
                    }
                    else
                    {
                        Console.WriteLine("mauvaise commande");
                    }
                }
                else if (command == "history")
                {
                    foreach (string item in history)
                    {
                        Console.WriteLine(item);
                    }
                }
                else if (command == "cls-history")
                {
                    history.Clear();
                }
                else if (command == "cls")
                {
                    Console.Clear();
                }
                else if (command == "exit")
                {
                    break;
                }                
                else if (command == "pwd")
                {
                    Pwd();
                }
                else if (command == "exit")
                {
                    Exits();
                }
                else if (command == "")
                {
                    Redirection(comm);
                }
                else
                {
                    Console.WriteLine(command + " n’est pas reconnu en tant que commande interne \nou externe, un programme exécutable ou un fichier de commandes.");
                }
            }
        }

        public String Prompt()
        {
            Console.Write(_currentDirectory + "> ");
            String command = Console.ReadLine();
            return command;

        }
        public void ChangeDirectoryCd(String newPath, string command)
        {
            string chem;
            if (command == CommandCd)
            {
                Console.WriteLine(_currentDirectory);
                return;
            }
            else if (_currentDirectory == root + BackSlash)
            {
                chem = _currentDirectory + commandSplit[1];
            }
            else
            {
                chem = _currentDirectory + BackSlash + commandSplit[1];
            }

            string[] tabfichier = Directory.GetFileSystemEntries(_currentDirectory);

            for (int i = 0; i < tabfichier.Length; i++)
            {
                if (tabfichier[i] == chem)
                {
                    if (_currentDirectory == root + BackSlash)
                    {

                        newPath = _currentDirectory + commandSplit[1];
                        _currentDirectory = newPath;
                        return;
                    }
                    else
                    {
                        newPath = _currentDirectory + BackSlash + commandSplit[1];
                        _currentDirectory = newPath;
                        return;
                    }
                }
            }
            Console.WriteLine("repertoire inexistant");
        }
        public void ChangeDirectory(String newPath)
        {
            try
            {
                string parent = Directory.GetParent(newPath).FullName;
                _currentDirectory = parent;
            }
            catch (ArgumentNullException)
            {
                System.Console.WriteLine("Path is a null reference.");
            }
            catch (ArgumentException)
            {
                System.Console.WriteLine("Path is an empty string, " +
                    "contains only white spaces, or " +
                    "contains invalid characters.");
            }

        }
        public void ListDirectory(String directoryPath)
        {
            IEnumerable<String> listedDirectories = Directory.EnumerateDirectories(directoryPath);
            foreach (string item in listedDirectories)
            {
                DateTime date = Directory.GetCreationTime(item);
                Console.WriteLine(date + " " + item);
            }
        }
        public void ListFiles(String directoryPath)
        {
            IEnumerable<String> listedFiles = Directory.EnumerateFiles(directoryPath);
            foreach (string item in listedFiles)
            {
                DateTime date = Directory.GetCreationTime(item);
                Console.WriteLine(date + " " + item);
            }
        }
        public static void Alphabetique(string path, string searchPattern)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(path, searchPattern);
                Console.WriteLine("The number of directories starting with {0} is {1}.", searchPattern, dirs.Length);

                foreach (string dir in dirs)

                {
                    Console.WriteLine(dir);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }
        public static void ChangeFgColor(string command)
        {
            ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));

            foreach (var color in colors)
            {
                if (Convert.ToString(color) == command)
                {
                    Console.ForegroundColor = color;
                    return;
                }
            }
            Console.WriteLine("La couleur ou la syntaxe n'existe pas, taper 'color' pour voir la liste des couleurs disponible");
        }
        public static void ListeColor()
        {
            ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));

            Console.WriteLine(String.Join("\n", colors));
        }
        public static void ChangeBgColor(string command)
        {
            ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));

            foreach (var color in colors)
            {
                if (Convert.ToString(color) == command)
                {
                    Console.BackgroundColor = color;
                    Console.Clear();
                    return;
                }
            }
            Console.WriteLine("La couleur ou la syntaxe n'existe pas, taper 'color' pour voir la liste des couleurs disponible");
            //hello
        }
        public static void Exits()
        {
            Environment.Exit(0);
        }
        public static void Pwd()
        {
            Console.WriteLine(Directory.GetCurrentDirectory() + "\n");
        }
        public static void Redirection(string command)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C " + command ;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.Close();
        }

    }
}
