using Microsoft.CodeAnalysis;
using MapTrick.Abstractions;
using MapTrick.Descriptors.Mappings;
using MapTrick.Helpers;

namespace MapTrick.Descriptors.MappingBuilders;

public static class StringToEnumMappingBuilder
{
    public static TypeMapping? TryBuildMapping(MappingBuilderContext ctx)
    {
        if (!ctx.IsConversionEnabled(MappingConversionType.StringToEnum))
            return null;

        if (ctx.Source.SpecialType != SpecialType.System_String || !ctx.Target.IsEnum())
            return null;
        ///<summary>
        /// from string => use an optimized method of Enum.Parse which would use slow reflection
        /// however we currently don't support all features of Enum.Parse yet (ex. flags)
        /// therefore we use Enum.Parse as fallback.
        /// </summary>
        var members = ctx.Target.GetMembers().OfType<IFieldSymbol>();
        var config = ctx.GetConfigurationOrDefault<MapEnumAttribute>();
        return new EnumFromStringMapping(ctx.Source, ctx.Target, members, config.IgnoreCase);
    }
}
