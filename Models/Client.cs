using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;

namespace DemoTab.Models;

public partial class Client
{
    public int Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Lastname { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateOnly Birthday { get; set; }

    public int IdGender { get; set; }

    public string? Photo { get; set; }

    public DateOnly Registration { get; set; }

    public virtual Gender IdGenderNavigation { get; set; } = null!;

    public string FullName => $"{Firstname} {Name} {Lastname}";
    public string BirthdayText => Birthday.ToString();
    public string RegDateText => Registration.ToString();
    public Bitmap Photopath => Photo != null ? new Bitmap($"Assets/{Photo}") : null;
    public bool GenderBool => IdGender == 1 ? true : false;
}
