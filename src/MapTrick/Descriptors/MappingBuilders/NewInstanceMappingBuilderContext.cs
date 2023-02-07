using MapTrick.Descriptors.Mappings;
using MapTrick.Descriptors.Mappings.PropertyMappings;

namespace MapTrick.Descriptors.MappingBuilders;

public class NewInstanceMappingBuilderContext : ObjectPropertyMappingBuilderContext<NewInstanceObjectPropertyMapping>
{
    public NewInstanceMappingBuilderContext(MappingBuilderContext builderContext, NewInstanceObjectPropertyMapping mapping)
     : base(builderContext, mapping)
    {
    }

    public void AddInitPropertyMapping(PropertyAssignmentMapping mapping)
    {
        SetSourcePropertyMapped(mapping.SourcePath);
        Mapping.AddInitPropertyMapping(mapping);
    }

    public void AddConstructorParameterMapping(ConstructorParameterMapping mapping)
    {
        PropertyConfigsByRootTargetName.Remove(mapping.Parameter.Name);

        SetSourcePropertyMapped(mapping.DelegateMapping.SourcePath);
        Mapping.AddConstructorParameterMapping(mapping);
    }
}
