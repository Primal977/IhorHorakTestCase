namespace DAL.Entities;

public class BookVolume
{
    public int CategoryID { get; set; }
    public string GoogleVolumeID { get; set; }

    public virtual Category Category { get; set; }
}
