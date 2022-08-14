﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicService.Data.Infrastructure.Models
{
    [Table("Pets")]
    public class Pet
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PetId { get; set; }

        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }

        [Column]
        [StringLength(20)]
        public string Name { get; set; }

        [Column]
        public DateTime Birthday { get; set; }

        public virtual Client Client { get; set; }

        [InverseProperty(nameof(Consultation.Pet))]
        public virtual ICollection<Consultation> Consultations { get; set; } = new HashSet<Consultation>();
    }
}
