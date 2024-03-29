﻿using System.Globalization;
using WOF.Application.TodoLists.Queries.ExportTodos;
using CsvHelper.Configuration;

namespace WOF.Infrastructure.Files.Maps;

public class TodoItemRecordMap : ClassMap<TodoItemRecord>
{
    public TodoItemRecordMap()
    {
        AutoMap(CultureInfo.InvariantCulture);

        Map(m => m.Done).Convert(c => c.Value.Done ? "Yes" : "No");
    }
}
