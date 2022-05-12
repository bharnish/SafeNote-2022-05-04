using System.Security.Cryptography;

namespace SafeNote.WebAPI
{
    public static class Exs
    {
        public static byte[] GetBytes(this RandomNumberGenerator self, int cb)
        {
            var buf = new byte[cb];
            self.GetBytes(buf);
            return buf;
        }
    }
}