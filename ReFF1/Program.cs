using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using SFML;
using SFML.Graphics;
using SFML.Window;

namespace ReFF1
{
    class Version
    {
        static readonly string versionNumber = System.IO.File.ReadAllText("version");

        public static void PrintVersionInfo()
        {
            Console.WriteLine("ReFF1 version " + versionNumber + "\n");

        }
    }

    class Config
    {
        static XDocument instancesXML;

        static string instancesConfigFile = "instances.xml";
        static string instancesRootNode = "instances";

        public static void LoadConfigFiles()
        {
            if (!File.Exists(instancesConfigFile))
            {
                Console.WriteLine(instancesConfigFile + " configuration file is not found. Creating a new one.");

                new XDocument(
                        new XElement(instancesRootNode)
                ).Save(instancesConfigFile);
            }

            instancesXML = XDocument.Load(instancesConfigFile);

        }

        public static void SetUpGameInstancesList()
        {
            IEnumerable<XElement> instanceList =
                from el in instancesXML.Root.Elements()
                select el;

            foreach (XElement e in instanceList)
                GameInstances.instances.Add(e.Attribute("name").Value);

        }
    }

    class Commandline
    {
        static string commandList = "add <rom path> - Add game instance\n" +
                             "cmdlist - Command list\n" +
                             "config <instance name> - Configure game instance\n" +
                             "help - Help\n" +
                             "list - Game instances list\n" +
                             "play <instance name> - Play game instance\n" +
                             "remove <instance name> - Remove game instance\n";

        public static bool IsBadArgumentLength(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No argument(s) specified!");
                return true;
            }

            if (args.Length > 2)
            {
                Console.WriteLine("Too many arguments!");
                return true;
            }

            return false;
        }

        public static void CommandNotImplemented()
        {
            Console.WriteLine("This command is not implemented yet!");
        }

        public static void RunCommand(string command, string parameter)
        {
            switch (command)
            {
                case "add":
                    if (String.IsNullOrEmpty(parameter))
                    {
                        Console.WriteLine("This command needs path parameter.");
                        break;
                    }

                    GameInstances.add(parameter);
                    break;
                case "cmdlist":
                    Console.WriteLine(commandList);

                    break;
                case "config":
                    CommandNotImplemented();

                    break;
                case "help":
                    Console.WriteLine("Open Help.md for help or execute program with argument 'cmdlist' for command list.");

                    break;
                case
                    "list":
                    GameInstances.showList();

                    break;
                case
                    "play":
                    if (String.IsNullOrEmpty(parameter))
                    {
                        Console.WriteLine("This command needs instance name parameter.");
                        break;
                    }

                    if (GameInstances.IsValid(parameter))
                        Game.Play();
                    else
                        Console.WriteLine("Unknown game instance: '" + parameter + "', execute program with argument 'list' for game instance list.");

                    break;
                case
                    "remove":
                    if (String.IsNullOrEmpty(parameter))
                    {
                        Console.WriteLine("This command needs instance name parameter.");
                        break;
                    }

                    if (GameInstances.IsValid(parameter))
                        GameInstances.remove(parameter);
                    else
                        Console.WriteLine("Unknown game instance: '" + parameter + "', execute program with argument 'list' for game instance list.");
                    break;
                default:
                    Console.WriteLine("Unknown command: '" + command + "', execute program with argument 'cmdlist' for command list.");

                    break;
            }
        }
    }

    class Game
    {
        public static void Play()
        {
            throw new NotImplementedException();
        }
    }

    class GameInstances
    {
        enum GameType
        {
            I_JPN,
            I_USA,
            I_AND_II
        }

        enum GameLanguage
        {
            Japanese,
            English
        }

        public static List<string> instances = new List<string>();

        public static bool IsValid(string name)
        {
            if (instances.Contains(name))
                return true;
            else
                return false;
        }

        public static void add(string path)
        {
            // Move this somewhere else

            void print_chars_at_range(int from, int to, int gameLang, ref byte[] romRef)
            {
                for (int i = from; i <= to; i++)
                {
                    if (gameLang == (int)GameLanguage.Japanese)
                    {
                        //Console.Write("({0:X})", romRef[i]);
                        Console.Write(print_jpn_char((char)romRef[i]));
                    }
                    else if (gameLang == (int)GameLanguage.English)
                        Console.Write(print_eng_char((char)romRef[i]));
                }
            }

            string print_jpn_char(char character)
            {
                if (character >= 0x80 && character <= 0x89) // 0-9
                    return Char.ToString((char)(character - 0x50));

                switch (character)
                {
                    case (char)0x0:
                        return "[END]\n";
                    case (char)0x1:
                        return "\n";
                    case (char)0x5:
                        return "\n\n";
                    case (char)0x48:
                        return "が";
                    case (char)0x49:
                        return "ぎ";
                    case (char)0x4A:
                        return "ぐ";
                    case (char)0x4B:
                        return "げ";
                    case (char)0x4C:
                        return "ご";
                    case (char)0x4D:
                        return "ざ";
                    case (char)0x4E:
                        return "じ";
                    case (char)0x4F:
                        return "ず";
                    case (char)0x50:
                        return "ぜ";
                    case (char)0x51:
                        return "ぞ";
                    case (char)0x52:
                        return "だ";
                    case (char)0x53:
                        return "ぢ";
                    case (char)0x54:
                        return "づ";
                    case (char)0x55:
                        return "で";
                    case (char)0x56:
                        return "ど";
                    case (char)0x57:
                        return "ば";
                    case (char)0x58:
                        return "び";
                    case (char)0x59:
                        return "ぶ";
                    case (char)0x5A:
                        return "べ";
                    case (char)0x5B:
                        return "ぼ";
                    case (char)0x5C:
                        return "ガ";
                    case (char)0x5D:
                        return "ギ";
                    case (char)0x5E:
                        return "グ";
                    case (char)0x5F:
                        return "ゲ";
                    case (char)0x60:
                        return "ゴ";
                    case (char)0x61:
                        return "ザ";
                    case (char)0x62:
                        return "ジ";
                    case (char)0x63:
                        return "ズ";
                    case (char)0x64:
                        return "ゼ";
                    case (char)0x65:
                        return "ゾ";
                    case (char)0x66:
                        return "ダ";
                    case (char)0x67:
                        return "ヂ";
                    case (char)0x68:
                        return "ヅ";
                    case (char)0x69:
                        return "デ";
                    case (char)0x6A:
                        return "ド";
                    case (char)0x6B:
                        return "バ";
                    case (char)0x6C:
                        return "ビ";
                    case (char)0x6D:
                        return "ブ";
                    case (char)0x6E:
                        return "ベ";
                    case (char)0x6F:
                        return "ボ";
                    case (char)0x70:
                        return "ぱ";
                    case (char)0x71:
                        return "ぴ";
                    case (char)0x72:
                        return "ぷ";
                    case (char)0x73:
                        return "ぺ";
                    case (char)0x74:
                        return "ぽ";
                    case (char)0x75:
                        return "パ";
                    case (char)0x76:
                        return "ピ";
                    case (char)0x77:
                        return "プ";
                    case (char)0x78:
                        return "ペ";
                    case (char)0x79:
                        return "ポ";
                    case (char)0x7A:
                        return "/";
                    case (char)0x7B:
                        return "を";
                    case (char)0x7C:
                        return "っ";
                    case (char)0x7D:
                        return "ゃ";
                    case (char)0x7E:
                        return "ゅ";
                    case (char)0x7F:
                        return "ょ";
                    case (char)0x8A:
                        return "あ";
                    case (char)0x8B:
                        return "い";
                    case (char)0x8C:
                        return "う";
                    case (char)0x8D:
                        return "え";
                    case (char)0x8E:
                        return "お";
                    case (char)0x8F:
                        return "か";
                    case (char)0x90:
                        return "き";
                    case (char)0x91:
                        return "く";
                    case (char)0x92:
                        return "け";
                    case (char)0x93:
                        return "こ";
                    case (char)0x94:
                        return "さ";
                    case (char)0x95:
                        return "し";
                    case (char)0x96:
                        return "す";
                    case (char)0x97:
                        return "せ";
                    case (char)0x98:
                        return "そ";
                    case (char)0x99:
                        return "た";
                    case (char)0x9A:
                        return "ち";
                    case (char)0x9B:
                        return "つ";
                    case (char)0x9C:
                        return "て";
                    case (char)0x9D:
                        return "と";
                    case (char)0x9E:
                        return "な";
                    case (char)0x9F:
                        return "に";
                    case (char)0xA0:
                        return "ぬ";
                    case (char)0xA1:
                        return "ね";
                    case (char)0xA2:
                        return "の";
                    case (char)0xA3:
                        return "は";
                    case (char)0xA4:
                        return "ひ";
                    case (char)0xA5:
                        return "ふ";
                    case (char)0xA6:
                        return "へ";
                    case (char)0xA7:
                        return "ほ";
                    case (char)0xA8:
                        return "ま";
                    case (char)0xA9:
                        return "み";
                    case (char)0xAA:
                        return "む";
                    case (char)0xAB:
                        return "め";
                    case (char)0xAC:
                        return "も";
                    case (char)0xAD:
                        return "や";
                    case (char)0xAE:
                        return "ゆ";
                    case (char)0xAF:
                        return "よ";
                    case (char)0xB0:
                        return "ら";
                    case (char)0xB1:
                        return "り";
                    case (char)0xB2:
                        return "る";
                    case (char)0xB3:
                        return "れ";
                    case (char)0xB4:
                        return "ろ";
                    case (char)0xB5:
                        return "わ";
                    case (char)0xB6:
                        return "ん";
                    case (char)0xB7:
                        return "ァ";
                    case (char)0xB8:
                        return "ィ";
                    case (char)0xB9:
                        return "。";
                    case (char)0xBA:
                        return "ェ";
                    case (char)0xBB:
                        return "ォ";
                    case (char)0xBC:
                        return "ッ";
                    case (char)0xBD:
                        return "ャ";
                    case (char)0xBE:
                        return "ュ";
                    case (char)0xBF:
                        return "ョ";
                    case (char)0xC0:
                        return "″";
                    case (char)0xC1:
                        return "°";
                    case (char)0xC2:
                        return "ー";
                    case (char)0xC3:
                        return "‥";
                    case (char)0xC4:
                        return "!";
                    case (char)0xC5:
                        return "?";
                    case (char)0xC6:
                        return "L";
                    case (char)0xC7:
                        return "E";
                    case (char)0xC8:
                        return "H";
                    case (char)0xC9:
                        return "P";
                    case (char)0xCA:
                        return "ア";
                    case (char)0xCB:
                        return "イ";
                    case (char)0xCC:
                        return "ウ";
                    case (char)0xCD:
                        return "エ";
                    case (char)0xCE:
                        return "オ";
                    case (char)0xCF:
                        return "カ";
                    case (char)0xD0:
                        return "キ";
                    case (char)0xD1:
                        return "ク";
                    case (char)0xD2:
                        return "ケ";
                    case (char)0xD3:
                        return "コ";
                    case (char)0xD4:
                        return "サ";
                    case (char)0xD5:
                        return "シ";
                    case (char)0xD6:
                        return "ス";
                    case (char)0xD7:
                        return "セ";
                    case (char)0xD8:
                        return "ソ";
                    case (char)0xD9:
                        return "タ";
                    case (char)0xDA:
                        return "チ";
                    case (char)0xDB:
                        return "ツ";
                    case (char)0xDC:
                        return "テ";
                    case (char)0xDD:
                        return "ト";
                    case (char)0xDE:
                        return "ナ";
                    case (char)0xDF:
                        return "ニ";
                    case (char)0xE0:
                        return "ヌ";
                    case (char)0xE1:
                        return "ネ";
                    case (char)0xE2:
                        return "ノ";
                    case (char)0xE3:
                        return "ハ";
                    case (char)0xE4:
                        return "ヒ";
                    case (char)0xE5:
                        return "フ";
                    case (char)0xE6:
                        return "ヘ";
                    case (char)0xE7:
                        return "ホ";
                    case (char)0xE8:
                        return "マ";
                    case (char)0xE9:
                        return "ミ";
                    case (char)0xEA:
                        return "ム";
                    case (char)0xEB:
                        return "メ";
                    case (char)0xEC:
                        return "モ";
                    case (char)0xED:
                        return "ヤ";
                    case (char)0xEE:
                        return "ユ";
                    case (char)0xEF:
                        return "ヨ";
                    case (char)0xF0:
                        return "ラ";
                    case (char)0xF1:
                        return "リ";
                    case (char)0xF2:
                        return "ル";
                    case (char)0xF3:
                        return "レ";
                    case (char)0xF4:
                        return "ロ";
                    case (char)0xF5:
                        return "ワ";
                    case (char)0xF6:
                        return "ン";
                    case (char)0xFF:
                        return " ";
                    default:
                        return "?";
                }
            }

            string print_eng_char(char character)
            {
                if (character >= 0x80 && character <= 0x89) // 0-9
                    return Char.ToString((char)(character - 0x50));

                if (character >= 0x8A && character <= 0xA3) // A-Z
                    return Char.ToString((char)(character - 0x49));

                if (character >= 0xA4 && character <= 0xBD) // a-z
                    return Char.ToString((char)(character - 0x43));

                switch (character)
                {
                    case (char)0x0:
                        return "[END]\n";
                    case (char)0x1:
                        return "\n";
                    case (char)0x5:
                        return "\n\n";
                    case (char)0x1A:
                        return "e ";
                    case (char)0x1B:
                        return " t";
                    case (char)0x1C:
                        return "th";
                    case (char)0x1D:
                        return "he";
                    case (char)0x1E:
                        return "s ";
                    case (char)0x1F:
                        return "in";
                    case (char)0x20:
                        return " a";
                    case (char)0x21:
                        return "t ";
                    case (char)0x22:
                        return "an";
                    case (char)0x23:
                        return "re";
                    case (char)0x24:
                        return " s";
                    case (char)0x25:
                        return "er";
                    case (char)0x26:
                        return "ou";
                    case (char)0x27:
                        return "d ";
                    case (char)0x28:
                        return "to";
                    case (char)0x29:
                        return "n ";
                    case (char)0x2A:
                        return "ng";
                    case (char)0x2B:
                        return "ea";
                    case (char)0x2C:
                        return "es";
                    case (char)0x2D:
                        return " i";
                    case (char)0x2E:
                        return "o ";
                    case (char)0x2F:
                        return "ar";
                    case (char)0x30:
                        return "is";
                    case (char)0x31:
                        return " b";
                    case (char)0x32:
                        return "ve";
                    case (char)0x33:
                        return " w";
                    case (char)0x34:
                        return "me";
                    case (char)0x35:
                        return "or";
                    case (char)0x36:
                        return " o";
                    case (char)0x37:
                        return "st";
                    case (char)0x38:
                        return " c";
                    case (char)0x39:
                        return "at";
                    case (char)0x3A:
                        return "en";
                    case (char)0x3B:
                        return "nd";
                    case (char)0x3C:
                        return "on";
                    case (char)0x3D:
                        return "hi";
                    case (char)0x3E:
                        return "se";
                    case (char)0x3F:
                        return "as";
                    case (char)0x40:
                        return "ed";
                    case (char)0x41:
                        return "ha";
                    case (char)0x42:
                        return " m";
                    case (char)0x43:
                        return " f";
                    case (char)0x44:
                        return "r ";
                    case (char)0x45:
                        return "le";
                    case (char)0x46:
                        return "ow";
                    case (char)0x47:
                        return "g ";
                    case (char)0x48:
                        return "ce";
                    case (char)0x49:
                        return "om";
                    case (char)0x4A:
                        return "GI";
                    case (char)0x4B:
                        return "y ";
                    case (char)0x4C:
                        return "of";
                    case (char)0x4D:
                        return "ro";
                    case (char)0x4E:
                        return "ll";
                    case (char)0x4F:
                        return " p";
                    case (char)0x50:
                        return " y";
                    case (char)0x51:
                        return "ca";
                    case (char)0x52:
                        return "MA";
                    case (char)0x53:
                        return "te";
                    case (char)0x54:
                        return "f ";
                    case (char)0x55:
                        return "ur";
                    case (char)0x56:
                        return "yo";
                    case (char)0x57:
                        return "ti";
                    case (char)0x58:
                        return "l ";
                    case (char)0x59:
                        return " h";
                    case (char)0x5A:
                        return "ne";
                    case (char)0x5B:
                        return "it";
                    case (char)0x5C:
                        return "ri";
                    case (char)0x5D:
                        return "wa";
                    case (char)0x5E:
                        return "ac";
                    case (char)0x5F:
                        return "al";
                    case (char)0x60:
                        return "we";
                    case (char)0x61:
                        return "il";
                    case (char)0x62:
                        return "be";
                    case (char)0x63:
                        return "rs";
                    case (char)0x64:
                        return "u ";
                    case (char)0x65:
                        return " l";
                    case (char)0x66:
                        return "ge";
                    case (char)0x67:
                        return " d";
                    case (char)0x68:
                        return "li";
                    case (char)0x69:
                        return "..";
                    case (char)0x6A:
                        return "ne";
                    case (char)0x6B:
                        return "it";
                    case (char)0x6C:
                        return "ri";
                    case (char)0x6D:
                        return "wa";
                    case (char)0x6E:
                        return "ac";
                    case (char)0x6F:
                        return "al";
                    case (char)0x7A:
                        return "/";
                    case (char)0xBE:
                        return "'";
                    case (char)0xBF:
                        return ",";
                    case (char)0xC0:
                        return ".";
                    case (char)0xC2:
                        return "-";
                    case (char)0xC4:
                        return "!";
                    case (char)0xC5:
                        return "?";
                    case (char)0xC8:
                        return "ee";
                    case (char)0xE0:
                        return "%";
                    case (char)0xFF:
                        return " ";
                    default:
                        return "[?]";
                }
            }

            if (!File.Exists(path))
            {
                Console.WriteLine("Specfied path: '" + path + "' doesn't exist!");

                return;
            }

            long gameROMSize = new System.IO.FileInfo(path).Length;

            long FF1ROMSize = 262_160; // 256 kiB + 16 kiB iNES header for original JPN and USA releases.
            long FF1and2ROMSize = 524_304; // 512 kIB + 16 kIb iNES header for the Final Fantasy I & II Japan release.

            int assumedGameType;

            if (gameROMSize == FF1ROMSize)
                assumedGameType = (int)GameType.I_JPN;

            else if (gameROMSize == FF1and2ROMSize)
                assumedGameType = (int)GameType.I_AND_II;

            else
            {
                Console.WriteLine("Error:\tThe specified ROM doesn't have valid size for known ROM.");

                return;
            }

            byte[] rom = System.IO.File.ReadAllBytes(path);

            int PRGROMSize = rom[4] * 16_384;
            int CHRROMSize = rom[5] * 8_192;

            BitArray flags6 = new BitArray(new byte[] { rom[6] });
            int flags7 = rom[7];
            int flags8 = rom[8];
            int flags9 = rom[9];
            int flags10 = rom[10];

            bool flagNametableMirroring = flags6.Get(0); // Vertical nametable mirroring used only in USA ROM. 
            bool flagHasBatteryBackedPRGRAM = flags6.Get(1); // Used
            bool flagHas512byteTrainer = flags6.Get(2); // Unused
            bool flagHasFourScreenVRAM = flags6.Get(3); // Used

            // Checking ROM header.

            if (rom[0] == 'N' && rom[1] == 'E' && rom[2] == 'S' && rom[3] == 0x1A)
                Console.WriteLine("Info:\tValid iNES header constant.");
            else
            {
                Console.WriteLine("Error:\tInvalid iNES header constant.");

                return;
            }

            if (PRGROMSize + 16 == gameROMSize)
                Console.WriteLine("Info:\tValid PRG ROM size ({0} bytes).", PRGROMSize);
            else
            {
                Console.WriteLine("Error:\tInvalid PRG ROM size: {0} bytes, instead of {1} bytes.", PRGROMSize, gameROMSize - 16);
            }

            if (CHRROMSize > 0)
            {
                Console.WriteLine("Error:\tCHR ROM size is >0. Not known Final Fantasy I ROM have CHR ROM data.");

                return;
            }

            if (flagNametableMirroring == false)
                Console.WriteLine("Info:\tHorizontal nametable mirroring.");
            else if (flagNametableMirroring == true)
                Console.WriteLine("Info:\tVertical nametable mirroring.");

            if (assumedGameType == (int)GameType.I_JPN)
            {
                if (flagNametableMirroring == false)
                { // Horizontal nametable mirroring.
                    assumedGameType = (int)GameType.I_JPN;

                    Console.WriteLine("Info:\tAssuming the ROM is Final Fantasy I (Japan)");
                }
                else if (flagNametableMirroring == true)
                { // Vertical nametable mirroring.
                    assumedGameType = (int)GameType.I_USA;

                    Console.WriteLine("Info:\tAssuming the ROM is Final Fantasy I (USA)");
                }

            }
            else if (assumedGameType == (int)GameType.I_AND_II)
                Console.WriteLine("Info:\tAssuming the ROM is Final Fantasy I & II (Japan)");

            if (!flagHasBatteryBackedPRGRAM)
            {
                Console.WriteLine("Error:\tROM doesn't contain battery-backed PRG RAM, but it should.");

                return;
            }

            if (flagHas512byteTrainer)
            {
                Console.WriteLine("Error:\tROM contains 512-byte trainer, but it shouldn't.");

                return;
            }

            if (flags7 != 0)
            {
                Console.WriteLine("Error:\tROM contains header flags at 0x7, but it shouldn't.");

                return;
            }

            if (flags8 != 0)
            {
                Console.WriteLine("Error:\tROM contains header flags at 0x8, but it shouldn't.");

                return;
            }

            if (flags9 != 0)
            {
                Console.WriteLine("Error:\tROM contains header flags at 0x9, but it shouldn't.");

                return;
            }

            if (flags10 != 0)
            {
                Console.WriteLine("Error:\tROM contains header flags at 0xA, but it shouldn't.");

                return;
            }

            Console.WriteLine("\nInfo:\tShowing game intro...\n");

            if (assumedGameType == (int)GameType.I_JPN || assumedGameType == (int)GameType.I_AND_II) // Intro text at 0x37F30 - 0x37FEE.
            {
                print_chars_at_range(0x37F30, 0x37FED, (int)GameLanguage.Japanese, ref rom);

            }
            else if (assumedGameType == (int)GameType.I_USA) // Intro text at 0x37F30 - 0x3800F.
            {
                print_chars_at_range(0x37F30, 0x3800F, (int)GameLanguage.English, ref rom);
            }

            //Console.WriteLine("\nTODO:\tRefactor this shit, and actually add game instance to appropriate directory (with XML of the Intro text).");
        }

        public static void remove(string name)
        {
            throw new NotImplementedException();
        }

        public static void showList()
        {
            foreach (string name in instances)
                Console.WriteLine(name);
        }
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            Version.PrintVersionInfo();

            Config.LoadConfigFiles();
            Config.SetUpGameInstancesList();

            if (Commandline.IsBadArgumentLength(args))
                Environment.Exit(1);

            string command = args[0];
            string parameter;

            if (args.Length == 1)
                parameter = string.Empty;
            else
                parameter = args[1];

            Commandline.RunCommand(command, parameter);
        }
    }
}
