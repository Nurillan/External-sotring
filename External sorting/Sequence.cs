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
        public bool ReadyForCopy { get; private set; }

        public Sequence(string FilePath)
        {
            File = new FileInfo(FilePath);
        }

        public void NewRun(int lenght)
        {
            Remainder = lenght;
        }

        public void StartRead()
        {
            Reader = new BinaryReader(System.IO.File.Open(File.FullName, FileMode.Open));
        }

        public void ReadElem()
        {
            if (!EndOfSeries())
            {
                Element = Reader.ReadInt32();
                ReadyForCopy = true;
            }                
        }

        public void CopyElemTo(Sequence s)
        {
            if (ReadyForCopy)
            {
                s.WriteElem(Element);
                Remainder--;
                ReadyForCopy = false;
            }
        }

        public void CopyAll(Sequence s)
        {
            ReadElem();
            do
            {
                CopyElemTo(s);
                ReadElem();
            }
            while (ReadyForCopy);
        }

        public void StopRead()
        {
            Reader.Close();
        }

        public void StartWrite()
        {
            Writer = new BinaryWriter(System.IO.File.Open(File.FullName, FileMode.Create));
        }

        public void WriteElem(Int32 element)
        {
            Writer.Write(element);
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
