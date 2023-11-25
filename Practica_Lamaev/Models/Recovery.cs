using System;

namespace Practica_Lamaev.Models;

public class Recovery
{
    public int ID { get; set; }
    public int Patient { get; set; }
    public int Recovery_indicators { get; set; }
    public DateTime Expected_date_of_discharge { get; set; }
}