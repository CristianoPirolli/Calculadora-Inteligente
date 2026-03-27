namespace CalculadoraInteligente.Application.Updates;

public sealed class UpdateConfig
{
    public bool Enabled { get; init; } = false;
    public string ManifestUrl { get; init; } = string.Empty;
    public int TimeoutSeconds { get; init; } = 8;
    public bool PromptBeforeInstall { get; init; } = true;
}
