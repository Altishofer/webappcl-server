using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToX.Models;
using ToX.Services;
using Word2vec.Tools;

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
        }
        
        // GET: api/WordVector/word2vec
        [HttpGet("word2vec")]
        [AllowAnonymous]
        public async Task<IActionResult> GetVector(string word)
        {
            WordVector? vw = await _context.WordVector.SingleOrDefaultAsync(v => v.WordOrNull == word);
            
            return Ok(new { WordVector = vw });
        }
        
        // GET: api/WordVector/closest
        [HttpGet("closest")]
        [AllowAnonymous]
        public async Task<IActionResult> GetClosest(string word)
        {
            WordVector vw = _wordVectorService.FindNearestVector(word).Result;
            
            return Ok(new { WordVector = vw.WordOrNull });
        }
        
        // GET: api/WordVector/word2vec
        [HttpGet("importData_DO_NOT_CLICK")]
        [AllowAnonymous]
        public async Task<IActionResult> ImportData()
        {
            //_wordVectorService.PushAllWordVectorsToDatabase();
            
            return Ok();
        }
    }
}
