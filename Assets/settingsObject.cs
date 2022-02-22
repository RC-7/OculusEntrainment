using System;
[Serializable]
public class SettingsObject
{
    public Visual visual;
    public Audio audio;
    public Neurofeedback neurofeedback;
}
[Serializable]
public class Visual
{
    public string colour;
    public string frequency;
}
[Serializable]
public class Audio
{
    public string baseFrequency;
    public string entrainmentFrequency;
}
[Serializable]
public class Neurofeedback
{
    public string colour;
}