﻿using AutoMapper;
using WOF.Application.Common.Mappings;
using WOF.Domain.Entities;

namespace WOF.Application.CompletedOrders.Queries.GetCompletedOrders;

public class ProductDto : IMapFrom<Product>
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string WalmartId { get; set; }

    public int UnitType { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Product, ProductDto>()
            .ForMember(d => d.UnitType, opt => opt.MapFrom(s => (int)s.UnitType));
    }
}