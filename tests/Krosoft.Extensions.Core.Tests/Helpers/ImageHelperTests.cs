using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class ImageHelperTests
{
    [TestMethod]
    public void CheckImage_Base64_Error()
    {
        Check.ThatCode(() => ImageHelper.CheckImage("invalid_base64_data", "name.fr"))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("Impossible de récupérer l'image à partir de l'url Base64 pour l'image 'name.fr'.");
    }

    [TestMethod]
    public void CheckImage_Base64Empty()
    {
        Check.ThatCode(() => ImageHelper.CheckImage(string.Empty, string.Empty))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'base64' est vide ou non renseignée.");
    }

    [TestMethod]
    public void CheckImage_NameEmpty()
    {
        Check.ThatCode(() => ImageHelper.CheckImage("data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9h", string.Empty))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'name' est vide ou non renseignée.");
    }

    [TestMethod]
    public void CheckImage_NoBase64()
    {
        Check.ThatCode(() => ImageHelper.CheckImage(null, null))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'base64' est vide ou non renseignée.");
    }

    [TestMethod]
    public void CheckImage_NoName()
    {
        Check.ThatCode(() => ImageHelper.CheckImage("data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9h", null))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'name' est vide ou non renseignée.");
    }

    [TestMethod]
    public void CheckImage_Ok()
    {
        Check.ThatCode(() => ImageHelper.CheckImage("data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9h", "valid_image"))
             .DoesNotThrow();
    }
}