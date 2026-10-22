using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SIRALAB_BL
{
    public static class SqltoEntitiesExtend
    {
        [System.Data.Objects.DataClasses.EdmFunction("SIRALAB_BL", "String_Like")]
        public static bool Like(this string input, string pattern)
        {
            /* Turn "off" all regular expression related syntax in
                * the pattern string. */
            pattern = Regex.Escape(pattern);

            /* Replace the SQL LIKE wildcard metacharacters with the
                * equivalent regular expression metacharacters. */
            pattern = pattern.Replace("%", ".*?").Replace("_", ".");

            /* The previous call to Regex.Escape actually turned off
                * too many metacharacters, i.e. those which are recognized by
                * both the regular expression engine and the SQL LIKE
                * statement ([...] and [^...]). Those metacharacters have
                * to be manually unescaped here. */
            pattern = pattern.Replace(@"\[", "[").Replace(@"\]", "]").Replace(@"\^", "^");

            bool returnValue = Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
            return returnValue;
        }

        /// <summary>
        /// Este método permite cambiar la cadena de entrada por una cadena que acepta tanto vocales con acento como sin acento
        /// </summary>
        /// <param name="Data">Cadena de entrada a convertir</param>
        /// <returns>retorna una cadena válida para buscar registros de texto con mayúsculas y minúsculas</returns>
        public static string FilterSQLData(this string Data)
        {
            string respuesta = "%";

            foreach (var s in Data)
            {
                string resp = s.ToString();

                if (s == 'a' || s == 'á') resp = "[aá]";
                if (s == 'e' || s == 'é') resp = "[eé]";
                if (s == 'i' || s == 'í') resp = "[ií]";
                if (s == 'o' || s == 'ó') resp = "[oó]";
                if (s == 'u' || s == 'ú' || s == 'ü') resp = "[uúü]";

                respuesta += resp;
            }

            return respuesta + "%";
        }
    }
}
