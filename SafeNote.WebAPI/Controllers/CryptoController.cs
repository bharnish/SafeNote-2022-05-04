using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SafeNote.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class CryptoController : ControllerBase
    {
        private readonly IDynamoDBContext _context;

        [DynamoDBTable("safenote-2022-05-04")]
        public class Note
        {
            [DynamoDBHashKey]
            public string Id { get; set; }

            public string Data { get; set; }

            public DateTime Created { get; set; } = DateTime.UtcNow;
        }

        public CryptoController(IDynamoDBContext context)
        {
            _context = context;
        }

        public class ReadData
        {
            public string Key { get; set; }
            public string IV { get; set; }
        }

        [HttpPost("read")]
        [ProducesResponseType(200, Type = typeof(string))]
        public async Task<IActionResult> Get([FromBody] ReadData readData)
        {
            var key = Decode(readData.Key);
            var iv = Decode(readData.IV);

            var id = GetId(iv, key);
            
            var note = await _context.LoadAsync<Note>(id);
            if (note == null) return NotFound();

            await _context.DeleteAsync<Note>(id);

            var bytes = Convert.FromBase64String(note.Data);

            var data = Decrypt(bytes, key, iv);

            return Ok(data);
        }

        private string GetId(byte[] iv, byte[] key)
        {
            using var hash = SHA256.Create();

            using var ms = new MemoryStream();

            using (var cs = new CryptoStream(ms, hash, CryptoStreamMode.Write))
            {
                cs.Write(iv, 0, iv.Length);
                cs.Write(key, 0, key.Length);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        public class PostData
        {
            public string Data { get; set; }
        }

        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(string))]
        public async Task<IActionResult> Post([FromBody] PostData data)
        {
            var enc = Encrypt(data.Data, out var key, out var iv);

            var id = GetId(iv, key);

            var note = new Note
            {
                Id = id,
                Data = Convert.ToBase64String(enc),
            };

            await _context.SaveAsync(note);

            var ks = Encode(key);
            var ivs = Encode(iv);

            return Ok($"{ks}?i={ivs}");
        }

        string Encode(byte[] data)
        {
            return Base64UrlTextEncoder.Encode(data);
        }

        byte[] Decode(string data)
        {
            return Base64UrlTextEncoder.Decode(data);
        }

        byte[] Encrypt(string data, out byte[] key, out byte[] iv)
        {
            using var aes = Aes.Create();
            key = aes.Key;
            iv = aes.IV;

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(data);
                }
            }

            return ms.ToArray();
        }
        
        string Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            using var ms = new MemoryStream(data);
            using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            
            return sr.ReadToEnd();
        }
    }
}
