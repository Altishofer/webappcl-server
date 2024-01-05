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
    
    public async Task<bool> IsValidWord(string word)
    {
        return _getWordOrNullVector(word.ToLower()).Result != _NullVector;
    }
    
    public async Task<double> FindDistance(string word, Representation vector)
    {
        Representation vectorB = await _getWordOrNullVector(word);
        if (vectorB == _NullVector)
        {
            return -1;
        }
        return vectorB.GetCosineDistanceTo(vector).DistanceValue;
    }
    
    public async Task<float[]> FindClosestVectorAsync(string word)
    {
        Representation vector = await _getWordOrNullVector(word);
        DistanceTo[] closest = await Task.Run(() => _vocabulary.Distance(word, 1));
        if (closest.Length == 0)
        {
            return vector.NumericVector;
        }
        return closest[0].Representation.NumericVector;
    }

    public async Task<List<string>> WordCalculation(List<string> addList, List<string> subList)
    {
        Representation resultVector = _NullVector;
        if (!addList.IsNullOrEmpty())
        {
            foreach (string word in addList)
            {
                resultVector = await _addWordToVector(word, resultVector);
                // Console.WriteLine(_vocabulary.Distance(resultVector, 1).ToList()[0].Representation.WordOrNull);
            }
        }
        
        if (!subList.IsNullOrEmpty())
        {
            foreach (string word in subList)
            {
                resultVector = await _subtWordFromVector(word, resultVector);
                // Console.WriteLine(_vocabulary.Distance(resultVector, 1).ToList()[0].Representation.WordOrNull);
            }
        }
        
        if (resultVector == _NullVector)
        {
            return new List<string>();
        }

        return _vocabulary
            .Distance(resultVector, 10)
            .Select(w => w.Representation.WordOrNull)
            .Where(w => !addList.Any(e => e == w) && !subList.Any(e => e == w))
            .ToList();
    }

    private async Task<Representation> _getWordOrNullVector(string word)
    {
        if (word.IsNullOrEmpty())
        {
            return _NullVector;
        }
        
        List<string> wordForms = new List<string>();
        wordForms.Add(word.ToLower());
        if (word.Length > 1)
        {
            wordForms.Add(char.ToUpper(word[0]) + word.Substring(1));
        }

        if (word.EndsWith("es"))
        {
            wordForms.Add(word.Substring(0, word.Length - 2));
            wordForms.Add(char.ToUpper(word[0]) + word.Substring(1, word.Length - 2));
        }
        if (word.EndsWith("s"))
        {
            wordForms.Add(word.Substring(0, word.Length - 1));
            wordForms.Add(word.Substring(0, word.Length - 1));
        }
        if (!word.EndsWith("s") && word.Length > 1)
        {
            wordForms.Add(word + "es");
            wordForms.Add(word + "s");
            wordForms.Add(char.ToUpper(word[0]) + word.Substring(1) + "s");
            wordForms.Add(char.ToUpper(word[0]) + word.Substring(1) + "es");
        }
    
        foreach (string form in wordForms)
        {
            Representation representation = _vocabulary.GetRepresentationOrNullFor(form);
            if (representation != null)
            {
                return representation;
            }
        }
        return _NullVector;
    }
    
    private async Task<Representation> _addWordToVector(string word, Representation vectorB)
    {
        Representation vectorA = await _getWordOrNullVector(word);
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
