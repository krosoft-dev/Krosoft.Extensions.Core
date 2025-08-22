using Krosoft.Extensions.Core.Models;

namespace Krosoft.Extensions.Core.Tests.Models;

[TestClass]
public class KrosoftTokenTests
{
    [TestMethod]
    public void Set_Id()
    {
        var krosoftToken = new KrosoftToken
        {
            Id = "Claim_Id"
        };

        Check.That(krosoftToken).IsNotNull();
        Check.That(krosoftToken.Id).IsEqualTo("Claim_Id");
    }

    [TestMethod]
    public void Set_Name()
    {
        var krosoftToken = new KrosoftToken
        {
            Id = "Claim_Id",
            Name = "Claim_Name"
        };

        Check.That(krosoftToken).IsNotNull();
        Check.That(krosoftToken.Name).IsEqualTo("Claim_Name");
    }

    [TestMethod]
    public void Set_Email()
    {
        var krosoftToken = new KrosoftToken
        {
            Id = "Claim_Id",
            Name = "Claim_Name",
            Email = "Claim_Email"
        };

        Check.That(krosoftToken).IsNotNull();
        Check.That(krosoftToken.Email).IsEqualTo("Claim_Email");
    }

    [TestMethod]
    public void Set_RoleId()
    {
        var krosoftToken = new KrosoftToken
        {
            Id = "Claim_Id",
            Name = "Claim_Name",
            Email = "Claim_Email",
            RoleId = "Claim_RoleId"
        };

        Check.That(krosoftToken).IsNotNull();
        Check.That(krosoftToken.RoleId).IsEqualTo("Claim_RoleId");
    }

    [TestMethod]
    public void Set_Properties()
    {
        var krosoftToken = new KrosoftToken
        {
            RoleId = "Claim_RoleId",
            LangueId = "Claim_LangueId",
            LangueCode = "Claim_LangueCode"
        };
        krosoftToken.AddProperty("tenantId", new Guid("00000000-1111-1111-1111-000000000001").ToString());
        krosoftToken.AddProperty("roleHomePage", "Claim_RoleHomePage");
        krosoftToken.AddProperty("roleIsInterne", true);

        Check.That(krosoftToken).IsNotNull();
        Check.That(krosoftToken.RoleId).IsEqualTo("Claim_RoleId");
        Check.That(krosoftToken.GetProperty<string>("roleHomePage")).IsEqualTo("Claim_RoleHomePage");
        Check.That(krosoftToken.GetProperty<bool>("roleIsInterne")).IsEqualTo(true);
    }

    [TestMethod]
    public void Set_Properties_WrongType()
    {
        var krosoftToken = new KrosoftToken
        {
            RoleId = "Claim_RoleId",
            LangueId = "Claim_LangueId",
            LangueCode = "Claim_LangueCode"
        };
        krosoftToken.AddProperty("tenantId", new Guid("00000000-1111-1111-1111-000000000001").ToString());
        krosoftToken.AddProperty("roleHomePage", "Claim_RoleHomePage");
        krosoftToken.AddProperty("roleIsInterne", true);

        Check.That(krosoftToken).IsNotNull();
        Check.That(krosoftToken.RoleId).IsEqualTo("Claim_RoleId");
        Check.That(krosoftToken.GetProperty<string>("roleHomePage")).IsEqualTo("Claim_RoleHomePage");
        Check.That(krosoftToken.GetProperty<string>("roleIsInterne")).IsEqualTo(null);
    }

    [TestMethod]
    public void Set_Properties_EmptyArrayValue()
    {
        var krosoftToken = new KrosoftToken
        {
            Id = "Claim_Id"
        };
        krosoftToken.AddProperty("customArray", Array.Empty<string>());

        Check.That(krosoftToken).IsNotNull();
        Check.That(krosoftToken.GetProperty<IEnumerable<string>>("customArray")).IsEqualTo(Array.Empty<string>());
    }

    [TestMethod]
    public void Set_Properties_ComplexObject()
    {
        var krosoftToken = new KrosoftToken
        {
            Id = "Claim_Id"
        };
        krosoftToken.AddProperty("customObject", new { Name = "test", Value = 42 });

        Check.That(krosoftToken).IsNotNull();
        Check.That(krosoftToken.GetProperty<object>("customObject")).IsEqualTo(new { Name = "test", Value = 42 });
    }
}