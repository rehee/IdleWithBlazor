namespace IdleWithBlazor.Web.Models
{
  public class Setting
  {
    public string ServerHost { get; set; }
    public string ReBaseUrl { get; set; }
    public string Url(string value)
    {
      return $"{ReBaseUrl ?? "/"}{value}";
    }
  }
}
