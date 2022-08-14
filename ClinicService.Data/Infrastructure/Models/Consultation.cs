﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicService.Data.Infrastructure.Models
{
    [Table("Consultations")]
    public class Consultation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConsultationId { get; set; }

        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }

        [ForeignKey(nameof(Pet))]
        public int PetId { get; set; }

        [Column]
        public DateTime ConsultationDate { get; set; }

        [Column]
        public string Description { get; set; }

        public virtual Client Client { get; set; }

        public virtual Pet Pet { get; set; }
    }
}
