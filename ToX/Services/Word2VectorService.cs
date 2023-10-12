using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToX.Models;
using Word2vec.Tools;

public class Word2VectorService
{
    private readonly Vocabulary _vocabulary;
    private readonly ApplicationContext _context;

    public Word2VectorService(ApplicationContext applicationContext)
    {
        _vocabulary = new Word2VecBinaryReader().Read(Path.GetFullPath("./GoogleNews-vectors-negative300.bin"));
        _context = applicationContext;
    }

    public void PrintModelInfo()
    {
        Console.WriteLine($"_vocabulary size: {_vocabulary.Words.Length}");
        Console.WriteLine($"w2v vector dimensions count: {_vocabulary.VectorDimensionsCount}");
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
