using System;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Mvc;

namespace SafeNote.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class LockerController : ControllerBase
    {
        private readonly IDynamoDBContext _context;

        public LockerController(IDynamoDBContext context)
        {
            _context = context;
        }

        public class LockerCode
        {
            public string Code { get; set; }
        }

        public class LockerCodeData
        {
            public string Code { get; set; }
            public string Data { get; set; }
        }

        [HttpGet("create")]
        [ProducesResponseType(200, Type = typeof(string))]
        public async Task<IActionResult> Get()
        {
            var code = Guid.NewGuid().ToString("N");

            return Ok(code);
        }

        [HttpPost("read")]
        [ProducesResponseType(200, Type = typeof(string))]
        public async Task<IActionResult> Post([FromBody] LockerCode data)
        {
            var code = data.Code;

            var id = BuildId(code);

            var rec = await _context.LoadAsync<CodeNote>(id);
            if (rec == null) return Ok("");

            var dec = Decrypt(rec.Data, code, rec.Salt);

            return Ok(dec);
        }

        private static string BuildId(string code) => "locker:" + Utils.Hash(code);

        [HttpPut("update")]
        public async Task<IActionResult> Put([FromBody] LockerCodeData data)
        {
            var code = data.Code;

            var id = BuildId(code);

            var rec = await _context.LoadAsync<CodeNote>(id) ?? new CodeNote();

            var enc = Encrypt(data.Data, code, out var salt);

            rec.Id = id;
            rec.Data = enc;
            rec.Salt = salt;
            rec.Updated = DateTime.UtcNow;

            await _context.SaveAsync(rec);

            return Ok();
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] LockerCode data)
        {
            var code = data.Code;

            var id = BuildId(code);

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
            var saltBytes = rng.GetBytes(12);
            salt = Utils.Encode(saltBytes);

            using var pbkdf = new Rfc2898DeriveBytes(code, saltBytes, 10_000);
            var key = pbkdf.GetBytes(32);
            var iv = pbkdf.GetBytes(16);

            return Utils.EncryptAES(data, key, iv);
        }

    }

    [Route("api/[controller]")]
    public class CodeController : ControllerBase
    {
        private readonly IDynamoDBContext _context;

        public CodeController(IDynamoDBContext context)
        {
            _context = context;
        }

        public class CodePostData
        {
            public string Code { get; set; }
        }

        [HttpGet("{code}")]
        [ProducesResponseType(200, Type=typeof(string))]
        [Obsolete]
        public async Task<IActionResult> Get([FromRoute] string code)
        {
            var id = BuildId(code);

            var rec = await _context.LoadAsync<CodeNote>(id);
            if (rec == null) return Ok("");

            var dec = Decrypt(rec.Data, code, rec.Salt);

            return Ok(dec);
        }

        [HttpPost]
        [ProducesResponseType(200, Type=typeof(string))]
        public async Task<IActionResult> Post([FromBody] CodePostData data)
        {
            var code = data.Code;

            var id = BuildId(code);

            var rec = await _context.LoadAsync<CodeNote>(id);
            if (rec == null) return Ok("");

            var dec = Decrypt(rec.Data, code, rec.Salt);

            return Ok(dec);
        }

        private static string BuildId(string code) => "code:" + Utils.Hash(code);

        [HttpPut("{code}")]
        [Obsolete]
        public async Task<IActionResult> Put([FromRoute] string code, [FromBody] CodeData data)
        {
            var id = BuildId(code);

            var rec = await _context.LoadAsync<CodeNote>(id) ?? new CodeNote();

            var enc = Encrypt(data.Data, code, out var salt);

            rec.Id = id;
            rec.Data = enc;
            rec.Salt = salt;
            rec.Updated = DateTime.UtcNow;

            await _context.SaveAsync(rec);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CodeData data)
        {
            var code = data.Code;

            var id = BuildId(code);

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
        [Obsolete]
        public async Task<IActionResult> Delete([FromRoute] string code)
        {
            var id = BuildId(code);

            await _context.DeleteAsync<CodeNote>(id);

            return Ok();
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] CodeData data)
        {
            var code = data.Code;

            var id = BuildId(code);

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
            var saltBytes = rng.GetBytes(12);
            salt = Utils.Encode(saltBytes);

            using var pbkdf = new Rfc2898DeriveBytes(code, saltBytes, 10_000);
            var key = pbkdf.GetBytes(32);
            var iv = pbkdf.GetBytes(16);

            return Utils.EncryptAES(data, key, iv);
        }
    }
}