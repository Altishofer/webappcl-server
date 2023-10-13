using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using ToX.Models;
using ToX.Services;

namespace ToX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Word2VectorController : ControllerBase
    {
        private readonly Word2VectorService _word2VectorService;
        private readonly ApplicationContext _context;

        public Word2VectorController(ApplicationContext applicationContext)
        {
            _word2VectorService = Word2VectorService.GetInstance(applicationContext);
            _context = applicationContext;
        }

        [HttpGet("modelInfo")]
        [AllowAnonymous]
        public IActionResult GetModelInfo()
        {
            _word2VectorService.PrintModelInfo();
            return Ok("Model information printed in the console.");
        }

        [HttpGet("closestWords/{word}/{count}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetClosestWords([FromRoute] string word, [FromRoute] int count)
        {
            var closestWords = await _word2VectorService.FindClosestWordsAsync(word, count);
            return Ok(closestWords);
        }
        
        [HttpGet("closestWordsSQL")]
        [AllowAnonymous]
        public async Task<IActionResult> GetClosestWordsSQL(string word, int count)
        {
            var closestWords = await _word2VectorService.FindClosestWordsAsyncSQL(word, count);
            return Ok(closestWords);
        }

        [HttpGet("analogy")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAnalogy(string wordA, string wordB, string wordC, int count)
        {
            var analogyWords = await _word2VectorService.AnalogyAsync(wordA, wordB, wordC, count);
            return Ok(analogyWords);
        }

        [HttpGet("wordAddition")]
        [AllowAnonymous]
        public async Task<IActionResult> GetWordAddition(string wordA, string wordB)
        {
            var additionWords = await _word2VectorService.WordAdditionAsync(wordA, wordB);
            return Ok(additionWords);
        }

        [HttpGet("wordSubtraction")]
        [AllowAnonymous]
        public async Task<IActionResult> GetWordSubtraction(string wordA, string wordB)
        {
            var subtractionWords = await _word2VectorService.WordSubtractionAsync(wordA, wordB);
            return Ok(subtractionWords);
        }
    }
}