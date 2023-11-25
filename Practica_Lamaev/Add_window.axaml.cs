using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySqlConnector;
using Practica_Lamaev.Models;

namespace Practica_Lamaev;

public partial class Add_window : Window
{
    
    private List<Appointments> AppointmentsList { get; set; }
    private Appointments New_Appointment { get; set; }
    private List<Patients> NewPatientsList { get; set; }
    private List<Doctors> NewDoctorsList { get; set; }
    private List<Diagnosis> NewDiagnosisList { get; set; }
    private List<Payments> NewPaymentsList { get; set; }
    private List<Recovery> NewRecoveryList { get; set; }
    
    private MySqlConnectionStringBuilder _connectionSb;

    private window mywin;
    public Add_window(window win)
    {
        InitializeComponent();
        mywin = win;
        AppointmentsList = new List<Appointments>();
        New_Appointment = new Appointments();
        NewPatientsList = new List<Patients>();
        NewDoctorsList = new List<Doctors>();
        NewDiagnosisList = new List<Diagnosis>();
        NewPaymentsList = new List<Payments>();
        NewRecoveryList = new List<Recovery>();
        DataContext = New_Appointment;
        _connectionSb = new MySqlConnectionStringBuilder
        {
            Server = "10.10.1.24",
            Database = "pro3",
            UserID = "user_01",
            Password = "user01pro"
             // Server = "localhost",
             // Database = "practica_lamaev",
             // UserID = "root",
             // Password = "123456"
        };
        UpdatePatients();
        UpdateDoctors();
        UpdateDiagnosis();
        UpdateRecovery();
#if DEBUG
        this.AttachDevTools();
#endif
        
        PatientsComboBox = this.Find<ComboBox>("PatientsComboBox");
        DoctorsComboBox = this.Find<ComboBox>("DoctorsComboBox");
        DiagnosisComboBox = this.Find<ComboBox>("DiagnosisComboBox");
        RecoveryComboBox = this.Find<ComboBox>("RecoveryComboBox");
    }
    void UpdatePatients()
    {
        
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            NewPatientsList.Clear();
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * From Patients";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        NewPatientsList.Add(new Patients()
                        {
                            ID = reader.GetInt32("id"),
                            LastName = reader.GetString("LastName"),
                            FirstName = reader.GetString("FirstName"),
                            Namber = reader.GetString("Namber"),
                            Mail = reader.GetString("Mail"),
                            DOB = reader.GetDateTime("DOB")
                        });
                    }
                }

            }
            connection.Close();
        }
        PatientsComboBox.ItemsSource = NewPatientsList;
    }
    
void UpdateDoctors()
    {
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            NewDoctorsList.Clear();
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * From Doctors";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        NewDoctorsList.Add(new Doctors()
                        {
                            ID = reader.GetInt32("id"),
                            LastName_ = reader.GetString("LastName_"),
                            FirstName = reader.GetString("FirstName"),
                            Namber = reader.GetString("Namber"),
                            Mail = reader.GetString("Mail")
                        });
                    }
                }

            }

            connection.Close();
        }
        
        DoctorsComboBox.ItemsSource = NewDoctorsList;
    }

    void UpdateDiagnosis()
    {
        NewDiagnosisList.Clear();
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * From Diagnosis";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        NewDiagnosisList.Add(new Diagnosis()
                        {
                            ID = reader.GetInt32("id"),
                            Name_Diagnosis = reader.GetString("Name_Diagnosis")
                        });
                    }
                }

            }

            connection.Close();
        }
        DiagnosisComboBox.ItemsSource = NewDiagnosisList;
    }
    void UpdateRecovery()
    {
        NewRecoveryList.Clear();
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * From Recovery";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        NewRecoveryList.Add(new Recovery()
                        {
                            ID = reader.GetInt32("id"),
                            Patient = reader.GetInt32("Patient"),
                            Recovery_indicators = reader.GetInt32("Recovery_indicators"),
                            Expected_date_of_discharge = reader.GetDateTime("Expected_date_of_discharge")
                        });
                    }
                }

            }

            connection.Close();
        }
        RecoveryComboBox.ItemsSource = NewRecoveryList;
    }
    private void Add_Appointments_OnClick(object? sender, RoutedEventArgs e)
    {
        DateTime selectedDate = DatePicker.SelectedDate.Value.DateTime;
        
        using (var c = new MySqlConnection(_connectionSb.ConnectionString))
        {
            c.Open();
            using (var cmd = c.CreateCommand())
            {
                cmd.CommandText =
                    "INSERT INTO Appointments (Patient, Doctor, Payment_Status, Diagnosis, Treatmen, Recovery, Date, Attendance )" +
                    "VALUES (@Patient, @Doctor, @Payment_Status, @Diagnosis, @Treatmen, @Recovery, @Date, @Attendance)";

                cmd.Parameters.AddWithValue("@Patient", (( PatientsComboBox.SelectedItem) as Patients).ID);
                cmd.Parameters.AddWithValue("@Doctor", ((DoctorsComboBox.SelectedItem) as Doctors).ID);
                cmd.Parameters.AddWithValue("@Payment_Status", CheckBox_status.IsChecked); 
                cmd.Parameters.AddWithValue("@Diagnosis", ((DiagnosisComboBox.SelectedItem) as Diagnosis).ID);
                cmd.Parameters.AddWithValue("@Treatmen", New_Appointment.Treatmen);
                cmd.Parameters.AddWithValue("@Recovery", ((RecoveryComboBox.SelectedItem) as Recovery).ID);
                cmd.Parameters.AddWithValue("@Date", selectedDate );
                cmd.Parameters.AddWithValue("@Attendance", CheckBox_attendance.IsChecked);

                var rowsCount = cmd.ExecuteNonQuery();
                if (rowsCount == 0) ;
                {
                    var box = MessageBoxManager.GetMessageBoxStandard("Успех", "Удалось выполнить добавление!",
                        ButtonEnum.Ok);
                    box.ShowAsync();
                }
            }
            c.Close();
        }
        mywin.UpdateAppointments();
        this.Close();
        
    }
}