﻿using Key_monitoring.Models;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Key_monitoring.DTOs;

public class KeyListDTO
{
    public List<KeyListElementDTO> List {  get; set; }
}