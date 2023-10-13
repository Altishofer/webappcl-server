using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ToX.Models;
using Word2vec.Tools;


namespace ToX.Models;

[Index(nameof(WordOrNull), IsUnique = true)]
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
}
