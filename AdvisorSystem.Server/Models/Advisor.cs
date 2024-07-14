using System.ComponentModel.DataAnnotations;

public class Advisor
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Name { get; set; }

    [Required]
    [StringLength(9, ErrorMessage = "SIN should have a fixed length of 9 characters.", MinimumLength = 9)]
    public string SIN { get; set; }

    [StringLength(255)]
    public string? Address { get; set; }

    [StringLength(8, ErrorMessage = "Phone should have a fixed length of 8 characters.", MinimumLength = 8)]
    public string? Phone { get; set; }

    public string? HealthStatus { get; set; }


    private static readonly Random _random = new Random();
    public static string GenerateHealthStatus()
    {
        int randomValue = _random.Next(0, 100);
        if (randomValue < 60) return "Green";
        else if (randomValue < 80) return "Yellow";
        else return "Red";
    }
}