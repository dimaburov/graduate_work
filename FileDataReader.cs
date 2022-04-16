using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WpfApp4
{
    class FileDataReader : SetSigFigs
    {
        private int accuracy = 2;
        //Храним путь к файлу
        private String path = "../../DataPhi.txt";
        //Выборочное изменение в файле
        public string selectiveChange(int size_array)
        {
            System.Diagnostics.Process txt = new System.Diagnostics.Process();
            txt.StartInfo.FileName = "notepad.exe";
            txt.StartInfo.Arguments = path;
            txt.Start();

            /*
             * Пока не получается отследить щакрывтие файла и его сохпанение из-з аэтого не получается узнать какая ошибка
             * */
            ////Проверка на правильность размера после изменений
            string error_message = "";
            //List<double> data_array = readerFile();
            ////Проверка на рамер 
            //if (data_array.Count() > size_array) error_message = "Кол-во значений превышает, заданное число";
            ////Проверка на правильность данных

            ////bool chech = bool.Parse(data_array.Select(x => x == 2).ToString());
            ////Console.WriteLine(chech);
            ////Console.WriteLine();

            ////chech = bool.Parse(data_array.Select(x => x == 2).ToString());
            ////Console.WriteLine();

            return error_message;
        }
        //Заполнение файла одинаковыми значениями
        public void fillDataFail(double fill_value, int size_array, int function_param)
        {
            List<double> arry_data = new List<double>();
            double new_value = fill_value;
            Random rnd = new Random();
            for (int i = 0; i < size_array; i++)
            {
                switch (function_param)
                {
                    case 0:
                        Console.WriteLine("fill_value " + Math.Round(fill_value, accuracy));
                        new_value = Math.Round(fill_value, accuracy);
                        arry_data.Add(new_value);
                        break;
                    case 1:
                        Console.WriteLine("fill_value + a " + Math.Round(new_value + fill_value, accuracy));
                        new_value = Math.Round(new_value + fill_value, accuracy);
                        arry_data.Add(new_value);
                        break;
                    case 2:
                        Console.WriteLine("fill_value * a " + Math.Round(new_value * fill_value, accuracy));
                        new_value = Math.Round(new_value * fill_value, accuracy);
                        arry_data.Add(new_value);
                        break;
                    case 3:
                        //Console.WriteLine("sqrt(new_value)" + Math.Round(Math.Sqrt(new_value), accuracy));
                        //new_value = Math.Round(Math.Sqrt(new_value), accuracy);
                        //arry_data.Add(new_value);
                        //break;
                        //Тестовое заполнение случайными значениями
                        fill_value = Math.Round(fill_value);
                        new_value = rnd.Next(0, int.Parse(fill_value.ToString()));
                        arry_data.Add(new_value);
                        break;
                }
            }

            Console.WriteLine(arry_data.Count());
            inputFile(arry_data);
        }
        //Чтение файла
        public List<double> readerFile()
        {
            string line;

            try
            {
                var array = new List<double>();
                var final_array = new List<double>();
                StreamReader reader = new StreamReader(path);
                
                while ((line = reader.ReadLine()) != null)
                {
                    //Обрабаываем линию - будем значения через пробел вставлять в массив
                    array = (line.Split(' ')).Select(x => double.Parse(x)).ToList();
                    foreach (var item in array)
                    {
                        final_array.Add(item);
                    }
                }
                reader.Close();
                return final_array;
            }
            catch(Exception e)
            {
                Console.WriteLine("Exeption: " + e.Message);
                return new List<double> {-9999};
            }
        }
        //Запись значений в файл
        //!!! Ошибка прии 1000 значений записывается 250 строк и выходит ошибка - возможное решение растянуть по 50 элементов в строчку 
        private void inputFile(List<double> array_data)
        {
            String line = "";
            try
            {

                StreamWriter writer = new StreamWriter(path, false);

                //всегда по 10 значений в линии
                int size_row = 0;
                size_row = (array_data.Count() == 1000) ? 100 : 10 ;

                int start_array = 0;
                int size = 0;
                while (array_data.Count() - start_array > 0)
                {
                    size = (array_data.Count() - start_array > size_row) ? size_row : array_data.Count() - start_array;
                    for (int i = start_array; i < start_array + size; i++)
                    {
                        line += array_data[i].ToString();
                        if (i +1 != start_array + size) line += " ";
                    }

                    Console.WriteLine(line);
                    Console.WriteLine(start_array);


                    writer.WriteLineAsync(line);
                    line = "";
                    start_array += size_row;
                }
                   
                writer.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exeption: " + e.Message);
            }
        }
    }
}
