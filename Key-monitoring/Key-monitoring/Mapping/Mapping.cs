using System;
using AutoMapper;
using Key_monitoring.DTOs;
using Key_monitoring.Models;
namespace Key_monitoring.Mapping;

public class Mapping : Profile
{
    public Mapping()
    {
        CreateMap<UserRegisterDTO, UserModel>();
    }
}

