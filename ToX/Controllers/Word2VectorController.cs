using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToX.Models;
using ToX.DTOs.VectorDto;
using Word2vec.Tools;

namespace ToX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Word2VectorController : ControllerBase
    {
        private readonly Word2VectorService _word2VectorService;
        private readonly ApplicationContext _context;
        private readonly IConfiguration _config;

        public Word2VectorController(ApplicationContext applicationContext, IConfiguration config)
        {
            _context = applicationContext;
            _config = config;
            _word2VectorService = Word2VectorService.GetInstance(_context, _config);
        }
        
        [AllowAnonymous]
        [HttpGet("status")]
        public async Task<ActionResult>  GetStatus()
        {
            int length;
            int dimensions;
            DistanceTo[] closest;
            string relativeRootPath;
            try
            {
                relativeRootPath = _config["VECTOR_BIN"];
                var voc = new Word2VecBinaryReader().Read(Path.GetFullPath(relativeRootPath));
                length = voc.Words.Length;
                dimensions = voc.VectorDimensionsCount;
                closest = await Task.Run(() => voc.Distance("dog", 1));
            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    exception = "File could not be loaded",
                    error = ex.Message,
                    rootPath = _config["VECTOR_BIN"],
                    currentPath = Environment.CurrentDirectory
                });
            }
            return Ok(new
            {
                length = length.ToString(),
                dimensions = dimensions.ToString(),
                similarWord = closest[0].Representation.WordOrNull,
                rootPath = _config["VECTOR_BIN"]
            });
        }
        
        [AllowAnonymous]
        [HttpGet("validate/{word}")]
        public async Task<IActionResult> CheckValidity([FromRoute] string word)
        {
            bool isValid = await _word2VectorService.IsValidWord(word);
            return Ok(isValid);
        }
        
        [AllowAnonymous]
        [HttpPut("wordCalculation")]
        public async Task<IActionResult> GetWordCalcDebug([FromBody] VectorCalculationDto vecCalcDto)
        {
            List<string> result =
                await _word2VectorService.WordCalculation(vecCalcDto.Additions, vecCalcDto.Subtractions);
            result = result.Take(3).ToList();
            return Ok(result);
        }
        
    }
}