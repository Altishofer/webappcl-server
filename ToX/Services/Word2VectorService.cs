using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ToX.DTOs.VectorDto;
using ToX.Models;
using Word2vec.Tools;

public class Word2VectorService
{
    private readonly Vocabulary _vocabulary;
    private readonly ApplicationContext _context;
    private static Word2VectorService? _instance;
    private static readonly object _lock = new object();
    private readonly IConfiguration _config;
    private readonly Representation _NullVector;

    public Word2VectorService(ApplicationContext applicationContext, IConfiguration config)
    {
        _context = applicationContext;
        _config = config;
        _vocabulary = new Word2VecBinaryReader().Read(Path.GetFullPath(_config["VECTOR_BIN"]));
        _NullVector = new Representation("null", new float[300]);
    }
    
    public static Word2VectorService GetInstance(ApplicationContext applicationContext, IConfiguration config)
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new Word2VectorService(applicationContext, config);
                }
            }
        }
        return _instance;
    }
    
    public async Task<double> FindDistance(string word, Representation vector)
    {
        Representation vectorB = await _getWordOrNullVector(word);
        return vectorB.GetCosineDistanceTo(vector).DistanceValue;
    }
    
    public async Task<List<string>> FindClosestWordsAsync(string word, int count)
    {
        Console.WriteLine($"top {count} closest to '{word}' words:");
        var closest = await Task.Run(() => _vocabulary.Distance(word, count));
        var result = new List<string>();
        foreach (var neighbor in closest)
        {
            result.Add(neighbor.Representation.WordOrNull);
        }
        return result;
    }
    
    public async Task<float[]> FindClosestVectorAsync(string word)
    {
        Representation vector = await _getWordOrNullVector(word);
        DistanceTo[] closest = await Task.Run(() => _vocabulary.Distance(word, 1));
        return closest[0].Representation.NumericVector;
    }

    public async Task<List<WordVector>> AnalogyAsync(string wordA, string wordB, string wordC, int count)
    {
        Console.WriteLine($"'{wordA}' relates to '{wordB}' as '{wordC}' relates to ...");
        var analogies = await Task.Run(() => _vocabulary.Analogy(wordA, wordB, wordC, count));
        var result = new List<WordVector>();
        foreach (var neighbor in analogies)
        {
            result.Add(new WordVector(neighbor.Representation.WordOrNull, neighbor.Representation.NumericVector));
        }
        return result;
    }

    public async Task<string> WordAdditionAsync(string wordA, string wordB)
    {
        Console.WriteLine($"'{wordA}' + '{wordB}' = ...");
        var additionRepresentation = await Task.Run(() => _vocabulary[wordA].Add(_vocabulary[wordB]));
        var closestAdditions = await Task.Run(() => _vocabulary.Distance(additionRepresentation, 1));
        return closestAdditions[0].Representation.WordOrNull;
    }

    public async Task<string> WordSubtractionAsync(string wordA, string wordB)
    {
        Console.WriteLine($"'{wordA}' - '{wordB}' = ...");
        Representation subtractionRepresentation = await Task.Run(() => _vocabulary[wordA].Substract(_vocabulary[wordB]));
        DistanceTo[] closestSubtractions = await Task.Run(() => _vocabulary.Distance(subtractionRepresentation,1));
        return closestSubtractions[0].Representation.WordOrNull;
    }

    public async Task<List<string>> WordCalculation(List<string> addList, List<string> subList)
    {
        Representation resultVector = _NullVector;
        if (!addList.IsNullOrEmpty())
        {
            foreach (string word in addList)
            {
                resultVector = await _addWordToVector(word, resultVector);
            }
        }
        
        if (!subList.IsNullOrEmpty())
        {
            foreach (string word in subList)
            {
                resultVector = await _subtWordFromVector(word, resultVector);
            }
        }

        return _vocabulary
            .Distance(resultVector, 3)
            .Select(w => w.Representation.WordOrNull)
            //.Where(w => !addList.Any(e => e == w) && !subList.Any(e => e == w))
            .ToList();
    }

    private async Task<Representation> _getWordOrNullVector(string word)
    {
        string[] wordForms = new string[]
        {
            word, 
            word.ToLower(),
            char.ToUpper(word[0]) + word.Substring(1), 
            word + "s",
            word + "es",
            word.ToLower() + "s",
            char.ToUpper(word[0]) + word.Substring(1) + "es",
            word.Substring(0, word.Length-1),
            char.ToUpper(word[0]) + word.Substring(0, word.Length-1)
        };
    
        foreach (string form in wordForms)
        {
            Representation representation = _vocabulary.GetRepresentationOrNullFor(form);
            if (representation != null)
            {
                return representation;
            }
        }
        return new Representation("null", new float[300]);
    }
    
    private async Task<Representation> _addWordToVector(string word, Representation vectorB)
    {
        Representation? vectorA = await _getWordOrNullVector(word);
        if (vectorA == _NullVector)
        {
            return vectorB;
        }

        return vectorB.Add(vectorA);
    }
    
    private async Task<Representation> _subtWordFromVector(string word, Representation vectorB)
    {
        Representation vectorA = await _getWordOrNullVector(word);
        if (vectorA == _NullVector)
        {
            return vectorB;
        }

        return vectorB.Substract(vectorA);
    }
}
