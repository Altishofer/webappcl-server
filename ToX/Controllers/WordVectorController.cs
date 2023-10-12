using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToX.Models;
using ToX.Services;

namespace ToX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordVectorController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly WordVectorService _wordVectorService;

        public WordVectorController(ApplicationContext context)
        {
            _context = context;
            _wordVectorService = new WordVectorService(context);
            GetVector("test");
        }
        
        // GET: api/WordVector/word2vec
        [HttpGet("word2vex")]
        [AllowAnonymous]
        public async Task<IActionResult> GetVector(string word)
        {
            WordVector? vw = await _context.WordVector.SingleOrDefaultAsync(v => v.WordOrNull == word);
            
            return Ok(new { WordVector = vw });
        }
    }
}
