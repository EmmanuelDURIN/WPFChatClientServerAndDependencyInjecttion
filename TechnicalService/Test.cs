//using Autofac;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace TechnicalService
//{
//  class Test
//  {

//    class TaskManager
//    static void Main(string[] args)
//    {
//      // Le conteneur contient les types
//      // et résoud les dépendances
//      var builder = new ContainerBuilder();
//      builder.Register(c => new TaskManager(c.Resolve<ITaskRepository>()));
//      builder.RegisterType<TaskManager>();
//      builder.RegisterInstance(new TaskManager());
//      builder.RegisterAssemblyTypes(managerAssembly);
//      var container = builder.Build();

//      var taskManager = container.Resolve<TaskManager>();
//      var taskRepository = container.Resolve<ITaskRepository>();
//    }

//    private interface ITaskRepository
//    {
//    }
//    private class TaskController
//    {
//      public TaskManager TaskManager { get; set; }

//      public TaskController(TaskManager taskManager)
//      {
//        this.TaskManager = taskManager;
//      }
//    }
//    private class TaskManager
//    {
//      private ITaskRepository taskRepository;

//      public TaskManager(ITaskRepository taskRepository)
//      {
//        this.taskRepository = taskRepository;
//      }
//    }
//  }
//}
