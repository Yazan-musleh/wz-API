namespace whatsapp.Domain.Entities;

public partial class TextMessage
{
    public long Id { get; set; }

    public string MessageBody { get; set; } = null!;

    public virtual Message Message { get; set; } = null!;
}
