using Microsoft.Extensions.Logging;
using Microsoft.ML.OnnxRuntime;

namespace PersonaEngine.Lib.Configuration;

/// <summary>
///     Selects the ONNX Runtime execution provider for PE's ONNX-backed models
///     (Kokoro synthesis, Silero VAD). Defaults to CUDA with a CPU fallback so
///     existing deployments behave identically.
/// </summary>
public record OnnxProviderOptions
{
    public OnnxExecutionProvider Provider { get; init; } = OnnxExecutionProvider.Cuda;
}

public enum OnnxExecutionProvider
{
    /// <summary>Try CUDA first; silently fall back to CPU if the provider is unavailable.</summary>
    Cuda,

    /// <summary>Force CPU execution. Saves ~2-3 GB VRAM for the small ONNX models.</summary>
    Cpu
}

public static class OnnxProviderExtensions
{
    public static void ApplyProvider(this SessionOptions sessionOptions, OnnxExecutionProvider provider, ILogger? logger = null)
    {
        if (provider == OnnxExecutionProvider.Cpu)
        {
            logger?.LogInformation("ONNX provider pinned to CPU by configuration");
            return;
        }

        try
        {
            sessionOptions.AppendExecutionProvider_CUDA();
            logger?.LogInformation("CUDA execution provider added successfully");
        }
        catch (Exception ex)
        {
            logger?.LogWarning("CUDA execution provider not available: {Message}. Using CPU.", ex.Message);
        }
    }
}
