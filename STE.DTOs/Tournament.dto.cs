using System.ComponentModel.DataAnnotations;

namespace smarttournamentengine.STE.DTOs;

public record class TournamentDTO(
    [Required][StringLength(30)] string Name
);