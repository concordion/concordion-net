using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Model;
using Gallio.Reflection;
using System.Reflection;
using Gallio.ConcordionAdapter.Properties;
using Concordion.Integration;
using Concordion.Api;
using System.IO;
using Concordion.Internal;

namespace Gallio.ConcordionAdapter.Model
{
    /// <summary>
    /// Finds tests in Concordion assemblies
    /// </summary>
    public class ConcordionTestExplorer : BaseTestExplorer
    {
        private static readonly string CONCORDION_ASSEMBLY_DISPLAY_NAME = @"Concordion";

        public readonly Dictionary<Version, ITest> frameworkTests;
        public readonly Dictionary<IAssemblyInfo, ITest> assemblyTests;
        public readonly Dictionary<ITypeInfo, ITest> typeTests;

        private DirectoryInfo BaseInputDirectory
        {
            get;
            set;
        }

        private DirectoryInfo BaseOutputDirectory
        {
            get;
            set;
        }

        public ConcordionTestExplorer(TestModel model)
            : base(model)
        {
            frameworkTests = new Dictionary<Version, ITest>();
            assemblyTests = new Dictionary<IAssemblyInfo, ITest>();
            typeTests = new Dictionary<ITypeInfo, ITest>();

            BaseInputDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
            BaseOutputDirectory = new DirectoryInfo(Environment.GetEnvironmentVariable("TEMP"));
        }

        public override void ExploreAssembly(Gallio.Reflection.IAssemblyInfo assembly, Action<ITest> consumer)
        {
            Version frameworkVersion = GetFrameworkVersion(assembly);

            if (frameworkVersion != null)
            {
                ITest frameworkTest = GetFrameworkTest(frameworkVersion, TestModel.RootTest);
                ITest assemblyTest = GetAssemblyTest(assembly, frameworkTest, true);

                if (consumer != null)
                {
                    consumer(assemblyTest);
                }
            }
        }

        public override void ExploreType(ITypeInfo type, Action<ITest> consumer)
        {
            IAssemblyInfo assembly = type.Assembly;
            Version frameworkVersion = GetFrameworkVersion(assembly);

            if (frameworkVersion != null)
            {
                ITest frameworkTest = GetFrameworkTest(frameworkVersion, TestModel.RootTest);
                ITest assemblyTest = GetAssemblyTest(assembly, frameworkTest, false);

                ITest typeTest = TryGetTypeTest(type, assemblyTest);
                if (typeTest != null && consumer != null)
                    consumer(typeTest);
            }
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


        private static Version GetFrameworkVersion(IAssemblyInfo assembly)
        {
            if (IsConcordionAttributePresent(assembly))
            {
                AssemblyName frameworkAssemblyName = ReflectionUtils.FindAssemblyReference(assembly, CONCORDION_ASSEMBLY_DISPLAY_NAME);
                return frameworkAssemblyName != null ? frameworkAssemblyName.Version : null;
            }

            return null;
        }


        private ITest GetFrameworkTest(Version frameworkVersion, RootTest rootTest)
        {
            ITest frameworkTest;
            if (!frameworkTests.TryGetValue(frameworkVersion, out frameworkTest))
            {
                frameworkTest = CreateFrameworkTest(frameworkVersion);
                rootTest.AddChild(frameworkTest);

                frameworkTests.Add(frameworkVersion, frameworkTest);
            }

            return frameworkTest;
        }

        private ITest CreateFrameworkTest(Version frameworkVersion)
        {
            BaseTest frameworkTest = new BaseTest(String.Format(Resources.ConcordionTestExplorer_FrameworkNameWithVersionFormat, frameworkVersion), null);
            frameworkTest.BaselineLocalId = Resources.ConcordionTestFramework_ConcordionFrameworkName;
            frameworkTest.Kind = TestKinds.Framework;

            return frameworkTest;
        }

        private ITest GetAssemblyTest(IAssemblyInfo assembly, ITest frameworkTest, bool populateRecursively)
        {
            ITest assemblyTest;
            if (!assemblyTests.TryGetValue(assembly, out assemblyTest))
            {
                assemblyTest = CreateAssemblyTest(assembly);
                frameworkTest.AddChild(assemblyTest);

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
            var config = new ConcordionConfig();
            config.Load();

            var baseInputDirectory = new DirectoryInfo(config.BaseInputDirectory);
            if (baseInputDirectory.Exists)
            {
                BaseInputDirectory = baseInputDirectory;
            }
            else
            {
                TestModel.AddAnnotation(new Annotation(AnnotationType.Error, assembly, "The Base Input Directory of the Concordion Assembly does not exist, reverting to default"));
            }

            var baseOutputDirectory = new DirectoryInfo(config.BaseOutputDirectory);
            BaseOutputDirectory = baseOutputDirectory;

            if (!baseOutputDirectory.Exists)
            {
                Directory.CreateDirectory(baseOutputDirectory.FullName);
            }
        }

        private ITest CreateAssemblyTest(IAssemblyInfo assembly)
        {
            BaseTest assemblyTest = new BaseTest(assembly.Name, assembly);
            assemblyTest.Kind = TestKinds.Assembly;

            ModelUtils.PopulateMetadataFromAssembly(assembly, assemblyTest.Metadata);

            return assemblyTest;
        }

        private ITest TryGetTypeTest(ITypeInfo type, ITest assemblyTest)
        {
            ITest typeTest;
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
                catch (Exception e)
                {
                    TestModel.AddAnnotation(new Annotation(AnnotationType.Error, type, "An exception was thrown while exploring an Concordion test type.", e));
                }

                if (typeTest != null)
                {
                    assemblyTest.AddChild(typeTest);
                    typeTests.Add(type, typeTest);
                }
            }

            return typeTest;
        }

        private object CreateFixture(ITypeInfo type)
        {
            var resolvedType = type.Resolve(false);

            if (resolvedType.IsClass)
            {
                ConstructorInfo constructor = resolvedType.GetConstructor(Type.EmptyTypes);

                if (constructor != null)
                {
                    return constructor.Invoke(new Object[] { });
                }
            }

            throw new InvalidOperationException("Cannot create the fixture");
        }

        private string ExtrapolateResourcePath(Type type)
        {
            var typeNamespace = type.Namespace;

            if (typeNamespace.StartsWith("Concordion."))
            {
                typeNamespace = typeNamespace.Remove(0, 11);
            }

            typeNamespace = typeNamespace.Replace(".", "\\");
            var fileName = type.Name.Remove(type.Name.Length - 4);

            return typeNamespace + "\\" + fileName + ".html";
        }

        private Resource CreateResource(string path)
        {
            return new Resource(path);
        }

        private ConcordionTest CreateTypeTest(ConcordionTypeInfoAdapter typeInfo)
        {
            var fixture = CreateFixture(typeInfo.Target);
            var resource = CreateResource(ExtrapolateResourcePath(fixture.GetType()));

            var typeTest = new ConcordionTest(typeInfo.Target.Name, typeInfo.Target, typeInfo, resource, fixture);
            typeTest.Source = new FileSource(BaseInputDirectory.FullName);
            typeTest.Target = new FileTarget(BaseOutputDirectory.FullName);
            typeTest.Kind = TestKinds.Fixture;
            typeTest.IsTestCase = true;

            // Add XML documentation.
            var xmlDocumentation = typeInfo.Target.GetXmlDocumentation();
            if (xmlDocumentation != null)
                typeTest.Metadata.SetValue(MetadataKeys.XmlDocumentation, xmlDocumentation);

            return typeTest;
        }
    }
}
