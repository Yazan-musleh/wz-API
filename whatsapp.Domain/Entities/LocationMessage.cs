namespace whatsapp.Domain.Entities;

public partial class LocationMessage
{
    public long Id { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string? Address { get; set; }

    public virtual Message Message { get; set; } = null!;
}
