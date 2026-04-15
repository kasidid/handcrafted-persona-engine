using Whisper.net;

namespace PersonaEngine.Lib.Configuration;

public record AsrConfiguration
{
    public WhisperConfigTemplate TtsMode { get; init; } = WhisperConfigTemplate.Performant;

    public string TtsPrompt { get; init; } = string.Empty;

    public float VadThreshold { get; init; } = 0.5f;

    public float VadThresholdGap { get; init; } = 0.15f;

    public float VadMinSpeechDuration { get; init; } = 250f;

    public float VadMinSilenceDuration { get; init; } = 200f;

    /// <summary>Primary Whisper model used once the fallback hands off.</summary>
    public WhisperModel WhisperPrimary { get; init; } = WhisperModel.Turbov3;

    /// <summary>Fast bootstrap Whisper model used while the primary warms up.</summary>
    public WhisperModel WhisperFallback { get; init; } = WhisperModel.Tiny;
}

public enum WhisperModel
{
    /// <summary>ggml-large-v3-turbo.bin — best accuracy, ~2 GB VRAM.</summary>
    Turbov3,

    /// <summary>ggml-tiny.en.bin — fastest/smallest, English only.</summary>
    Tiny
}

public enum WhisperConfigTemplate
{
    Performant,

    Balanced,

    Precise
}

public static class WhisperConfigTemplateExtensions
{
    public static WhisperProcessorBuilder ApplyTemplate(this WhisperProcessorBuilder builder, WhisperConfigTemplate template)
    {
        switch ( template )
        {
            case WhisperConfigTemplate.Performant:
                var sampleBuilderA = (GreedySamplingStrategyBuilder)builder.WithGreedySamplingStrategy();
                sampleBuilderA.WithBestOf(1);

                builder = sampleBuilderA.ParentBuilder;
                builder.WithStringPool();

                break;
            case WhisperConfigTemplate.Balanced:
                var sampleBuilderB = (BeamSearchSamplingStrategyBuilder)builder.WithBeamSearchSamplingStrategy();
                sampleBuilderB.WithBeamSize(2);
                sampleBuilderB.WithPatience(1f);

                builder = sampleBuilderB.ParentBuilder;
                builder.WithStringPool();
                builder.WithTemperature(0.0f);

                break;
            case WhisperConfigTemplate.Precise:
                var sampleBuilderC = (BeamSearchSamplingStrategyBuilder)builder.WithBeamSearchSamplingStrategy();
                sampleBuilderC.WithBeamSize(5);
                sampleBuilderC.WithPatience(1f);

                builder = sampleBuilderC.ParentBuilder;
                builder.WithStringPool();
                builder.WithTemperature(0.0f);

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(template), template, null);
        }

        return builder;
    }
}