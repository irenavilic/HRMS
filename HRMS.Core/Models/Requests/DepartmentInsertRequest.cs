﻿using System.ComponentModel.DataAnnotations;

namespace HRMS.Core.Models.Requests;

public class DepartmentInsertRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int Level { get; set; }
}