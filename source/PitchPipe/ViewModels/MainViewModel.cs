using CommunityToolkit.Mvvm.ComponentModel;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Composing;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Music=Melanchall.DryWetMidi.MusicTheory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PitchPipe.ViewModels;

public partial class MainViewModel : ViewModelBase, IDisposable
{
    private OutputDevice outputDevice;

    public MainViewModel()
    {
        Notes = SharpNotes;
        outputDevice = OutputDevice.GetByName("Microsoft GS Wavetable Synth");
        octave = 4;
    }

    public List<string> SharpNotes = new List<string>()
    {
        "C", "C♯", "D", "D♯", "E","F", "F♯", "G","G♯", "A", "A♯", "B"
    };

    public List<string> FlatNotes = new List<string>()
    {
        "C", "D♭", "D", "E♭","E", "F", "G♭","G", "A♭", "A", "B♭", "B",
    };

    [ObservableProperty]
    private List<string> notes;

    [ObservableProperty]
    private int octave;

    public void PlayNote(string name)
    {
        var index = Notes.IndexOf(name);
        
        var octave = Music.Octave.Get(this.octave);
        var note = octave.GetNote((Music.NoteName)index);

        Task.Run(() =>
        {
            var pattern = new PatternBuilder()
                .Note(note, MusicalTimeSpan.Half, (SevenBitNumber)100)
                .Build();
            var midiFile = pattern.ToFile(TempoMap.Default);
            using (var playback = midiFile.GetPlayback(outputDevice))
            {
                // playback.Speed = 2.0;
                playback.Play();
            }
        });
    }

    public void ToggleNotes()
    {
        Notes = Notes == SharpNotes ? FlatNotes : SharpNotes;
    }

    public void UseSharpNotes()
    {
        Notes = SharpNotes;
    }

    public void UseFlatNotes()
    {
        Notes = FlatNotes;
    }

    public void Dispose()
    {
        if (this.outputDevice != null)
        {
            this.outputDevice.Dispose();
            this.outputDevice = null;
        }
    }
}
