using System;
using static RPG_Maker_LDB_Printer.Program;
namespace RPG_Maker_LDB_Printer
{
    public class Event
    {
        public static void GetEvent()
        {
            Formating.ContinueIndent();
            int commoneventdata = Data.GetInt();
            Formating.OpenIndent();
            Formating.ContinueIndent();
            Byte[] level = br.ReadBytes(1);
            sw.Write(BitConverter.ToString(level));
            Formating.OpenIndent();
            Formating.ContinueIndent();
            Data.GetString();
            Formating.ContinueIndent();
            int[] gotInts = Data.GetInts();
            switch (commoneventdata)
            {
                case 0:
                case 10://0A
                case 20141://81 9D 2D
                    stop = true;
                    break;
                case 10140://CF 1C
                case 20140://81 9D 2C
                    while (stop == false)
                    {
                        GetEvent();
                    }
                    stop = false;
                    break;
                case 12010://DD 6A
                case 13310://E7 7E
                    while (stop == false)
                    {
                        GetEvent();
                    }
                    stop = false;
                    if (gotInts[5] == 1)
                    {
                        while (stop == false)
                        {
                            GetEvent();
                        }
                        stop = false;
                    }
                    GetEvent();
                    break;
                case 10710://D3 56
                    if (gotInts[3] == 2 || gotInts[4] == 1)
                    {
                        GetEvent();
                        while (stop == false)
                        {
                            GetEvent();
                        }
                        stop = false;
                        GetEvent();
                        while (stop == false)
                        {
                            GetEvent();
                        }
                        stop = false;
                        GetEvent();
                    }
                    break;
                case 10720://D3 60
                case 10730://D3 6A
                    if (gotInts[2] == 1)
                    {
                        stop = false;
                        GetEvent();
                        while (stop == false)
                        {
                            GetEvent();
                        }
                        stop = false;
                        GetEvent();
                        while (stop == false)
                        {
                            GetEvent();
                        }
                        stop = false;
                        GetEvent();
                    }
                    break;
                case 12210://DF 32
                    while (stop == false)
                    {
                        GetEvent();
                    }
                    stop = false;
                    GetEvent();
                    break;
            }
            Formating.CloseIndent();
            Formating.CloseIndent();
        }
    }
}
