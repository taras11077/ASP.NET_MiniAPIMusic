using AutoMapper;
using MinAPIMusicProject.DTOs;
using MinAPIMusicProject.Models;
using MinAPIMusicProject.Requests;

namespace MinAPIMusicProject;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Artist, ArtistDTO>().ReverseMap();
        CreateMap<Track, TrackDTO>().ReverseMap();
        CreateMap<Track, AddTrackDTO>().ReverseMap();
        CreateMap<Genre, GenreDTO>().ReverseMap();
        CreateMap<Genre, AddGenreDTO>().ReverseMap();
        CreateMap<User, CreateUserRequest>().ReverseMap();
        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<Like, LikeDTO>().ReverseMap();
    }
}