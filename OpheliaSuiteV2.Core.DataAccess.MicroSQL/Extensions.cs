using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL {

    /// <summary>
    /// Provee métodos extendidos
    /// </summary>
    internal static class Extensions {

        #region String

        /// <summary>
        /// Convierte la cadena en su correspondiente hash MD5
        /// </summary>
        /// <param name="str">Objeto que extiende el método</param>
        /// <returns>Hash MD5 resultado</returns>
        public static string ToMD5(this string str) {
            if (string.IsNullOrWhiteSpace(str))
                return null;

            using (MD5 md5Hash = MD5.Create()) {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(str));
                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++) {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

        /// <summary>
        /// Comprime la cadena de texto
        /// </summary>
        /// <param name="inputStr">Cadena de texto a comprimir</param>
        /// <returns>Cadena de testo comprimida</returns>
        public static string Compress(this string inputStr) {
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputStr);

            using (var outputStream = new MemoryStream()) {
                using (var gZipStream = new GZipStream(outputStream, CompressionMode.Compress))
                    gZipStream.Write(inputBytes, 0, inputBytes.Length);

                var outputBytes = outputStream.ToArray();

                var outputStr = Convert.ToBase64String(outputBytes);
                return outputStr;
            }
        }

        /// <summary>
        /// Descomprime una cadena de texto comprimida con <see cref="Compress(string)"/>
        /// </summary>
        /// <param name="inputStr">Cadena a descomprimir</param>
        /// <returns>Cadena descomprimida</returns>
        public static string Decompress(this string inputStr) {
            byte[] inputBytes = Convert.FromBase64String(inputStr);

            using (var inputStream = new MemoryStream(inputBytes))
            using (var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress))
            using (var outputStream = new MemoryStream()) {
                gZipStream.CopyTo(outputStream);
                var outputBytes = outputStream.ToArray();

                string decompressed = Encoding.UTF8.GetString(outputBytes);
                return decompressed;
            }
        }

        #endregion
    }
}
