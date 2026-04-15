using PersonaEngine.Lib.Configuration;

namespace PersonaEngine.Lib.ASR.VAD;

public class SileroVadOptions(string modelPath)
{
    public string ModelPath { get; set; } = modelPath;

    public float ThresholdGap { get; set; } = 0.15f;

    public float Threshold { get; set; } = 0.5f;

    public OnnxExecutionProvider Provider { get; set; } = OnnxExecutionProvider.Cuda;
}
