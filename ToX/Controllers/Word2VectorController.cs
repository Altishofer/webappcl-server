using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ToX.Services;

namespace ToX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Word2VectorController : ControllerBase
    {
        private readonly Word2VectorService _word2VectorService;

        public Word2VectorController()
        {
            _word2VectorService = new Word2VectorService();
        }

        [HttpGet("modelInfo")]
        public IActionResult GetModelInfo()
        {
            _word2VectorService.PrintModelInfo();
            return Ok("Model information printed in the console.");
        }

        [HttpGet("closestWords")]
        public async Task<IActionResult> GetClosestWords(string word, int count)
        {
            var closestWords = await _word2VectorService.FindClosestWordsAsync(word, count);
            return Ok(closestWords);
        }

        [HttpGet("analogy")]
        public async Task<IActionResult> GetAnalogy(string wordA, string wordB, string wordC, int count)
        {
            var analogyWords = await _word2VectorService.AnalogyAsync(wordA, wordB, wordC, count);
            return Ok(analogyWords);
        }

        [HttpGet("wordAddition")]
        public async Task<IActionResult> GetWordAddition(string wordA, string wordB, int count)
        {
            var additionWords = await _word2VectorService.WordAdditionAsync(wordA, wordB, count);
            return Ok(additionWords);
        }

        [HttpGet("wordSubtraction")]
        public async Task<IActionResult> GetWordSubtraction(string wordA, string wordB, int count)
        {
            var subtractionWords = await _word2VectorService.WordSubtractionAsync(wordA, wordB, count);
            return Ok(subtractionWords);
        }
    }
}