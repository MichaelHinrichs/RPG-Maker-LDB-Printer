using System;
using System.IO;
using static RPG_Maker_LDB_Printer.Formating;

namespace RPG_Maker_LDB_Printer
{
    public static class Program
    {
        public static BinaryReader br;
        public static StreamWriter sw;
        public static bool stop;

        static void Main(string[] args)
        {
            br = new BinaryReader(File.OpenRead(args[0]));
            sw = new StreamWriter(File.Create(args[0] + ".txt"));

            Data.GetString();//LcfDataBase
            ContinueIndent();

            for (byte[] PartID = new byte[1]; PartID[0] < 0x20;)
            {
                string PartHex;
                PartID[0] = br.ReadByte();
                PartHex = BitConverter.ToString(PartID);
                sw.Write(PartHex + " ");
                if (PartID[0] == 0x15 || PartID[0] == 0x16 || PartID[0] == 0x1D)
                {
                    Data.GetInt();
                    OpenIndent();
                    bool stop = false;
                    while ( stop == false)
                    {
                        ContinueIndent();
                        int data = Data.GetInt();

                        if (data == 0x00)
                            stop = true;
                        else if (PartID[0] == 0x15 || PartID[0] == 0x16 & data != 0x0A & data != 0x0f & data != 0x10 & data != 0x15 & data != 0x16 & data != 0x1A & data != 0x1B & data != 0x3D & data != 0x3E & data != 0x3F & data != 0x40 & data != 0x41 & data != 0x42 & data != 0x48 & data != 0x51 & data != 0x52 & data != 0x55 & data != 0x5B & data != 0x60 & data != 0x61)
                            Data.GetString();
                        else if (PartID[0] == 0x1D & data == 0x0A)
                            Data.Subdata(0x1D, 0x0A);
                        else
                            Data.GetBytes();
                    }
                    CloseIndent();
                    sw.Write("\r\n");
                }
                else
                {
                    int length = Data.GetInt();
                    if (length > 0)
                    {
                        int entry = Data.GetInt();
                        OpenIndent();
                        for (int i = 0; i < entry; i++)
                        {
                            ContinueIndent();
                            Data.GetInt();
                            OpenIndent();
                            stop = false;
                            while (stop == false)
                            {
                                ContinueIndent();
                                int data = Data.GetInt();

                                if (data == 0x00)
                                    stop = true;
                                else if (data == 0x01)
                                {
                                    Data.GetString();
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
                                            Data.GetString();
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
                                            Data.Subdata(PartID[0], data);
                                            break;
                                        #endregion
                                        case (0x0c, 0x10):
                                        case (0x10, 0x0f):
                                            Data.GetSound();
                                            break;
                                        case (0x0b, 0x1f):
                                        case (0x1e, 0x1f):
                                            #region system
                                            int system = Data.GetInt();
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
                                            Data.GetInt();
                                            int AnimationTiming = Data.GetInt();
                                            if (AnimationTiming > 0)
                                                OpenIndent();
                                            for (int t = 0; t < AnimationTiming; t++)
                                            {
                                                ContinueIndent();
                                                Data.GetInt();
                                                OpenIndent();
                                                bool timingStop = false;
                                                while (timingStop == false)
                                                {
                                                    ContinueIndent();
                                                    int timingData = Data.GetInt();
                                                    if (timingData == 0x00)
                                                    {
                                                        CloseIndent();
                                                        timingStop = true;
                                                    }
                                                    else if (timingData == 0x02)
                                                    {
                                                        Data.GetSound();
                                                    }
                                                    else
                                                    {
                                                        Data.GetBytes();
                                                    }
                                                }
                                            }
                                            if (AnimationTiming > 0)
                                                CloseIndent();
                                            break;
                                        #endregion
                                        case (0x13, 0x0c):
                                            #region Animation Frame
                                            Data.GetInt();
                                            int AnimationFrame = Data.GetInt();
                                            if (AnimationFrame > 0)
                                                OpenIndent();
                                            for (int f = 0; f < AnimationFrame; f++)
                                            {
                                                ContinueIndent();
                                                Data.GetInt();
                                                OpenIndent();
                                                bool frameStop = false;
                                                while(frameStop == false)
                                                {
                                                    ContinueIndent();
                                                    int frameData = Data.GetInt();
                                                    if (frameData == 0x00)
                                                    {
                                                        CloseIndent();
                                                        frameStop = true;
                                                    }
                                                    else if (frameData == 0x01)
                                                    {
                                                        Data.GetInt();
                                                        int cellData = Data.GetInt();
                                                        if (cellData > 0)
                                                            OpenIndent();
                                                        for (int cd = 0; cd < cellData; cd++)
                                                        {
                                                            ContinueIndent();
                                                            Data.GetInt();
                                                            OpenIndent();
                                                            bool cellDataStop = false;
                                                            while (cellDataStop == false)
                                                            {
                                                                ContinueIndent();
                                                                int Cell_Data = Data.GetInt();
                                                                if (Cell_Data == 0x00)
                                                                {
                                                                    cellDataStop = true;
                                                                    CloseIndent();
                                                                }
                                                                else
                                                                {
                                                                    Data.GetBytes();
                                                                }
                                                            }
                                                        }
                                                        if (cellData > 0)
                                                            CloseIndent();
                                                    }
                                                    else
                                                        Data.GetBytes();
                                                }
                                            }
                                            if (AnimationFrame > 0)
                                                CloseIndent();
                                            break;
                                            #endregion
                                        case (0x19, 0x16):
                                            #region Event
                                            Data.GetInt();
                                            OpenIndent();
                                            stop = false;
                                            while (stop == false)
                                            {
                                                Event.GetEvent();
                                            }
                                            stop = false;
                                            CloseIndent();
                                            break;
                                        #endregion
                                        case (0x0f, 0x0b):
                                            #region subEntries
                                            Data.GetInt();
                                            int subEntries = Data.GetInt();
                                            if (subEntries > 0)
                                            {
                                                OpenIndent();
                                                for (int se = 0; se < subEntries; se++)
                                                {
                                                    ContinueIndent();
                                                    Data.GetInt();
                                                    OpenIndent();
                                                    while (stop == false)
                                                    {
                                                        ContinueIndent();
                                                        int subData = Data.GetInt();
                                                        if (subData == 0x00)
                                                            stop = true;
                                                        else if (subData == 0x02)
                                                        {
                                                            Data.GetInt();
                                                            OpenIndent();
                                                            while (stop == false)
                                                            {
                                                                ContinueIndent();
                                                                int subSubData = Data.GetInt();
                                                                if (subSubData == 0x00)
                                                                    stop = true;
                                                                else
                                                                    Data.GetBytes();
                                                            }
                                                            CloseIndent();
                                                            stop = false;
                                                        }
                                                        else if (subData == 0x0C)
                                                        {
                                                            Data.GetInt();
                                                            OpenIndent();
                                                            ContinueIndent();
                                                            while (stop == false)
                                                            {
                                                                Event.GetEvent();
                                                            }
                                                            stop = false;
                                                            CloseIndent();
                                                        }
                                                        else
                                                            Data.GetBytes();
                                                    }
                                                    stop = false;
                                                    CloseIndent();
                                                }
                                                CloseIndent();
                                            }
                                            break;
                                        #endregion
                                        default:
                                            Data.GetBytes();
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
    }
}
