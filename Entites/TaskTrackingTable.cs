using System;
using System.Collections.Generic;

namespace muslim_helper.Entites;

public partial class TaskTrackingTable
{
    public int Id { get; set; }

    public int TaskTypesId { get; set; }

    public int UserId { get; set; }

    public DateOnly CompletionDate { get; set; }

    public bool CompletionCheck { get; set; }

    public bool ObligationBool { get; set; }

    public virtual TaskTypesTable TaskTypes { get; set; } = null!;

    public virtual UsersTable User { get; set; } = null!;
}
