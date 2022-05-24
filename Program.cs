using System;
using System.IO;

namespace RPG_Maker_LDB_Printer
{
    class Program
    {
        public static BinaryReader br;
        public static StreamWriter sw;
        public static int indent = 0;
        public static bool stop;

        static void Main(string[] args)
        {
            br = new BinaryReader(File.OpenRead(args[0]));
            sw = new StreamWriter(File.Create(args[0] + ".txt"));

            GetString();//LcfDataBase
            ContinueIndent();

            for (byte[] PartID = new byte[1]; PartID[0] < 0x20;)
            {
                string PartHex;
                PartID[0] = br.ReadByte();
                PartHex = BitConverter.ToString(PartID);
                sw.Write(PartHex + " ");
                if (PartID[0] == 0x15 || PartID[0] == 0x16 || PartID[0] == 0x1D)
                {
                    GetInt();
                    OpenIndent();
                    for (bool stop = false; stop == false;)
                    {
                        ContinueIndent();
                        int data = GetInt();

                        if (data == 0x00)
                            stop = true;
                        else if (PartID[0] == 0x15 || PartID[0] == 0x16 & data != 0x0A & data != 0x0f & data != 0x10 & data != 0x15 & data != 0x16 & data != 0x1A & data != 0x1B & data != 0x3D & data != 0x3E & data != 0x3F & data != 0x40 & data != 0x41 & data != 0x42 & data != 0x48 & data != 0x51 & data != 0x52 & data != 0x55 & data != 0x5B & data != 0x60 & data != 0x61)
                            GetString();
                        else if (PartID[0] == 0x1D & data == 0x0A)
                            Subdata(0x1D, 0x0A);
                        else
                            GetBytes();
                    }
                    CloseIndent();
                    sw.Write("\r\n");
                }
                else
                {
                    int length = GetInt();
                    if (length > 0)
                    {
                        int entry = GetInt();
                        OpenIndent();
                        for (int i = 0; i < entry; i++)
                        {
                            ContinueIndent();
                            GetInt();
                            OpenIndent();
                            stop = false;
                            while (stop == false)
                            {
                                ContinueIndent();
                                int data = GetInt();

                                if (data == 0x00)
                                    stop = true;
                                else if (data == 0x01)
                                {
                                    GetString();
                                }
                                else
                                {
                                    switch (PartID[0], data)
                                    {
                                        #region String
                                        case (0x0B, 0x02):
                                        case (0x0C, 0x02):
                                        case (0x0D, 0x02):
                                        case (0x0E, 0x02):
                                        case (0x0B, 0x03):
                                        case (0x0B, 0x0F):
                                        case (0x10, 0x04):
                                        case (0x12, 0x33):
                                        case (0x12, 0x34):
                                        case (0x12, 0x35):
                                        case (0x12, 0x36):
                                        case (0x12, 0x37):
                                        case (0x13, 0x02):
                                        case (0x14, 0x02):
                                            GetString();
                                            break;
                                        #endregion
                                        #region Subdata
                                        case (0x0b, 0x3f):
                                        case (0x0d, 0x46):
                                        case (0x0e, 0x2a):
                                        case (0x0f, 0x02):
                                        case (0x16, 0x55):
                                        case (0x1e, 0x3f):
                                        case (0x20, 0x0a):
                                        case (0x20, 0x0b):
                                            Subdata(PartID[0], data);
                                            break;
                                        #endregion
                                        case (0x0c, 0x10):
                                        case (0x10, 0x0f):
                                            GetSound();
                                            break;
                                        case (0x0b, 0x1f):
                                        case (0x1e, 0x1f):
                                            #region system
                                            int system = GetInt();
                                            OpenIndent();
                                            ContinueIndent();
                                            sw.Write(BitConverter.ToString(br.ReadBytes(system / 6)));
                                            ContinueIndent();
                                            sw.Write(BitConverter.ToString(br.ReadBytes(system / 6)));
                                            ContinueIndent();
                                            sw.Write(BitConverter.ToString(br.ReadBytes(system / 6)));
                                            ContinueIndent();
                                            sw.Write(BitConverter.ToString(br.ReadBytes(system / 6)));
                                            ContinueIndent();
                                            sw.Write(BitConverter.ToString(br.ReadBytes(system / 6)));
                                            ContinueIndent();
                                            sw.Write(BitConverter.ToString(br.ReadBytes(system / 6)));
                                            CloseIndent();
                                            break;
                                        #endregion
                                        case (0x13, 0x06):
                                            #region AnimationTiming
                                            GetInt();
                                            int AnimationTiming = GetInt();
                                            if (AnimationTiming > 0)
                                                OpenIndent();
                                            for (int t = 0; t < AnimationTiming; t++)
                                            {
                                                ContinueIndent();
                                                GetInt();
                                                OpenIndent();
                                                for (bool timingStop = false; timingStop == false;)
                                                {
                                                    ContinueIndent();
                                                    int timingData = GetInt();
                                                    if (timingData == 0x00)
                                                    {
                                                        CloseIndent();
                                                        timingStop = true;
                                                    }
                                                    else if (timingData == 0x02)
                                                    {
                                                        GetSound();
                                                    }
                                                    else
                                                    {
                                                        GetBytes();
                                                    }
                                                }
                                            }
                                            if (AnimationTiming > 0)
                                                CloseIndent();
                                            break;
                                        #endregion
                                        case (0x13, 0x0c):
                                            #region Animation Frame
                                            GetInt();
                                            int AnimationFrame = GetInt();
                                            if (AnimationFrame > 0)
                                                OpenIndent();
                                            for (int f = 0; f < AnimationFrame; f++)
                                            {
                                                ContinueIndent();
                                                GetInt();
                                                OpenIndent();
                                                for (bool frameStop = false; frameStop == false;)
                                                {
                                                    ContinueIndent();
                                                    int frameData = GetInt();
                                                    if (frameData == 0x00)
                                                    {
                                                        CloseIndent();
                                                        frameStop = true;
                                                    }
                                                    else if (frameData == 0x01)
                                                    {
                                                        GetInt();
                                                        int cellData = GetInt();
                                                        if (cellData > 0)
                                                            OpenIndent();
                                                        for (int cd = 0; cd < cellData; cd++)
                                                        {
                                                            ContinueIndent();
                                                            GetInt();
                                                            OpenIndent();
                                                            for (bool cellDataStop = false; cellDataStop == false;)
                                                            {
                                                                ContinueIndent();
                                                                int Cell_Data = GetInt();
                                                                if (Cell_Data == 0x00)
                                                                {
                                                                    cellDataStop = true;
                                                                    CloseIndent();
                                                                }
                                                                else
                                                                {
                                                                    GetBytes();
                                                                }
                                                            }
                                                        }
                                                        if (cellData > 0)
                                                            CloseIndent();
                                                    }
                                                    else
                                                        GetBytes();
                                                }
                                            }
                                            if (AnimationFrame > 0)
                                                CloseIndent();
                                            break;
                                            #endregion
                                        case (0x19, 0x16):
                                            #region Event
                                            GetInt();
                                            OpenIndent();
                                            stop = false;
                                            while (stop == false)
                                            {
                                                GetEvent();
                                            }
                                            stop = false;
                                            CloseIndent();
                                            break;
                                        #endregion
                                        case (0x0f, 0x0b):
                                            #region subEntries
                                            GetInt();
                                            int subEntries = GetInt();
                                            if (subEntries > 0)
                                            {
                                                OpenIndent();
                                                for (int se = 0; se < subEntries; se++)
                                                {
                                                    ContinueIndent();
                                                    GetInt();
                                                    OpenIndent();
                                                    while (stop == false)
                                                    {
                                                        ContinueIndent();
                                                        int subData = GetInt();
                                                        if (subData == 0x00)
                                                            stop = true;
                                                        else if (subData == 0x02)
                                                        {
                                                            GetInt();
                                                            OpenIndent();
                                                            while (stop == false)
                                                            {
                                                                ContinueIndent();
                                                                int subSubData = GetInt();
                                                                if (subSubData == 0x00)
                                                                    stop = true;
                                                                else
                                                                    GetBytes();
                                                            }
                                                            CloseIndent();
                                                            stop = false;
                                                        }
                                                        else if (subData == 0x0C)
                                                        {
                                                            GetInt();
                                                            OpenIndent();
                                                            ContinueIndent();
                                                            while (stop == false)
                                                            {
                                                                GetEvent();
                                                            }
                                                            stop = false;
                                                            CloseIndent();
                                                        }
                                                        else
                                                            GetBytes();
                                                    }
                                                    stop = false;
                                                    CloseIndent();
                                                }
                                                CloseIndent();
                                            }
                                            break;
                                        #endregion
                                        default:
                                            GetBytes();
                                            break;
                                    }
                                }
                            }
                            CloseIndent();
                        }
                        CloseIndent();
                    }
                    sw.Write("\r\n");
                }
            }
            sw.Close();
        }

        static void OpenIndent()
        {
            ContinueIndent();
            sw.Write("{");
            indent++;
        }

        static void ContinueIndent()
        {
            sw.Write("\r\n");
            for (int tab = 0; tab < indent; tab++)
                sw.Write("\t");
        }

        static void CloseIndent()
        {
            indent--;
            ContinueIndent();
            sw.Write("}");
        }

        static void GetSound()
        {
            GetInt();
                OpenIndent();
                for (bool Data_end = false; Data_end == false;)
                {
                    ContinueIndent();
                    int subData = GetInt();
                if (subData == 0x00)
                    Data_end = true;
                else if (subData == 0x01)
                    GetString();
                else
                    GetBytes();
                }
                CloseIndent();
        }
        
        static void Subdata(byte PartID, int data)
        {
            GetInt();
            int subEntries = GetInt();
            if (subEntries > 0)
            {
                OpenIndent();
                for (int se = 0; se < subEntries; se++)
                {
                    ContinueIndent();
                    GetInt();
                    OpenIndent();
                    for (bool Data_end = false; Data_end == false;)
                    {
                        ContinueIndent();
                        int subData = GetInt();
                        if (subData == 0x00)
                            Data_end = true;
                        else if (PartID == 0x1D & data == 0x0A & subData == 0x01 | PartID == 0x20 & data == 0x0A & subData == 0x01 | PartID == 0x20 & data == 0x0A & subData == 0x02 | PartID == 0x20 & data == 0x0B & subData == 0x01 | PartID == 0x20 & data == 0x0B & subData == 0x02)
                            GetString();
                        else
                            GetBytes();
                    }
                    CloseIndent();
                }
                CloseIndent();
            }
        }

        static int GetInt()
        {
            return Convert7BitHex(0);
        }

        static int Convert7BitHex(int Prefix)
        {
            Prefix *= 0x80;//Shift higher-order bytes
            byte[] EntryNumber_FirstByte = br.ReadBytes(1);
            string ByteHex = BitConverter.ToString(EntryNumber_FirstByte);
            sw.Write(ByteHex + " ");
            int EntryNumber = EntryNumber_FirstByte[0];
            if (EntryNumber_FirstByte[0] > 0x7f)
            {
                EntryNumber -= 0x80;//Subtract 8th bit
                EntryNumber = Prefix + EntryNumber;
                EntryNumber = Convert7BitHex(EntryNumber);//process remaining bytes
            }
            else
            {
                EntryNumber = Prefix + EntryNumber;//case of least significant byte
            }
            return EntryNumber;
        }

        static byte[] GetBytes()
        {
            int length = GetInt();
            byte[] unknownBytes = br.ReadBytes(length);
            string ByteHex = BitConverter.ToString(unknownBytes);
            sw.Write(ByteHex + " ");
            return unknownBytes;
        }

        static int[] GetInts()
        {
            int values = GetInt();
            int[] unknownInts = new int[values];
            OpenIndent();
            for (int i = 0; i < values; i++)
            {
                ContinueIndent();
                unknownInts[i] = GetInt();
            }
            CloseIndent();
            return unknownInts;
        }
        
        static void GetString()
        {
            string text = br.ReadString();
            br.BaseStream.Position -= text.Length + 1;
            sw.Write(BitConverter.ToString(br.ReadBytes(text.Length + 1)));
            sw.Write("//" + text);
        }
        
        static void GetEvent()
        {
            ContinueIndent();
            int commoneventdata = GetInt();
            OpenIndent();
            ContinueIndent();
            Byte[] level = br.ReadBytes(1);
            sw.Write(BitConverter.ToString(level));
            OpenIndent();
            ContinueIndent();
            GetString();
            ContinueIndent();
            int[] gotInts = GetInts();
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
            CloseIndent();
            CloseIndent();
        }
    }
}
