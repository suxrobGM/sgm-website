namespace SGM.WebApp.Components.ViceCity;

/// <summary>A page section presented as a radio station: drives the pause menu, radar and station banner.</summary>
/// <param name="Id">Section element id and anchor.</param>
/// <param name="Label">Plain section name, shown in the station banner and menu.</param>
/// <param name="GameName">In-game name, used as the section kicker ("Missions", "Payphone").</param>
/// <param name="Icon">Font Awesome classes for the radar blip.</param>
/// <param name="BlipClass">Optional radar blip color modifier.</param>
public sealed record ViceCityStation(
    string Id,
    string Freq,
    string Label,
    string GameName,
    string Icon,
    string? BlipClass = null);

public static class ViceCityStations
{
    public static readonly IReadOnlyList<ViceCityStation> All =
    [
        new("home", "88.1", "Title Screen", "Start", "fa-solid fa-play"),
        new("about", "91.3", "About", "Stats", "fa-solid fa-user"),
        new("hire", "93.5", "Hire Me", "Wanted Level", "fa-solid fa-star", "blip-star"),
        new("skills", "95.7", "Tech Stack", "Loadout", "fa-solid fa-wrench"),
        new("experience", "98.3", "Experience", "Missions", "fa-solid fa-flag", "blip-mission"),
        new("research", "103.4", "Research", "Tapes", "fa-solid fa-compact-disc"),
        new("projects", "105.9", "Projects", "Properties", "fa-solid fa-house", "blip-house"),
        new("education", "107.7", "Education", "Load Game", "fa-solid fa-floppy-disk"),
        new("contact", "110.3", "Contact", "Payphone", "fa-solid fa-phone", "blip-phone"),
    ];

    public static ViceCityStation Get(string id) => All.First(s => s.Id == id);
}
