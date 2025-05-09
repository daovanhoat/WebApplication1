﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("User")]
    public class UserModel
    {
        [Key]
        [MaxLength(20)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public string Gener { get; set; }
        [Range(0, 100)]
        public int Age { get; set; } 
        [Range(0, 30)]
        public int  Cong { get; set; }
        public string Avatar { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [ForeignKey("Position")]
        public int PositionId { get; set; }

        public PositionModel Position { get; set; }
        public int DepartmentId { get; set; }
        public int AccountId { get; set; }
    }
}
