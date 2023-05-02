using WOF.Application.TodoLists.Queries.ExportTodos;

namespace WOF.Application.Common.Interfaces;

public interface ICsvFileBuilder
{
    byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
}
