using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SafeNote.WebAPI.Domain;
using SafeNote.WebAPI.DTOs;

namespace SafeNote.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ShortenerController : ControllerBase
    {
        private readonly IDynamoDBContext _context;

        public ShortenerController(IDynamoDBContext context)
        {
            _context = context;
        }

        [HttpPost("read")]
        [ProducesResponseType(200, Type = typeof(string))]
        public async Task<IActionResult> Read([FromBody] ShortenerReadData readData)
        {
            var id = readData.Id;

            var key = Hash(id);

            var rec = await _context.LoadAsync<ShortenerRecord>(key);
            if (rec == null) return NotFound();

            await _context.DeleteAsync(rec);

            var data = Decrypt(rec.Data, id, rec.Salt);

            return Ok(data);
        }

        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(string))]
        public async Task<IActionResult> Create([FromBody] ShortenerPostData data)
        {
            var enc = Encrypt(data.Data, out var password, out var salt);

            var rec = new ShortenerRecord
            {
                Id = Hash(password),
                Data = enc,
                Salt = salt
            };

            await _context.SaveAsync(rec);

            return Ok(password);
        }

        string Hash(string data)
        {
            using var sha = SHA256.Create();

            var hash = sha.ComputeHash(Decode(data));

            return Encode(hash);
        }

        string Decrypt(string data, string password, string salt)
        {
            using var rng = RandomNumberGenerator.Create();

            var pwbuf = Decode(password);
            var saltbuf = Decode(salt);

            using var pbkdf = new Rfc2898DeriveBytes(pwbuf, saltbuf, 10000);

            using var aes = Aes.Create();
            aes.Key = pbkdf.GetBytes(32);
            aes.IV = pbkdf.GetBytes(16);

            using var ms = new MemoryStream(Decode(data));
            using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }

        string Encrypt(string data, out string password, out string salt)
        {
            using var rng = RandomNumberGenerator.Create();

            var pwbuf = rng.GetBytes(6);
            var saltbuf = rng.GetBytes(18);

            password = Encode(pwbuf);
            salt = Encode(saltbuf);

            using var pbkdf = new Rfc2898DeriveBytes(pwbuf, saltbuf, 10000);

            using var aes = Aes.Create();
            aes.Key = pbkdf.GetBytes(32);
            aes.IV = pbkdf.GetBytes(16);

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                using var sw = new StreamWriter(cs);
                sw.Write(data);
            }

            return Encode(ms.ToArray());
        }

        private static byte[] Decode(string password)
        {
            return Base64UrlTextEncoder.Decode(password);
        }

        private static string Encode(byte[] pwbuf)
        {
            return Base64UrlTextEncoder.Encode(pwbuf);
        }
    }
}