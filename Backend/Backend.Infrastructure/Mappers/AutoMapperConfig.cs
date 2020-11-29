using AutoMapper;
using Backend.Core.Domain;
using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend.Infrastructure.Mappers
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>()
                   .ForMember(dest => dest.Role, opt => opt.MapFrom(x => x.Role.Name))
                   .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(x => x.CreatedAt.ToShortDateString()));
                cfg.CreateMap<User, UserDetailsDto>()
                   .ForMember(dest => dest.Role, opt => opt.MapFrom(x => x.Role.Name))
                   .ForMember(dest => dest.Products, opt => opt.MapFrom(x => x.Products
                   .Select(y => new UserProduct
                   {
                       Id = y.Id,
                       Product = y.Product,
                       Weight = y.Weight,
                       Calories = y.Calories,
                       Proteins = y.Proteins,
                       Carbohydrates = y.Carbohydrates,
                       Fats = y.Fats,                
                       AddedAt = y.AddedAt
                   }).ToList()))
                   .ForMember(dest => dest.Exercises, opt => opt.MapFrom(x => x.Exercises
                   .Select(y => new UserExercise
                   {
                       Id = y.Id,
                       Exercise = y.Exercise,
                       Weight = y.Weight,
                       NumberOfSets = y.NumberOfSets,
                       NumberOfReps = y.NumberOfReps,
                       Day = y.Day
                   }).ToList()));
                cfg.CreateMap<Product, ProductDto>();
                cfg.CreateMap<UserProduct, UserProductDto>()
                    .ForMember(dest => dest.AddedAt, opt => opt.MapFrom(x => x.AddedAt.ToShortDateString()));
                cfg.CreateMap<UserProduct, UserProductDetailsDto>();
                cfg.CreateMap<Exercise, ExerciseDto>();                 
                cfg.CreateMap<UserExercise, UserExerciseDto>()
                   .ForMember(dest => dest.Day, opt => opt.MapFrom(x => x.Day.Name))
                   .ForMember(dest => dest.AddedAt, opt => opt.MapFrom(x => x.AddedAt.ToShortDateString()));
                cfg.CreateMap<DietGoals, DietGoalsDto>();
            })
            .CreateMapper();
    }
}
