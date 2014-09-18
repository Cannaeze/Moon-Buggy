using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Runtime.InteropServices;

namespace MoonBuggy
{
    class Highscore
    {
        Program program = new Program();

        private string currentPath = Directory.GetCurrentDirectory();
        private string[] header = null;
        private List<string> points = new List<string>();
        private Hashtable highscore = new Hashtable();
        private List<PlayerHolder> playerHolders = new List<PlayerHolder>();
        private StringBuilder sb = new StringBuilder();

        //Hier wird das File beim aufruf der Klasse gelesen
        public Highscore()
        {
            removeFileSave();
            String line = null; //save the line from file
            if (!File.Exists(this.getPath()))
            {
                string[] lines = new string[21];
                Console.WriteLine("Highscore-Datei nicht gefunden. Es wird eine Neue erstellt");
                //File.Create(@currentPath + "\\highscore.txt");
                lines[0] = "|Platz|Name|Punkte|";
                for (int i = 0; i < 20; i++)
                {
                    lines[(i + 1)] = "|" + (i + 1) + "|-------|0|";
                }
                File.WriteAllLines(this.getPath(), lines);
            }

            StreamReader sr = new StreamReader(this.getPath()); //open the txt-file at path

            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                string[] splitparts = line.Split('|');

                foreach (char part in line)
                {
                    if (part.Equals("") || part.Equals(" "))
                    {
                        break;
                    }
                }

                if (header == null)
                {
                    header = new string[splitparts.Length];
                    for (int i = 0; i < splitparts.Length - 1; i++)
                    {
                        header[i] = splitparts[i].Trim();
                    }
                    continue;
                }

                if (splitparts.Length != header.Length)
                {
                    Console.WriteLine("Anzahl der Spalten stimmt nicht!");
                    continue;
                }

                PlayerHolder playerHolder = new PlayerHolder();

                for (int i = 1; i < splitparts.Length - 1; i++)
                {
                    if (splitparts[i].Equals(""))
                    {
                        splitparts[i] = null;
                    }
                    playerHolder.setAttribut(header[i], splitparts[i].Trim());
                }
                playerHolders.Add(playerHolder);
            }
            sr.Close();
            setFileSave();
        }

        //Hier wird ein String zusammen gebaut und ausgegeben (aus dem was im Kontruktor gelesen wurde)
        public void ausgabeHighscore()
        {
            Console.Clear();
            int count = 0; // counts for write header only one time
            for (int i = 0; i < playerHolders.Count; i++)
            {
                if (count == 0)
                {
                    sb.Append("Platz").Append("\t").Append("Name").Append("\t").Append("Punkte").Append("\n").Append("---------------------------------------------------------------").Append("\n");
                    count++;
                }
                sb.Append(playerHolders[i].getAttribut("Platz")).
                    Append("\t").
                    Append(playerHolders[i].getAttribut("Name")).
                    Append("\t").
                    Append(playerHolders[i].getAttribut("Punkte")).
                    Append("\n");
            }
            Console.WriteLine(sb.ToString());
        }

        //Fragt nach Namen und Schreibt wenn unter den ersten 20 die Punkte in das Hihscore-File an der Richtigen Stelle
        //bei gleichen Punkten werden die Punkte über dem Alten Punktestand gespeichert
        public void writeHighscore()
        {
            bool nameExist = false;
            bool replaced = false;
            string name;
            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);

            do
            {
                nameExist = false;
                Console.Clear();
                Console.WriteLine(program.getPoints());
                Console.Write("Spielername (min 3 oder max 7 Zeichen und kein\"-\"): ");
                name = Console.ReadLine().Trim();

                if (name.Length > 7 || name.Length < 3 || name.Equals(""))
                {
                    Console.WriteLine("Der Name ist zu kurz oder zu lang!");
                    Console.ReadKey();
                    nameExist = true;
                }
                if (name.Contains("-"))
                {
                    Console.WriteLine("No \" - \"");
                    Console.ReadKey();
                    nameExist = true;
                }

            } while (nameExist == true);

            for (int i = 0; i < playerHolders.Count; i++)
            {
                if (!(Convert.ToInt32(program.getPoints()) < Convert.ToInt32(playerHolders[i].getAttribut("Punkte"))) && replaced != true)
                {
                    for (int x = 19; x >= (i + 1); x--)
                    {
                        playerHolders[x].removeAttribut("Name");
                        playerHolders[x].removeAttribut("Punkte");
                        playerHolders[x].setAttribut("Name", playerHolders[(x - 1)].getAttribut("Name"));
                        playerHolders[x].setAttribut("Punkte", playerHolders[(x - 1)].getAttribut("Punkte"));
                    }
                    playerHolders[i].getHighscore().Replace(playerHolders[i].getHighscore(), program.getPoints());
                    playerHolders[i].removeAttribut("Name");
                    playerHolders[i].setAttribut("Name", name);
                    playerHolders[i].removeAttribut("Punkte");
                    playerHolders[i].setAttribut("Punkte", program.getPoints());
                    replaced = true;
                }
                if (Convert.ToInt32(program.getPoints()) < Convert.ToInt32(playerHolders[19].getAttribut("Punkte")))
                {
                    Console.WriteLine("Deine Punkte sind zu niedrig für die obersten Top 20!");
                    Console.ReadKey();
                    break;
                }
            }

            removeFileSave();
            StreamWriter sw = new StreamWriter(@getPath());
            sw.WriteLine("|Platz|Name|Punkte|");
            for (int i = 0; i < 20; i++)
            {
                playerHolders[i].removeAttribut("Platz");
                playerHolders[i].setAttribut("Platz", Convert.ToString((i + 1)));
                sw.WriteLine("|" + playerHolders[i].getAttribut("Platz") + "|" + playerHolders[i].getAttribut("Name") + "|" + playerHolders[i].getAttribut("Punkte") + "|");
            }
            sw.Close();
            setFileSave();
        }

        //getPath ist fuer den Pfad und wird im ganzen Highscore verwendet also vosicht mit aenderungen
        public string getPath()
        {
            return currentPath + "\\highscore.txt";
        }

        public void setFileSave()
        {
            File.SetAttributes(this.getPath(), FileAttributes.Hidden | FileAttributes.ReadOnly);
        }

        public void removeFileSave()
        {
            File.SetAttributes(this.getPath(), File.GetAttributes(this.getPath()) & ~FileAttributes.Hidden & ~FileAttributes.ReadOnly);
        }

        bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                Console.WriteLine("Test ausgabe");
                setFileSave();
            }
            return false;
        }
        static ConsoleEventDelegate handler;   // Keeps it from getting garbage collected
        // Pinvoke
        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
    }
}
