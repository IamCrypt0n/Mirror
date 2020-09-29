using NUnit.Framework;

namespace Mirror.Tests.MessageTests
{
    struct NoArgMethodMessage : NetworkMessage
    {
        public int someValue;

        // Weaver should ignore these methods because they have no args
        public void Serialize() { /* method with no arg */ }
        public void Deserialize() { /* method with no arg */ }

        // Mirror will fill out these empty methods
        public void Serialize(NetworkWriter writer) { }
        public void Deserialize(NetworkReader reader) { }
    }

    struct TwoArgMethodMessage : NetworkMessage
    {
        public int someValue;

        // Weaver should ignore these methods because they have two args
        public void Serialize(NetworkWriter writer, int AnotherValue) { /* method with 2 args */ }
        public void Deserialize(NetworkReader reader, int AnotherValue) { /* method with 2 args */ }


        // Mirror will fill out these empty methods
        public void Serialize(NetworkWriter writer) { }
        public void Deserialize(NetworkReader reader) { }
    }

    public class OverloadMethodTest
    {
        [Test]
        public void MethodsWithNoArgs()
        {
            const int value = 10;
            NoArgMethodMessage intMessage = new NoArgMethodMessage
            {
                someValue = value
            };

            byte[] data = MessagePackerTest.PackToByteArray(intMessage);

            NoArgMethodMessage unpacked = MessagePacker.Unpack<NoArgMethodMessage>(data);

            Assert.That(unpacked.someValue, Is.EqualTo(value));
        }

        [Test]
        public void MethodsWithTwoArgs()
        {
            const int value = 10;
            TwoArgMethodMessage intMessage = new TwoArgMethodMessage
            {
                someValue = value
            };

            byte[] data = MessagePackerTest.PackToByteArray(intMessage);

            TwoArgMethodMessage unpacked = MessagePacker.Unpack<TwoArgMethodMessage>(data);

            Assert.That(unpacked.someValue, Is.EqualTo(value));
        }
    }
}
