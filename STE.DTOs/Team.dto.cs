using System.ComponentModel.DataAnnotations;

namespace smarttournamentengine.STE.DTOs;
public record class TeamDTO(
    [Required][StringLength(30)] string TeamName
);