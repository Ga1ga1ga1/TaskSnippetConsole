using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TaskSnippetConsole
{
    //Чтобы метод мог использоваться как асинхр.,
    //Он должен возвращать Task, Task<>, void.
    //Task - спец. класс (using System.Threading.Tasks)
    class Program
    {
        //(*)Локер. Объект синхронизации (обязательно ссылочный тип).
        //Глобальный общий объект, который будет служить "переключателем" для участка кода.
        //Чтобы участок кода был доступен только для одного потока.
        //Мы участок оборачиваем в lock (стр.59).
        public static object locker = new object();

        //1 Основной поток начинается в Main 
        static void Main(string[] args)
        {
            #region async/await
            /*Console.WriteLine("Begin Main");
            //Синхронно вызываем асинхр метод, описанный ниже
            //2 Поток идет в DoWorkAsync
            DoWorkAsync(40);
            Console.WriteLine("Continue Main");

            int j = 0;
            for (int i = 0; i < 20; i++)
            {
                j++;

                if (j % 2 == 0)
                {
                    Console.WriteLine("Main");
                }
            }
            Console.WriteLine("End Main");*/
            #endregion

            var result = SaveDataAsync(path: "C:\\repos\\test.txt");
            var input = Console.ReadLine();
            Console.WriteLine(result.Result);
            Console.ReadLine();
        }

        //асинхронная обёртка для метода SaveData
        static async Task<bool> SaveDataAsync(string path)
        {
            var result = await Task.Run(() => SaveData(path));
            return result;
        }

        static bool SaveData(string path)
        {
            lock (locker)
            {
                var rnd = new Random();
                var text = "";
                for (int i = 0; i < 30000; i++)
                {
                    text += rnd.Next();
                }
            }

            using (var sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                sw.WriteLine();
            }
            return true;
        }

        //Делаем асинхронный метод-обёртку над методом.
        //async и await - ключевые слова
        static async Task DoWorkAsync(int max)
        {
            Console.WriteLine("Begin Async");
            //вызов самого метода
            //3 в await запускается новый поток, Console.WriteLine() останавливается,
            //пока не выполнится Task DoWork(), DoWorkAsync не продолжит работу.
            //Но при этом основной поток продолжает работу, Main не блокируется.
            await Task.Run(() => DoWork(max));
            Console.WriteLine("End Async");

        }

        static void DoWork(int max)
        {
            int j = 0;
            for (int i = 0; i < max; i++)
            {
                j++;

                if (j % 2 == 0)
                {
                    Console.WriteLine("Do Work");
                }
            }
        }
    }
}
