namespace Fatora.Models.Api;

public class PlanDto
{
    public int PlanId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DurationDays { get; set; }
    public int MaxDevices { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreatePlanRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DurationDays { get; set; }
    public int MaxDevices { get; set; }
    public decimal Price { get; set; }
}

public class UpdatePlanRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DurationDays { get; set; }
    public int MaxDevices { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
}
