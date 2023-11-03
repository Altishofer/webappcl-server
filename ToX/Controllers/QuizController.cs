﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ToX.DTOs;
using ToX.Models;

using ToX.DTOs.QuizDto;
using ToX.DTOs.ResultDto;
using ToX.DTOs.RoundDto;
using ToX.Hubs;
using ToX.Services;
using Host = ToX.Models.Host;

namespace ToX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IConfiguration _config;
        private readonly QuizService _quizService;
        private readonly RoundService _roundService;
        private readonly AnswerService _answerService;
        private readonly Word2VectorService _word2VectorService;
        private readonly HostService _hostService;
        private readonly QuizHub _quizHub;
        private readonly PlayerService _playerService;

        public QuizController(ApplicationContext context, IConfiguration config, IHubContext<QuizHub> hubContext)
        {
            _context = context;
            _config = config;
            _word2VectorService = Word2VectorService.GetInstance(_context, _config);
            _quizService = new QuizService(_context);
            _roundService = new RoundService(_context, _word2VectorService);
            _answerService = new AnswerService(_context, _word2VectorService);
            _hostService = new HostService(_context, _config);
            _playerService = new PlayerService(_context, _config);
            _quizHub = new QuizHub(hubContext);
        }

        [HttpGet("GetAllQuizzes")]
        public async Task<IActionResult> GetAllQuizzes()
        {
            List<Quiz> quizzes = await _quizService.GetAllQuizzes();
            return Ok(quizzes);
        }
        
        [HttpGet("GetAllQuizzesByHost")]
        [Authorize]
        public async Task<IActionResult> GetAllQuizzesByHost()
        {
            Host? claimHost = await _hostService.VerifyHost(HttpContext.User);
            if (claimHost == null)
            {
                return Unauthorized("The token could not be validated");
            }

            List<Quiz> quizzes = await _quizService.GetAllQuizzesByHost(claimHost);
            quizzes.Sort((a, b) => a.Id < b.Id ? -1 : 1);
            return Ok(quizzes);
        }

        // ToDo: remove helper method
        [HttpGet("GetAllRounds")]
        public async Task<IActionResult> GetAllRounds()
        {
            List<RoundDto> rounds = _roundService.GetAllRounds().Result.Select(r => new RoundDto(r)).ToList();
            return Ok(rounds);
        }
        
        [HttpGet("GetAllRoundsByQuiz")]
        [Authorize]
        public async Task<IActionResult> GetAllRoundsByQuiz([FromQuery(Name= "quizId")] long quizId)
        {
            Host? claimHost = await _hostService.VerifyHost(HttpContext.User);
            if (claimHost == null)
            {
                return Unauthorized("The token could not be validated");
            }
            List<RoundDto> rounds = _roundService.GetAllRoundsByQuiz(quizId).Result.Select(r => new RoundDto(r)).ToList();
            rounds.Sort((a, b) => a.Id < b.Id ? -1 : 1);
            return Ok(rounds);
        }
        
        [HttpGet("GetAllRoundIdsByQuiz/{quizId}")]
        [Authorize]
        public async Task<IActionResult> GetAllRoundIdsByQuiz([FromRoute] long quizId)
        {
            Host? claimHost = await _hostService.VerifyHost(HttpContext.User);
            if (claimHost == null)
            {
                return Unauthorized("The token could not be validated");
            }
            Quiz? quiz = await _quizService.GetQuizOrNull(quizId);
            if (quiz == null)
            {
                return BadRequest("Quiz does not exist");
            }
            
            List<Round> rounds = await _roundService.GetAllRoundsByQuiz(quizId);
            List<long> roundIds = rounds.Select(r => r.Id).ToList();
            roundIds.Sort((a, b) => a < b ? -1 : 1);
            return Ok(roundIds);
        }

        // ToDo: remove helper method
        [HttpGet("GetAllAnswers")]
        public async Task<IActionResult> GetAllAnswers()
        {
            List<Answer> answers = await _answerService.GetAllAnswers();
            return Ok(answers);
        }

        [HttpPost("CreateQuiz")]
        [Authorize]
        public async Task<IActionResult> CreateQuiz([FromBody] QuizDto quizDto)
        {
            Host? claimHost = await _hostService.VerifyHost(HttpContext.User);
            if (claimHost == null)
            {
                return Unauthorized("The token could not be validated");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid quiz object");
            }

            Quiz quiz = await _quizService.CreateQuiz(quizDto);
            QuizDto returnQuizDto = new QuizDto(quiz);

            return CreatedAtAction(nameof(CreateQuiz), new { returnQuizDto });
        }
        
        [HttpPost("CreateQuizWithRounds")]
        [Authorize]
        public async Task<IActionResult> CreateQuizWithRounds([FromBody] QuizRoundDto quizRoundDto)
        {
            Host? claimHost = await _hostService.VerifyHost(HttpContext.User);
            if (claimHost == null)
            {
                return Unauthorized("The token could not be validated");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid quiz object");
            }
            
            QuizDto quizDto = new QuizDto(quizRoundDto.QuizId, quizRoundDto.HostId, quizRoundDto.Title);

            Quiz quiz = await _quizService.CreateQuiz(quizDto);
            QuizDto returnQuizDto = new QuizDto(quiz);

            foreach (RoundDto roundDto in quizRoundDto.Rounds)
            {
                roundDto.QuizId = quiz.Id;
                await _roundService.CreateRound(roundDto);
            }
            QuizRoundDto quizRoundDtoNew = await GetQuizRoundDto(quiz.Id);
            return CreatedAtAction(nameof(CreateQuiz), quizRoundDtoNew);
        }
        
        [HttpGet("PushRound/{roundId}")]
        [Authorize]
        public async Task<IActionResult> PushRound([FromRoute] long roundId)
        {
            Host? claimHost = await _hostService.VerifyHost(HttpContext.User);
            if (claimHost == null)
            {
                return Unauthorized("The token could not be validated");
            }
            
            Round? round = await _roundService.GetRoundOrNull(roundId);
            if (round == null)
            {
                return BadRequest("Round does not exist");
            }
            
            Quiz? quiz = await _quizService.GetQuizOrNull(round.QuizId);
            if (quiz == null)
            {
                return BadRequest("Quiz does not exist");
            }

            await _quizHub.SendNextRoundToGroup(quiz.Id.ToString(), roundId.ToString());
            string response = $"Round {roundId} was broadcasted to {quiz.Id}";
            return Ok(new {response});
        }

        [HttpPost("CreateRound")]
        [Authorize]
        public async Task<IActionResult> CreateRound(RoundDto roundDto)
        {
            Host? claimHost = await _hostService.VerifyHost(HttpContext.User);
            if (claimHost == null)
            {
                return Unauthorized("The token could not be validated");
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid round object");
            }

            Round round = await _roundService.CreateRound(roundDto);
            RoundDto returnRoundDto = new RoundDto(round);
            return CreatedAtAction(nameof(CreateRound), returnRoundDto);
        }

        [Authorize]
        [HttpPost("CreateAnswer")]
        public async Task<IActionResult> CreateAnswer(AnswerDto answerDto)
        {
            Player? claimPlayer = await _playerService.VerifyPlayer(HttpContext.User);
            if (claimPlayer == null)
            {
                return Unauthorized("The token could not be validated");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid answer");
            }
            
            if (await _answerService.AnswerExists(answerDto.PlayerName, answerDto.RoundId))
            {
                return BadRequest("Answer already exists");
            }

            Round? round = await _roundService.GetRoundOrNull(answerDto.RoundId);
            if (round == null)
            {
                return BadRequest("Round does not exist");
            }
            
            Quiz? quiz = await _quizService.GetQuizOrNull(answerDto.QuizId);
            if (quiz == null)
            {
                return BadRequest("Quiz does not exist");
            }
            
            Answer answer = await _answerService.CreateAnswer(answerDto, round.RoundTargetVector);
            AnswerDto returnAnswerDto = new AnswerDto(answer);
            
            WaitResultDto waitResultDto = await GetWaitResultDto(answerDto.QuizId, answerDto.RoundId);
            await _quizHub.SendWaitRankingToGroup(answerDto.QuizId.ToString(), waitResultDto);
            
            return CreatedAtAction(nameof(CreateAnswer), returnAnswerDto);
        }

        [HttpGet("WaitResult/{quizId}/{roundId}")]
        [Authorize]
        public async Task<ActionResult<WaitResultDto>> GetWaitResult([FromRoute] long quizId, [FromRoute] long roundId)
        {
            Host? claimHost = await _hostService.VerifyHost(HttpContext.User);
            if (claimHost == null)
            {
                return Unauthorized("The token could not be validated");
            }
            Round? round = await _roundService.GetRoundOrNull(roundId);
            if (round == null)
            {
                return BadRequest("Round does not exist");
            }
            
            Quiz? quiz = await _quizService.GetQuizOrNull(quizId);
            if (quiz == null)
            {
                return BadRequest("Quiz does not exist");
            }

            WaitResultDto waitResultDto = await GetWaitResultDto(quizId, roundId);
            await _quizHub.SendWaitRankingToGroup(quizId.ToString(), waitResultDto);
            return Ok(waitResultDto);
        }

        private async Task<WaitResultDto> GetWaitResultDto(long quizId, long roundId)
        {
            List<Answer> answered = await _answerService.GetAnswersByRoundId(roundId);
            List<Player> all = await _playerService.GetPlayersByQuiz(quizId);
            
            List<string> answeredPlayers = answered.Select(a => a.PlayerName).ToList();
            List<string> allPlayers = all.Select(p => p.PlayerName).ToList();
            
            List<string> notAnsweredPlayers = allPlayers.Except(answeredPlayers).ToList();

            return new WaitResultDto(notAnsweredPlayers, answeredPlayers);
        }
        
        [Authorize]
        [HttpGet("IntermediateResult/{quizId}/{roundId}")]
        public async Task<ActionResult<WaitResultDto>> GetIntermediateResult([FromRoute] long quizId, [FromRoute] long roundId)
        {
            Host? claimHost = await _hostService.VerifyHost(HttpContext.User);
            if (claimHost == null)
            {
                return Unauthorized("The token could not be validated");
            }
            Round? round = await _roundService.GetRoundOrNull(roundId);
            if (round == null)
            {
                return BadRequest("Round does not exist");
            }
            
            Quiz? quiz = await _quizService.GetQuizOrNull(quizId);
            if (quiz == null)
            {
                return BadRequest("Quiz does not exist");
            }

            List<Answer> answers = await _answerService.GetAnswersByRoundId(roundId);
            List<IntermediateResultDto> intermediateResultDtos = answers.Select(a => new IntermediateResultDto(a)).ToList();
            intermediateResultDtos.Sort((a, b) => a.Points > b.Points ? -1 : 1);
            _quizHub.SendIntermediateResultToGroup(quizId.ToString(), intermediateResultDtos);
            return Ok(intermediateResultDtos);
        }
        
        [Authorize]
        [HttpGet("FinalResult/{quizId}")]
        public async Task<ActionResult<WaitResultDto>> GetFinalResult([FromRoute] long quizId)
        {
            Host? claimHost = await _hostService.VerifyHost(HttpContext.User);
            if (claimHost == null)
            {
                return Unauthorized("The token could not be validated");
            }
            
            Quiz? quiz = await _quizService.GetQuizOrNull(quizId);
            if (quiz == null)
            {
                return BadRequest("Quiz does not exist");
            }
            
            List<FinalResultDto> finalResultDtos = new List<FinalResultDto>();
            List<Round> rounds = await _roundService.GetAllRoundsByQuiz(quizId);
            List<Answer> answers = new List<Answer>();
            foreach (Round round in rounds)
            {
                List<Answer> roundAnswers = await _answerService.GetAnswersByRoundId(round.Id);
                answers.AddRange(roundAnswers);
            }
            
            foreach (Answer answer in answers)
            {
                if (finalResultDtos.Exists(r => answer.PlayerName == r.PlayerName))
                {
                    finalResultDtos.Find(r => r.PlayerName == answer.PlayerName).Points += answer.Points;
                }
                else
                {
                    finalResultDtos.Add(new FinalResultDto(answer.PlayerName, answer.Points));
                }
            }
            
            finalResultDtos.Sort((a, b) => a.Points > b.Points ? -1 : 1);
            _quizHub.SendFinalResultToGroup(quizId.ToString(), finalResultDtos);
            return Ok(finalResultDtos);
        }
        
        [Authorize]
        [HttpGet("GetPlayers/{quizId}")]
        public async Task<ActionResult<string>> GetPlayersByQuiz([FromRoute] long quizId)
        {
            Host? claimHost = await _hostService.VerifyHost(HttpContext.User);
            if (claimHost == null)
            {
                return Unauthorized("The token could not be validated");
            }
            Quiz? quiz = await _quizService.GetQuizOrNull(quizId);
            if (quiz == null)
            {
                return BadRequest("Quiz does not exist");
            }
            List<Player> playerList = await _playerService.GetPlayersByQuiz(quizId);
            List<string> playerNames = playerList.Select(p => p.PlayerName).ToList();
            string message = string.Join(" ", playerNames);
            return Ok(new {message});
        }

        // ToDo: remove helper method
        [HttpGet("GetQuiz/{id}")]
        public async Task<IActionResult> GetAllQuiz([FromRoute] long id)
        {
            Host? claimHost = await _hostService.VerifyHost(HttpContext.User);
            if (claimHost == null)
            {
                return Unauthorized("The token could not be validated");
            }
            
            Quiz? quiz = await _quizService.GetQuizOrNull(id);
            if (quiz == null)
            {
                return NotFound("quiz not found");
            }

            QuizDto quizDto = new QuizDto(quiz);
            return Ok(new { quizDto });
        }
        
        [Authorize]
        [HttpGet("GetQuizzesWithRounds")]
        public async Task<IActionResult> GetAllQuiz()
        {
            Host? claimHost = await _hostService.VerifyHost(HttpContext.User);
            if (claimHost == null)
            {
                return Unauthorized("The token could not be validated");
            }

            List<QuizRoundDto> quizRoundDto = await GetQuizRoundDtos(claimHost);
            quizRoundDto.Sort((a, b) => a.QuizId < b.QuizId ? -1 : 1);
            return Ok(quizRoundDto);
        }

        private async Task<List<QuizRoundDto>> GetQuizRoundDtos(Host host)
        {
            List<Quiz> quizzes = await _quizService.GetAllQuizzesByHost(host);
            List<QuizRoundDto> result =  quizzes.Select(q => 
            {
                var quizRoundDto = new QuizRoundDto(q);
                quizRoundDto.Rounds = _roundService.GetAllRoundsByQuiz(q).Result.Select(r => new RoundDto(r)).ToList();
                quizRoundDto.Rounds.Sort((a, b) => a.Id < b.Id ? -1 : 1);
                return quizRoundDto;
            }).ToList();
            result.Sort((a, b) => a.QuizId < b.QuizId ? -1 : 1);
            return result;
        }
        
        private async Task<QuizRoundDto> GetQuizRoundDto(long quizId)
        {
            Quiz? quiz = await _quizService.GetQuizOrNull(quizId);
            if (quiz == null)
            {
                return null;
            }
            QuizRoundDto quizRoundDto = new QuizRoundDto(quiz);
            quizRoundDto.Rounds = _roundService.GetAllRoundsByQuiz(quiz).Result.Select(r => new RoundDto(r)).ToList();
            return quizRoundDto;
        }

        [Authorize]
        [HttpGet("GetRound/{id}")]
        public async Task<IActionResult> GetRound([FromRoute] long id)
        {
            Host? claimHost = await _hostService.VerifyHost(HttpContext.User);
            if (claimHost == null)
            {
                return Unauthorized("The token could not be validated");
            }
            Round? round = await _roundService.GetRoundOrNull(id);
            if (round == null)
            {
                return NotFound("round not found");
            }

            RoundDto roundDto = new RoundDto(round);
            return Ok(roundDto);
        }

        //Todo: Activate authorization
        [Authorize]
        [HttpPut("UpdateRound")]
        public async Task<IActionResult> ChangeRound(RoundDto roundDto)
        {
            Host? claimHost = await _hostService.VerifyHost(HttpContext.User);
            if (claimHost == null)
            {
                return Unauthorized("The token could not be validated");
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid round object");
            }
            
            Round? foundRound = await _roundService.GetRoundOrNull(roundDto.Id);
            if (foundRound == null)
            {
                return BadRequest("Round does not exist");
            }
            
            Round round = await _roundService.ChangeRound(roundDto, foundRound);
            RoundDto returnRoundDto = new RoundDto(round);
            
            return CreatedAtAction(nameof(ChangeRound), returnRoundDto);
        }
        
        //Todo: Activate authorization
        [Authorize]
        [HttpPut("UpdateQuiz")]
        public async Task<IActionResult> ChangeQuiz(QuizRoundDto quizRoundDto)
        {
            Host? claimHost = await _hostService.VerifyHost(HttpContext.User);
            if (claimHost == null)
            {
                return Unauthorized("The token could not be validated");
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid object");
            }
            
            Quiz? quiz = await _quizService.GetQuizOrNull(quizRoundDto.QuizId);
            if (quiz == null)
            {
                return NotFound("quiz not found");
            }
            
            quiz.Title = quizRoundDto.Title == "" ? quiz.Title : quizRoundDto.Title;
            _context.Update(quiz);
            List<long> currentRoundIds = _roundService.GetAllRoundsByQuiz(quizRoundDto.QuizId).Result.Select(r => r.Id).ToList();
            List<long> roundIds = quizRoundDto.Rounds.Select(r => r.Id).ToList();
            List<long> roundsToRemove = currentRoundIds.Except(roundIds).ToList();
            foreach (long roundId in roundsToRemove)
            {
                await _roundService.Delete(roundId);
            }

            foreach (RoundDto roundDto in quizRoundDto.Rounds)
            {
                Round? round = await _roundService.GetRoundOrNull(roundDto.Id);
                if (round == null || round.QuizId != quiz.Id)
                {
                    await _roundService.CreateRound(roundDto);
                    continue;
                } 
                await _roundService.ChangeRound(roundDto, round);
            }

            _context.SaveChanges();
            QuizRoundDto quizRoundDtoNew = await GetQuizRoundDto(quizRoundDto.QuizId);
            return CreatedAtAction(nameof(ChangeQuiz), quizRoundDtoNew);
        }

        
        // ToDo: remove helper method
        [HttpPut("CleanUp")]
        public async Task<IActionResult> CleanUp()
        {
            List<Answer> answers = await _answerService.GetAllAnswers();
            foreach (Answer answer in answers)
            {
                if (answer.Additions.Count == 0 && answer.Subtractions.Count == 0)
                {
                    _answerService.Delete(answer);
                }
            }
            
            // List<Round> rounds = await _roundService.GetAllRounds();
            // foreach (Round round in rounds)
            // {
            //     if (_answerService.GetAnswersByRoundId(round.QuizId).Result.Count == 0)
            //     {
            //         _roundService.Delete(round);
            //     }
            // }
            
            List<Quiz> quizzes = await _quizService.GetAllQuizzes();
            foreach (Quiz quiz in quizzes)
            {
                if (_roundService.GetAllRoundsByQuiz(quiz).Result.Count == 0)
                {
                    _quizService.Delete(quiz);
                }
            }
            
            List<Player> players = await _playerService.GetAllPlayers();
            foreach (Player player in players)
            {
                if (_answerService.GetAnswersByPlayer(player).Result.Count == 0)
                {
                    _playerService.Delete(player);
                }
            }
            
            List<Host> hosts = await _hostService.GetAllHosts();
            foreach (Host host in hosts)
            {
                if (_quizService.GetAllQuizzesByHost(host).Result.Count == 0)
                {
                    _hostService.Delete(host);
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        // ToDo: remove helper method
        [HttpGet("GetAnswer/{id}")]
        public async Task<IActionResult> GetAnswer([FromRoute] long id)
        {
            Answer? answer = await _answerService.GetAnswerOrNull(id);
            if (answer == null)
            {
                return NotFound("answer not found");
            }

            AnswerDto answerDto = new AnswerDto(answer);
            return Ok(new { answerDto });
        }
        
        // ToDo: remove helper method
        [HttpGet("GetAnswerByQuestion/{questionId}")]
        public async Task<IActionResult> GetAnswerByRoundId([FromRoute] long questionId)
        {
            List<Answer> answers = await _answerService.GetAnswersByRoundId(questionId);
            return Ok(new {answers});
        }
    }
}