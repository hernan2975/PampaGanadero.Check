using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls;
using PampaGanadero.Core.Interfaces;

namespace PampaGanadero.Presentation.Maui;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    private readonly ITagReader _reader;
    private readonly ISenasaLocalDb _db;

    public string TagId { get; set; } = "—";
    public string Status { get; set; } = "—";
    public string Location { get; set; } = "—";
    public string Message { get; set; } = string.Empty;
    public bool IsResultVisible { get; set; }
    public bool HasAlerts { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
        _reader = App.Current!.Handler!.MauiContext!.Services.GetRequiredService<ITagReader>();
        _db = App.Current!.Handler!.MauiContext!.Services.GetRequiredService<ISenasaLocalDb>();
    }

    private async void OnScanClicked(object sender, EventArgs e)
    {
        try
        {
            await _reader.InitializeAsync();
            var tags = await _reader.ScanAsync(TimeSpan.FromSeconds(3));

            if (tags.Count > 0)
            {
                var tag = tags[0];
                TagId = tag.SenasaNumber.Value;
                Status = tag.Status.ToString();
                Location = "Santa Rosa, LP";
                Message = tag.Status == Core.Enums.TagStatus.NotRegistered ? "❌ Caravana no registrada" : "";
                HasAlerts = !string.IsNullOrEmpty(Message);
                IsResultVisible = true;
                OnPropertyChanged(nameof(TagId));
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(Location));
                OnPropertyChanged(nameof(Message));
                OnPropertyChanged(nameof(HasAlerts));
                OnPropertyChanged(nameof(IsResultVisible));
            }
            else
            {
                await DisplayAlert("Sin lectura", "No se detectaron caravanas.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
