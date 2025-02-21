using System;
using System.Collections.Generic;

namespace whatsapp.Domain.Entities;

public partial class Message
{
    public long Id { get; set; }

    public string WaMessageId { get; set; } = null!;

    public string Sender { get; set; } = null!;

    public string Receiver { get; set; } = null!;

    public string? MessageType { get; set; }

    public DateTime LocalTime { get; set; }

    public virtual LocationMessage? LocationMessage { get; set; }

    public virtual TextMessage? TextMessage { get; set; }
}
