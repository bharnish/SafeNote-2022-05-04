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
using Microsoft.AspNetCore.Mvc;
using SafeNote.WebAPI.Domain;
using SafeNote.WebAPI.DTOs;

namespace SafeNote.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class CryptoController : ControllerBase
    {
        private readonly IDynamoDBContext _context;

        public CryptoController(IDynamoDBContext context)
        {
            _context = context;
        }

        [HttpPost("read")]
        [ProducesResponseType(200, Type = typeof(string))]
        public async Task<IActionResult> Read([FromBody] ReadData readData)
        {
            var key = Decode(readData.Key);
            var iv = Decode(readData.IV);

            var id = BuildId(iv, key);
            
            var note = await _context.LoadAsync<Note>(id);
            if (note == null) return NotFound();

            await _context.DeleteAsync<Note>(id);

            var bytes = Convert.FromBase64String(note.Data);

            var data = Decrypt(bytes, key, iv);

            return Ok(data);
        }

        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(string))]
        public async Task<IActionResult> Create([FromBody] PostData data)
        {
            var enc = Encrypt(data.Data, out var key, out var iv);

            var id = BuildId(iv, key);

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

        private string BuildId(byte[] iv, byte[] key)
        {
            using var ms = new MemoryStream();
            ms.Write(iv, 0, iv.Length);
            ms.Write(key, 0, key.Length);
            return "long:" + Utils.Hash(ms.ToArray());
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
