using Key_monitoring.Enum;

namespace Key_monitoring.Interfaces;

public interface IDeanOffice
{
    Task GiveRole(Guid id, RoleEnum roleEnum, Guid IdUser);
}