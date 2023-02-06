using MapTrick.Descriptors.Mappings;
using MapTrick.Descriptors.Mappings.ExistingTarget;
using MapTrick.Diagnostics;
using MapTrick.Helpers;

namespace MapTrick.Descriptors.MappingBodyBuilders;

public static class UserMethodMappingBodyBuilder
{
    public static void BuildMappingBody(MappingBuilderContext ctx, UserDefinedExistingTargetMethodMapping mapping)
    {
        // UserDefinedExistingTargetMethodMapping handles null already
        var delegateMapping = ctx.FindOrBuildExistingTargetMapping(
            mapping.SourceType.NonNullable(),
            mapping.TargetType.NonNullable());

        if (delegateMapping != null)
        {
            mapping.SetDelegateMapping(delegateMapping);
            return;
        }

        ctx.ReportDiagnostic(
            DiagnosticDescriptors.CouldNotCreateMapping,
            mapping.SourceType,
            mapping.TargetType);
    }

    public static void BuildMappingBody(MappingBuilderContext ctx, UserDefinedNewInstanceMethodMapping mapping)
    {
        var delegateMapping = mapping.CallableByOtherMappings
            ? ctx.BuildDelegateMapping(mapping.SourceType, mapping.TargetType)
            : ctx.BuildMappingWithUserSymbol(mapping.SourceType, mapping.TargetType);

        if (delegateMapping != null)
        {
            mapping.SetDelegateMapping(delegateMapping);
            return;
        }

        ctx.ReportDiagnostic(
            DiagnosticDescriptors.CouldNotCreateMapping,
            mapping.SourceType,
            mapping.TargetType);
    }
}
