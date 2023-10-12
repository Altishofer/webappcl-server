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
        PushAllWordVectorsToDatabase();
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
    
}
