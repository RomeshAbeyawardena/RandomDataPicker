﻿using AutoMapper;

namespace RandomDataPicker.Web.Features.Entry;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Models.Entry, Persistence.Models.Entry>()
            .ForMember(m => m.Id, opt => opt.Ignore());
    }
}
