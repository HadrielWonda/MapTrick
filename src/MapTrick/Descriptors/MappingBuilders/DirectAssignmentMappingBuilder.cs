using Microsoft.CodeAnalysis;
using MapTrick.Descriptors.Mappings;
using MapTrick.Helpers;

namespace MapTrick.Descriptors.MappingBuilders;

public static class DirectAssignmentMappingBuilder
{
    public static TypeMapping? TryBuildMapping(MappingBuilderContext ctx)
    {
        return SymbolEqualityComparer.IncludeNullability.Equals(ctx.Source, ctx.Target)
            && (!ctx.MapperConfiguration.UseDeepCloning || ctx.Source.IsImmutable())
            ? new DirectAssignmentMapping(ctx.Source)
            : null;
    }
}
