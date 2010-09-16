using System;
using System.Collections.Generic;
using System.Reflection;
using Gallio.ConcordionAdapter.Properties;
using Concordion.Integration;
using Concordion.Api;
using System.IO;
using Concordion.Internal;
using Gallio.Model;
using Gallio.Common.Reflection;
using Gallio.Model.Helpers;
using Gallio.Model.Tree;


namespace Gallio.ConcordionAdapter.Model
{
    /// <summary>
    /// Explores an assembly for Concordion tests
    /// </summary>
    internal class ConcordionTestExplorer : TestExplorer
    {

        internal const string AssemblyKind = "Concordion Assembly";

        private static readonly string CONCORDION_ASSEMBLY_DISPLAY_NAME = @"Concordion";
        private readonly Dictionary<IAssemblyInfo, Test> assemblyTests;
        private readonly Dictionary<ITypeInfo, Test> typeTests;
        private DirectoryInfo _baseOutputDirectory;

        public ConcordionTestExplorer()
        {
            assemblyTests = new Dictionary<IAssemblyInfo, Test>();
            typeTests = new Dictionary<ITypeInfo, Test>();
        }

        #region Override Methods

        protected override void ExploreImpl(IReflectionPolicy reflectionPolicy, ICodeElementInfo codeElement)
        {

            IAssemblyInfo assembly = ReflectionUtils.GetAssembly(codeElement);
            Version frameworkVersion = GetFrameworkVersion(assembly);
            if (frameworkVersion != null)
            {
                ITypeInfo type = ReflectionUtils.GetType(codeElement);
                Test assemblyTest = GetAssemblyTest(assembly, TestModel.RootTest, frameworkVersion, type == null);
                if (type != null)
                {
                    TryGetTypeTest(type, assemblyTest);
                }

            }
        }

        #endregion

        private static Version GetFrameworkVersion(IAssemblyInfo assembly)
        {
            if (IsConcordionAttributePresent(assembly))
            {
                AssemblyName frameworkAssemblyName = ReflectionUtils.FindAssemblyReference(assembly, CONCORDION_ASSEMBLY_DISPLAY_NAME);
                return frameworkAssemblyName != null ? frameworkAssemblyName.Version : null;
            }

            return null;
        }

        private static bool IsConcordionAttributePresent(IAssemblyInfo assembly)
        {
            foreach (var assemblyAttribute in assembly.GetAttributes(null, false))
            {
                if (assemblyAttribute is ConcordionAssemblyAttribute)
                {
                    return true;
                }
            }

            return false;
        }

        private Test GetAssemblyTest(IAssemblyInfo assembly, Test parentTest, Version frameworkVersion, bool populateRecursively)
        {
            Test assemblyTest;
            if (!assemblyTests.TryGetValue(assembly, out assemblyTest))
            {
                assemblyTest = CreateAssemblyTest(assembly);

                String frameworkName = CONCORDION_ASSEMBLY_DISPLAY_NAME;
                assemblyTest.Metadata.SetValue(MetadataKeys.Framework, frameworkName);
                assemblyTest.Metadata.SetValue(MetadataKeys.File, assembly.Path);
                assemblyTest.Kind = AssemblyKind;

                parentTest.AddChild(assemblyTest);
                assemblyTests.Add(assembly, assemblyTest);
            }

            GetInputOutputDirectories(assembly);

            if (populateRecursively)
            {
                foreach (var type in assembly.GetExportedTypes())
                    TryGetTypeTest(type, assemblyTest);
            }

            return assemblyTest;
        }

        private void GetInputOutputDirectories(IAssemblyInfo assembly)
        {
            var config = new SpecificationConfig().Load(assembly.Resolve(false));

            var baseInputDirectoryInfo = new DirectoryInfo(config.BaseInputDirectory);
            if (baseInputDirectoryInfo.Exists)
            {
            }
            else
            {
               TestModel.AddAnnotation(new Annotation(AnnotationType.Error, assembly, String.Format("The Base Input Directory {0} does not exist, reverting to default", config.BaseInputDirectory)));
            }

            var baseOutputDirectoryInfo = new DirectoryInfo(config.BaseOutputDirectory);
            this._baseOutputDirectory = baseOutputDirectoryInfo;

            if (!_baseOutputDirectory.Exists)
            {
                Directory.CreateDirectory(_baseOutputDirectory.FullName);
            }
        }


        private Test CreateAssemblyTest(IAssemblyInfo assembly)
        {
            Test assemblyTest = new Test(assembly.Name, assembly);
            assemblyTest.Kind = TestKinds.Assembly;
            ModelUtils.PopulateMetadataFromAssembly(assembly, assemblyTest.Metadata);
            return assemblyTest;
        }

        private Test TryGetTypeTest(ITypeInfo type, Test assemblyTest)
        {
            Test typeTest;
            if (!typeTests.TryGetValue(type, out typeTest))
            {
                try
                {
                    foreach (var attribute in type.GetAttributes(null, false))
                    {
                        if (attribute is ConcordionTestAttribute)
                        {
                            typeTest = CreateTypeTest(new ConcordionTypeInfoAdapter(type));
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    TestModel.AddAnnotation(new Annotation(AnnotationType.Error, type, "An exception was thrown while exploring an concordion test type.", ex));
                }

                if (typeTest != null)
                {
                    assemblyTest.AddChild(typeTest);
                    typeTests.Add(type, typeTest);
                }
            }
            return typeTest;
        }

        private ConcordionTest CreateTypeTest(ConcordionTypeInfoAdapter typeInfo)
        {
            var fixtureType = CreateFixtureType(typeInfo.Target);
            var resource = CreateResource(ExtrapolateResourcePath(fixtureType));

            var typeTest = new ConcordionTest(typeInfo.Target.Name, typeInfo.Target, typeInfo, resource, fixtureType);
            typeTest.Source = new EmbeddedResourceSource(fixtureType.Assembly);
            typeTest.Target = new FileTarget(_baseOutputDirectory.FullName);
            typeTest.Kind = TestKinds.Fixture;
            typeTest.IsTestCase = true;

            // Add XML documentation.
            var xmlDocumentation = typeInfo.Target.GetXmlDocumentation();
            if (xmlDocumentation != null)
                typeTest.Metadata.SetValue(MetadataKeys.XmlDocumentation, xmlDocumentation);

            return typeTest;
        }

        private Type CreateFixtureType(ITypeInfo type)
        {
            var resolvedType = type.Resolve(false);
            if (resolvedType.IsClass)
            {
                ConstructorInfo constructor = resolvedType.GetConstructor(Type.EmptyTypes);

                if (constructor != null)
                {
                    return resolvedType;
                }
            }

            throw new InvalidOperationException("Cannot create the fixture");
        }

        private string ExtrapolateResourcePath(Type type)
        {
            var typeNamespace = type.Namespace;
            typeNamespace = typeNamespace.Replace(".", "\\");
            var fileName = type.Name;
            if (fileName.EndsWith("Test"))
            {
                fileName = fileName.Remove(fileName.Length - 4);
            }
            return typeNamespace + "\\" + fileName + ".html";
        }

        private Resource CreateResource(string path)
        {
            return new Resource(path);
        }
    }
}
