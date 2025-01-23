using System;
using System.Globalization;
using System.Resources;

namespace HecklHelper.Resources;

public class ResourcePair
{
    static readonly string intFormat = "N0";
    static readonly string floatFormat2 = "N2";


    public string LocaleString { get; set; }
    public string EnglishString { get; set; }

    private ResourcePair(string member1, string member2)
    {
        LocaleString = member1;
        EnglishString = member2;
    }

    public ResourcePair(string key, ResourceManager resourceManager)
    {
        LocaleString = resourceManager.GetString(key);
        if (LocaleString is null)
            throw new Exception("LocaleString is not found for " + key);
        EnglishString = resourceManager.GetString(key, CultureInfo.InvariantCulture);
        if (EnglishString is null)
            throw new Exception("EnglishString is not found for " + key);
    }

    public static ResourcePair operator +(ResourcePair resourcePair1, string resourcePair2)
    {
        return new ResourcePair(
            resourcePair1.LocaleString + resourcePair2,
            resourcePair1.EnglishString + resourcePair2
        );
    }

    public static ResourcePair operator +(string value, ResourcePair resourcePair)
    {
        return new ResourcePair(
            value + resourcePair.LocaleString,
            value + resourcePair.EnglishString
        );
    }

    public static ResourcePair operator +(ResourcePair resourcePair, int value)
    {
        return new ResourcePair(
            resourcePair.LocaleString + value.ToString(intFormat),
            resourcePair.EnglishString + value.ToString(intFormat, CultureInfo.InvariantCulture)
        );
    }

    public static ResourcePair operator +(int value, ResourcePair resourcePair)
    {
        return new ResourcePair(
            value.ToString(intFormat) + resourcePair.LocaleString,
            value.ToString(intFormat, CultureInfo.InvariantCulture) + resourcePair.EnglishString
        );
    }

    public static ResourcePair operator +(ResourcePair resourcePair, float value)
    {
        return new ResourcePair(
            resourcePair.LocaleString + value.ToString(floatFormat2),
            resourcePair.EnglishString + value.ToString(floatFormat2, CultureInfo.InvariantCulture)
        );
    }

    public static ResourcePair operator +(float value, ResourcePair resourcePair)
    {
        return new ResourcePair(
            value.ToString(floatFormat2) + resourcePair.LocaleString,
            value.ToString(floatFormat2, CultureInfo.InvariantCulture) + resourcePair.EnglishString
        );
    }

    public static ResourcePair operator +(ResourcePair resourcePair, double value)
    {
        return new ResourcePair(
            resourcePair.LocaleString + value.ToString(floatFormat2),
            resourcePair.EnglishString + value.ToString(floatFormat2, CultureInfo.InvariantCulture)
        );
    }

    public static ResourcePair operator +(double value, ResourcePair resourcePair)
    {
        return new ResourcePair(
            value.ToString(floatFormat2) + resourcePair.LocaleString,
            value.ToString(floatFormat2, CultureInfo.InvariantCulture) + resourcePair.EnglishString
        );
    }

    public override string ToString()
    {
        return $"LocaleString: {LocaleString}, EnglishString: {EnglishString}";
    }
}
