using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External_sorting
{
    static class Handler
    {
        public static int max = 10000;
        public static int count = 1000;
        public static int AmountOfPasses { get; private set; }

        public static void MakeRandomFile(string FileName)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(FileName, FileMode.Create)))
            {
                Random random = new Random();
                int element;
                for (int i = 0; i < count; i++)
                {
                    element = random.Next(max);
                    writer.Write(element);
                }
            }
        }

        public static void MakeReverseFile(string SourceFile, string DestFile)
        {
            Int32[] mas = new Int32[count];
            int i = 0;
            using (BinaryReader reader = new BinaryReader(File.Open(SourceFile, FileMode.Open)))
            {
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    mas[i] = reader.ReadInt32();
                    i++;
                }
            }
            i--;
            using (BinaryWriter writer = new BinaryWriter(File.Open(DestFile, FileMode.Create)))
            {
                while (i >= 0)
                {
                    writer.Write(mas[i]);
                    i--;
                }
            }
        }

        public static string PrintFile(string FileName)
        {
            string str = "";
            using (BinaryReader reader = new BinaryReader(File.Open(FileName, FileMode.Open)))
            {
                while(reader?.BaseStream.Position != reader.BaseStream.Length)
                {
                    str += reader.ReadInt32() + ", ";
                }
            }
            return str;
        }

        public static void Distribute(Sequence f, int lenght, params Sequence[] mas)
        {
            f.StartRead(lenght);
            foreach (Sequence s in mas)
            {
                s.StartWrite();
            }

            while (!f.EndOfFile)
            {
                foreach (Sequence s in mas)
                {
                    f.CopyAll(s);
                    f.StartRun(lenght);
                    if (f.EndOfFile)
                        return;
                }
            }

            f.Dispose();
            foreach (Sequence s in mas)
            {
                s.Dispose();
            }
        }

        public static void Merge(Sequence f, int lenght, params Sequence[] mas)
        {
            foreach(Sequence s in mas)
            {
                s.StartRead(lenght);
            }
            int UndoneSection = mas.Length;
            int UndoneFile = mas.Length;

            f.StartWrite();
            
            while(UndoneFile > 0)
            {
                while(UndoneSection > 0)
                {
                    int i = GetIndexOfMax(mas);
                    mas[i].Copy(f);
                    UndoneSection = GetUndoneSection(mas);
                }

                foreach (Sequence s in mas)
                {
                    s.StartRun(lenght);
                }
                UndoneSection = GetUndoneSection(mas);
                UndoneFile = GetUndoneFile(mas);
            }

            foreach (Sequence s in mas)
            {
                s.Dispose();
            }
            f.Dispose();
        }

        private static int GetIndexOfMax(Sequence[] mas)
        {
            int max = 0;
            while (mas[max].EndOfSection)
                max++;

            for (int j = max + 1; j < mas.Length; j++)
            {
                if (!mas[j].EndOfSection && (mas[j].element > mas[max].element))
                    max = j;
            }
            return max;
        }

        private static int GetUndoneSection(Sequence[] mas)
        {
            int sum = 0;
            foreach(Sequence s in mas)
            {
                if (!s.EndOfSection)
                    sum++;
            }
            return sum;
        }

        private static int GetUndoneFile(Sequence[] mas)
        {
            int sum = 0;
            foreach (Sequence s in mas)
            {
                if (!s.EndOfFile)
                    sum++;
            }
            return sum;
        }

        public static void SortFile(string FileName)
        {
            AmountOfPasses = 0;
            FileInfo file = new FileInfo(FileName);
            long n = file.Length / 4; //length in bytes, int32 take 4 bytes, n is the amount of numbers
            Sequence[] mas = new Sequence[3];
            Sequence f = new Sequence(FileName);
            
            for (int i = 0; i < mas.Length; i++)
            {
                string name = "Temp" + i.ToString();
                mas[i] = new Sequence(name);
            }
            int lenght = 1;
            do
            {
                Distribute(f, lenght, mas);
                AmountOfPasses += 1;
                Merge(f, lenght, mas);
                AmountOfPasses += 3;
                lenght *= mas.Length;
            }
            while (lenght < n);
            foreach (Sequence s in mas)
            {
                s.file.Delete();
                s.Dispose();
            }
            f.file.Delete();
            f.Dispose();
        }

    }
}
