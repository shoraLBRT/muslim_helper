using System;
using System.Collections.Generic;

namespace muslim_helper.Entites;

public partial class TaskTypesTable
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<TaskTrackingTable> TaskTrackingTables { get; set; } = new List<TaskTrackingTable>();
}
