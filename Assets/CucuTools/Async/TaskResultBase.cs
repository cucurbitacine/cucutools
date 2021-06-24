using System.Threading.Tasks;

namespace CucuTools.Async
{
    public abstract class TaskResultBase<T>
        where T : Task
    {
        public T Task { get;  }
        public bool TimeOut { get; }
        public int Milliseconds { get; }

        public TaskResultBase(T task, bool timeOut, int milliseconds)
        {
            Task = task;
            TimeOut = timeOut;
            Milliseconds = milliseconds;
        }
    }
    
    public class TaskResult : TaskResultBase<Task>
    {
        public static explicit operator Task(TaskResult tr) => tr.Task;

        public TaskResult(Task task, bool timeOut, int milliseconds) : base(task, timeOut, milliseconds)
        {
        }
    }
    
    public class TaskResult<T> : TaskResultBase<Task<T>>
    {
        public static explicit operator Task<T>(TaskResult<T> tr) => tr.Task;

        public T Result => Task.Result;
        
        public TaskResult(Task<T> task, bool timeOut, int milliseconds) : base(task, timeOut, milliseconds)
        {
        }
    }
}