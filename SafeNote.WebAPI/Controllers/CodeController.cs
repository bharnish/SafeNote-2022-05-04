using System;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Mvc;

namespace SafeNote.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class CodeController : ControllerBase
    {
        private readonly IDynamoDBContext _context;

        public CodeController(IDynamoDBContext context)
        {
            _context = context;
        }

        [HttpGet("{code}")]
        [ProducesResponseType(200, Type=typeof(string))]
        public async Task<IActionResult> Get([FromRoute] string code)
        {
            var id = Utils.Hash(code);

            var rec = await _context.LoadAsync<CodeNote>(id);
            if (rec == null) return Ok("");

            var dec = Decrypt(rec.Data, code, rec.Salt);

            return Ok(dec);
        }

        [HttpPut("{code}")]
        public async Task<IActionResult> Put([FromRoute] string code, [FromBody] CodeData data)
        {
            var id = Utils.Hash(code);

            var rec = await _context.LoadAsync<CodeNote>(id) ?? new CodeNote();

            var enc = Encrypt(data.Data, code, out var salt);

            rec.Id = id;
            rec.Data = enc;
            rec.Salt = salt;
            rec.Updated = DateTime.UtcNow;

            await _context.SaveAsync(rec);

            return Ok();
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete([FromRoute] string code)
        {
            var id = Utils.Hash(code);

            await _context.DeleteAsync<CodeNote>(id);

            return Ok();
        }

        string Decrypt(string data, string code, string salt)
        {
            var saltBytes = Utils.Decode(salt);

            using var pbkdf = new Rfc2898DeriveBytes(code, saltBytes, 10_000);
            var key = pbkdf.GetBytes(32);
            var iv = pbkdf.GetBytes(16);

            return Utils.DecryptAES(data, key, iv);
        }

        string Encrypt(string data, string code, out string salt)
        {
            using var rng = RandomNumberGenerator.Create();
            var saltBytes = rng.GetBytes(18);
            salt = Utils.Encode(saltBytes);

            using var pbkdf = new Rfc2898DeriveBytes(code, saltBytes, 10_000);
            var key = pbkdf.GetBytes(32);
            var iv = pbkdf.GetBytes(16);

            return Utils.EncryptAES(data, key, iv);
        }
    }
}