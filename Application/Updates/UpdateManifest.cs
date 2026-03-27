namespace CalculadoraInteligente.Application.Updates;

public sealed class UpdateManifest
{
    public string Version { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public string? Notes { get; init; }
}
