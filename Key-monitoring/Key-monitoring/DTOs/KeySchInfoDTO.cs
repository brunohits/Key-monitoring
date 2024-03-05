using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Key_monitoring.Models;

public class KeySchInfoDTO
{
    public List<KeyFullModelDTO> mn { get; set; }
    public List<KeyFullModelDTO> tu { get; set; }
    public List<KeyFullModelDTO> we { get; set; }
    public List<KeyFullModelDTO> th { get; set; }
    public List<KeyFullModelDTO> fr { get; set; }
    public List<KeyFullModelDTO> st { get; set; }
}
