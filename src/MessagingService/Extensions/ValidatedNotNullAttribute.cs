using System;

namespace MessagingService.Extensions
{
    /// <summary>
    /// Defines attribute for sonar compliance with rule S3900 in <see cref="Preconditions"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}