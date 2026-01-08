using System.Reflection;
using JetBrains.Annotations;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Interfaces;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Core.Tests.Services;

[TestClass]
[TestSubject(typeof(IXmlLoaderService))]
public class XmlLoaderServiceTests : BaseTest
{
    //TestInitialize
    private IXmlLoaderService _validationService = null!;

    [TestMethod]
    public void Load_Ok()
    {
        var bytes = Assembly.GetExecutingAssembly().Read("xml.xml").ReadAsByteArray();
        var result = _validationService.Load(new KrosoftFile(string.Empty, bytes));

        Check.That(result.Exception).IsNull();
        Check.That(result.Value).IsNotNull();
        Check.That(result.Value!.LocalName).IsEqualTo("#document");
    }
    [TestMethod]
    public void Load_Wrong_Type()
    {
        var bytes = Assembly.GetExecutingAssembly().Read("sample.txt").ReadAsByteArray();
        var result = _validationService.Load(new KrosoftFile(string.Empty, bytes));
        Check.That(result.Value).IsNull();
        Check.That(result.Exception).IsNotNull();
        Check.That(result.Exception!.Message).IsEqualTo("Erreur lors de la lecture du fichier '', message : Data at the root level is invalid. Line 1, position 1.");
    }

    [TestMethod]
    public void Load_Invalid_Xml()
    {
        var bytes = Assembly.GetExecutingAssembly().Read("invoice_ubl_invalide.txt").ReadAsByteArray();
        var result = _validationService.Load(new KrosoftFile(string.Empty, bytes));
        Check.That(result.Value).IsNull();
        Check.That(result.Exception).IsNotNull();
        Check.That(result.Exception!.Message).IsEqualTo("Erreur lors de la lecture du fichier '', message : Unexpected end of file has occurred. The following elements are not closed: Invoice. Line 8, position 2.");
    }

    [TestMethod]
    public void Load_File_Empty()
    {
        var result = _validationService.Load(new KrosoftFile(string.Empty, Array.Empty<byte>()));
        Check.That(result.Value).IsNull();
        Check.That(result.Exception).IsNotNull();
        Check.That(result.Exception!.Message).IsEqualTo("Le fichier '' est vide.");
    }

    [TestMethod]
    public void Load_Null()
    {
        Check.ThatCode(() => _validationService.Load(null!))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'file' n'est pas renseignée.");
    }

    [TestInitialize]
    public void SetUp()
    {
        var serviceProvider = CreateServiceCollection();
        _validationService = serviceProvider.GetRequiredService<IXmlLoaderService>();
    }

    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddXmlLoaderService();

        base.AddServices(services, configuration);
    }
}