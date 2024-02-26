using System.ComponentModel.DataAnnotations;

namespace Key_monitoring.Models;

public class KeyReservationModel
{
    [Key]
    public Guid id { get; set; }
    public RaspisanieModel pair { get; set; }
    public KeyModel key { get; set; }
    public UserModel user { get; set; }
}
