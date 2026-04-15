using PersonaEngine.Lib;

namespace PersonaEngine.Lib.Configuration;

/// <summary>
///     Maps the user-facing <see cref="WhisperModel"/> config enum to the internal
///     <see cref="ModelType"/> embedded-resource enum. Isolated here so the internal
///     enum can grow without breaking the public configuration surface.
/// </summary>
public static class WhisperModelMap
{
    public static ModelType ToModelType(WhisperModel model) => model switch
    {
        WhisperModel.Turbov3 => ModelType.WhisperGgmlTurbov3,
        WhisperModel.Tiny    => ModelType.WhisperGgmlTiny,
        _ => throw new ArgumentOutOfRangeException(nameof(model), model, "Unknown WhisperModel")
    };
}
