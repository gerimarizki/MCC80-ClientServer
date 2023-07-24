namespace API.Utilities
{
    public class HandlerGenerator
    {
        public static string Nik(string? nik = null)
        {
            if (nik is null)
            {
                return "00001";
            }

            var generatedNik = int.Parse(nik) + 1;

            return generatedNik.ToString();
        }
    }
}
