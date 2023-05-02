using WOF.Application.Common.Mappings;
using WOF.Domain.Entities;

namespace WOF.Application.TodoLists.Queries.ExportTodos;

public class TodoItemRecord : IMapFrom<TodoItem>
{
    public string? Title { get; set; }

    public bool Done { get; set; }
}
