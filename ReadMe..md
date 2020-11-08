# Handlers

## General use handlers with DI config

### See Tests project for full example

1. Create an interface for the message

    ```csharp
    public interface IMessage { }
    ```

2. add a class implementing that interface

    ```csharp
    public class SpecialMessage : IMessage {

        public SpecialMessage(string text) { Text = text; }
        public string Text{ get; }
    }
   ```

3. add a handler for the message

    ```csharp
    public class SpecialHandler : IHandler<SpecialMessage> {

        // inject services
        public SpecialHandler(
            IDisplayService service
        )
        {
            _service = service;
        }

        readonly IDisplayService _service;

        // handle the message
        public async Task HandleAsync(
            SpecialMessage message)
        {
            await _service.DisplayAsync(
                $"{message.text}\nHandled with ♥ in SpecialHandler");
        }
    }
    ```

4. register the handlers

    ```csharp
    public class Startup
    {
        ...

        public void ConfigureServices(IServiceCollection services)
        {
            services
                AddHandlers<IMessage>();
        }

        ...
    }
    ```

5. a controller

    ```csharp
    public class Controller
    {
        readonly Executor<IMessage> _executor;

        // inject Executor, which will contain all registered handlers
        public Controller(
            Executor<IMessage> executor)
        {
            _executor = executor;
        }

        protected void Message(IMessage message)
        {
            _executor.Execute(message);
        }
    }

    ```
