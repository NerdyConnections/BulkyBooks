﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBook.Models;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string ISBN { get; set; }
    [Required]
    public string Author { get; set; }
    [Required]
    [Range(1, 10000, ErrorMessage = "Order must be between 1 to 10000")]
    public double ListPrice { get; set; }
    [Required]
    [Range(1, 10000, ErrorMessage = "Order must be between 1 to 10000")]
    public double Price { get; set; }
    [Required]
    [Range(1, 10000, ErrorMessage = "Order must be between 1 to 10000")]
    public double Price50 { get; set; }
    [Required]
    [Range(1, 10000, ErrorMessage = "Order must be between 1 to 10000")]
    public double Price100 { get; set; }

    public string ImageUrl { get; set; }
    [Required]
    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public Category Category { get; set; }
    [Required]
    public int CoverTypeId { get; set; }

    public CoverType CoverType { get; set; }
}

