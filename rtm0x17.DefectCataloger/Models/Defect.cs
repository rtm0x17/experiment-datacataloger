using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace rtm0x17.DefectCataloger.Models;

internal class Defect : INotifyPropertyChanged
{
    private Guid guid = Guid.NewGuid();
    private DateTime dateTime = DateTime.Now;
    private string windowsAccount = string.Empty;
    private string? type;
    private string? note = null;
    private byte[]? photo;
    private byte[]? audioNote;
    private bool hasBeenUploaded;
    private bool operatorAskToExclude;

    public Defect(string defect)
    {
        ArgumentNullException.ThrowIfNull(defect);
        Type = defect;
        OperatorAskToExclude = false;
    }

    /// <summary>
    /// 
    /// </summary>
    public Guid Guid { get => guid; set => guid = value; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime DateTime
    {
        get => dateTime; set => dateTime = value;
    }

    /// <summary>
    /// 
    /// </summary>
    public string WindowsAccount { get => windowsAccount; set => windowsAccount = value; }

    /// <summary>
    /// 
    /// </summary>
    public string? Type { get => type; set => type = value; }

    /// <summary>
    /// 
    /// </summary>
    public string? Note
    {
        get => note;
        set
        {
            note = value;
            NotifyPropertyChanged();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public byte[]? Photo
    {
        get => photo; set
        {
            photo = value;
            NotifyPropertyChanged();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public byte[]? AudioNote
    {
        get => audioNote;
        set
        {
            audioNote = value;
            NotifyPropertyChanged();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool HasBeenUploaded
    {
        get => hasBeenUploaded; set
        {
            hasBeenUploaded = value;
            NotifyPropertyChanged();
        }
    }

    public bool OperatorAskToExclude
    {
        get => operatorAskToExclude; set
        {
            operatorAskToExclude = value;
            NotifyPropertyChanged();
        }
    }

    public string? OperatorReasonForExclusion { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
}
