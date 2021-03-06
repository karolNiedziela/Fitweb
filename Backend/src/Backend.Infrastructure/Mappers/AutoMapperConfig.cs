﻿using AutoMapper;
using Backend.Core.Entities;
using Backend.Core.Helpers;
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
                   .ForMember(dest => dest.Role, opt => opt.MapFrom(u => u.UserRoles.Select(ur => ur.Role.Name).FirstOrDefault()));
                cfg.CreateMap<Athlete, AthleteDto>()
                   .ForMember(dest => dest.Products, opt => opt.MapFrom(a => a.AthleteProducts.Select(
                     ap => new AthleteProduct
                     {
                         Id = ap.Id,
                         Product = new Product
                         {
                             Id = ap.Product.Id,
                             Name = ap.Product.Name,
                             Calories = ap.Product.Calories,
                             Proteins = ap.Product.Proteins,
                             Carbohydrates = ap.Product.Carbohydrates,
                             Fats = ap.Product.Fats,
                             CategoryOfProduct = ap.Product.CategoryOfProduct
                         },
                         Weight = ap.Weight,
                         DateCreated = ap.DateCreated
                     }).ToList()))
                   .ForMember(dest => dest.Exercises, opt => opt.MapFrom(x => x.AthleteExercises.Select(
                    ae => new AthleteExercise
                    {
                        Exercise = new Exercise
                        {
                            Id = ae.Exercise.Id,
                            PartOfBody = ae.Exercise.PartOfBody,
                            Name = ae.Exercise.Name
                        },
                        Weight = ae.Weight,
                        NumberOfSets = ae.NumberOfSets,
                        NumberOfReps = ae.NumberOfReps,
                        Day = ae.Day,
                    }).ToList()))
                   .ForMember(dest => dest.DietStats, opt => opt.MapFrom(x => x.AthleteDietStats.Select(
                       ads => new AthleteDietStats
                       {
                           DietStat = new DietStat
                           {
                               Id = ads.Id,
                               TotalCalories = Math.Round(ads.DietStat.TotalCalories, 2),
                               TotalProteins = Math.Round(ads.DietStat.TotalProteins, 2),
                               TotalCarbohydrates = Math.Round(ads.DietStat.TotalCarbohydrates, 2),
                               TotalFats = Math.Round(ads.DietStat.TotalFats, 2),
                               DateCreated = ads.DietStat.DateCreated
                           },

                       }).ToList()));
                cfg.CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(p => p.CategoryOfProduct.Name.ToString().Replace("_", " ")));
                cfg.CreateMap<AthleteProduct, AthleteProductDto>().ForMember(dest => dest.DateCreated, opt => opt.MapFrom(ap => ap.DateCreated.ToShortDateString()))
                .ForMember(dest => dest.DateUpdated, opt => opt.MapFrom(ap => ap.DateUpdated.ToShortDateString()));
                cfg.CreateMap<Exercise, ExerciseDto>()
                   .ForMember(dest => dest.PartOfBody, opt => opt.MapFrom(e => e.PartOfBody.Name));
                cfg.CreateMap<AthleteExercise, AthleteExerciseDto>()
                   .ForMember(dest => dest.Day, opt => opt.MapFrom(ae => ae.Day.Name));
                cfg.CreateMap<CaloricDemand, CaloricDemandDto>();
                cfg.CreateMap(typeof(PagedList<>), typeof(PageResultDto<>)).ConvertUsing(typeof(PagedListToPageResultDtoConverter<,>));
                cfg.CreateMap<DietStat, DietStatDto>();
                cfg.CreateMap<AthleteDietStats, AthleteDietStatsDto>();
            })
            .CreateMapper();
    }
}
