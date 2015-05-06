using System.Linq;
using JCsTools.JCQ.IcqInterface.DataTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jcq.IcqProtocol.Tests
{
    static internal class SerializationTools
    {
        /// <summary>
        ///     Deserialize a Flap object from the given byte array and perfom sanity checks.
        /// </summary>
        /// <param name="data">The data containing the flap.</param>
        internal static Flap DeserializeFlap(byte[] data)
        {
            var f = new Flap();

            f.Deserialize(data.ToList());

            // flap
            Assert.AreEqual(data.Length, f.TotalSize, "Flap Total Size");
            Assert.AreEqual(data.Length - 6, f.DataSize, "Flap Data Size");

            Assert.AreEqual(ByteConverter.ToUInt16(new[] {data[2], data[3]}), f.DatagramSequenceNumber,
                "Flap DatagramSequenceNumber");

            Assert.IsNotNull(f.DataItems, "Flap DataItems");
            Assert.AreEqual(1, f.DataItems.Count, "Flap DataItems Count");

            return f;
        }

        /// <summary>
        ///     Deserialize a Snac(T) object from the given Flap and perform sanity checks.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="f"></param>
        /// <returns></returns>
        internal static T DeserializeSnac<T>(Flap f)
            where T : Snac
        {
            Assert.AreEqual(FlapChannel.SnacData, f.Channel, "Flap Channel");

            // snac
            var item = f.DataItems.First();

            Assert.IsInstanceOfType(item, typeof (T), "Flap DataItem Type");

            var s = (T) item;

            Assert.AreEqual(f.DataSize - 10, s.DataSize, "Snac Data Size");

            return (T) item;
        }
    }
}