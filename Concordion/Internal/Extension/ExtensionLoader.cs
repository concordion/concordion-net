using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Concordion.Api.Extension;
using Concordion.Internal.Util;

namespace Concordion.Internal.Extension
{
    public class ExtensionLoader
    {
        private SpecificationConfig Configuration { get; set; }

        public ExtensionLoader(SpecificationConfig configuration)
        {
            Configuration = configuration;
        }

        public void AddExtensions(object fixture, IConcordionExtender concordionExtender)
        {
            foreach (var concordionExtension in GetExtensionsFromConfiguration())
            {
                concordionExtension.AddTo(concordionExtender);
            }

            foreach (var concordionExtension in GetExtensionsForFixture(fixture))
            {
                concordionExtension.AddTo(concordionExtender);
            }
        }

        private IEnumerable<IConcordionExtension> GetExtensionsFromConfiguration()
        {
            if (Configuration == null) return Enumerable.Empty<IConcordionExtension>();

            var extensions = new List<IConcordionExtension>();
            foreach (var extension in Configuration.ConcordionExtensions)
            {
                var extensionTypeName = extension.Key;
                var extensionTypeFullyQualifiedName = extension.Value;
                //extensions.Add(CreateConcordionExtension(extensionTypeName, extensionAsseblyName));
		// Need to use FullyQualifiedName
		var extensionType = Type.GetType(extensionTypeFullyQualifiedName);
		extensions.Add(CreateConcordionExtension(extensionType));
            }
            return extensions;
        }

        private IEnumerable<IConcordionExtension> GetExtensionsForFixture(object fixture)
        {
            var extensions = new List<IConcordionExtension>();
            foreach (var fixtureType in GetClassHierarchyParentFirst(fixture.GetType()))
            {
                extensions.AddRange(GetExtensionsFromFieldAttributes(fixture, fixtureType));
                extensions.AddRange(GetExtensionsFromClassAttributes(fixtureType));
            }
            return extensions;
        }

        private IEnumerable<Type> GetClassHierarchyParentFirst(Type fixtureType)
        {
            var fixtureTypes = new List<Type>();

            var current = fixtureType;
            while (current != null && current != typeof(object))
            {
                fixtureTypes.Add(current);
                current = current.BaseType;
            }
            fixtureTypes.Reverse();

            return fixtureTypes;
        }

        private IEnumerable<IConcordionExtension> GetExtensionsFromFieldAttributes(object fixture, Type fixtureType)
        {
            var extensions = new List<IConcordionExtension>();
            FieldInfo[] fieldInfos = fixtureType.GetFields(BindingFlags.Public | BindingFlags.DeclaredOnly |
                                                            BindingFlags.Instance | BindingFlags.Static);
            foreach (var fieldInfo in fieldInfos)
            {
                if (HasAttribute(fieldInfo, typeof(ExtensionAttribute), false))
                {
                    var extension = fieldInfo.GetValue(fixture) as IConcordionExtension;
                    Check.NotNull(extension, string.Format("Extension field '{0}' must be non-null", fieldInfo.Name));
                    extensions.Add(extension);
                }
            }
            return extensions;
        }

        private IEnumerable<IConcordionExtension> GetExtensionsFromClassAttributes(Type fixtureType)
        {
            if (!HasAttribute(fixtureType, typeof(ExtensionsAttribute), false)) return Enumerable.Empty<IConcordionExtension>();

            var extensions = new List<IConcordionExtension>();
            foreach (var attribute in fixtureType.GetCustomAttributes(typeof(ExtensionsAttribute), false))
            {
                var extensionsAttribute = attribute as ExtensionsAttribute;
                foreach (var extensionType in extensionsAttribute.ExtensionTypes)
                {
                    var extensionTypeName = extensionType.FullName;
                    var extensionAssemblyName = extensionType.Assembly.GetName().Name;
                    //extensions.Add(CreateConcordionExtension(extensionTypeName, extensionAssemblyName));
                    extensions.Add(CreateConcordionExtension(extensionType));
                }
            }
            return extensions;
        }

        private static IConcordionExtension CreateConcordionExtension(Type type)
        {
            IConcordionExtension extension;
            var instance = Activator.CreateInstance(type);
            if (instance is IConcordionExtension)
            {
                extension = instance as IConcordionExtension;
            }
            else if(instance is IConcordionExtensionFactory)
            {
                var extensionFactory = instance as IConcordionExtensionFactory;
                extension = extensionFactory.CreateExtension();
            }
            else
            {
                throw new InvalidCastException(
                    string.Format("Extension {0} must implement {1} or {2}",
                                  type.FullName, typeof(IConcordionExtension), typeof(IConcordionExtensionFactory)));
            }
            return extension;
        }

        private bool HasAttribute(MemberInfo memberInfo, Type attributeType, bool inherit)
        {
            return memberInfo.GetCustomAttributes(attributeType, inherit).Any(attribute => attribute.GetType() == attributeType);
        }
    }
}
