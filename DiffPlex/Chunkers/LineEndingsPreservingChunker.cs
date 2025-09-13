using System.Collections.Generic;

namespace DiffPlex.Chunkers
{
    public class LineEndingsPreservingChunker:IChunker
    {
        private static readonly string[] EmptyArray = new string[0];

        /// <summary>
        /// Gets the default singleton instance of the chunker.
        /// </summary>
        public static LineEndingsPreservingChunker Instance { get; } = new LineEndingsPreservingChunker();

        public IReadOnlyList<string> Chunk(string text)
        {
            if (string.IsNullOrEmpty(text))
                return EmptyArray;

            var output = new List<string>();
            var lastCut = 0;

            for (int i = 0; i < text.Length; i++)
            {
                int lineEndLen = 0;

                if (text[i] == '\r')
                {
                    lineEndLen = 1;
                    if (i + 1 < text.Length && text[i + 1] == '\n')
                        lineEndLen = 2;           // CRLF
                }
                else if (text[i] == '\n')
                {
                    lineEndLen = 1;               // LF
                }

                if (lineEndLen > 0)
                {
                    int sliceLen = i - lastCut + lineEndLen;
                    output.Add(text.Substring(lastCut, sliceLen));

                    i += lineEndLen - 1;    // we already consumed them
                    lastCut += sliceLen;
                }
            }

            if (lastCut != text.Length)           // trailing line without EOL
                output.Add(text.Substring(lastCut));

            return output;
        }
    }
}