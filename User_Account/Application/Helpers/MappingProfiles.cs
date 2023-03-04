using Application.DTOs;
using AutoMapper;
using Domain;

namespace Application.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<AppUser, LoginDto>();
        CreateMap<AppUser, UserDto>();
    }
}