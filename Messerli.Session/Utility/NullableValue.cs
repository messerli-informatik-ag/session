using System;

namespace Messerli.Session.Utility
{
    internal static class NullableValue
    {
        public static TOutput? Map<TInput, TOutput>(TInput? input, Func<TInput, TOutput?> transform)
            where TInput : class
            where TOutput : class
            => input is { } notNullInput
                ? transform(notNullInput)
                : null;

        public static TOutput? Map<TInput, TOutput>(TInput? input, Func<TInput, TOutput?> transform)
            where TInput : class
            where TOutput : struct
            => input is { } notNullInput
                ? transform(notNullInput)
                : null;

        public static TOutput? Map<TInput, TOutput>(TInput? input, Func<TInput, TOutput?> transform)
            where TInput : struct
            where TOutput : class
            => input.HasValue
                ? transform(input.Value)
                : null;

        public static TOutput? Map<TInput, TOutput>(TInput? input, Func<TInput, TOutput?> transform)
            where TInput : struct
            where TOutput : struct
            => input.HasValue
                ? transform(input.Value)
                : null;
    }
}
