using AutoMapper;
using MinAPIMusicProject.DTOs;
using MinAPIMusicProject.Models;

namespace MinAPIMusicProject;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Artist, ArtistDTO>().ReverseMap();
        CreateMap<Track, TrackDTO>().ReverseMap();
        CreateMap<Track, AddTrackDTO>().ReverseMap();
        CreateMap<Genre, GenreDTO>().ReverseMap();
    }
}