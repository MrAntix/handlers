# Handlers

## General use handlers with DI config

### See Tests project for full example

1. Create an interface for the data

   ```csharp
   public interface IData { }
   ```

2. add a class implementing that interface

   ```csharp
   public class SpecialMessageData : IData {

       public SpecialMessageData(string text) { Text = text; }
       public string Text{ get; }
   }
   ```

3. add a handler for the data

   ```csharp
   public class SpecialMessageHandler : IHandler<SpecialMessageData> {

       public static string Template(string text)
           =>  $"{text}\nHandled with ♥ in SpecialMessageHandler";

       // inject services
       public SpecialMessageHandler(
           IDisplayService service
       )
       {
           _service = service;
       }

       readonly IDisplayService _service;

       // handle the data
       public async Task HandleAsync(
           SpecialMessageData data)
       {
           await _service.DisplayAsync(
              SpecialMessageHandler.Template(data.text)
              );
       }
   }
   ```

4. a controller/service that receives data to be handled

   ```csharp
   public class Controller
   {
       readonly Executor<IData> _executor;

       // inject Executor, which will contain all registered handlers
       public Controller(
           Executor<IData> executor)
       {
           _executor = executor;
       }

       protected Task PostAsync(IData data)
       {
           try {

               await _executor.ExecuteAsync(data);

           } catch(HandlerException<IData> ex) {

               // handle errors
               throw;
           }
       }
   }

   ```

5. register the handlers and contoller

   ```csharp
   public static class DataConfiguration
   {
       ...

       public IServiceCollection AddDataServices(
           this IServiceCollection services)
       {
           return services
               .AddTransient<Controller>()
               .AddHandlers<IData>();
       }

       ...
   }
   ```

6. Test

   ```csharp
   [TestMethod]
   public Task Calls_Handler() {

       const string message = "Hello Handler";

       var mock = new Mock<IDisplayService>(); // note, any old mocking lib
       mock.Setup(o => o.DisplayAsync(It.IsAny(string))).ReturnsAsync();

       var serviceProvider = new ServiceCollection()
               .AddSingleton<IDisplayService>(mock.Object)
               .AddDataSerices()
               .BuildServiceProvider();

       var controller - serviceProvider.GetRequiredService<Controller>();

       await controller.PostAsync(new SpecialMessageData(message));

       var expected = SpecialMessageHandler.Template(message);
       mock.Verify(o => o.DisplayAsync(expected), Times.Once());
   }
   ```

## Notes

1. You can only have one handler per implementation    
    i.e. SpecialMessageData [1:1] SpecialMessageHandler

2. You can register as many handlers by as many interfaces as you need

   ```csharp
   return services
           .AddHandlers<IDataOne>()
           .AddHandlers<IDataTwo>()
           .AddHandlers<IDataThree>();
   ```
