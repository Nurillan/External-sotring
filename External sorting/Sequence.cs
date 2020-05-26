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
        private BinaryReader Reader { get; set; }
        private BinaryWriter Writer { get; set; }
        private int Remainder { get; set; }
        public FileInfo File { get; }
        public Int32 Element { get; private set; }

        public Sequence(string FilePath)
        {
            File = new FileInfo(FilePath);
        }

        public void NewRun(int lenght)
        {
            Remainder = lenght;
        }

        public void StartRead(int lenght)
        {
            Reader = new BinaryReader(System.IO.File.Open(File.FullName, FileMode.Open));
            NewRun(lenght);
        }

        public void Copy(Sequence s)
        {
            ReadElem();
            Remainder--;
            s.Writer.Write(Element);
        }

        private void ReadElem()
        {
            if (!EndOfSeries())
                Element = Reader.ReadInt32();
        }

        public void CopyAll(Sequence s)
        {
            do
                Copy(s);
            while (!EndOfSeries());
        }

        public void StopRead()
        {
            Reader.Close();
        }

        public void StartWrite()
        {
            Writer = new BinaryWriter(System.IO.File.Open(File.FullName, FileMode.Create));
        }

        public void StopWrite()
        {
            Writer.Close();
        }
        
        public bool EndOfFile()
        {
            return Reader.BaseStream.Position == Reader.BaseStream.Length;
        }

        public bool EndOfSeries()
        {
            return EndOfFile() || (Remainder == 0);
        }

        public void Dispose()
        {
            Reader?.Dispose();
            Writer?.Dispose();
        }
    }
}
