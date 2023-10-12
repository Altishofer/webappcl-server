using Microsoft.EntityFrameworkCore;
using ToX.Models;

namespace ToX.Services;
using System.Collections.Generic;
using System.Linq;
using Word2vec.Tools;

public class WordVectorService
{
    private readonly ApplicationContext _dbContext;

    public WordVectorService(ApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void PushAllWordVectorsToDatabase()
    {
        var vocabulary = new Word2VecBinaryReader().Read(Path.GetFullPath("./GoogleNews-vectors-negative300.bin"));
        Console.WriteLine($"vocabulary size: {vocabulary.Words.Length}");
        Console.WriteLine($"w2v vector dimensions count: {vocabulary.VectorDimensionsCount}");
        
        foreach (var word in vocabulary.Words)
        {
            var existingWordVector = _dbContext.WordVector.SingleOrDefault(wv => wv.WordOrNull == word.WordOrNull);

            if (existingWordVector == null)
            {
                var newWordVector = new WordVector(word.WordOrNull, word.NumericVector);
                _dbContext.WordVector.Add(newWordVector);
            }
        }
        _dbContext.SaveChanges();
    }
    
    public async Task<WordVector> FindNearestVector(string word)
    {
        var target = _dbContext.WordVector.SingleOrDefault(wv => wv.WordOrNull == word);
        var nearestVector = await _dbContext.WordVector
            .FromSqlRaw("SELECT * FROM public.wordvector ORDER BY calculate_euclidean_distance(\"NumericVector\", {0}) LIMIT 1", target.NumericVector)
            .FirstOrDefaultAsync();

        return nearestVector;
    }
}
