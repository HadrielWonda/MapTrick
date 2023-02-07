using Microsoft.CodeAnalysis;
using MapTrick.Abstractions;
using MapTrick.Descriptors.Mappings;
using MapTrick.Helpers;

namespace MapTrick.Descriptors.MappingBuilders;

public static class ParseMappingBuilder
{
    private const string ParseMethodName = "Parse";

    public static StaticMethodMapping? TryBuildMapping(MappingBuilderContext ctx)
    {
        if (!ctx.IsConversionEnabled(MappingConversionType.ParseMethod))
            return null;

        if (ctx.Source.SpecialType != SpecialType.System_String)
            return null;

        var targetIsNullable = ctx.Target.NonNullable(out var nonNullableTarget);

        var parseMethodCandidates = nonNullableTarget.GetAllMembers(ParseMethodName)
            .OfType<IMethodSymbol>()
            .Where(m =>
                m.IsStatic
                && !m.ReturnsVoid
                && !m.IsAsync
                && m.Parameters.Length == 1
                && SymbolEqualityComparer.Default.Equals(m.Parameters[0].Type, ctx.Source))
            .ToList();

        // try to find parse method with equal nullability return type
        var parseMethod = parseMethodCandidates.FirstOrDefault(x => SymbolEqualityComparer.IncludeNullability.Equals(x.ReturnType, ctx.Target));
        if (parseMethod != null)
            return new StaticMethodMapping(parseMethod);

        if (!targetIsNullable)
            return null;

        // otherwise try to find parse method ignoring the nullability
        parseMethod = parseMethodCandidates.FirstOrDefault(x => SymbolEqualityComparer.Default.Equals(x.ReturnType, nonNullableTarget));
        return parseMethod == null
            ? null
            : new StaticMethodMapping(parseMethod);
    }
}
