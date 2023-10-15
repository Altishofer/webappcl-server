using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToX.Models;
using Word2vec.Tools;

public class Word2VectorService
{
    private readonly Vocabulary _vocabulary;
    private readonly ApplicationContext _context;
    private static Word2VectorService? _instance;
    private static readonly object _lock = new object();
    private readonly IConfiguration _config;

    public Word2VectorService(ApplicationContext applicationContext, IConfiguration config)
    {
        _context = applicationContext;
        _config = config;
        _vocabulary = new Word2VecBinaryReader().Read(Path.GetFullPath(_config["VECTOR_BIN"]));
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

    public void PrintModelInfo()
    {
        Console.WriteLine($"_vocabulary size: {_vocabulary.Words.Length}");
        Console.WriteLine($"w2v vector dimensions count: {_vocabulary.VectorDimensionsCount}");
    }
    
    public async Task<List<string>> FindClosestWordsAsyncSQL(string word, int count)
    {
        var sqlQuery = $"SELECT * FROM find_nearest_words('{word}', {count})";
        var result = new List<string>();
        {
            var nearestWords = _context.WordVector.FromSqlRaw(sqlQuery);;
            foreach (var neighbor in nearestWords)
            {
                result.Add(neighbor.WordOrNull);
            }
        }

        return result;
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
        var subtractionRepresentation = await Task.Run(() => _vocabulary[wordA].Substract(_vocabulary[wordB]));
        var closestSubtractions = await Task.Run(() => _vocabulary.Distance(subtractionRepresentation,1));
        return closestSubtractions[0].Representation.WordOrNull;
    }
}
