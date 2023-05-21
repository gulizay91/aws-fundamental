namespace User.Application.ViewModels;

public record UserViewModel : BaseViewModel
{
  public string UserName { get; set; }
  public string ExternalAccount { get; set; }
  public string ExternalAccountId { get; set; }
  public string EmailAddress { get; set; }
  public string AvatarCode { get; set; }
  public DateTime BirthDate { get; set; }
}