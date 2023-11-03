using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToX.DTOs;
using ToX.Models;
using ToX.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ToX.DTOs.VectorDto;
using ToX.Models;
using ToX.Services;
using Word2vec.Tools;
using Word2vec = ToX.Models.WordVector;

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
        
        // GET: api/Word2Vector/status
        [HttpGet("status")]
        [AllowAnonymous]
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
        
        [HttpGet("validate/{word}")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckValidity([FromRoute] string word)
        {
            bool isValid = await _word2VectorService.IsValidWord(word);
            return Ok(isValid);
        }

        [HttpGet("closestWords/{word}/{count}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetClosestWords([FromRoute] string word, [FromRoute] int count)
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
        public async Task<IActionResult> GetWordAddition(string wordA, string wordB)
        {
            var additionWords = await _word2VectorService.WordAdditionAsync(wordA, wordB);
            return Ok(additionWords);
        }

        [HttpGet("wordSubtraction")]
        public async Task<IActionResult> GetWordSubtraction(string wordA, string wordB)
        {
            var subtractionWords = await _word2VectorService.WordSubtractionAsync(wordA, wordB);
            return Ok(subtractionWords);
        }
        
        [HttpPut("wordCalculation")]
        public async Task<IActionResult> GetWordSubtraction([FromBody] VectorCalculationDto vecCalcDto)
        {
            return Ok(await _word2VectorService.WordCalculation(vecCalcDto.Additions, vecCalcDto.Subtractions));
        }
        
    }
}