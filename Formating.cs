using static RPG_Maker_LDB_Printer.Program;
namespace RPG_Maker_LDB_Printer
{
    internal class Formating
    {
        public static int indent = 0;
        public static void OpenIndent()
        {
            ContinueIndent();
            sw.Write("{");
            indent++;
        }

        public static void ContinueIndent()
        {
            sw.Write("\r\n");
            for (int tab = 0; tab < indent; tab++)
                sw.Write("\t");
        }

        public static void CloseIndent()
        {
            indent--;
            ContinueIndent();
            sw.Write("}");
        }
    }
}
