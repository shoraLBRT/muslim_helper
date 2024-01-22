using System;
using System.Collections.Generic;

namespace muslim_helper.Entites;

public partial class UsersTable
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public int Chatid { get; set; }

    public bool NamazNotification { get; set; }

    public bool TaskTracking { get; set; }

    public virtual ICollection<TaskTrackingTable> TaskTrackingTables { get; set; } = new List<TaskTrackingTable>();
}
