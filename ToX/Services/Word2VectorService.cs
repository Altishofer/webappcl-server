using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToX.Models;
using Word2vec.Tools;

public class Word2VectorService
{
    private readonly Vocabulary _vocabulary;

    public Word2VectorService()
    {
        _vocabulary = new Word2VecBinaryReader().Read(Path.GetFullPath("./GoogleNews-vectors-negative300.bin"));
    }

    public void PrintModelInfo()
    {
        Console.WriteLine($"_vocabulary size: {_vocabulary.Words.Length}");
        Console.WriteLine($"w2v vector dimensions count: {_vocabulary.VectorDimensionsCount}");
    }
    
    public async Task<List<WordVector>> FindClosestWordsAsync(string word, int count)
    {
        Console.WriteLine($"top {count} closest to '{word}' words:");
        var closest = await Task.Run(() => _vocabulary.Distance(word, count));
        var result = new List<WordVector>();
        foreach (var neighbor in closest)
        {
            result.Add(new WordVector(neighbor.Representation.WordOrNull, neighbor.Representation.NumericVector));
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

    public async Task<List<WordVector>> WordAdditionAsync(string wordA, string wordB, int count)
    {
        Console.WriteLine($"'{wordA}' + '{wordB}' = ...");
        var additionRepresentation = await Task.Run(() => _vocabulary[wordA].Add(_vocabulary[wordB]));
        var closestAdditions = await Task.Run(() => _vocabulary.Distance(additionRepresentation, count));
        var result = new List<WordVector>();
        foreach (var neighbor in closestAdditions)
        {
            result.Add(new WordVector(neighbor.Representation.WordOrNull, neighbor.Representation.NumericVector));
        }
        return result;
    }

    public async Task<List<WordVector>> WordSubtractionAsync(string wordA, string wordB, int count)
    {
        Console.WriteLine($"'{wordA}' - '{wordB}' = ...");
        var subtractionRepresentation = await Task.Run(() => _vocabulary[wordA].Substract(_vocabulary[wordB]));
        var closestSubtractions = await Task.Run(() => _vocabulary.Distance(subtractionRepresentation, count));
        var result = new List<WordVector>();
        foreach (var neighbor in closestSubtractions)
        {
            result.Add(new WordVector(neighbor.Representation.WordOrNull, neighbor.Representation.NumericVector));
        }
        return result;
    }
}
