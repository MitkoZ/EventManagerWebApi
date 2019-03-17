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
        public DateTime StartDateTime { get; set; }
        [Required]
        public DateTime EndDateTime { get; set; }
    }
}