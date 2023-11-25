using System;

namespace Practica_Lamaev.Models;

public class Appointments
{
    public int ID { get; set; }
    public string Patient { get; set; }
    public string Doctor { get; set; }
    public bool Payment_Status { get; set; }
    public string Diagnosis { get; set; }
    public string Treatmen { get; set; }
    public int Recovery { get; set; }
    public DateTime Date { get; set; }
    public bool Attendance { get; set; }
}