using System;
using System.ComponentModel.DataAnnotations;
using Word2vec.Tools;

namespace ToX.Models
{
    public class WordVector
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string WordOrNull { get; set; }

        [Required]
        public float[] NumericVector { get; set; }

        [Required]
        public double MetricLength { get; set; }

        public WordVector(){}

        public WordVector(string word, float[] vector)
        {
            WordOrNull = word;
            NumericVector = vector;
            MetricLength = Math.Sqrt(vector.Sum(v => v * v));
        }

        public DistanceTo GetCosineDistanceTo(WordVector wordVector)
        {
            double distance = 0;
            for (var i = 0; i < NumericVector.Length; i++)
                distance += NumericVector[i] * wordVector.NumericVector[i];
            Representation representation = new Representation(wordVector.WordOrNull, wordVector.NumericVector);
            return new DistanceTo(representation, distance / (MetricLength * representation.MetricLength));
        }

        public WordVector Subtract(WordVector representation)
        {
            var ans = new float[NumericVector.Length];
            for (int i = 0; i < NumericVector.Length; i++)
                ans[i] = NumericVector[i] - representation.NumericVector[i];

            return new WordVector(null, ans);
        }

        public WordVector Add(WordVector representation)
        {
            var ans = new float[NumericVector.Length];

            for (int i = 0; i < NumericVector.Length; i++)
                ans[i] = NumericVector[i] + representation.NumericVector[i];

            return new WordVector(null, ans);
        }

        public DistanceTo[] GetClosestFrom(IEnumerable<WordVector> representations, int maxCount)
        {
            return representations.Select(GetCosineDistanceTo)
                .OrderByDescending(s => s.DistanceValue)
                .Take(maxCount)
                .ToArray();
        }

        public LinkedDistance<T>[] GetClosestFrom<T>(IEnumerable<T> representationsWrappers, Func<T, WordVector> locator, int maxCount)
        {
            return representationsWrappers.Select(r =>
                    new LinkedDistance<T>(r, GetCosineDistanceTo(locator(r))))
                .OrderByDescending(s => s.Distance.DistanceValue)
                .Take(maxCount)
                .ToArray();
        }
    }
}
