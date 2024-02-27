using Key_monitoring.Enum;
using Key_monitoring.Models;
using System.ComponentModel.DataAnnotations;

namespace Key_monitoring.DTOs;

public class ApplicationsListElementDTO
{
    public required Guid Id { get; set; }
    public required DateTime date { get; set; }
    public required Guid OwnerId { get; set; }
    public required string OwnerName { get; set; }
    public required RoleEnum OwnerRole { get; set; }
    public required Guid KeyId { get; set; }
    public required int KeyNumber { get; set; }
    public required bool Repetitive { get; set; }
    public required DateTime PairStart { get; set; }
    public required ApplicationStatusEnum Status { get; set; }
}
