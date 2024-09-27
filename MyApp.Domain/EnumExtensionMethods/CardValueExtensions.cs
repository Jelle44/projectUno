using MyApp.Domain.Enums;

namespace MyApp.Domain.EnumExtensionMethods;

internal static class CardValueExtensions
{
    public static bool IsInvalidForColours(this Enum value)
    {
        return value is Value.DRAW4 or Value.CHANGE;
    }
}

