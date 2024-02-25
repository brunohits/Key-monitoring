using System.ComponentModel.DataAnnotations;

namespace Key_monitoring.Models;

public class KeyReservationModel
{
    public RaspisanieModel pair { get; set; }
    public KeyModel key { get; set; }
}
