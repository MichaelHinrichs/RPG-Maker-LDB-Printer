using System;
using static RPG_Maker_LDB_Printer.Program;
using static RPG_Maker_LDB_Printer.Formating;
namespace RPG_Maker_LDB_Printer
{
    public class Data
    {
        public static void GetSound()
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

        public static void Subdata(byte PartID, int data)
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

        public static int GetInt()
        {
            return Convert7BitHex(0);
        }

        public static int Convert7BitHex(int Prefix)
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

        public static byte[] GetBytes()
        {
            int length = GetInt();
            byte[] unknownBytes = br.ReadBytes(length);
            string ByteHex = BitConverter.ToString(unknownBytes);
            sw.Write(ByteHex + " ");
            return unknownBytes;
        }

        public static int[] GetInts()
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

        public static void GetString()
        {
            string text = br.ReadString();
            br.BaseStream.Position -= text.Length + 1;
            sw.Write(BitConverter.ToString(br.ReadBytes(text.Length + 1)));
            sw.Write("//" + text);
        }
    }
}
