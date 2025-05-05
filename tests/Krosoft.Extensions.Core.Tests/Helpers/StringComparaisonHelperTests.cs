using Krosoft.Extensions.Core.Helpers;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class StringComparaisonHelperTests
{
    private void CalculateJaroWinkler(string source, string target, decimal attendu)
    {
        var resultat = Math.Round(StringComparaisonHelper.CalculateJaroWinkler(source.ToLower(), target.ToLower()), 4);

        Check.That(resultat).IsEqualTo(attendu);
    }

    private void CalculateJaroWinklerInPourcent(string source, string target, decimal attendu)
    {
        var resultat = Math.Round(StringComparaisonHelper.CalculateJaroWinklerInPourcent(source.ToLower(), target.ToLower()), 4);

        Check.That(resultat).IsEqualTo(attendu);
    }

    [TestMethod]
    public void CalculateJaroWinklerInPourcentTest()
    {
        var serie = "HDTV";
        CalculateJaroWinklerInPourcent(serie, "HDTV", 100);
        CalculateJaroWinklerInPourcent(serie, "AAAA", 0);

        var serie1 = "HDTV.x264-AVS";
        CalculateJaroWinklerInPourcent(serie1, "720p.AVS", 60.3205m);
        CalculateJaroWinklerInPourcent(serie1, "iNTERNAL.720p.WEBRip-STRiFE", 45.0142m);
        CalculateJaroWinklerInPourcent(serie1, "WEBRip-RARBG", 44.0171m);

        var serie2 = "HDTV.x264-FLEET";
        CalculateJaroWinklerInPourcent(serie2, "PROPER-KILLERS", 47.1429m);
        CalculateJaroWinklerInPourcent(serie2, "WEB-DL", 45.5556m);

        var serie3 = "PROPER.HDTV.x264-KILLERS";
        CalculateJaroWinklerInPourcent(serie3, "KILLERS", 28.9683m);
        CalculateJaroWinklerInPourcent(serie3, "PROPER-KILLERS", 91.6667m);
        CalculateJaroWinklerInPourcent(serie3, "WEB-DL", 47.2222m);

        var serie4 = "Dragonstone.AMZN.WEB-DL.DDP5.1.H.264-GoT";
        CalculateJaroWinklerInPourcent(serie4, "SVA-AVS", 39.0476m);
        CalculateJaroWinklerInPourcent(serie4, "AMZN.WEBRip", 58.6364m);
        CalculateJaroWinklerInPourcent(serie4, "WEB.h264-TBS", 44.3254m);
        CalculateJaroWinklerInPourcent(serie4, "iNTERNAL.TURBO", 52.1429m);

        var serie5 = "Dragonstone.REPACK.AMZN.WEBRip.DDP5.1.x264-GoT";
        CalculateJaroWinklerInPourcent(serie5, "SVA-AVS", 38.6818m);
        CalculateJaroWinklerInPourcent(serie5, "AMZN.WEBRip", 59.4862m);
        CalculateJaroWinklerInPourcent(serie5, "WEB.h264-TBS", 37.5121m);
        CalculateJaroWinklerInPourcent(serie5, "iNTERNAL.TURBO", 47.7226m);
    }

    [TestMethod]
    public void CalculateJaroWinklerTest()
    {
        var serie = "HDTV";
        CalculateJaroWinkler(serie, "HDTV", 1);
        CalculateJaroWinkler(serie, "AAAA", 0);

        var serie1 = "HDTV.x264-AVS";
        CalculateJaroWinkler(serie1, "720p.AVS", 0.6032m);
        CalculateJaroWinkler(serie1, "iNTERNAL.720p.WEBRip-STRiFE", 0.4501m);
        CalculateJaroWinkler(serie1, "WEBRip-RARBG", 0.4402m);

        var serie2 = "HDTV.x264-FLEET";
        CalculateJaroWinkler(serie2, "PROPER-KILLERS", 0.4714m);
        CalculateJaroWinkler(serie2, "WEB-DL", 0.4556m);

        var serie3 = "PROPER.HDTV.x264-KILLERS";
        CalculateJaroWinkler(serie3, "KILLERS", 0.2897m);
        CalculateJaroWinkler(serie3, "PROPER-KILLERS", 0.9167m);
        CalculateJaroWinkler(serie3, "WEB-DL", 0.4722m);

        var serie4 = "Dragonstone.AMZN.WEB-DL.DDP5.1.H.264-GoT";
        CalculateJaroWinkler(serie4, "SVA-AVS", 0.3905m);
        CalculateJaroWinkler(serie4, "AMZN.WEBRip", 0.5864m);
        CalculateJaroWinkler(serie4, "WEB.h264-TBS", 0.4433m);
        CalculateJaroWinkler(serie4, "iNTERNAL.TURBO", 0.5214m);

        var serie5 = "Dragonstone.REPACK.AMZN.WEBRip.DDP5.1.x264-GoT";
        CalculateJaroWinkler(serie5, "SVA-AVS", 0.3868m);
        CalculateJaroWinkler(serie5, "AMZN.WEBRip", 0.5949m);
        CalculateJaroWinkler(serie5, "WEB.h264-TBS", 0.3751m);
        CalculateJaroWinkler(serie5, "iNTERNAL.TURBO", 0.4772m);
    }

    private void CalculateLevenshtein(string source, string target, int attendu)
    {
        var resultat = StringComparaisonHelper.CalculateLevenshtein(source.ToLower(), target.ToLower());

        Check.That(resultat).IsEqualTo(attendu);
    }

    private void CalculateLevenshteinInPourcent(string source, string target, decimal attendu)
    {
        var resultat = Math.Round(StringComparaisonHelper.CalculateLevenshteinInPourcent(source.ToLower(), target.ToLower()), 4);

        Check.That(resultat).IsEqualTo(attendu);
    }

    [TestMethod]
    public void CalculateLevenshteinInPourcentTest()
    {
        var serie = "HDTV";
        CalculateLevenshteinInPourcent(serie, "HDTV", 100);
        CalculateLevenshteinInPourcent(serie, "AAAA", 0);

        var serie1 = "HDTV.x264-AVS";
        CalculateLevenshteinInPourcent(serie1, "720p.AVS", 30.7692m);
        CalculateLevenshteinInPourcent(serie1, "iNTERNAL.720p.WEBRip-STRiFE", 14.8148m);
        CalculateLevenshteinInPourcent(serie1, "WEBRip-RARBG", 0);

        var serie2 = "HDTV.x264-FLEET";
        CalculateLevenshteinInPourcent(serie2, "PROPER-KILLERS", 13.3333m);
        CalculateLevenshteinInPourcent(serie2, "WEB-DL", 13.3333m);

        var serie3 = "PROPER.HDTV.x264-KILLERS";
        CalculateLevenshteinInPourcent(serie3, "KILLERS", 29.1667m);
        CalculateLevenshteinInPourcent(serie3, "PROPER-KILLERS", 58.3333m);
        CalculateLevenshteinInPourcent(serie3, "WEB-DL", 012.5m);

        var serie4 = "Dragonstone.AMZN.WEB-DL.DDP5.1.H.264-GoT";
        CalculateLevenshteinInPourcent(serie4, "SVA-AVS", 7.5m);
        CalculateLevenshteinInPourcent(serie4, "AMZN.WEBRip", 22.5m);
        CalculateLevenshteinInPourcent(serie4, "WEB.h264-TBS", 22.5m);
        CalculateLevenshteinInPourcent(serie4, "iNTERNAL.TURBO", 17.5m);

        var serie5 = "Dragonstone.REPACK.AMZN.WEBRip.DDP5.1.x264-GoT";
        CalculateLevenshteinInPourcent(serie5, "SVA-AVS", 6.5217m);
        CalculateLevenshteinInPourcent(serie5, "AMZN.WEBRip", 23.913m);
        CalculateLevenshteinInPourcent(serie5, "WEB.h264-TBS", 17.3913m);
        CalculateLevenshteinInPourcent(serie5, "iNTERNAL.TURBO", 17.3913m);
    }

    [TestMethod]
    public void CalculateLevenshteinTest()
    {
        var serie = "HDTV";
        CalculateLevenshtein(serie, "HDTV", 0);
        CalculateLevenshtein(serie, "AAAA", 4);

        var serie1 = "HDTV.x264-AVS";
        CalculateLevenshtein(serie1, "720p.AVS", 9);
        CalculateLevenshtein(serie1, "iNTERNAL.720p.WEBRip-STRiFE", 23);
        CalculateLevenshtein(serie1, "WEBRip-RARBG", 13);

        var serie2 = "HDTV.x264-FLEET";
        CalculateLevenshtein(serie2, "PROPER-KILLERS", 13);
        CalculateLevenshtein(serie2, "WEB-DL", 13);

        var serie3 = "PROPER.HDTV.x264-KILLERS";
        CalculateLevenshtein(serie3, "KILLERS", 17);
        CalculateLevenshtein(serie3, "PROPER-KILLERS", 10);
        CalculateLevenshtein(serie3, "WEB-DL", 21);

        var serie4 = "Dragonstone.AMZN.WEB-DL.DDP5.1.H.264-GoT";
        CalculateLevenshtein(serie4, "SVA-AVS", 37);
        CalculateLevenshtein(serie4, "AMZN.WEBRip", 31);
        CalculateLevenshtein(serie4, "WEB.h264-TBS", 31);
        CalculateLevenshtein(serie4, "iNTERNAL.TURBO", 33);

        var serie5 = "Dragonstone.REPACK.AMZN.WEBRip.DDP5.1.x264-GoT";
        CalculateLevenshtein(serie5, "SVA-AVS", 43);
        CalculateLevenshtein(serie5, "AMZN.WEBRip", 35);
        CalculateLevenshtein(serie5, "WEB.h264-TBS", 38);
        CalculateLevenshtein(serie5, "iNTERNAL.TURBO", 38);
    }
}