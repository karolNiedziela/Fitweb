using AutoMapper;
using Backend.Core.Helpers;
using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Mappers
{
    public class PagedListToPageResultDtoConverter<TSource, TDestination> : ITypeConverter<PagedList<TSource>, PageResultDto<TDestination>>
    {
        public PageResultDto<TDestination> Convert(PagedList<TSource> source, 
            PageResultDto<TDestination> destination, 
            ResolutionContext context)
        {
            var itemMapping = context.Mapper.Map<IEnumerable<TDestination>>(source.Items);

            var pageResultDto = new PageResultDto<TDestination>
            {
                TotalItems = source.TotalItems,
                Items = itemMapping.ToList(),
                HasNext = source.HasNext,
                HasPrevious = source.HasPrevious,
                PageNumber = source.PageNumber,
                PageSize = source.PageSize,
                TotalPages = source.TotalPages
            };

            return pageResultDto;
        }
    }
}
