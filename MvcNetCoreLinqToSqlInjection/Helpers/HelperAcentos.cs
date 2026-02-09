using System.Globalization;
using System.Text;

namespace MvcNetCoreLinqToSqlInjection.Helpers
{
    public static class HelperAcentos
    {
        
            public static string RemoveAccents(this string text)
            {
                

                string normalizedString = text.Normalize(NormalizationForm.FormD);
                StringBuilder sb = new StringBuilder();

                foreach (char c in normalizedString)
                {
                    if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    {
                        sb.Append(c);
                    }
                }
                return sb.ToString().Normalize(NormalizationForm.FormC);
            
        }
    }
}
