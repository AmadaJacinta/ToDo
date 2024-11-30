using System;
using ToDoList.Repositories;
using ToDoList.Services;
using ToDoList.UI;

namespace ToDoList
{
    class Program
    {
        static void Main(string[] args)
        {
            // Путь к файлу для хранения данных
            string filePath = "tasks.json"; // Или любой другой путь

            // Создаем экземпляры репозитория, сервиса и UI
            ITaskRepositories taskRepository = new FileTaskRepository(filePath);
            TaskService taskService = new TaskService(taskRepository);
            ConsoleUI ui = new ConsoleUI(taskService);

            // Запускаем пользовательский интерфейс
            ui.Run();


            Console.WriteLine("Press any key to exit."); //added to view the result in VS
            Console.ReadKey();
        }
    }
}