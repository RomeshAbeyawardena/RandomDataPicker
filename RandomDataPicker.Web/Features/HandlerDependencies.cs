using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using RandomDataPicker.Contracts;

namespace RandomDataPicker.Web.Features;

public class HandlerDependencies
{
    public IMapper? Mapper { get; set; }
    public IDistributedCache? Cache { get; set; }
    public IEntryProvider? EntryProvider { get; set; }
    public IEntryInjector? EntryInjector { get; set; }
    public IEntryPicker? EntryPicker { get; set; }
    public Random? Random { get; set; }
}
