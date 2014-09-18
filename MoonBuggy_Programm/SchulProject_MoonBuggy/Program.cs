using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MoonBuggy
{
    class Program
    {
        static Highscore highscore = new Highscore();
        static bool active = false;
        static int schwierigkeitsgrad = 100;
        static int punkte = 0;


        static string[] luft5 = new string[72];
        static string[] luft4 = new string[72];
        static string[] luft3 = new string[72];
        static string[] luft2 = new string[72];
        static string[] luft1 = new string[72];

        static string luft1_arsch = "";
        static string luft2_arsch = "";
        static string luft3_arsch = "";
        static string luft4_arsch = "";
        static string luft5_arsch = "";


        static string[] boden = new string[80];
        static string arsch = "";
        static Random rand = new Random();
        static bool ground = false;
        static int earth = 0;
        static int hole = 0;

        static string surface = "";

        static string line1 = "                                                                        ";
        static string line2 = "                                                                        ";
        static string line3 = "                                                                        ";
        static string line4 = "                                                                        ";
        static string line5 = "                                                                        ";
        static string buggy1 = "        ";
        static string buggy2 = "        ";
        static string buggy3 = "        ";
        static string buggy4 = "        ";
        static string buggy5 = "        ";

        static string zeichen = "";




        public static ConsoleKeyInfo cki { get; set; }
        static int buggy_height = 1;
        static int buggy_jumptime = 0;
        static int buggy_dietime = 0;

        static string buggy_action = "";
        static int buggy_tires = 0;
        static string tires;
        static string chassis = " i./TM7 ";

        static bool buggy_dying = false;

        static bool buggy_fire = false;
        static int buggy_reload = 0;








        static void Main(string[] args)
        {
            int modus = menu();

            if (modus == 1)
            {
                run();
				highscore.ausgabeHighscore();
				Console.ReadLine();
            }
            if (modus == 2)
            {
                highscore.ausgabeHighscore();
                Console.ReadLine();
            }
            if (modus == 3)
            {
                Console.WriteLine("Exit...");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }





        static int menu()
        {
            string eingabe = "";
            int ergebnis = 0;
            Console.WriteLine("");
            Console.WriteLine("     M   M   OOO    OOO   N   N");
            Console.WriteLine("     MM MM  O   O  O   O  NN  N");
            Console.WriteLine("     M M M  O   O  O   O  N N N");
            Console.WriteLine("     M   M  O   O  O   O  N  NN");
            Console.WriteLine("     M   M   OOO    OOO   N   N");
            Console.WriteLine("");
            Console.WriteLine("            BBBB   U   U  GGGGG  GGGGG  Y   Y");
            Console.WriteLine("            B   B  U   U  G      G       Y  Y");
            Console.WriteLine("            BBBB   U   U  G GGG  G GGG    Y Y");
            Console.WriteLine("            B   B  U   U  G   G  G   G     Y ");
            Console.WriteLine("            BBBB   UUUUU  GGGGG  GGGGG  YYY  ");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("[ENTER] zum starten...");
            Console.ReadLine();
            Console.Clear();
            while (true)
            {
                Console.WriteLine("Neues Spiel  [1]");
                Console.WriteLine("Highscore    [2]");
                Console.WriteLine("Beenden      [3]");
                Console.WriteLine("");
                Console.WriteLine("Bitte geben Sie eine Zahl ein, um die jeweilige Aktion auszuführen...");

                eingabe = Console.ReadLine();
                bool result = Int32.TryParse(eingabe, out ergebnis);
                if (result)
                {
                    if ((ergebnis > 0) && (ergebnis < 4))
                    {
                        return ergebnis;
                    }
                }
            }
        }



        static void run()
        {
            buggy2 = chassis;
            new Thread(() =>
            {
                while ((buggy_action != "die") && (buggy_action != "dead"))
                {
                    cki = Console.ReadKey();
                    switch (cki.Key)
                    {
                        case ConsoleKey.Spacebar:
                            buggy_action = "jump";
                            break;
                        case ConsoleKey.Enter:
                            buggy_fire = true;
                            break;
                        case ConsoleKey.RightArrow:
                            break;

                    }
                        //}
                }
            }).Start(); //END THREAD



            for (int i = 0; i < 80; i++)
            {
                boden[i] = "#";
            }
            for (int i = 0; i < 72; i++)
            {
                luft1[i] = " ";
                luft2[i] = " ";
                luft3[i] = " ";
                luft4[i] = " ";
                luft5[i] = " ";
            }

            while (true)
            {
                if (!buggy_dying)
                {
                    surface = calc_surface();
                }
                else
                {
                    highscore.removeFileSave();
                    highscore.writeHighscore();
                    highscore.setFileSave();
                    return;
                }
                calc_buggy();

                line1 = calc_line1();
                line2 = calc_line2();
                line3 = calc_line3();
                line4 = calc_line4();
                line5 = calc_line5();


                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.Write(line5 + buggy5);
                Console.Write(line4 + buggy4);
                Console.Write(line3 + buggy3);
                Console.Write(line2 + buggy2);
                Console.Write(line1 + buggy1);
                Console.Write(surface);
                Console.WriteLine("################################################################################");
                Console.Write("Punkte: " + punkte + "\tDruecke LEERTASTE zum springen");
                System.Threading.Thread.Sleep(5000 / schwierigkeitsgrad);
                Console.Clear();

            }
        }


        static string calc_line1()
        {
            for (int i = 0; i < 71; i++)
            {
                if (luft1[i + 1] == "-")
                {
                    luft1[i] = luft1[i + 1];
                    luft1[i + 1] = " ";
                }
            }
            if (!buggy_fire)
            {
                luft1[71] = " ";
            }

            luft1[0] = " ";
            luft1_arsch = string.Concat(luft1);
            return luft1_arsch;
        }
        static string calc_line2()
        {
            for (int i = 0; i < 71; i++)
            {
                if (luft2[i + 1] == "-")
                {
                    luft2[i] = luft2[i + 1];
                    luft2[i + 1] = " ";
                }
            }
            if (!buggy_fire)
            {
                luft2[71] = " ";
            }


            luft2[0] = " ";
            luft2_arsch = string.Concat(luft2);
            return luft2_arsch;
        }
        static string calc_line3()
        {
            for (int i = 0; i < 71; i++)
            {
                if (luft3[i + 1] == "-")
                {
                    luft3[i] = luft3[i + 1];
                    luft3[i + 1] = " ";
                }
            }
            if (!buggy_fire)
            {
                luft3[71] = " ";
            }


            luft3[0] = " ";
            luft3_arsch = string.Concat(luft3);
            return luft3_arsch;
        }


        static string calc_line4()
        {
            for (int i = 0; i < 71; i++)
            {
                if (luft4[i + 1] == "-")
                {
                    luft4[i] = luft4[i + 1];
                    luft4[i + 1] = " ";
                }
            }
            if (!buggy_fire)
            {
                luft4[71] = " ";
            }


            luft4[0] = " ";
            luft4_arsch = string.Concat(luft4);
            return luft4_arsch;
        }
        static string calc_line5()
        {
            for (int i = 0; i < 71; i++)
            {
                if (luft5[i + 1] == "-")
                {
                    luft5[i] = luft5[i + 1];
                    luft5[i + 1] = " ";
                }
            }
            if (!buggy_fire)
            {
                luft5[71] = " ";
            }


            luft5[0] = " ";
            luft5_arsch = string.Concat(luft5);
            return luft5_arsch;
        }

        static void calc_buggy()
        {
            if ((buggy_action != "jump") && (buggy_action != "dead") && ((boden[78] == " ") || (boden[73] == " ")))
            {
                buggy_action = "die";
                buggy_dying = true;
                func_buggy_die();
            }

            if ((buggy_fire) && (!buggy_dying))
            {
                func_buggy_fire();
            }


            func_buggy_drive();

            if (buggy_action == "jump")
            {
                if (!buggy_dying)
                {
                    func_buggy_jump();
                }
            }
            else
            {
                buggy1 = tires;
            }
        }


        static void func_buggy_fire()
        {
            buggy_reload += 1;
            if ((buggy_reload < 3) && (buggy_height == 1))
            {
                luft1[71] = "-";
            }
            else
            {
                buggy_reload = 0;
                buggy_fire = false;
            }
        }

        static void func_buggy_die()
        {
            buggy_dietime += 1;
            switch (buggy_dietime)
            {
                case 1:
                    buggy1 = tires;
                    buggy2 = " i.-TM\\ ";
                    buggy3 = "   O O  ";
                    buggy4 = "        ";
                    buggy5 = "        ";
                    buggy_action = "die";
                    break;
                case 2:
                    buggy1 = tires;
                    buggy2 = " ,.-y3\\ ";
                    buggy3 = "   o o  ";
                    buggy4 = "  O  O  ";
                    buggy5 = "        ";
                    buggy_action = "die";
                    break;
                case 3:
                    buggy1 = tires;
                    buggy2 = " ,.-yq+ ";
                    buggy3 = "   ° °  ";
                    buggy4 = "  o  o  ";
                    buggy5 = " O    O ";
                    buggy_action = "die";
                    break;
                case 4:
                    buggy1 = tires;
                    buggy2 = " ,__yq< ";
                    buggy3 = "        ";
                    buggy4 = "O °  °O ";
                    buggy5 = " o    o ";
                    buggy_action = "die";
                    break;
                case 5:
                    buggy1 = tires;
                    buggy2 = " ,__y<_ ";
                    buggy3 = "O      O";
                    buggy4 = "o     o ";
                    buggy5 = " °    ° ";
                    buggy_action = "die";
                    break;
                case 6:
                    buggy1 = tires;
                    buggy2 = "O.__<._O";
                    buggy3 = "o      o";
                    buggy4 = "°     ° ";
                    buggy5 = "        ";
                    break;
                case 7:
                    buggy1 = tires;
                    buggy2 = "o___.__o";
                    buggy3 = "°      °";
                    buggy4 = "        ";
                    buggy5 = "        ";
                    buggy_action = "die";
                    break;
                case 8:
                    buggy1 = tires;
                    buggy2 = "________";
                    buggy3 = "        ";
                    buggy4 = "        ";
                    buggy5 = "        ";
                    buggy_action = "dead";
                    break;
            }
        }


        static void func_buggy_drive()
        {
            buggy_tires += 1;
            if (buggy_tires >= 4)
            {
                buggy_tires -= 4;
            }


            if ((buggy_action != "die") && (buggy_action != "dead"))
            {
                switch (buggy_tires)
                {
                    case 0:
                        tires = "(-)  (|)";
                        break;
                    case 1:
                        tires = "(/)  (\\)";
                        break;
                    case 2:
                        tires = "(|)  (-)";
                        break;
                    case 3:
                        tires = "(\\)  (/)";
                        break;
                }
            }

        }

        static void func_buggy_jump()
        {

            buggy_jumptime += 1;

            switch (buggy_jumptime)
            {
                case 1:
                    buggy1 = "        ";
                    buggy2 = tires;
                    buggy3 = chassis;
                    buggy4 = "        ";
                    buggy5 = "        ";
                    buggy_height = 2;
                    break;
                case 2:
                    buggy1 = "        ";
                    buggy2 = "        ";
                    buggy3 = tires;
                    buggy4 = chassis;
                    buggy5 = "        ";
                    buggy_height = 3;
                    break;
                case 3:
                    buggy1 = "        ";
                    buggy2 = "        ";
                    buggy3 = tires;
                    buggy4 = chassis;
                    buggy5 = "        ";
                    buggy_height = 3;
                    break;
                case 4:
                    buggy1 = "        ";
                    buggy2 = "        ";
                    buggy3 = "        ";
                    buggy4 = tires;
                    buggy5 = chassis;
                    break;
                case 5:
                    buggy1 = "        ";
                    buggy2 = "        ";
                    buggy3 = "        ";
                    buggy4 = tires;
                    buggy5 = chassis;
                    buggy_height = 4;
                    break;
                case 6:
                    buggy1 = "        ";
                    buggy2 = "        ";
                    buggy3 = "        ";
                    buggy4 = tires;
                    buggy5 = chassis;
                    buggy_height = 4;
                    break;
                case 7:
                    buggy1 = "        ";
                    buggy2 = "        ";
                    buggy3 = "        ";
                    buggy4 = tires;
                    buggy5 = chassis;
                    buggy_height = 4;
                    break;
                case 8:
                    buggy1 = "        ";
                    buggy2 = "        ";
                    buggy3 = "        ";
                    buggy4 = tires;
                    buggy5 = chassis;
                    buggy_height = 4;
                    break;
                case 9:
                    buggy1 = "        ";
                    buggy2 = "        ";
                    buggy3 = "        ";
                    buggy4 = tires;
                    buggy5 = chassis;
                    buggy_height = 4;
                    break;
                case 10:
                    buggy1 = "        ";
                    buggy2 = "        ";
                    buggy3 = "        ";
                    buggy4 = tires;
                    buggy5 = chassis;
                    buggy_height = 4;
                    break;
                case 11:
                    buggy1 = "        ";
                    buggy2 = "        ";
                    buggy3 = "        ";
                    buggy4 = tires;
                    buggy5 = chassis;
                    buggy_height = 4;
                    break;
                case 12:
                    buggy1 = "        ";
                    buggy2 = "        ";
                    buggy3 = "        ";
                    buggy4 = tires;
                    buggy5 = chassis;
                    buggy_height = 4;
                    break;
                case 13:
                    buggy1 = "        ";
                    buggy2 = "        ";
                    buggy3 = tires;
                    buggy4 = chassis;
                    buggy5 = "        ";
                    buggy_height = 3;
                    break;
                case 14:
                    buggy1 = "        ";
                    buggy2 = "        ";
                    buggy3 = tires;
                    buggy4 = chassis;
                    buggy5 = "        ";
                    buggy_height = 3;
                    break;
                case 15:
                    buggy1 = "        ";
                    buggy2 = tires;
                    buggy3 = chassis;
                    buggy4 = "        ";
                    buggy5 = "        ";
                    buggy_height = 2;
                    break;
                case 16:
                    buggy_jumptime = 0;
                    buggy_action = "";
                    buggy1 = tires;
                    buggy2 = chassis;
                    buggy3 = "        ";
                    buggy4 = "        ";
                    buggy5 = "        ";
                    buggy_height = 1;
                    break;

            }
        }

        static string calc_surface()
        {

            randomizer();

            for (int i = 78; i >= 0; i--)
            {
                boden[i + 1] = boden[i];
            }
            boden[0] = zeichen;

            if (boden[79] == " ")
            {
                schwierigkeitsgrad++;
                punkte++;
            }

            arsch = string.Concat(boden);

            return arsch;
        }
        static string randomizer()
        {
            if (!active)
            {
                if (!ground)
                {
                    hole = rand.Next(7) + 1;
                    active = true;
                }
                else
                {
                    earth = rand.Next(25) + 16;
                    active = true;
                }

            }

            if (hole > 0)
            {
                zeichen = " ";
                hole -= 1;
            }
            if (earth > 0)
            {
                zeichen = "#";
                earth -= 1;
            }

            if ((earth <= 0) && (hole <= 0))
            {
                if (ground)
                {
                    ground = false;
                }
                else
                {
                    ground = true;
                }
                active = false;
            }

            return zeichen;
        }

        public string getPoints()
        {
            return Convert.ToString(punkte);
        }
    }
}