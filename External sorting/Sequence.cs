using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External_sorting
{
    class Sequence: IDisposable
    {
        public FileInfo file { get; set; }
        private BinaryReader reader { get; set; }
        private BinaryWriter writer { get; set; }
        public Int32 element { get; set; }
        public bool EndOfFile { get; set; }
        public bool EndOfSection { get; set; }
        private int remainder;

        public Sequence(string FilePath)
        {
            file = new FileInfo(FilePath);
        }

        public void StartRead(int length)
        {
            writer?.Close();
            reader = new BinaryReader(File.Open(file.FullName, FileMode.Open));
            if (reader?.BaseStream.Position != reader.BaseStream.Length)
                remainder = length;
            CheckEnd();
        }

        public void StartWrite()
        {
            reader?.Close();
            writer = new BinaryWriter(File.Open(file.FullName, FileMode.Create));
        }

        public void StartRun(int lenght)
        {
            remainder = lenght;
            EndOfSection = EndOfFile || (remainder == 0);
        }
        
        public void Copy(Sequence s)
        {
            s.writer.Write(element);
            remainder--;
            CheckEnd();
        }

        public void CopyAll(Sequence s)
        {
            do
            {
                Copy(s);
            }
            while (!EndOfSection);
        }

        public void CheckEnd()
        {
            EndOfFile = reader?.BaseStream.Position == reader.BaseStream.Length;
            if (!EndOfFile)
            {
                element = reader.ReadInt32();
            }
            else
            {
                remainder = 0;
            }
            EndOfSection = EndOfFile || (remainder == 0);
        }

        public void Dispose()
        {
            reader?.Dispose();
            writer?.Dispose();
        }
    }
}
