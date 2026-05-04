using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComicReader.Models;
public class Customer
{
    public int Id {get; set;}
    public string Name {get; set;} = string.Empty;
    public string Email {get; set;} = string.Empty;
    public string PhoneNumber {get; set;} = string.Empty;
    public string Role {get; set;} = "User";
    public bool IsSubscribe {get; set;} = false;
    public DateTime? SubscribeEndDate {get; set;}

    public virtual ICollection<SubscribeHistory> SubscribeHistories {get; set;} = new List<SubscribeHistory>();
}