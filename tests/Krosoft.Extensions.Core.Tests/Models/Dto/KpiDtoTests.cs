using System.Text.Json;
using Krosoft.Extensions.Core.Models.Dto;

namespace Krosoft.Extensions.Core.Tests.Models.Dto;

[TestClass]
public class KpiDtoTests
{
    [TestMethod]
    public void Serialize_Web_RenvoieCountEtIncreaseEnCamelCase()
    {
        var dto = new KpiDto { Count = 42, Increase = 7 };

        var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        Check.That(json).IsEqualTo("{\"count\":42,\"increase\":7}");
    }
}
