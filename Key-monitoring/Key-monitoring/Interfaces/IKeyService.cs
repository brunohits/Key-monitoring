using Key_monitoring.DTOs;
using Key_monitoring.Enum;
using Key_monitoring.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Key_monitoring.Interfaces;

public interface IKeyService
{ 
    Task<Exception?> CreateKey(KeyCreateDTO newKey);

    Task<KeyListDTO> GetList(int page, int size);

    Task<KeySchInfoDTO> GetKeyInfo(Guid id, DateTime start);

    Task<bool> ChangeKeyStatus(Guid keyId, Guid? userId);
    Task<KeyDayInfoDTO> GetKeyDayInfo(GetKeyDayInfoDTO data);
}