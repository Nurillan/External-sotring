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

            int i;
            while (!f.EndOfFile())
            {
                i = 0;
                while (!f.EndOfFile() && i < mas.Length)
                {
                    f.CopyAll(mas[i]);
                    f.NewRun(lenght);
                    i++;
                }
            }

            f.StopRead();
            foreach (Sequence s in mas)
            {
                s.StopWrite();
            }
        }

        public static void Merge(Sequence f, int lenght, params Sequence[] mas)
        {
            f.StartWrite();
            foreach (Sequence s in mas)
            {
                s.StartRead(lenght);
            }
            
            while(FirstUndone(mas) > -1)
            {
                while(FirstUndone(mas) > -1)
                {
                    int i = GetIndexOfMax(mas);
                    mas[i].Copy(f);
                }

                foreach (Sequence s in mas)
                {
                    s.NewRun(lenght);
                }
            }

            foreach (Sequence s in mas)
            {
                s.StopRead();
            }
            f.StopWrite();
        }

        private static int GetIndexOfMax(Sequence[] mas)
        {
            int max = FirstUndone(mas);
            if (max == -1)
                throw new Exception("no matching items");

            for (int j = max + 1; j < mas.Length; j++)
            {
                if ((mas[j].EndOfSeries() == false) && (mas[j].Element > mas[max].Element))
                    max = j;
            }
            return max;
        }

        private static int FirstUndone(Sequence[] mas)
        {
            int first = 0;
            while (first < mas.Length && mas[first].EndOfSeries() == true)
                first++;
            if (first == mas.Length)
                return -1;
            else return first;
        }

        public static void SortFile(string FileName)
        {
            AmountOfPasses = 0;
            FileInfo file = new FileInfo(FileName);
            long n = file.Length / 4; //length in bytes, int32 take 4 bytes, n is the amount of numbers
            Sequence f = new Sequence(FileName);
            Sequence[] mas = InitFiles(3);

            int lenght = 1;
            do
            {
                Distribute(f, lenght, mas);
                Merge(f, lenght, mas);
                lenght *= mas.Length;
                AmountOfPasses += 1;
            }
            while (lenght < n);

            foreach (Sequence s in mas)
            {
                s.File.Delete();
                s.Dispose();
            }
            f.File.Delete();
            f.Dispose();
        }

        private static Sequence[] InitFiles(int count)
        {
            Sequence[] mas = new Sequence[count];
            string name;
            for (int i = 0; i < count; i++)
            {
                name = "Temp" + i.ToString();
                mas[i] = new Sequence(name);
            }
            return mas;
        }

    }
}
