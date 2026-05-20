using SchoolAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs.People;

public record StudentCreateDto(
[Required, MinLength(1)] string FullName,
[Required, MinLength(1)] string LatinName,
[Required] string Contact,
[Required] string Email,
[Required] string Password,
GenderType Gender,
DateTime DateOfBirth,
string? PlaceOfBirth,
string? BackgroundStudy,
string? FatherName,
string? MotherName,
string? Address
);
