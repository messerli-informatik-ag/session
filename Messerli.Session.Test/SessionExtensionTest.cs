using System;
using Moq;
using Xunit;

namespace Messerli.Session.Test
{
    public class SessionExtensionTest
    {
        private const string Key = "foo";

        private const string StringValue = "hello";

        private static readonly byte[] StringValueAsBytes = { 0x68, 0x65, 0x6C, 0x6C, 0x6F };

        private const int IntegerValue = 42;

        private static readonly byte[] IntegerValueAsBytes = { 42, 0, 0, 0 };

        private static readonly byte[] TrueAsBytes = { 1 };

        private static readonly byte[] FalseAsBytes = { 0 };

        [Fact]
        public void StringCanBeWritten()
        {
            TestValueCanBeSet(StringValueAsBytes, session => session.SetString(Key, StringValue));
        }

        [Fact]
        public void StringCanBeRead()
        {
            TestValueCanBeRead(
                StringValue,
                StringValueAsBytes,
                session => session.GetString(Key));
        }

        [Fact]
        public void ReturnsNullWhenReadingNonExistentString()
        {
            TestValueCanBeRead(
                null,
                null,
                session => session.GetString(Key));
        }

        [Fact]
        public void IntegerCanBeWritten()
        {
            TestValueCanBeSet(IntegerValueAsBytes, session => session.SetInt32(Key, IntegerValue));
        }

        [Fact]
        public void IntegerCanBeRead()
        {
            TestValueCanBeRead(
                IntegerValue,
                IntegerValueAsBytes,
                session => session.GetInt32(Key));
        }

        [Fact]
        public void ReturnsNullWhenReadingNonExistentInteger()
        {
            TestValueCanBeRead(
                null,
                null,
                session => session.GetInt32(Key));
        }

        [Fact]
        public void TrueCanBeWritten()
        {
            TestValueCanBeSet(TrueAsBytes, session => session.SetBoolean(Key, true));
        }

        [Fact]
        public void TrueCanBeRead()
        {
            TestValueCanBeRead(
                true,
                TrueAsBytes,
                session => session.GetBoolean(Key));
        }

        [Fact]
        public void FalseCanBeWritten()
        {
            TestValueCanBeSet(FalseAsBytes, session => session.SetBoolean(Key, false));
        }

        [Fact]
        public void FalseCanBeRead()
        {
            TestValueCanBeRead(
                false,
                FalseAsBytes,
                session => session.GetBoolean(Key));
        }

        [Fact]
        public void ReturnsNullWhenReadingNonExistentBoolean()
        {
            TestValueCanBeRead(
                null,
                null,
                session => session.GetBoolean(Key));
        }

        private static void TestValueCanBeRead<T>(T expectedValue, byte[]? valueAsBytes, Func<ISession, T> readValue)
        {
            var session = new Mock<ISession>();
            session
                .Setup(s => s.Get(Key))
                .Returns(valueAsBytes);
            Assert.Equal(expectedValue, readValue(session.Object));
        }

        private static void TestValueCanBeSet(byte[] expectedValueAsBytes, Action<ISession> action)
        {
            var session = new Mock<ISession>();
            action(session.Object);
            session.Verify(s => s.Set(Key, expectedValueAsBytes));
            session.VerifyNoOtherCalls();
        }
    }
}
