using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;

namespace MoonBuggy
{
    //Das ist eine Hilfsklasse die mir beim Auslesen und beschrieben der Highscore Datei hilft
    public class PlayerHolder
    {
        private Hashtable player = new Hashtable();

        public void setAttribut(string key, string value)
        {

            player.Add(key, value);
        }
        public void removeAttribut(string key)
        {
            player.Remove(key);
        }
        public string getAttribut(string key)
        {
            return player[key].ToString();
        }


        //Diese Methoden sind noch unbenutzt
        public string getName()
        {
            return getAttribut("Name");
        }
        public string getHighscore()
        {
            return getAttribut("Punkte");
        }
        public string getPlatz()
        {
            return getAttribut("Platz");
        }
    }
}
