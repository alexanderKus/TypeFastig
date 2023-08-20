using Application.Auth.Commands.Register;
using Application.Auth.Queries.Login;
using Application.Scores.Commands;
using AutoMapper;
using Domain.Models;
using Domain.Models.Auth;
using Domain.Models.Dtos;
using Domain.Models.Entities;

namespace Api.Common.AutoMapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<LoginRequest, LoginQuery>();
            CreateMap<RegisterRequest, RegisterCommand>();
            CreateMap<ScoreRequest, ScoreAddCommand>();
        }
    }
}

