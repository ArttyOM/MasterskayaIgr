using System.IO;
using System.IO.Compression;

namespace MasterServerToolkit.Extensions
{
    public static class BytesExtensions
    {
        public static byte[] CompressDeflate(this byte[] data)
        {
            using (var compressedStream = new MemoryStream())
            {
                using (var deflateStream = new DeflateStream(compressedStream, CompressionMode.Compress))
                {
                    deflateStream.Write(data, 0, data.Length);
                }

                return compressedStream.ToArray();
            }
        }

        public static byte[] DecompressDeflate(this byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            {
                using (var deflateStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
                {
                    using (var resultStream = new MemoryStream())
                    {
                        deflateStream.CopyTo(resultStream);
                        return resultStream.ToArray();
                    }
                }
            }
        }

        public static byte[] CompressGzip(this byte[] data)
        {
            using (var compressedStream = new MemoryStream())
            {
                using (var deflateStream = new GZipStream(compressedStream, CompressionMode.Compress))
                {
                    deflateStream.Write(data, 0, data.Length);
                }

                return compressedStream.ToArray();
            }
        }

        public static byte[] DecompressGzip(this byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            {
                using (var deflateStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                {
                    using (var resultStream = new MemoryStream())
                    {
                        deflateStream.CopyTo(resultStream);
                        return resultStream.ToArray();
                    }
                }
            }
        }
    }
}