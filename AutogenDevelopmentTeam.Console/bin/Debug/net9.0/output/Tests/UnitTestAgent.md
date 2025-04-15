Here are some unit tests for the code using xunit and NSubstitute:

**Domain Layer**

```csharp
// Domain/TodoTests.cs
using Xunit;
using Moq;

public class TodoTests
{
    [Fact]
    public void Constructor_SetsTitle()
    {
        // Arrange
        var title = "Test Title";

        // Act
        var todo = new Todo(title);

        // Assert
        Assert.Equal(title, todo.Title);
    }
}
```

**Infrastructure Layer**

```csharp
// Infrastructure/EFCore/TodoDbContextTests.cs
using Xunit;
using NSubstitute;

public class TodoDbContextTests
{
    [Fact]
    public async Task GetByIdAsync_ReturnsTodo()
    {
        // Arrange
        var id = 1;
        var todo = new Todo("Test Title");
        var dbContext = Substitute.For<TodoDbContext>();
        dbContext.Todos.Find(id).Returns(todo);

        // Act
        var result = await dbContext.GetByIdAsync(id);

        // Assert
        Assert.Equal(todo, result);
    }

    [Fact]
    public async Task GetAllTodosAsync_ReturnsTodos()
    {
        // Arrange
        var todos = new[] { new Todo("Test Title"), new Todo("Test Title 2") };
        var dbContext = Substitute.For<TodoDbContext>();
        dbContext.Todos.ToList().Returns(todos);

        // Act
        var result = await dbContext.GetAllTodosAsync();

        // Assert
        Assert.Equal(todos, result);
    }

    [Fact]
    public async Task AddTodoAsync_AddsTodo()
    {
        // Arrange
        var todo = new Todo("Test Title");
        var dbContext = Substitute.For<TodoDbContext>();

        // Act
        await dbContext.AddTodoAsync(todo);

        // Assert
        await dbContext.Received(1).Todos.AddAsync(todo);
    }

    [Fact]
    public async Task UpdateTodoAsync_UpdatesTodo()
    {
        // Arrange
        var todo = new Todo("Test Title");
        var dbContext = Substitute.For<TodoDbContext>();

        // Act
        await dbContext.UpdateTodoAsync(todo);

        // Assert
        await dbContext.Received(1).Entry(todo).State.Set(EntityState.Modified);
    }

    [Fact]
    public async Task DeleteTodoAsync_DeletesTodo()
    {
        // Arrange
        var id = 1;
        var dbContext = Substitute.For<TodoDbContext>();

        // Act
        await dbContext.DeleteTodoAsync(id);

        // Assert
        await dbContext.Received(1).Todos.Remove(await dbContext.Todos.FindAsync(id));
    }
}
```

**Application Layer**

```csharp
// Application/TodoServiceTests.cs
using Xunit;
using NSubstitute;

public class TodoServiceTests
{
    [Fact]
    public async Task GetByIdAsync_ReturnsTodo()
    {
        // Arrange
        var id = 1;
        var todoRepository = Substitute.For<ITodoRepository>();
        var todoService = new TodoService(todoRepository);

        // Act
        var result = await todoService.GetByIdAsync(id);

        // Assert
        await todoRepository.Received(1).GetByIdAsync(id);
    }

    [Fact]
    public async Task GetAllTodosAsync_ReturnsTodos()
    {
        // Arrange
        var todos = new[] { new Todo("Test Title"), new Todo("Test Title 2") };
        var todoRepository = Substitute.For<ITodoRepository>();
        var todoService = new TodoService(todoRepository);

        // Act
        var result = await todoService.GetAllTodosAsync();

        // Assert
        await todoRepository.Received(1).GetAllTodosAsync();
    }

    [Fact]
    public async Task AddTodoAsync_AddsTodo()
    {
        // Arrange
        var todo = new Todo("Test Title");
        var todoRepository = Substitute.For<ITodoRepository>();
        var todoService = new TodoService(todoRepository);

        // Act
        await todoService.AddTodoAsync(todo);

        // Assert
        await todoRepository.Received(1).AddTodoAsync(todo);
    }

    [Fact]
    public async Task UpdateTodoAsync_UpdatesTodo()
    {
        // Arrange
        var todo = new Todo("Test Title");
        var todoRepository = Substitute.For<ITodoRepository>();
        var todoService = new TodoService(todoRepository);

        // Act
        await todoService.UpdateTodoAsync(todo);

        // Assert
        await todoRepository.Received(1).UpdateTodoAsync(todo);
    }

    [Fact]
    public async Task DeleteTodoAsync_DeletesTodo()
    {
        // Arrange
        var id = 1;
        var todoRepository = Substitute.For<ITodoRepository>();
        var todoService = new TodoService(todoRepository);

        // Act
        await todoService.DeleteTodoAsync(id);

        // Assert
        await todoRepository.Received(1).DeleteTodoAsync(id);
    }
}
```

**Presentation Layer**

```csharp
// Presentation/TodosControllerTests.cs
using Xunit;
using NSubstitute;

public class TodosControllerTests
{
    [Fact]
    public async Task Get_ReturnsTodos()
    {
        // Arrange
        var todos = new[] { new Todo("Test Title"), new Todo("Test Title 2") };
        var todoRepository = Substitute.For<ITodoRepository>();
        var controller = new TodosController(todoRepository);

        // Act
        var result = await controller.Get();

        // Assert
        await todoRepository.Received(1).GetAllTodosAsync();
    }

    [Fact]
    public async Task Post_AddsTodo()
    {
        // Arrange
        var todo = new Todo("Test Title");
        var todoRepository = Substitute.For<ITodoRepository>();
        var controller = new TodosController(todoRepository);

        // Act
        await controller.Post(todo);

        // Assert
        await todoRepository.Received(1).AddTodoAsync(todo);
    }

    [Fact]
    public async Task Put_UpdatesTodo()
    {
        // Arrange
        var id = 1;
        var todo = new Todo("Test Title");
        var todoRepository = Substitute.For<ITodoRepository>();
        var controller = new TodosController(todoRepository);

        // Act
        await controller.Put(id, todo);

        // Assert
        await todoRepository.Received(1).UpdateTodoAsync(todo);
    }

    [Fact]
    public async Task Delete_DeletesTodo()
    {
        // Arrange
        var id = 1;
        var todoRepository = Substitute.For<ITodoRepository>();
        var controller = new TodosController(todoRepository);

        // Act
        await controller.Delete(id);

        // Assert
        await todoRepository.Received(1).DeleteTodoAsync(id);
    }
}
```

This is not an exhaustive list of unit tests, but it should give you a good starting point. Remember to test the happy path and error cases for each method. Also, make sure to use mocking libraries like NSubstitute or Moq to isolate dependencies.