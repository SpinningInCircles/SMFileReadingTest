using System.Collections.Generic;

namespace SMFileReadingTest;

public class Songs
{
    // Title, Artist
    public string Title { get; set; }
    public string Artist { get; set; } = string.Empty;
    
    // Subtitle
    public string Subtitle { get; set; } = string.Empty;
    
    // Transliteration
    public string TitleTransliteration { get; set; } = string.Empty;
    public string ArtistTransliteration { get; set; } = string.Empty;
    public string SubtitleTransliteration { get; set; } = string.Empty;
    
    // Songs details
    public string Genre { get; set; } = string.Empty;
    public string Credit { get; set; } = string.Empty;
    public string BannerPath { get; set; } = string.Empty;
    public string BackgroundPath { get; set; } = string.Empty;
    public string LyricsPath { get; set; } = string.Empty;
    public string CDTitlePath { get; set; } = string.Empty;
    public string MusicPath { get; set; } = string.Empty;
    public string BPMs { get; set; } = string.Empty;
    
    // Numeric details
    public double? Version { get; set; }
    public double? Offset { get; set; }
    public double? SampleStart { get; set; }
    public double? SampleLength { get; set; }
    
    
    // Gameplay details
    public string Selectable { get; set; } = string.Empty;
    public string Stops { get; set; } = string.Empty;
    public string Freezes { get; set; } = string.Empty;
    public string Delays { get; set; } = string.Empty;
    public string BGChanges { get; set; } = string.Empty;
    public string Keysounds { get; set; } = string.Empty;
    public string Attacks { get; set; } = string.Empty;
    public string DisplayBPM { get; set; } = string.Empty;
    
    // This has got to be the stupidest most annoying thing I've ever done in my life
    // I'm not even into Stepmania like that
    // I'm not adding the rest of the parameters to this right now 
    
    public override string ToString()
    {
        List<string> output = new List<string>
        {
            $"Title: {Title}",
            $"Subtitle: {(string.IsNullOrWhiteSpace(Subtitle) ? "no subtitle" : Subtitle)}",
            $"Artist: {Artist}"
        };

        // Add optional properties only if they have values
        if (!string.IsNullOrWhiteSpace(Genre)) output.Add($"Genre: {Genre}");
        if (!string.IsNullOrWhiteSpace(Credit)) output.Add($"Credit: {Credit}");
        if (!string.IsNullOrWhiteSpace(BannerPath)) output.Add($"Banner: {BannerPath}");
        if (!string.IsNullOrWhiteSpace(BackgroundPath)) output.Add($"Background: {BackgroundPath}");
        if (!string.IsNullOrWhiteSpace(MusicPath)) output.Add($"Music: {MusicPath}");
        if (!string.IsNullOrWhiteSpace(BPMs)) output.Add($"BPMs: {BPMs}");
    
        // Add numeric properties only if they have values
        if (Offset.HasValue) output.Add($"Offset: {Offset}");
        if (SampleStart.HasValue) output.Add($"Sample Start: {SampleStart}");
        if (SampleLength.HasValue) output.Add($"Sample Length: {SampleLength}");

        // Join all the lines with newlines
        return string.Join("\n", output);
    }

    
}