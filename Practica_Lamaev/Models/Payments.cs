using System;

namespace Practica_Lamaev.Models;

public class Payments
{
    public int ID { get; set; }
    public int Patient { get; set; }
    public int Amount { get; set; }
    public bool Status { get; set; }
    public DateTime Date { get; set; }
}