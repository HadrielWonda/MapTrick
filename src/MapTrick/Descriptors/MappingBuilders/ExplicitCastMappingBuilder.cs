using Microsoft.CodeAnalysis.CSharp;
using MapTrick.Abstractions;
using MapTrick.Descriptors.Mappings;
using MapTrick.Helpers;

namespace MapTrick.Descriptors.MappingBuilders;

public static class ExplicitCastMappingBuilder
{
    public static CastMapping? TryBuildMapping(MappingBuilderContext ctx)
    {
        if (!ctx.IsConversionEnabled(MappingConversionType.ExplicitCast))
            return null;

        if (ctx.MapperConfiguration.UseDeepCloning && !ctx.Source.IsImmutable() && !ctx.Target.IsImmutable())
            return null;

        var conversion = ctx.Compilation.ClassifyConversion(ctx.Source, ctx.Target);
       ///<summary>
       ///  Only allow user defined explicit reference conversions
        /// since other may return an extra runtime type check or may throw InvalidCastException.
        /// see c# language specification section 11.0.1 explicit reference conversions
        /// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/conversions#1035-explicit-reference-conversions
        /// </summary>
        return conversion.IsExplicit && (!conversion.IsReference || conversion.IsUserDefined)
            ? new CastMapping(ctx.Source, ctx.Target)
            : null;
    }
}
