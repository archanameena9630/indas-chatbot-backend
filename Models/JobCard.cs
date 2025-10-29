using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;  
namespace JobCardBackend.Models
{
  public class JobCard
  {
    public int Id { get; set; }
    public string ClientName { get; set; }
    public string JobName { get; set; }
  
    public string OrderNo { get; set; }
    public string PaperDetails { get; set; }
    public string? JobSize { get; set; }
    public string Processes { get; set; } = "[]";
    public DateTime JobDate { get; set; }
    public DateTime DeliveryDate { get; set; }
     public int UserId { get; set; }

      [ForeignKey("UserId")]
      public User User { get; set; }
  }
}