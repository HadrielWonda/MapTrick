using Microsoft.CodeAnalysis;
using MapTrick.Abstractions;
using MapTrick.Descriptors.Mappings;
using MapTrick.Helpers;

namespace MapTrick.Descriptors.MappingBuilders;

public static class EnumToStringMappingBuilder
{
    public static TypeMapping? TryBuildMapping(MappingBuilderContext ctx)
    {
        if (!ctx.IsConversionEnabled(MappingConversionType.EnumToString))
            return null;

        if (ctx.Target.SpecialType != SpecialType.System_String || !ctx.Source.IsEnum())
            return null;

        // to string => use an optimized method of Enum.ToString which would use slow reflection
        // use Enum.ToString as fallback (for ex. for flags)
        return new EnumToStringMapping(ctx.Source, ctx.Target, ctx.Source.GetMembers().OfType<IFieldSymbol>());
    }
}
