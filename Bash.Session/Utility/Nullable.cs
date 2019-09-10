using System;

namespace Bash.Session.Utility
{
    internal static class NullableValue
    {
        public static TOutput? Map<TInput, TOutput>(TInput? input, Func<TInput, TOutput?> transform)
            where TInput: class
            where TOutput: class
        {
            return input is { } notNullInput
                ? transform(notNullInput)
                : null;
        }

        public static TOutput? Map<TInput, TOutput>(TInput? input, Func<TInput, TOutput?> transform)
            where TInput: class
            where TOutput: struct
        {
            return input is { } notNullInput
                ? transform(notNullInput)
                : null;
        }

        public static TOutput? Map<TInput, TOutput>(TInput? input, Func<TInput, TOutput?> transform)
            where TInput: struct
            where TOutput: class
        {
            return input.HasValue
                ? transform(input.Value)
                : new TOutput?();
        }

        public static TOutput? Map<TInput, TOutput>(TInput? input, Func<TInput, TOutput?> transform)
            where TInput: struct
            where TOutput: struct
        {
            return input.HasValue
                ? transform(input.Value)
                : new TOutput?();
        }
    }
}
