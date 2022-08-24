using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace UseEveryOpCode.Setup;

[TypeConverter(typeof(AttributeOptionsTypeConverter))]
public class AttributeOptions
{
    public bool? Exclude { get; set; }
    public string Feature { get; set; }
    public bool? ApplyToMembers { get; set; }

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append(Feature);
        if (Exclude is not null) builder.Append($"|{Exclude}");
        if (ApplyToMembers is not null) builder.Append($"|{ApplyToMembers}");

        return builder.ToString();
    }
}

public class AttributeOptionsTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? obj,
        Type destinationType)
    {
        if (destinationType != typeof(string) || obj is not AttributeOptions options) return null;
        var builder = new StringBuilder();
        builder.Append(options.Feature);
        if (options.Exclude is not null) builder.Append($"|{options.Exclude}");
        if (options.ApplyToMembers is not null) builder.Append($"|{options.ApplyToMembers}");

        return builder.ToString();
    }

    public override AttributeOptions? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is not string str || string.IsNullOrWhiteSpace(str))
            return null;
        var split = str.Split('|');

        var attribute = new AttributeOptions();
        try
        {
            switch (split.Length)
            {
                case 1:
                    attribute.Feature = split[0];
                    break;
                case 2:
                    attribute.Feature = split[0];
                    attribute.Exclude = Convert.ToBoolean(split[1]);
                    break;
                case 3:
                    attribute.Feature = split[0];
                    attribute.Exclude = Convert.ToBoolean(split[1]);
                    attribute.ApplyToMembers = Convert.ToBoolean(split[2]);
                    break;
                default:
                    var builder = new StringBuilder();
                    builder.AppendLine("Error parsing attribute! Please use the format: ");
                    builder.AppendLine("[FeatureName]|[Exclude]|[ApplyToMembers]");
                    builder.AppendLine("FeatureName is the name of the feature to be applied");
                    builder.AppendLine("Exclude is a boolean value indicating whether the feature should be excluded");
                    builder.AppendLine(
                        "ApplyToMembers is a boolean value indicating whether the feature should be applied to members");
                    builder.AppendLine("Example: ");
                    builder.AppendLine("[FeatureName]|[Exclude]|[ApplyToMembers]");
                    builder.AppendLine("[FeatureName]|[Exclude]");
                    builder.AppendLine("[FeatureName]");
                    throw new Exception(builder.ToString());
            }
        }
        catch (Exception ex)
        {
            var builder = new StringBuilder();
            builder.AppendLine("Error parsing attribute! Please use the format: ");
            builder.AppendLine("[FeatureName]|[Exclude]|[ApplyToMembers]");
            builder.AppendLine("FeatureName is the name of the feature to be applied");
            builder.AppendLine("Exclude is a boolean value indicating whether the feature should be excluded");
            builder.AppendLine(
                "ApplyToMembers is a boolean value indicating whether the feature should be applied to members");
            builder.AppendLine("Example: ");
            builder.AppendLine("[FeatureName]|[Exclude]|[ApplyToMembers]");
            builder.AppendLine("[FeatureName]|[Exclude]");
            builder.AppendLine("[FeatureName]");
            throw new Exception(builder.ToString(), ex);
        }

        return attribute;
    }
}