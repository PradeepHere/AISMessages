namespace AISMessages.models;

public class AISMessage
{
    public int imoNumber { get; set; }

    public string? timestamp { get; set; }

    public decimal latitude { get; set; }

    public decimal longitude { get; set; }
}
