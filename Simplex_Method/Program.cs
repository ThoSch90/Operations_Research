using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Исследование_операций1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] strings = File.ReadAllLines("input.txt");
            int M = int.Parse(strings[0].Split(' ')[0]);
            int N = int.Parse(strings[0].Split(' ')[1]);
            float[,] array = new float[M, N];
            for (int i = 0; i < M; i++)
            {
                string[] line = strings[i + 1].Split(' ');
                for (int j = 0; j < N; j++)
                {
                    array[i, j] = float.Parse(line[j]);
                    Console.Write("{0, 3} ", array[i, j]);
                }
                Console.WriteLine();
            }
            bool flag = true;
            for (int i = 0; i < N; i++)
            {
                if (array[0, i] < 0) flag = false;
            }
            if (flag == true)
            {
                Console.WriteLine("That's all");
                return;
            }
            
            while (flag == false)
            {
                float min_z = float.MaxValue;
                int index_for_maxz_line = -1;
                for (int i = 0; i < N; i++)
                {
                    if (array[0, i] < min_z)
                    {
                        min_z = array[0, i];
                        index_for_maxz_line = i; // индекс столбца!!, вставляемой в базис
                    }
                }
                float min_of_base = float.MaxValue;
                int index_for_new_base_line = -1;
                for (int i = 1; i < M; i++)
                {
                    if (array[i, index_for_maxz_line] != 0)
                    {
                        float temp = array[i, N - 1] / array[i, index_for_maxz_line];
                        if (temp >= 0 && temp < min_of_base)
                        {
                            min_of_base = temp;
                            index_for_new_base_line = i;
                        }
                    }
                }
                //Пересечение строки и столбца - ведущий элемент
                float main_elem = array[index_for_new_base_line, index_for_maxz_line];
                
                //Новая ведущая строка
                for (int i = 0; i < N; i++)
                {
                    array[index_for_new_base_line, i] = array[index_for_new_base_line, i] / main_elem;
                }
                //main_elem = array[index_for_new_base_line, index_for_maxz_line];
                for (int i = 0; i < M; i++)
                {
                    float main_elem_in_line = array[i, index_for_maxz_line];
                    //Console.Write("{0} ", main_elem_in_line);
                    if (i != index_for_new_base_line)
                    {
                        for (int j = 0; j < N; j++)
                        {
                            array[i, j] = array[i, j] - main_elem_in_line * array[index_for_new_base_line, j];
                            //Console.Write("{0} ", array[i, j]);
                        }
                    }
                }
                flag = true;
                for (int i = 0; i < N; i++)
                {
                    if (array[0, i] < 0) flag = false;
                }
            }
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Console.Write("{0, 5:0.##} ", array[i, j]);
                }
                Console.WriteLine();
            }
            Console.Read();
        }
    }
}
