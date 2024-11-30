using System;

namespace ToDoList.Models
{
    public class ToDoTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public Priority TaskPriority { get; set; }

        public enum Priority
        {
            Low,
            Medium,
            High
        }

        public ToDoTask(string title, string description, DateTime? dueDate, Priority priority)
        {
            try
            {
                if(Title == null) throw new ArgumentNullException("Title can`t be null");

                Title = title;
                Description = description;
                DueDate = dueDate;
                TaskPriority = priority;
            }
            catch 
            {
                Console.WriteLine("Please enter task title");
            }
            
        }

        public override string ToString()
        {
            string dueDateString = DueDate.HasValue ? DueDate.Value.ToShortDateString() : "Whithout deadline";
            return $"[{Id}] {Title} - {Description} (Deadline: {dueDateString}, Priority: {TaskPriority})";
        }
    }
}