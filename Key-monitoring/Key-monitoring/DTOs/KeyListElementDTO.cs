using Key_monitoring.Models;
using System.ComponentModel.DataAnnotations;

namespace Key_monitoring.DTOs;

public class KeyListElementDTO
{
    public Guid Id { get; set; }
    public int CabinetNumber { get; set; }
    public string? CabinetName { get; set; }
    public string KeyStatus { get; set; }
    public Guid? OwnerId { get; set; }
    public string? OwnerName { get; set; }
}