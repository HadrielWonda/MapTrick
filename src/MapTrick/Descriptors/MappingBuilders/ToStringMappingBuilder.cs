using Microsoft.CodeAnalysis;
using MapTrick.Abstractions;
using MapTrick.Descriptors.Mappings;

namespace MapTrick.Descriptors.MappingBuilders;

public static class ToStringMappingBuilder
{

    public static TypeMapping? TryBuildMapping(MappingBuilderContext ctx)
    {
        if (!ctx.IsConversionEnabled(MappingConversionType.ToStringMethod))
            return null;

        return ctx.Target.SpecialType == SpecialType.System_String
            ? new SourceObjectMethodMapping(ctx.Source, ctx.Target, nameof(ToString))
            : null;
    }
}
