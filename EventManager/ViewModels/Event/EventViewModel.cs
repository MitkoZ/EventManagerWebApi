using System;
using System.ComponentModel.DataAnnotations;

namespace EventManager.ViewModels.Event
{
    public class EventViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        [Display(Name = "Start Date Time")]
        public DateTime StartDateTime { get; set; }
        [Required]
        [Display(Name = "End Date Time")]
        public DateTime EndDateTime { get; set; }
    }
}