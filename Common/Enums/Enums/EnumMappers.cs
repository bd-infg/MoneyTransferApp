using System;
using System.Collections.Generic;
using System.Text;

namespace Enums
{
    public static class EnumMappers
    {
        public static string MapTransactionType(byte id)
        {
            var result = id switch
            {
                1 => "Uplata od banke",
                2 => "Uplata banci",
                3 => "Transfer",
                4 => "Provizija",
                _ => "Nepoznato"
            };
            return result;
        }

        public static string MapFlowType(byte id)
        {
            var result = id switch
            {
                1 => "Uplata",
                2 => "Isplata",
                _ => "Nepoznato"
            };
            return result;
        }
    }
}
