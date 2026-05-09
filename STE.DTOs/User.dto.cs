using System.ComponentModel.DataAnnotations;

namespace smarttournamentengine.STE.DTOs;

public record class UserDTO(
    [Required][StringLength(30)] string Email,
    [Required][StringLength(30)] string Password
);