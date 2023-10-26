namespace DataAggregation.Domain;

public abstract class DbModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
}