Here's an example of how I would build a WebAPI for Todos using .NET 9 with Clean Architecture.

**Project Structure**
```markdown
TodoApi/
|--- TodoApi.csproj
|--- Domain/
    |--- Todo.cs
    |--- ITodoRepository.cs
|--- Infrastructure/
    |--- EFCore/
        |--- TodoDbContext.cs
    |--- Repository/
        |--- EfCoreTodoRepository.cs
|--- Application/
    |--- ITodoService.cs
    |--- TodoService.cs
|--- Presentation/
    |--- Controllers/
        |--- TodosController.cs
|--- Startup.cs
```
**Domain Layer**
```csharp
// Domain/Todo.cs
public class Todo : EntityBase<int>
{
    public string Title { get; set; }
    public bool IsCompleted { get; set; }

    public Todo(string title)
    {
        Title = title;
    }
}

// Domain/ITodoRepository.cs
public interface ITodoRepository
{
    Task<Todo> GetByIdAsync(int id);
    Task<IEnumerable<Todo>> GetAllTodosAsync();
    Task AddTodoAsync(Todo todo);
    Task UpdateTodoAsync(Todo todo);
    Task DeleteTodoAsync(int id);
}
```
**Infrastructure Layer**
```csharp
// Infrastructure/EFCore/TodoDbContext.cs
public class TodoDbContext : DbContext
{
    public DbSet<Todo> Todos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TodosApiDb;");
    }
}

// Infrastructure/Repository/EfCoreTodoRepository.cs
public class EfCoreTodoRepository : ITodoRepository
{
    private readonly TodoDbContext _context;

    public EfCoreTodoRepository(TodoDbContext context)
    {
        _context = context;
    }

    public async Task<Todo> GetByIdAsync(int id)
    {
        return await _context.Todos.FindAsync(id);
    }

    public async Task<IEnumerable<Todo>> GetAllTodosAsync()
    {
        return await _context.Todos.ToListAsync();
    }

    public async Task AddTodoAsync(Todo todo)
    {
        await _context.Todos.AddAsync(todo);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTodoAsync(Todo todo)
    {
        _context.Entry(todo).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTodoAsync(int id)
    {
        var todo = await GetByIdAsync(id);
        if (todo != null)
        {
            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
        }
    }
}
```
**Application Layer**
```csharp
// Application/ITodoService.cs
public interface ITodoService
{
    Task<Todo> GetByIdAsync(int id);
    Task<IEnumerable<Todo>> GetAllTodosAsync();
    Task AddTodoAsync(Todo todo);
    Task UpdateTodoAsync(Todo todo);
    Task DeleteTodoAsync(int id);
}

// Application/TodoService.cs
public class TodoService : ITodoService
{
    private readonly ITodoRepository _repository;

    public TodoService(ITodoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Todo> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Todo>> GetAllTodosAsync()
    {
        return await _repository.GetAllTodosAsync();
    }

    public async Task AddTodoAsync(Todo todo)
    {
        await _repository.AddTodoAsync(todo);
    }

    public async Task UpdateTodoAsync(Todo todo)
    {
        await _repository.UpdateTodoAsync(todo);
    }

    public async Task DeleteTodoAsync(int id)
    {
        await _repository.DeleteTodoAsync(id);
    }
}
```
**Presentation Layer**
```csharp
// Presentation/Controllers/TodosController.cs
[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly ITodoService _service;

    public TodosController(ITodoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Todo>>> GetAllTodosAsync()
    {
        return await _service.GetAllTodosAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Todo>> GetByIdAsync(int id)
    {
        return await _service.GetByIdAsync(id);
    }

    [HttpPost]
    public async Task<ActionResult<Todo>> AddTodoAsync(Todo todo)
    {
        return await _service.AddTodoAsync(todo);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTodoAsync(int id, Todo todo)
    {
        await _service.UpdateTodoAsync(todo);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTodoAsync(int id)
    {
        await _service.DeleteTodoAsync(id);
        return NoContent();
    }
}
```
**Startup.cs**
```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<TodoDbContext>(options =>
            options.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TodosApiDb;"));
        services.AddTransient<ITodoRepository, EfCoreTodoRepository>();
        services.AddTransient<ITodoService, TodoService>();
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // ...
    }
}
```
This is a basic example of how to build a WebAPI for Todos using .NET 9 with Clean Architecture. The project structure consists of four layers: Domain, Infrastructure, Application, and Presentation.

The Domain layer contains the Todo entity and the ITodoRepository interface.

The Infrastructure layer contains the EFCoreTodoRepository class that implements the ITodoRepository interface.

The Application layer contains the ITodoService interface and the TodoService class that implements the ITodoService interface.

The Presentation layer contains the TodosController class that handles HTTP requests and returns HTTP responses.

In the Startup.cs file, we configure the services and add the controllers to the pipeline.