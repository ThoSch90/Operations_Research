using System;
using System.IO;

namespace Исследование_2
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] strings = File.ReadAllLines("input.txt");
            int M = int.Parse(strings[0].Split(' ')[0]);
            int[,] array = new int[M, M];
            for (int i = 0; i < M; i++)
            {
                string[] line = strings[i + 1].Split(' ');
                for (int j = 0; j < M; j++)
                {
                    array[i, j] = int.Parse(line[j]);
                    Console.Write("{0, 3} ", array[i, j]);
                }
                Console.WriteLine();
            }
            int[] index_of_connected_vertex = new int [M]; //индексы связных вершин
            for (int i = 0; i < M; i++)
                index_of_connected_vertex[i] = -1;
            int[] weight_of_connected_vertex = new int[M]; // веса связных вершин
            int amount_of_connected_vertex = 0;
            int number_of_table = 0;
            int k = M - 1;
            //для самой последней вершины
            for (int j = 0; j < M; j++)
            {
                if (array[j, M - 1] > 0)
                {
                    index_of_connected_vertex[amount_of_connected_vertex] = j;
                    weight_of_connected_vertex[amount_of_connected_vertex] = array[j, k];
                    amount_of_connected_vertex++;
                }
            }
            int[,] matrix = new int[amount_of_connected_vertex, 2];
            using (StreamWriter stream = new StreamWriter("table" + number_of_table))
            {
                stream.WriteLine("{0}", amount_of_connected_vertex);
                for (int i = 0; i < amount_of_connected_vertex; i++)
                {
                    matrix[i, 0] = weight_of_connected_vertex[i];
                    matrix[i, 1] = M - 1;
                    for (int j = 0; j < 2; j++)
                        stream.Write("{0} ", matrix[i, j]);
                    stream.WriteLine();
                }
            }
            int length_of_shortest_way = 0;
            //Обратная прогонка
            while (k != 0)
            {
                string[] pt = File.ReadAllLines("table" + number_of_table);
                int size = int.Parse(pt[0].Split(' ')[0]); // количество вершин с предыдущей таблицы
                int[,] previous_table = new int[size, 2];
                for (int i = 0; i < size; i++)
                {
                    string[] line = pt[i + 1].Split(' ');
                    for (int j = 0; j < 2; j++)
                    {
                        previous_table[i, j] = int.Parse(line[j]);
                        //Console.Write("{0, 3} ", array[i, j]);
                    }
                    //Console.WriteLine();
                }
                amount_of_connected_vertex = 0;
                int[] temp_index = new int[M];
                for (int i = 0; i < M; i++)
                    temp_index[i] = -1;
                int temp = 0;
                for (int i = 0; i < M; i++)
                {
                    if (index_of_connected_vertex[i] == -1) break;
                    for (int j = 0; j < M; j++)
                    {
                        if (array[j, index_of_connected_vertex[i]] > 0)
                        {
                            bool flag = false;
                            for (int z = 0; z < temp_index.Length; z++)
                                if (temp_index[z] == j) flag = true;
                            if (flag == false)
                            {
                                temp_index[temp] = j;
                                temp++;
                                amount_of_connected_vertex++;
                            }
                        }
                    }
                }
                int[,] new_matrix = new int[amount_of_connected_vertex, 2];
                
                for (int i = 0; i < amount_of_connected_vertex; i++)
                {
                    int min = int.MaxValue;
                    int solution = -1;
                    for (int j = 0; j < size; j++)
                    {
                        if (array[temp_index[i], index_of_connected_vertex[j]] > 0)
                            if (array[temp_index[i], index_of_connected_vertex[j]] + previous_table[j, 0] < min) {
                                min = array[temp_index[i], index_of_connected_vertex[j]] + previous_table[j, 0];
                                solution = index_of_connected_vertex[j];
                            }
                    }
                    if (solution != -1)
                    {
                        new_matrix[i, 0] = min;
                        new_matrix[i, 1] = solution;
                    }
                }
                number_of_table++;
                for (int i = 0; i < M; i++)
                {
                    index_of_connected_vertex[i] = temp_index[i];
                }
                using (StreamWriter stream = new StreamWriter("table" + number_of_table))
                {
                    stream.WriteLine("{0}", amount_of_connected_vertex);
                    for (int i = 0; i < amount_of_connected_vertex; i++)
                    {
                        for (int j = 0; j < 2; j++)
                            stream.Write("{0} ", new_matrix[i, j]);
                        stream.WriteLine();
                    }
                }
                int min_index = M - 1;
                for (int i = 0; i < amount_of_connected_vertex; i++)
                {
                    if (index_of_connected_vertex[i] <= min_index)
                        min_index = index_of_connected_vertex[i];
                }
                k = min_index;
                if (k == 0)
                {
                    length_of_shortest_way = new_matrix[0, 0];
                }
            }
            Console.WriteLine("The shortest way is {0}", length_of_shortest_way);
            int[] way = new int[number_of_table + 2];
            way[0] = 1;
            int index_for_way = 1;
            for (int i = number_of_table; i >= 0; i--)
            {
                string[] pt = File.ReadAllLines("table" + i);
                int size = int.Parse(pt[0].Split(' ')[0]); // количество вершин с предыдущей таблицы
                int[,] table = new int[size, 2];
                for (int j = 0; j < size; j++)
                {
                    string[] line = pt[j + 1].Split(' ');
                    for (int z = 0; z < 2; z++)
                    {
                        table[j, z] = int.Parse(line[z]);
                    }
                }
                int min_way = int.MaxValue;
                int min_index = -1;
                for (int j = 0; j < size; j++)
                {
                    if (table[j, 0] < min_way)
                    {
                        min_way = table[j, 0];
                        min_index = table[j, 1];
                    }
                }
                way[index_for_way] = min_index + 1;
                index_for_way++;
            }
            for (int i = 0; i < way.Length; i++)
            {
                if (i == way.Length - 1)
                    Console.Write("{0}", way[i]);
                else
                 Console.Write("{0} -> ", way[i]);
            }
            Console.Read();
        }
    }
}
