using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Concordion.Internal.Util
{
    public class IOUtil
    {
        private const int BufferSize = 4096;

        public static void Copy(TextReader inputReader, TextWriter outputWriter)
        {
            var buffer = new char[BufferSize];
            var len = 0;
            while ((len = inputReader.Read(buffer, 0, BufferSize)) != -1)
            {
                outputWriter.Write(buffer, 0, len);
            }
        }
    }
}
